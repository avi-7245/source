using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GGC.Common
{
    public class GenerateDeemGCDoc
    {
        public string FilePath { get; set; }
        public string ApplicationNo { get; set; }
        public string ApprovalDate { get; set; }
        public string Subject { get; set; }
        public string SpvName { get; set; }
        public string AddCorrespondence { get; set; }
        public string QuantumPowerInjectedMW { get; set; }
        public string MsgData { get; set; }
        public string EmpName { get; set; }
        public string Zone { get; set; }
        public string ZoneName { get; set; }

        private readonly HttpServerUtility _server;

        public GenerateDeemGCDoc(HttpServerUtility server)
        {
            _server = server;
        }

        public FileResult GeneratePdf()
        {
            string templateFilePath = _server.MapPath($"~/HtmlTemplates/deem-gc.xhtml");

            var htmlContent = new StringBuilder(File.ReadAllText(templateFilePath));
            htmlContent
                .Replace("{{ApprovalDate}}", ApprovalDate)
                .Replace("{{ApplicationNo}}", ApplicationNo)
                .Replace("{{dsDeemed_Table.SPV_Name}}", SpvName)
                .Replace("{{dsDeemed_Table.Add_correspondence}}", AddCorrespondence.ToAddress())
                .Replace("{{dsDeemed_Data.subject}}", Subject)
                .Replace("{{dsDeemed_Table.Quantum_power_injected_MW}}", QuantumPowerInjectedMW)
                .Replace("{{dsDeemed_Data.MsgData}}", MsgData)
                .Replace("{{dsDeemed_EmpData.EMP_NAME}}", EmpName)
                .Replace("{{dsDeemed_Table.ZONE}}", Zone)
                .Replace("{{dsDeemed_FWC.zone_name}}", ZoneName);

            var pdf = new TextSharpPdf(_server, filePath: FilePath, htmlContent: htmlContent.ToString(), footerText: $"Disclaimer: This is system generated report (Online Application No: {ApplicationNo}).Signature not required.");
            return pdf.GeneratePDF();
        }
    }
}