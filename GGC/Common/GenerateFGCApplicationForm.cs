using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace GGC.Common
{
    public class GenerateFGCApplicationForm
    {

        public string APPLICATION_NO { get; set; }
        public string MEDAProjectID { get; set; }
        public string LAT { get; set; }
        public string Longt { get; set; }
        public string NAME_OF_TRAN_LIC { get; set; }
        public string NAME_OF_EHV_SS { get; set; }
        public string VOLT_LVL_INTER { get; set; }
        public string DET_OF_INTER_CON { get; set; }
        public string GEN_VOLT_STEPUP_VOLT { get; set; }
        public string DET_FEEDER_PROT { get; set; }
        public string NAME_OF_SPD { get; set; }
        public string ADDRESS_FOR_CORRESPONDENCE { get; set; }
        public string CONT_PER_NAME_1 { get; set; }
        public string CONT_PER_DESIG_1 { get; set; }
        public string CONT_PER_PHONE_1 { get; set; }
        public string CONT_PER_MOBILE_1 { get; set; }
        public string CONT_PER_EMAIL_1 { get; set; }
        public string PROJECT_CAPACITY_MW { get; set; }
        public string PROJECT_LOC { get; set; }
        public string PROJECT_DISTRICT { get; set; }
        public DataTable CommDet { get; set; }
        public DataTable CertDet { get; set; }

        private readonly HttpServerUtility _server;
        private readonly string _filePath;

        public GenerateFGCApplicationForm(HttpServerUtility server, string filePath)
        {
            _server = server;
            _filePath = filePath;
        }

        public FileResult GeneratePdf()
        {
            string templateFilePath = _server.MapPath($"~/HtmlTemplates/fgc-application-form.xhtml");
            var htmlContent = new StringBuilder(File.ReadAllText(templateFilePath));

            var commDet = CommDet.AsEnumerable().Select(dr => $"<tr>" +
            $"<td>{dr["UNIT_NO"]}</td>" +
            $"<td>{dr["UNIT_SIZE"]}</td>" +
            $"<td>{Convert.ToDateTime(dr["DT_OF_WORK_COMMENCMENT"]):dd/MM/yyyy}</td>" +
            $"<td>{Convert.ToDateTime(dr["DT_WORK_COMPLETE"]):dd/MM/yyyy}</td>" +
            $"<td>{Convert.ToDateTime(dr["DT_SYNCH"]):dd/MM/yyyy}</td>" +
            $"<td>{Convert.ToDateTime(dr["DT_SCH_COD"]):dd/MM/yyyy}</td></tr>");

            var certDet = CertDet.AsEnumerable().Select(dr => $"<tr>" +
            $"<td>{dr["certification_detail"]}</td>" +
            $"<td>{(Convert.ToBoolean(dr["cert_value"]) ? "Yes" : "No")}</td></tr>");


            htmlContent
                .Replace("{{APPLICATION_NO}}", APPLICATION_NO)
                .Replace("{{MEDAProjectID}}", MEDAProjectID)
                .Replace("{{NAME_OF_SPD}}", NAME_OF_SPD)
                .Replace("{{ADDRESS_FOR_CORRESPONDENCE}}", ADDRESS_FOR_CORRESPONDENCE)
                .Replace("{{CONT_PER_NAME_1}}", CONT_PER_NAME_1)
                .Replace("{{CONT_PER_DESIG_1}}", CONT_PER_DESIG_1)
                .Replace("{{CONT_PER_PHONE_1}}", CONT_PER_PHONE_1)
                .Replace("{{CONT_PER_MOBILE_1}}", CONT_PER_MOBILE_1)
                .Replace("{{CONT_PER_EMAIL_1}}", CONT_PER_EMAIL_1)
                .Replace("{{PROJECT_CAPACITY_MW}}", PROJECT_CAPACITY_MW.ToString())
                .Replace("{{PROJECT_LOC}}", PROJECT_LOC)
                .Replace("{{PROJECT_DISTRICT}}", PROJECT_DISTRICT)
                .Replace("{{LAT}}", LAT)
                .Replace("{{Longt}}", Longt)
                .Replace("{{NAME_OF_TRAN_LIC}}", NAME_OF_TRAN_LIC)
                .Replace("{{NAME_OF_EHV_SS}}", NAME_OF_EHV_SS)
                .Replace("{{VOLT_LVL_INTER}}", VOLT_LVL_INTER)
                .Replace("{{DET_OF_INTER_CON}}", DET_OF_INTER_CON)
                .Replace("{{GEN_VOLT_STEPUP_VOLT}}", GEN_VOLT_STEPUP_VOLT)
                .Replace("{{DET_FEEDER_PROT}}", DET_FEEDER_PROT)
                .Replace("{{CommDet}}", commDet.Any() ? string.Join("", commDet) : "")
                .Replace("{{CertDet}}", certDet.Any() ? string.Join("", certDet) : "");

            var pdf = new TextSharpPdf(_server, filePath: _filePath, htmlContent: htmlContent.ToString(), showLogo: false);
            return pdf.GeneratePDF();

        }
    }
}