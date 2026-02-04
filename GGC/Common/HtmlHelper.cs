using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public static class HtmlHelper
    {
        public static string GenerateEmbeddedPdf(string pdfUrl)
        {
            const string EmbeddedPdfTemplate = "<object data=\"{0}\" type=\"application/pdf\" class=\"rounded-4 embedded-pdf\"" +
                                               "If you are unable to view file, you can download from <a href = \"{0}\">here</a>" +
                                               " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file." +
                                               "</object>";

            return string.Format(EmbeddedPdfTemplate, pdfUrl);
        }
    }
}