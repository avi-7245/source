using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;
using System.Net.Mail;
using log4net;
using System.Reflection;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using GGC.Scheduler;
using GGC.Common;

namespace GGC.UI.Emp
{
    public partial class ViewDocFGC : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ViewDocSPD));
        int filetype = 1;
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["filetype"] = filetype;
                if (Request.QueryString["application"] != null)
                {
                    string ID = Request.QueryString["application"].ToString();
                    lblAppNo.Text = ID;
                    Session["APPID"] = ID;
                    getDocument(ID, filetype);
                }
                HdfDocType.Value = filetype.ToString();
            }
        }
        protected string getDocument(string applicationId, int doctype)
        {
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strFolderName = string.Empty;

            try
            {
                strQuery = "select File_Name as docname from fgc_app_doc where APPLICATION_NO='" + applicationId + "' and DOC_TYPE=" + doctype;

                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                DataSet dsFileName = new DataSet();
                dsFileName = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM fgc_doc_list_master WHERE srno=" + doctype);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0][0].ToString() != "")
                    {
                        strFileName = dsResult.Tables[0].Rows[0]["docname"].ToString();
                        lblDocName.Text = dsFileName.Tables[0].Rows[0]["doc_name"].ToString();
                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                        embed += "</object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/MSKVY/FGC/" + applicationId + "/" + strFileName));
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>alert('No document found.');</script>");
                    }
                }

            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors

            }


            return strFileName;
        }

        protected void btnAppForm_Click(object sender, EventArgs e)
        {
            string strQuery = "select a.APPLICATION_NO, a.MEDAProjectID, a.LAT, a.Longt, a.NAME_OF_TRAN_LIC, a.NAME_OF_EHV_SS, a.VOLT_LVL_INTER, a.DET_OF_INTER_CON, a.GEN_VOLT_STEPUP_VOLT, a.DET_FEEDER_PROT,b.NAME_OF_SPD, b.GSTIN_NO, b.PAN_NUMBER, b.ADDRESS_FOR_CORRESPONDENCE, b.CONT_PER_NAME_1, b.CONT_PER_DESIG_1, b.CONT_PER_PHONE_1, b.CONT_PER_MOBILE_1, b.CONT_PER_EMAIL_1,b.SPV_Name,b.ClusterName,b.PROJECT_TYPE,b.PROJECT_CAPACITY_MW,b.PROJECT_LOC,b.PROJECT_TALUKA,b.PROJECT_DISTRICT,b.GCLetter_No, b.GCLetter_Date, b.GCLetter_ValidityDate, b.INTERCONNECTION_AT, b.MSEDCL_Tender_No, b.PPA_DETAILS, b.TOTAL_REQUIRED_LAND, b.LAND_IN_POSSESSION, b.TOTAL_FOREST_LAND, b.IS_FOREST_LAND, b.STATUS_FOREST_LAND, b.BIRD_SANCTURY_ETC,b.ZONE from finalgcapproval a, mskvy_applicantdetails_spd b where a.APPLICATION_NO=b.APPLICATION_NO and a.APPLICATION_NO='" + Session["APPID"].ToString() + "'";
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

            DataSet dsCommDet = new DataSet();
            strQuery = "SELECT * FROM fgc_comm_schedule where APPLICATION_NO='" + Session["APPID"].ToString() + "'";
            dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            dsCommDet.Tables[0].TableName = "CommDet";
            dsResult.Tables.Add(dsCommDet.Tables[0].Copy());

            DataSet dscertDet = new DataSet();
            strQuery = "SELECT a.certification_detail,cert_value FROM fgc_cert_list a,fgc_cert_data b WHERE a.srno=b.cert_type_srno and b.application_no='" + Session["APPID"].ToString() + "'";
            dscertDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            dscertDet.Tables[0].TableName = "CertDet";
            dsResult.Tables.Add(dscertDet.Tables[0].Copy());

            //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\TestRDLC\testRDLCWeb\FGCAppl.xsd");

            if (dsResult.Tables[0].Rows.Count > 0)
            {
                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                string FileName = "FGCForm_" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                string serverFilePath = Server.MapPath("~/Reports/") + FileName;
                try
                {

                    LocalReport report = new LocalReport();
                    report.ReportPath = Server.MapPath("~/") + "PDFReport/FGCApplicationForm.rdlc";

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "dsFGC_Table";//This refers to the dataset name in the RDLC file
                    rds.Value = dsResult.Tables[0];
                    report.DataSources.Add(rds);

                    ReportDataSource rds2 = new ReportDataSource();
                    rds2.Name = "dsFGC_Comm";//This refers to the dataset name in the RDLC file
                    rds2.Value = dsResult.Tables["CommDet"];
                    report.DataSources.Add(rds2);

                    ReportDataSource rds3 = new ReportDataSource();
                    rds3.Name = "dsFGC_cert";//This refers to the dataset name in the RDLC file
                    rds3.Value = dsResult.Tables["CertDet"];
                    report.DataSources.Add(rds3);

                    //ReportDataSource rds3 = new ReportDataSource();
                    //rds3.Name = "DataSet3";//This refers to the dataset name in the RDLC file
                    //rds3.Value = dsResult.Tables["Table3"];
                    //report.DataSources.Add(rds3);

                    //ReportDataSource rds4 = new ReportDataSource();
                    //rds4.Name = "DataSet4";//This refers to the dataset name in the RDLC file
                    //rds4.Value = dsResult.Tables["Table4"];
                    //report.DataSources.Add(rds4);

                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.ReportPath = Server.MapPath("~/") + "PDFReport/FGCApplicationForm.rdlc";
                    reportViewer.LocalReport.DataSources.Clear();

                    reportViewer.LocalReport.DataSources.Add(rds);
                    reportViewer.LocalReport.DataSources.Add(rds2);
                    reportViewer.LocalReport.DataSources.Add(rds3);
                    // reportViewer.LocalReport.DataSources.Add(rds3);


                    reportViewer.LocalReport.Refresh();


                    string mimeType, encoding, extension, deviceInfo;
                    string[] streamids;
                    Warning[] warnings;
                    string format = "PDF";

                    deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                    byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                    FileStream fs1 = new FileStream(serverFilePath, FileMode.Create);
                    fs1.Write(bytes, 0, bytes.Length);
                    fs1.Close();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "PDF";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.BinaryWrite(bytes);
                    Response.Flush();
                    Response.End();

                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    //   log.Error(ErrorMessage);

                }
            }
        }

        protected void btnViewDocNext_Click(object sender, EventArgs e)
        {
            filetype = int.Parse(Session["filetype"].ToString());
            
            if (filetype == 15)
            {
                filetype = 1;
            }
            else
            {
                filetype++;
            }
            HdfDocType.Value = filetype.ToString();
            getDocument(Session["APPID"].ToString(), filetype);
            Session["filetype"] = filetype;
        }

        protected void btnViewDocPrev_Click(object sender, EventArgs e)
        {
            filetype = int.Parse(Session["filetype"].ToString());
            
            if (filetype == 1)
            {
                filetype = 15;
            }
            else
            {
                filetype--;
            }
            HdfDocType.Value = filetype.ToString();
            getDocument(Session["APPID"].ToString(), filetype);
            Session["filetype"] = filetype;
        }

        protected void btnApp_Click(object sender, EventArgs e)
        {
            string strQuery = "update fgc_app_doc set Remark='" + txtRemark.Text + "',isApp_Ret='Y' where application_no='" + Session["APPID"].ToString() + "' and DOC_TYPE='"+HdfDocType.Value+"'";
            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
            lblResult.Text = "Document approved.";
        }

        protected void btnRet_Click(object sender, EventArgs e)
        {
            string strQuery = "delete from fgc_app_doc where application_no='" + Session["APPID"].ToString() + "' and DOC_TYPE='" + HdfDocType.Value + "'";
            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

            //strQuery = "update finalgcapproval set isAppr_Rej_Ret='R' ,WF_STATUS_CD=1 , APP_STATUS='Application returned due to problem in document.',APP_STATUS_DT=now() where application_no='" + Session["APPID"].ToString() + "' and DOC_TYPE='" + HdfDocType.Value + "'";
            strQuery = "update finalgcapproval set isAppr_Rej_Ret='R' ,WF_STATUS_CD=1 , APP_STATUS='Application returned due to problem in document.',APP_STATUS_DT=now() where application_no='" + Session["APPID"].ToString() + "'";
            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
            
            lblResult.Text = "Document returned.";
            strQuery = "select * from mskvy_applicantdetails_spd where application_no='" + Session["APPID"].ToString() + "' ";
            DataSet dsResult=SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            SendEmail sm = new SendEmail();

            string strBody = string.Empty;

            strQuery = "select DOC_NAME from fgc_doc_list_master where DOC_TYPE='" + HdfDocType.Value + "'";
            DataSet dsDocName=SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            
            
            strBody = "M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " , <br/>" +
                "Your Final Grid connectivity document for application no " + Session["APPID"].ToString() + " has been returned. Kindly upload it again. <br/>";
            strBody += "<b>" + dsDocName.Tables[0].Rows[0]["DOC_NAME"].ToString() + "</b><br/>";
            strBody += "State Transmission Utility (STU)" + "<br/>";
            strBody += "MSETCL  " + "<br/>";
            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

            sm.Send(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString(), ConfigurationManager.AppSettings["MMCCEmailID"].ToString(), "Final GC document Returned", strBody);

        }

     }
}