using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GGC.Common
{
    public class GenerateFinalGridConnectivityDoc
    {
        public string Date { get; set; }
        public string SPV_Name { get; set; }
        public string Add_correspondence { get; set; }
        public string Subject { get; set; }
        public string MsgData { get; set; }
        public string MsgData2 { get; set; }
        public string EMP_NAME { get; set; }

        private readonly HttpServerUtility _server;
        private string _filePath;
        public GenerateFinalGridConnectivityDoc(HttpServerUtility server, string filePath)
        {
            _server = server;
            _filePath = filePath;
        }
        public FileResult GeneratePdf()
        {
            string templateFilePath = _server.MapPath($"~/HtmlTemplates/final-grid-connectivity.xhtml");
            var htmlContent = new StringBuilder(File.ReadAllText(templateFilePath));

            htmlContent.Replace("{{Date}}", Date)
                          .Replace("{{SPV_Name}}", SPV_Name)
                          .Replace("{{Add_correspondence}}", Add_correspondence.ToAddress())
                          .Replace("{{Subject}}", Subject)
                          .Replace("{{MsgData}}", MsgData)
                          .Replace("{{MsgData2}}", MsgData2)
                          .Replace("{{EMP_NAME}}", EMP_NAME);
            var pdf = new TextSharpPdf(_server, filePath: _filePath, htmlContent: htmlContent.ToString());
            return pdf.GeneratePDF();
        }
    }
}