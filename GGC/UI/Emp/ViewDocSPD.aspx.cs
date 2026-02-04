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

namespace GGC.UI.Emp
{
    public partial class ViewDocSPD : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ViewDocSPD));
        int filetype = 0;
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {

            int roleID = int.Parse(Session["EmpRole"].ToString());
            if (roleID < 10)
            {
                lnkAdmin.Visible = true;
                lnkEE.Visible = false;
            }
            else
            {
                if (roleID > 50)
                {
                    lnkEE.Visible = true;
                    lnkAdmin.Visible = false;
                }
            }

            if (!Page.IsPostBack)
            {
                Session["filetype"] = filetype;
                if (Request.QueryString["application"] != null)
                {
                    string ID = Request.QueryString["application"].ToString();
                    lblAppNo.Text = ID;
                    Session["APPID"] = ID;

                    var dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"SELECT muds.FileType, mdls.DOC_NAME, muds.FieName AS FileName FROM mskvy_upload_doc_spd muds JOIN ( SELECT muds.FileType , MAX(muds.CreateDT) AS CreateDT FROM mskvy_upload_doc_spd muds WHERE muds.Application_No = '{ID}' GROUP BY muds.FileType ) temp ON temp.FileType = muds.FileType AND temp.CreateDT = muds.CreateDT ,mskvy_doc_list_spd mdls WHERE mdls.DOC_SEQ = muds.FileType ORDER BY muds.FileType;");
                    Session["TotalDocuments"] = dsResult.Tables[0].Rows.Count;
                    Session["filetype"] = 1;
                    getDocument(ID, 1);
                }
            }
        }

        protected string getDocument(string applicationId, int doctype)
        {
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strFolderName = string.Empty;


            try
            {
                //  strQuery = "select FieName as docname from mskvy_upload_doc_spd where APPLICATION_NO='" + applicationId + "' and FileType=" + doctype + " order by CreateDT desc";
                //strQuery = "select FieName as filename,DOC_NAME as DOC_NAME from mskvy_upload_doc_spd a,mskvy_doc_list_spd b where b.doc_seq=a.filetype and APPLICATION_NO='" + applicationId + "' and FileType=" + doctype + " order by CreateDT ";
                strQuery = $"SELECT muds.FileType, mdls.DOC_NAME, muds.FieName AS FileName FROM mskvy_upload_doc_spd muds JOIN ( SELECT muds.FileType , MAX(muds.CreateDT) AS CreateDT FROM mskvy_upload_doc_spd muds WHERE muds.Application_No = '{applicationId}' GROUP BY muds.FileType ) temp ON temp.FileType = muds.FileType AND temp.CreateDT = muds.CreateDT ,mskvy_doc_list_spd mdls WHERE mdls.DOC_SEQ = muds.FileType ORDER BY muds.FileType;";
                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    var dataRow = dsResult.Tables[0].AsEnumerable().Skip(doctype - 1).First();

                    if (dsResult.Tables[0].Rows[0][0].ToString() != "")
                    {
                        strFileName = dataRow["DOC_NAME"].ToString();
                        lblDocName.Text = strFileName;
                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                        embed += "</object>";
                        //ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/MSKVY/" + applicationId + "/" + dsResult.Tables[0].Rows[0]["filename"].ToString()));
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/MSKVY/" + applicationId + "/" + dataRow["FileName"].ToString()));
                        log.Error("ltEmbed" + ltEmbed);
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
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strFolderName = string.Empty;


            try
            {




                strQuery = "select AppFormDocName as docname from mskvy_applicantdetails_spd where APPLICATION_NO='" + Session["APPID"].ToString() + "'";

                DataSet dsResult = new DataSet();

                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0][0].ToString() != "")
                    {
                        strFileName = dsResult.Tables[0].Rows[0]["docname"].ToString();
                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                        embed += "</object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/AppFormSPD/" + Session["APPID"].ToString() + "/" + strFileName));
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
        }

        protected void btnViewDocNext_Click(object sender, EventArgs e)
        {
            filetype = int.Parse(Session["filetype"].ToString());
            //if (filetype == 7)
            if (filetype == int.Parse(Session["TotalDocuments"].ToString()))
            {
                filetype = 1;
            }
            else
            {
                filetype++;
            }
            getDocument(Session["APPID"].ToString(), filetype);
            Session["filetype"] = filetype;
        }

        protected void btnViewDocPrev_Click(object sender, EventArgs e)
        {
            filetype = int.Parse(Session["filetype"].ToString());
            if (filetype == 1)
            {
                //filetype = 7;
                filetype = int.Parse(Session["TotalDocuments"].ToString());
            }
            else
            {
                filetype--;
            }
            getDocument(Session["APPID"].ToString(), filetype);
            Session["filetype"] = filetype;
        }

        protected void btnAppReceipt_Click(object sender, EventArgs e)
        {
            string strPostName = string.Empty;
            string strUserName = Session["user_name"].ToString();
            string strQuery = "select * from billdesk_txn where ApplicationNo='" + Session["APPID"].ToString() + "' and typeofpay='MSKVYSPDRegistration' and AuthStatus='0300' ";

            DataSet dsResult = new DataSet();
            dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            if (dsResult.Tables[0].Rows.Count > 0)
            {
                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                string FileName = "Receipt_" + strUserName + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                string serverFilePath = Server.MapPath("~/Reports/") + FileName;
                try
                {

                    LocalReport report = new LocalReport();
                    report.ReportPath = Server.MapPath("~/PDFReport/") + "Receipt.rdlc";

                    ReportDataSource rds = new ReportDataSource();
                    rds.Name = "DS_Receipt";//This refers to the dataset name in the RDLC file
                    rds.Value = dsResult.Tables[0];
                    report.DataSources.Add(rds);



                    //ReportDataSource rds3 = new ReportDataSource();
                    //rds3.Name = "DataSet3";//This refers to the dataset name in the RDLC file
                    //rds3.Value = dsResult.Tables["Table3"];
                    //report.DataSources.Add(rds3);

                    //ReportDataSource rds4 = new ReportDataSource();
                    //rds4.Name = "DataSet4";//This refers to the dataset name in the RDLC file
                    //rds4.Value = dsResult.Tables["Table4"];
                    //report.DataSources.Add(rds4);

                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "Receipt.rdlc";
                    reportViewer.LocalReport.DataSources.Clear();

                    reportViewer.LocalReport.DataSources.Add(rds);

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
                    log.Error(ErrorMessage);

                }
            }
        }
    }
}