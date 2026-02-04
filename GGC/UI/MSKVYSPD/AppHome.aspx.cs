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
using MySqlX.XDevAPI;

namespace GGC.UI.MSKVYSPD
{
    public partial class AppHome : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppHome));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            //            log.Error("1");
            if (!Page.IsPostBack)
            {
                fillRegDet();
                //          log.Error("2");

                fillGrid();
                //            log.Error("3");

                Session["NewApplication"] = "false";
            }
            //          log.Error("4");

        }

        protected void fillRegDet()
        {

            string strUserName = string.Empty;
            strUserName = Session["user_name"].ToString();
            try
            {

                string strQuery = "select * from APPLICANT_REG_DET where user_name='" + strUserName + "'";
                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    txtOrganizationName.Text = "Welcome " + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                    ViewState["OrgName"] = dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                    Session["PROMOTOR_NAME"] = dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                }
                else
                {
                    //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
                }


            }

            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
            }


        }
        protected void fillGrid()
        {

            string strUserName = Session["user_name"].ToString();
            DataSet dsResult = new DataSet();
            try
            {

                string strQuery = string.Empty;
                MySqlCommand cmd;
                //                        strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from mskvy_applicantdetails_SPD where USER_NAME='" + strUserName + "'";
                strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from mskvy_applicantdetails_spd where USER_NAME='" + strUserName + "'";
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                Session["AppDet"] = dsResult;
                //log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResult.Tables[0];
                    GVApplications.DataBind();

                }
                else
                {
                    //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
                }


            }

            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Sql ExceptionMethod Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "FilData Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
            }


        }

        protected void lnkCons_Click(object sender, EventArgs e)
        {
            Session["NewApplication"] = "true";
            Response.Redirect("~/UI/mskvyspd/AppDetail.aspx", false);
        }

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                string strPAN = row.Cells[1].Text;
                Session["APPID"] = row.Cells[0].Text;
                //Session["PROMOTOR_NAME"] = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                Response.Redirect("~/UI/mskvyspd/AppDetail.aspx?appid=" + applicationId + "", false);


            }

            if (e.CommandName == "PayNow")
            {
                //log.Error("Before TRY " );

                try
                {

                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    //log.Error("Row Index "+rowIndex);

                    GridViewRow row = GVApplications.Rows[rowIndex];

                    string applicationId = row.Cells[0].Text;
                    //log.Error("applicationId" + applicationId);
                    string strPAN = row.Cells[1].Text;
                    //log.Error("strPAN" + strPAN);
                    string strName = ViewState["OrgName"].ToString();
                    //log.Error("Application ID : " + applicationId);
                    Session["AppType"] = row.Cells[7].Text.Trim();
                    //log.Error("App Home : " + Session["AppType"].ToString());
                    Response.Redirect("~/UI/mskvyspd/PayRegConfirm.aspx?appID=" + applicationId + "&PAN=" + strPAN + "&orgName=" + strName, false);

                    //   log.Error(strMerchantCode + " " + strCurrencyType + " " + strSecurityID + " " + checkSumKey + " " + strPGURL + " " + strRU);
                    //DataSet dsFullDet = new DataSet();
                    //dsFullDet = (DataSet)Session["fulldet"];
                    //string strRegNo = Session["Registrationno"].ToString();
                    //strName = dsFullDet.Tables[0].Rows[0]["Fname"].ToString() + " " + dsFullDet.Tables[0].Rows[0]["Mname"].ToString() + " " + dsFullDet.Tables[0].Rows[0]["Lname"].ToString();
                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    log.Error(ErrorMessage);

                }
            }

            if (e.CommandName == "DownloadGC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                string applicationId = row.Cells[0].Text;
                string folderPath = Server.MapPath("~/Files/MSKVY/" + applicationId + "/");
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"SELECT * FROM mskvy_upload_doc_spd a WHERE a.APPLICATION_NO = '{applicationId}' AND a.FileType=7 ORDER BY a.SRNO DESC LIMIT 1;");
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=MSKVY_GC_" + applicationId + ".pdf");
                    Response.TransmitFile(folderPath + dsResult.Tables[0].Rows[0]["FieName"].ToString());
                    Response.End();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "FileNotFoundAlert", "alert('The requested file is not available.');", true);
                }
            }
            if (e.CommandName == "DownloadFGC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                string applicationId = row.Cells[0].Text;

                string folderPath = Server.MapPath("~/Files/MSKVY/FGC/" + applicationId + "/");
             /*   FUDoc.SaveAs(folderPath + Path.GetFileName(FUDoc.FileName));
                newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUDoc.FileName;
                System.IO.File.Move(folderPath + FUDoc.FileName, folderPath + newFileName);
                string strQuery = string.Empty;

                strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + strAppID + "', 15, '" + newFileName + "',  '" + strAppID + "')";
*/
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"SELECT * FROM fgc_app_doc a WHERE a.APPLICATION_NO = '{applicationId}' AND a.Doc_Type=15 ORDER BY a.SRNO DESC LIMIT 1;");
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=MSKVY_FGC_" + applicationId + ".pdf");
                    Response.TransmitFile(folderPath + dsResult.Tables[0].Rows[0]["File_Name"].ToString());
                    Response.End();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "FileNotFoundAlert", "alert('The requested file is not available.');", true);
                }
            }
          
            if (e.CommandName == "CommittmentFees")
            {
                log.Error("Before TRY ");

                try
                {

                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    //log.Error("Row Index " + rowIndex);

                    GridViewRow row = GVApplications.Rows[rowIndex];

                    string applicationId = row.Cells[0].Text;
                    //log.Error("applicationId" + applicationId);
                    string strPAN = row.Cells[1].Text;
                    //log.Error("strPAN" + strPAN);
                    string strName = ViewState["OrgName"].ToString();
                    //log.Error("Application ID : " + applicationId);
                    Session["AppType"] = row.Cells[2].Text;
                    //log.Error("App Home : " + Session["AppType"].ToString());
                    Response.Redirect("~/UI/mskvySPD/PayCommConfirm.aspx?appid=" + applicationId + "&PAN=" + strPAN + "&orgName=" + strName, false);

                    //   log.Error(strMerchantCode + " " + strCurrencyType + " " + strSecurityID + " " + checkSumKey + " " + strPGURL + " " + strRU);
                    //DataSet dsFullDet = new DataSet();
                    //dsFullDet = (DataSet)Session["fulldet"];
                    //string strRegNo = Session["Registrationno"].ToString();
                    //strName = dsFullDet.Tables[0].Rows[0]["Fname"].ToString() + " " + dsFullDet.Tables[0].Rows[0]["Mname"].ToString() + " " + dsFullDet.Tables[0].Rows[0]["Lname"].ToString();
                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    log.Error(ErrorMessage);

                }
            }

            if (e.CommandName == "Download")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];
                string strUserName = Session["user_name"].ToString();
                string applicationId = row.Cells[0].Text;



                var doc = new GenerateApplicantionSPDDoc(Server, MethodBase.GetCurrentMethod(), log, applicationId, strUserName);
                var result = doc.GenerateDoc2(false);

                if (result.Success)
                {
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "PDF";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + result.FileName);
                    Response.BinaryWrite(result.Data);
                    Response.Flush();
                    Response.End();
                }

                //string strName = row.Cells[4].Text;
                //string strPostName = string.Empty;

                //string strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from mskvy_applicantdetails_spd where APPLICATION_NO='" + applicationId + "'";
                //DataSet dsResult = new DataSet();
                //dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                //DataSet dsRegDet = new DataSet();
                //dsRegDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicant_reg_det where USER_NAME='" + strUserName + "'");
                //dsRegDet.Tables[0].TableName = "AppRegDet";
                //dsResult.Tables.Add(dsRegDet.Tables[0].Copy());

                //strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and AuthStatus='0300' and typeofpay='MSKVYSPDRegistration'";

                //DataSet dsReceipt = new DataSet();
                //dsReceipt = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                //dsReceipt.Tables[0].TableName = "TableReceipt";
                //dsResult.Tables.Add(dsReceipt.Tables[0].Copy());


                //if (dsResult.Tables[0].Rows.Count > 0)
                //{
                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                //string FileName = "ApplicationForm_" + strUserName + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                //string serverFilePath = Server.MapPath("~/Reports/") + FileName;
                //try
                //{

                //LocalReport report = new LocalReport();
                //report.ReportPath = Server.MapPath("~/PDFReport/") + "Application.rdlc";

                //ReportDataSource rds = new ReportDataSource();
                //rds.Name = "dsApplication_AppDet";//This refers to the dataset name in the RDLC file
                //rds.Value = dsResult.Tables[0];
                //report.DataSources.Add(rds);

                //ReportDataSource rds2 = new ReportDataSource();
                //rds2.Name = "dsApplication_AppRegDet";//This refers to the dataset name in the RDLC file
                //rds2.Value = dsResult.Tables["AppRegDet"];
                //report.DataSources.Add(rds2);

                //ReportDataSource rds3 = new ReportDataSource();
                //rds3.Name = "DataSetReceipt";//This refers to the dataset name in the RDLC file
                //rds3.Value = dsResult.Tables["TableReceipt"];
                //report.DataSources.Add(rds3);

                //ReportDataSource rds3 = new ReportDataSource();
                //rds3.Name = "DataSet3";//This refers to the dataset name in the RDLC file
                //rds3.Value = dsResult.Tables["Table3"];
                //report.DataSources.Add(rds3);

                //ReportDataSource rds4 = new ReportDataSource();
                //rds4.Name = "DataSet4";//This refers to the dataset name in the RDLC file
                //rds4.Value = dsResult.Tables["Table4"];
                //report.DataSources.Add(rds4);

                //ReportViewer reportViewer = new ReportViewer();
                //reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "Application.rdlc";
                //reportViewer.LocalReport.DataSources.Clear();

                //reportViewer.LocalReport.DataSources.Add(rds);
                //reportViewer.LocalReport.DataSources.Add(rds2);
                //reportViewer.LocalReport.DataSources.Add(rds3);

                // reportViewer.LocalReport.DataSources.Add(rds3);


                //reportViewer.LocalReport.Refresh();


                //string mimeType, encoding, extension, deviceInfo;
                //string[] streamids;
                //Warning[] warnings;
                //string format = "PDF";

                //deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                //byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                //FileStream fs1 = new FileStream(serverFilePath, FileMode.Create);
                //fs1.Write(bytes, 0, bytes.Length);
                //fs1.Close();
                //Response.Clear();
                //Response.Buffer = true;
                //Response.Charset = "";
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.ContentType = "PDF";
                //Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                //Response.BinaryWrite(bytes);
                //Response.Flush();
                //Response.End();

                //}
                //catch (Exception ex)
                //{
                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                //    log.Error(ErrorMessage);

                //}
                //}
                //else
                //{
                //lblResult.Text = "Error!!";

                //}

            }

            if (e.CommandName == "ReceiptDownload")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];


                string applicationId = row.Cells[0].Text;
                //string GSTIN = row.Cells[1].Text;
                // string strName = row.Cells[4].Text;


                string strPostName = string.Empty;
                string strUserName = Session["user_name"].ToString();
                string strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and typeofpay='MSKVYSPDRegistration' and AuthStatus='0300' ";

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
                else
                {
                    //lblResult.Text = "Error!!";

                }

            }

            if (e.CommandName == "UploadForm")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                Session["ProjectID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/mskvyspd/UploadForm.aspx?appid=" + applicationId, false);


            }

            if (e.CommandName == "CommittmentReceiptDownload")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = GVApplications.Rows[rowIndex];


                string applicationId = row.Cells[0].Text;
                string strPostName = string.Empty;
                string strUserName = Session["user_name"].ToString();

                string strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and typeofpay='Committment' and AuthStatus='0300' ";
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

                        deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>12.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

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
                else
                {
                    //lblResult.Text = "Error!!";

                }
            }

            if (e.CommandName == "ViewDoc")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                Response.Redirect("~/UI/MSKVYSPD/ViewDoc.aspx?appid=" + applicationId, false);


            }
        }

        protected void GVApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["AppDet"];
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int rowno = e.Row.RowIndex;
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    ImageButton btnEdit = (ImageButton)e.Row.Cells[7].FindControl("btnSelect");
                    ImageButton btnPay = (ImageButton)e.Row.Cells[16].FindControl("btnPayNow");
                    ImageButton btnDownload = (ImageButton)e.Row.Cells[18].FindControl("btnDownload");
                    ImageButton btnRecDownload = (ImageButton)e.Row.Cells[17].FindControl("btnRecDownload");
                    ImageButton btnUploadForm = (ImageButton)e.Row.Cells[19].FindControl("btnUploadForm");
                    ImageButton btnCommFees = (ImageButton)e.Row.Cells[20].FindControl("btnCommFees");
                    ImageButton btnCommittmentDownload = (ImageButton)e.Row.Cells[21].FindControl("btnCommittmentDownload");
                    ImageButton btnDownloadGC = (ImageButton)e.Row.FindControl("btnDownloadGC");
                    ImageButton btnDownloadFGC = (ImageButton)e.Row.FindControl("btnDownloadFGC");

                    btnDownloadGC.Enabled = false;
                    btnDownloadFGC.Enabled = false;
                    ImageButton btnViewDoc = (ImageButton)e.Row.Cells[22].FindControl("btnViewDoc");

                    // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                    string app = e.Row.Cells[0].Text;
                    int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());
                    string isAppApproved = ds.Tables[0].Rows[rowno]["isAppApproved"].ToString();
                    // string IsIssueLetter = ds.Tables[0].Rows[rowno]["IssueLetter"].ToString();
                    log.Error(wfStatus.ToString());

                    #region Process flow change
                    if (wfStatus >= 1 && wfStatus <= 4)
                    {

                        btnEdit.Enabled = true;
                        btnPay.Enabled = true;
                        btnDownload.Enabled = false;
                        btnRecDownload.Enabled = false;
                        btnUploadForm.Enabled = false;
                        btnCommFees.Enabled = false;
                        btnCommittmentDownload.Enabled = false;
                        //btnViewDoc.Enabled = false;
                    }
                    if (wfStatus == 5)
                    {

                        btnEdit.Enabled = false;
                        btnPay.Enabled = false;
                        btnDownload.Enabled = true;
                        btnRecDownload.Enabled = true;
                        btnUploadForm.Enabled = true;
                        btnCommFees.Enabled = false;
                        btnCommittmentDownload.Enabled = false;
                        btnViewDoc.Enabled = true;
                    }
                    //if (wfStatus == 15)
                    if (wfStatus >= 5 && wfStatus <= 15)
                    {
                        btnEdit.Enabled = false;
                        btnPay.Enabled = false;
                        btnDownload.Enabled = true;
                        btnRecDownload.Enabled = true;
                        btnUploadForm.Enabled = false;
                        btnCommFees.Enabled = true;
                        btnCommittmentDownload.Enabled = false;
                        //btnViewDoc.Enabled = true;
                    }
                    if (wfStatus >= 16)
                    {
                        btnEdit.Enabled = false;
                        btnPay.Enabled = false;
                        btnDownload.Enabled = true;
                        btnRecDownload.Enabled = true;
                        btnUploadForm.Enabled = false;
                        btnCommFees.Enabled = false;
                        btnCommittmentDownload.Enabled = false;
                        btnViewDoc.Enabled = true;
                        btnDownloadGC.Enabled = true;
                    }
                    if (wfStatus == 18)
                    {
                        btnDownloadFGC.Enabled = true;
                    }
                    if (isAppApproved == "N")
                    {
                        btnEdit.Enabled = true;
                    }

                    #endregion

                    //if (wfStatus >= 1 && wfStatus <= 3)
                    //{

                    //    btnEdit.Enabled = true;
                    //    btnPay.Enabled = true;
                    //    btnDownload.Enabled = false;
                    //    btnRecDownload.Enabled = false;
                    //    btnUploadForm.Enabled = false;
                    //    btnCommFees.Enabled = false;
                    //    btnViewDoc.Enabled = true;
                    //}
                    //if (wfStatus == 4)
                    //{
                    //    btnEdit.Enabled = false;
                    //    btnPay.Enabled = false;
                    //    btnDownload.Enabled = true;
                    //    btnRecDownload.Enabled = true;
                    //    btnUploadForm.Enabled = false;
                    //    btnCommFees.Enabled = false;
                    //    btnViewDoc.Enabled = true;
                    //}
                    //if (wfStatus == 5)
                    //{
                    //    btnEdit.Enabled = false;
                    //    btnPay.Enabled = false;
                    //    btnDownload.Enabled = true;
                    //    btnRecDownload.Enabled = true;
                    //    btnUploadForm.Enabled = true;
                    //    btnCommFees.Enabled = false;
                    //    btnViewDoc.Enabled = true;
                    //}
                    //if (wfStatus >= 6 && wfStatus <= 14)
                    //{
                    //    btnEdit.Enabled = false;
                    //    btnPay.Enabled = false;
                    //    btnDownload.Enabled = true;
                    //    btnRecDownload.Enabled = true;
                    //    btnUploadForm.Enabled = false;
                    //    btnCommFees.Enabled = false;
                    //    btnViewDoc.Enabled = true;
                    //}
                    //if (wfStatus == 15)
                    //{
                    //    btnEdit.Enabled = false;
                    //    btnPay.Enabled = false;
                    //    btnDownload.Enabled = true;
                    //    btnRecDownload.Enabled = true;
                    //    btnUploadForm.Enabled = false;
                    //    btnCommFees.Enabled = true;
                    //    btnViewDoc.Enabled = true;
                    //}
                    //if (wfStatus >= 16)
                    //{
                    //    btnEdit.Enabled = false;
                    //    btnPay.Enabled = false;
                    //    btnDownload.Enabled = true;
                    //    btnRecDownload.Enabled = true;
                    //    btnUploadForm.Enabled = false;
                    //    btnCommFees.Enabled = false;
                    //    btnViewDoc.Enabled = true;
                    //    btnCommittmentDownload.Enabled = true;
                    //}

                    //}

                    if (isAppApproved == "N")
                    {
                        btnEdit.Enabled = true;
                    }
                    else
                    {
                        btnEdit.Enabled = false;
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Row Bound Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                //lblResult.Text = "Some problem during registration.Please try again.";
            }
        }

    }
}