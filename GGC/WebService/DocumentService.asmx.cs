using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using GGC.Common;
using GGC.Scheduler;
using log4net;

namespace GGC.WebService
{
    /// <summary>
    /// Summary description for DocumentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DocumentService : System.Web.Services.WebService
    {
        readonly string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected static readonly ILog _log = LogManager.GetLogger(typeof(DocumentService));

        [WebMethod]
        public void DownloadGC(string applicationId, string fileName)
        {

            string folderPath = Server.MapPath("~/Files/MSKVY/" + applicationId + "/");
            //string strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + applicationId + "'";
            string strQuery = "SELECT maspd.ADDRESS_FOR_CORRESPONDENCE" +
                ",maspd.SPV_Name" +
                ",maspd.CONT_PER_MOBILE_1" +
                ",ma.APPLICATION_NO" +
                ",ma.MEDAProjectID" +
                ",ma.Quantum_power_injected_MW" +
                ",ma.SPV_Name" +
                ",ma.STU_INJECTION_VOLTAGE" +
                ",ma.STU_POINT_OF_INJECT" +
                ",ma.ZONE,ma.PROJECT_LOC" +
                ",ma.PROJECT_TALUKA" +
                ",ma.PROJECT_DISTRICT" +
                " FROM mskvy_applicantdetails_spd maspd LEFT JOIN mskvy_applicantdetails ma ON ma.APPLICATION_NO = maspd.APPLICATION_NO AND ma.MEDAProjectID = maspd.MEDAProjectID" +
                $" where ma.APPLICATION_NO='{applicationId}'";

            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);


            DataSet dsFWC = new DataSet();
            strQuery = $"SELECT z.zone_name FROM zone_circle_district_map z WHERE dist_name = '{dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"]}' LIMIT 1;";
            dsFWC = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

            DataTable zoneTable = dsFWC.Tables[0].Copy();
            zoneTable.TableName = "ZoneData";
            dsResult.Tables.Add(zoneTable);

            DataSet dsCommDet = new DataSet();
            strQuery = "SELECT * FROM empmaster WHERE role_id=53 and isactive='Y' order by 1";
            dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            dsCommDet.Tables[0].TableName = "empDet";
            dsResult.Tables.Add(dsCommDet.Tables[0].Copy());

            DataSet dsLandDet = new DataSet();
            strQuery = "SELECT * FROM mskvy_landdet WHERE APPLICATION_NO='" + applicationId + "'";
            dsLandDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            dsLandDet.Tables[0].TableName = "LandDet";
            dsResult.Tables.Add(dsLandDet.Tables[0].Copy());


            string strSubject = "Approval for Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 (Project Code : " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online GC Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ") .";

            string strData = "In view of above, as approved by competent authority, In-principle Grid Connectivity for your " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + " KV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

            string strData2 = "The evacuation arrangement shall be totally at the risk and cost of M/s " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + ". In case of tripping and/or outage on this " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + " kV Single Circuit (S/C) line and/or bay, there will be loss of generation and which will be M/s " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " responsibility and MSETCL will not be held responsible for the said loss and you will not claim for the loss of generation by what so ever may be the reason from MSETCL.";

            if (dsResult.Tables[0].Rows.Count > 0)
            {
                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                //string FileName = "GC" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + "F.pdf";
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }

                var doc = new GenerateGCDoc(Server, folderPath + fileName, applicationId)
                {
                    EMP_NAME = dsResult.Tables["empDet"].Rows[0]["EMP_NAME"].ToString(),
                    Date = DateTime.Now.ToString(format: "dd/MM/yyyy"),
                    Add_correspondence = dsResult.Tables[0].Rows[0]["ADDRESS_FOR_CORRESPONDENCE"].ToString(),
                    MsgData = strData,
                    ApplicationNo= dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(),
                    MsgData2 = strData2,
                    Quantum_power_injected_MW = dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString(),
                    SPV_Name = dsResult.Tables[0].Rows[0]["SPV_Name"].ToString(),
                    STU_INJECTION_VOLTAGE = dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString(),
                    STU_POINT_OF_INJECT = dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString(),
                    Subject = strSubject,
                    ZONE = dsResult.Tables[0].Rows[0]["ZONE"].ToString(),
                    ZONE_NAME = dsResult.Tables["ZoneData"].Rows[0]["zone_name"].ToString(),
                    AnnexureB = dsResult.Tables["LandDet"]
                };

                try
                {
                    var result = doc.GeneratePdf();

                    // Set response headers
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");

                    // Write PDF bytes to response stream
                    HttpContext.Current.Response.BinaryWrite(result.Data);
                    HttpContext.Current.Response.Flush(); 
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);
                }
                finally
                {
                    // Ensure that the response is closed and resources are released
                    HttpContext.Current.Response.Close();
                }


            }
        }
    }
}
