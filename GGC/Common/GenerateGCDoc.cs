using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public class GenerateGCDoc
    {
        public string Date { get; set; }
        public string SPV_Name { get; set; }
        public string ApplicationNo { get; set; }
        public string Add_correspondence { get; set; }
        public string Subject { get; set; }
        public string Quantum_power_injected_MW { get; set; }
        public string MsgData { get; set; }
        public string STU_INJECTION_VOLTAGE { get; set; }
        public string STU_POINT_OF_INJECT { get; set; }
        public string ZONE { get; set; }
        public string MsgData2 { get; set; }
        public string EMP_NAME { get; set; }
        public string ZONE_NAME { get; set; }
        public DataTable AnnexureB { get; set; }

        private readonly HttpServerUtility _server;

        private readonly string _filePath;
        private readonly string _applicationNo;

        public GenerateGCDoc(HttpServerUtility server, string filePath, string applicationId)
        {
            _server = server;
            _filePath = filePath;
            _applicationNo = applicationId;
        }

        public FileResult GeneratePdf()
        {
            string templateFilePath = _server.MapPath($"~/HtmlTemplates/gc.xhtml");
            string htmlContent = File.ReadAllText(templateFilePath);

            var landDetails = AnnexureB.AsEnumerable().Select(dr => $"<tr>" +
            $"<td>{dr["landDistrictName"]}</td>" +
            $"<td>{dr["landTalukaName"]}</td>" +
            $"<td>{dr["landVillageName"]}</td>" +
            $"<td>{dr["landPanchayatName"]}</td>" +
            $"<td>{dr["landSurveyNo"]}</td>" +
            $"<td>{dr["landSubSurveyNo"]}</td>" +
            $"<td>{dr["landDistanceFromSs"]}</td>" +
            $"<td>{dr["landArea"]}</td></tr>");

            htmlContent = htmlContent
                .Replace("{{Date}}", Date)
                .Replace("{{ApplicationNo}}", ApplicationNo)
                .Replace("{{SPV_Name}}", SPV_Name)
                .Replace("{{Add_correspondence}}", Add_correspondence.ToAddress())
                .Replace("{{Subject}}", Subject)
                .Replace("{{Quantum_power_injected_MW}}", Quantum_power_injected_MW)
                .Replace("{{MsgData}}", MsgData)
                .Replace("{{STU_INJECTION_VOLTAGE}}", STU_INJECTION_VOLTAGE)
                .Replace("{{STU_POINT_OF_INJECT}}", STU_POINT_OF_INJECT)
                .Replace("{{ZONE}}", ZONE)
                .Replace("{{MsgData2}}", MsgData2)
                .Replace("{{EMP_NAME}}", EMP_NAME)
                .Replace("{{ZONE_NAME}}", ZONE_NAME)
                .Replace("{{AnnexureBData}}", string.Join("", landDetails));

            var pdf = new TextSharpPdf(_server, filePath: _filePath, htmlContent: htmlContent, footerText: $"Disclaimer: This is system generated report (Online Application No: {_applicationNo}).Signature not required.");
            return pdf.GeneratePDF();
        }
    }
}