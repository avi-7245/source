using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace GGC.Common
{
    public class FileResult
    {
        public bool Success { get; set; }
        public string FileName { get; set; }
        public string ErrorMessage { get; set; }
        public byte[] Data { get; set; }
    }

    public class Footer : PdfPageEventHelper
    {
        private readonly Phrase footerPhrase;
        public Footer(string text)
        {
            footerPhrase = new Phrase(text);
        }
        /*        public override void OnEndPage(PdfWriter writer, Document document)
                {
                    base.OnEndPage(writer, document);

                    // Create a footer
                    PdfPTable footer = new PdfPTable(1);
                    //footer.TotalWidth = 450;
                    footer.SetTotalWidth(new float[] { PageSize.A4.Width - document.LeftMargin - document.RightMargin });
                    footer.LockedWidth = true;

                    footer.HorizontalAlignment = Element.ALIGN_CENTER;
                    PdfPCell cell = new PdfPCell(footerPhrase);
                    cell.Border = 0;
                    footer.AddCell(cell);

                    footer.WriteSelectedRows(0, -1, document.LeftMargin + 50, document.Bottom, writer.DirectContent);
                }*/
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Create a footer table with one column
            PdfPTable footer = new PdfPTable(1);
            footer.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            footer.HorizontalAlignment = Element.ALIGN_CENTER;
            footer.LockedWidth = true;

            // Create the cell
            PdfPCell cell = new PdfPCell(footerPhrase);
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_LEFT; // Align text to the left
            cell.PaddingLeft = 0;
            cell.PaddingRight = 0;
            cell.PaddingTop = 0;
            cell.PaddingBottom = 0;

            footer.AddCell(cell);

            // Position the footer within the page margins
            float x = document.LeftMargin;
            float y = document.BottomMargin - 10; // Adjust as needed
            footer.WriteSelectedRows(0, -1, x, y, writer.DirectContent);
        }

    }
    public class TextSharpPdf
    {
        private readonly HttpServerUtility _server;
        private readonly string _filePath;
        private readonly string _htmlContent;
        private readonly string _footerText;
        private readonly bool _showLogo;
        public TextSharpPdf(HttpServerUtility server, string filePath, string htmlContent, string footerText = null, bool showLogo = true)
        {
            _server = server;
            _filePath = filePath;
            _htmlContent = htmlContent;
            _footerText = footerText;
            _showLogo = showLogo;

        }
        public FileResult GeneratePDF()
        {
            string directoryPath = Path.GetDirectoryName(_filePath);

            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            var result = new FileResult();
            result.Success = false;
            string logoFilePath = _server.MapPath($"~/HtmlTemplates/logo.jpg");
            var document = new Document();
            try
            {
                var writer = PdfWriter.GetInstance(document, new FileStream(_filePath, FileMode.Create));

                if (!string.IsNullOrEmpty(_footerText))
                {
                    writer.PageEvent = new Footer(_footerText);
                }

                document.Open();

                if (_showLogo)
                {
                    var image = iTextSharp.text.Image.GetInstance(logoFilePath);
                    image.ScaleToFit(100f, 100f);
                    float horizontalPos = (document.PageSize.Width - image.ScaledWidth) / 2;
                    float verticalPos = document.PageSize.Height - image.ScaledHeight;
                    image.SetAbsolutePosition(horizontalPos - 10, verticalPos - 10);
                    document.Add(image);

                    var spaceParagraph = new Paragraph();
                    spaceParagraph.SpacingBefore = 30f;
                    document.Add(spaceParagraph);

                }


                var stringReader = new StringReader(_htmlContent);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, stringReader);

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                result.Success = false;
            }
            finally
            {
                document.Close();
            }

            result.Success = true;
            result.Data = File.ReadAllBytes(_filePath);

            return result;
        }
    }
}