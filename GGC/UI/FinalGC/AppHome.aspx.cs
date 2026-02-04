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

namespace GGC.UI.FinalGC
{
    public partial class AppHome : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppHome));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Session["user_name"] = "";
                Session["strMEDAPrjID"] = Request.QueryString["projectID"];

                fillRegDet();
                //          log.Error("2");

                fillGrid();
                //            log.Error("3");

                Session["NewApplication"] = "false";
            }
            //    
        }

        protected void fillRegDet()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //string strGSTIN = string.Empty;
            //if (Session["GSTIN"] != null)
            //    strGSTIN = Session["GSTIN"].ToString();
            string strUserName = string.Empty;
            strUserName = Session["user_name"].ToString();
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd = new MySqlCommand("select * from APPLICANT_REG_DET where user_name='" + strUserName + "'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            txtOrganizationName.Text = "Welcome " + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                            Session["PROMOTOR_NAME"] = dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                            ViewState["OrgName"] = dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                        }
                        else
                        {
                            //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
                        }

                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:

                        // Connection is actively doing something else

                        break;

                }


                // Place Your Code Here to Process Data //

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

            finally
            {

                // Make sure to only close connections that are not in a closed state

                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {

                    // Close the connection as a good Garbage Collecting practice

                    mySqlConnection.Close();

                }

            }
        }
        protected void fillGrid()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //string strGSTIN = string.Empty;
            //if (Session["GSTIN"] != null)
            //    strGSTIN = Session["GSTIN"].ToString();
            string strUserName = Session["user_name"].ToString(); ;
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from mskvy_applicantdetails where USER_NAME='" + strUserName + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        //log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
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

                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:

                        // Connection is actively doing something else

                        break;

                }


                // Place Your Code Here to Process Data //

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

            finally
            {

                // Make sure to only close connections that are not in a closed state

                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {

                    // Close the connection as a good Garbage Collecting practice

                    mySqlConnection.Close();

                }

            }
        }

        protected void lnkCons_Click(object sender, EventArgs e)
        {
            Session["NewApplication"] = "true";
            Response.Redirect("~/UI/mskvy/AppDetail.aspx", false);
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

                Response.Redirect("~/UI/mskvy/AppDetail.aspx?appid=" + applicationId + "&PAN=" + strPAN, false);


            }
            if (e.CommandName == "Apply")
            {
                try
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

                    Response.Redirect("~/UI/mskvyspd/Home.aspx?usrName=" + Session["user_name"].ToString(), false);
                }
                catch (Exception exception)
                {
                    string ErrorMessage = "FilData Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);

                }

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
                    Response.Redirect("~/UI/mskvy/PayRegConfirm.aspx?appid=" + applicationId + "&PAN=" + strPAN + "&orgName=" + strName, false);

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
                    Response.Redirect("~/UI/mskvy/PayCommConfirm.aspx?appid=" + applicationId + "&PAN=" + strPAN + "&orgName=" + strName, false);

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


                string applicationId = row.Cells[0].Text;
                //string GSTIN = row.Cells[1].Text;
                string strName = row.Cells[4].Text;


                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                string strPostName = string.Empty;
                string strUserName = Session["user_name"].ToString();
                try
                {

                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F  from mskvy_applicantdetails where APPLICATION_NO='" + applicationId + "'";
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);

                            cmd = new MySqlCommand("select * from applicant_reg_det where USER_NAME='" + strUserName + "'", mySqlConnection);
                            DataSet dsRegDet = new DataSet();
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dsRegDet);
                            dsRegDet.Tables[0].TableName = "AppRegDet";
                            dsResult.Tables.Add(dsRegDet.Tables[0].Copy());

                            strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and AuthStatus='0300' and typeofpay='MSKVYRegistration'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsReceipt = new DataSet();
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dsReceipt);
                            dsReceipt.Tables[0].TableName = "TableReceipt";
                            dsResult.Tables.Add(dsReceipt.Tables[0].Copy());

                            //cmd = new MySqlCommand("select * from PostQualExperience where RegistrationId='" + strRegistrationNo + "' order by srno", mySqlConnection);
                            //DataSet dsExperience = new DataSet();
                            //da = new MySqlDataAdapter(cmd);
                            //da.Fill(dsExperience);
                            //dsExperience.Tables[0].TableName = "Table3";
                            //dsResult.Tables.Add(dsExperience.Tables[0].Copy());

                            //cmd = new MySqlCommand("select * from BillDesk_TXN where RegistrationId='" + strRegistrationNo + "' order by srno", mySqlConnection);
                            //DataSet dsReceipt = new DataSet();
                            //da = new MySqlDataAdapter(cmd);
                            //da.Fill(dsReceipt);
                            //dsReceipt.Tables[0].TableName = "Table4";
                            //dsResult.Tables.Add(dsReceipt.Tables[0].Copy());

                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                                string FileName = "ApplicationForm_" + strUserName + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                                string serverFilePath = Server.MapPath("~/Files/MSKVY/" + applicationId + "/") + FileName;
                                if (!Directory.Exists(Server.MapPath("~/Files/MSKVY/" + applicationId + "/")))
                                {
                                    //If Directory (Folder) does not exists Create it.
                                    Directory.CreateDirectory(Server.MapPath("~/Files/MSKVY/" + applicationId + "/"));
                                }
                                try
                                {

                                    LocalReport report = new LocalReport();
                                    report.ReportPath = Server.MapPath("~/PDFReport/") + "Application_MSKVY.rdlc";

                                    ReportDataSource rds = new ReportDataSource();
                                    rds.Name = "dsApplication_AppDet";//This refers to the dataset name in the RDLC file
                                    rds.Value = dsResult.Tables[0];
                                    report.DataSources.Add(rds);

                                    ReportDataSource rds2 = new ReportDataSource();
                                    rds2.Name = "dsApplication_AppRegDet";//This refers to the dataset name in the RDLC file
                                    rds2.Value = dsResult.Tables["AppRegDet"];
                                    report.DataSources.Add(rds2);

                                    ReportDataSource rds3 = new ReportDataSource();
                                    rds3.Name = "DataSetReceipt";//This refers to the dataset name in the RDLC file
                                    rds3.Value = dsResult.Tables["TableReceipt"];
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
                                    reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "Application_MSKVY.rdlc";
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
                                    log.Error(ErrorMessage);

                                }
                            }
                            else
                            {
                                //lblResult.Text = "Error!!";

                            }

                            break;

                        case System.Data.ConnectionState.Closed:

                            // Connection could not be made, throw an error

                            throw new Exception("The database connection state is Closed");

                            break;


                        default:

                            // Connection is actively doing something else

                            break;

                    }


                    // Place Your Code Here to Process Data //

                }

                catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                    log.Error(ErrorMessage);

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);

                }

                finally
                {

                    // Make sure to only close connections that are not in a closed state

                    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                    {

                        // Close the connection as a good Garbage Collecting practice

                        mySqlConnection.Close();

                    }

                }
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


                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                string strPostName = string.Empty;
                string strUserName = Session["user_name"].ToString();
                try
                {

                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and typeofpay='MSKVYRegistration' and AuthStatus='0300' ";
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);



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

                            break;

                        case System.Data.ConnectionState.Closed:

                            // Connection could not be made, throw an error

                            throw new Exception("The database connection state is Closed");

                            break;


                        default:

                            // Connection is actively doing something else

                            break;

                    }


                    // Place Your Code Here to Process Data //

                }

                catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                    log.Error(ErrorMessage);

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);

                }

                finally
                {

                    // Make sure to only close connections that are not in a closed state

                    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                    {

                        // Close the connection as a good Garbage Collecting practice

                        mySqlConnection.Close();

                    }

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
                Response.Redirect("~/UI/Cons/UploadForm.aspx?appid=" + applicationId, false);


            }
            if (e.CommandName == "CommittmentReceiptDownload")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];


                string applicationId = row.Cells[0].Text;
                //string GSTIN = row.Cells[1].Text;
                // string strName = row.Cells[4].Text;


                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                string strPostName = string.Empty;
                string strUserName = Session["user_name"].ToString();
                try
                {

                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and typeofpay='Committment' and AuthStatus='0300' ";
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);



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

                            break;

                        case System.Data.ConnectionState.Closed:

                            // Connection could not be made, throw an error

                            throw new Exception("The database connection state is Closed");

                            break;


                        default:

                            // Connection is actively doing something else

                            break;

                    }


                    // Place Your Code Here to Process Data //

                }

                catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                    log.Error(ErrorMessage);

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);

                }

                finally
                {

                    // Make sure to only close connections that are not in a closed state

                    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                    {

                        // Close the connection as a good Garbage Collecting practice

                        mySqlConnection.Close();

                    }

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
                Response.Redirect("~/UI/Cons/ViewDoc.aspx?appid=" + applicationId, false);


            }
            if (e.CommandName == "DownloadGC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                string applicationId = row.Cells[0].Text;
                string folderPath = Server.MapPath("~/Files/MSKVY/" + applicationId + "/");
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from  mskvy_upload_doc_spd where Application_no='" + applicationId + "' and FileType=3");

                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=MSKVY_DeemedGC_" + applicationId + ".pdf");
                    Response.TransmitFile(folderPath + dsResult.Tables[0].Rows[0]["FieName"].ToString());
                    Response.End();
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "FileNotFoundAlert", "alert('The requested file is not available.');", true);
                }
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
                    ImageButton btnEdit = (ImageButton)e.Row.Cells[15].FindControl("btnSelect");
                    ImageButton btnPay = (ImageButton)e.Row.Cells[16].FindControl("btnPayNow");
                    ImageButton btnRecDownload = (ImageButton)e.Row.Cells[17].FindControl("btnRecDownload");
                    ImageButton btnDownload = (ImageButton)e.Row.Cells[18].FindControl("btnDownload");
                    //ImageButton btnUploadForm = (ImageButton)e.Row.Cells[19].FindControl("btnUploadForm");
                    ImageButton btnCommFees = (ImageButton)e.Row.Cells[20].FindControl("btnCommFees");
                    ImageButton btnCommittmentDownload = (ImageButton)e.Row.Cells[21].FindControl("btnCommittmentDownload");
                    ImageButton btnApply = (ImageButton)e.Row.Cells[21].FindControl("btnApply");
                    ImageButton btnDownloadGC = (ImageButton)e.Row.FindControl("btnDownloadGC");
                    //ImageButton btnViewDoc = (ImageButton)e.Row.Cells[22].FindControl("btnViewDoc");

                    // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                    string app = e.Row.Cells[0].Text;
                    int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());
                    string isAppApproved = ds.Tables[0].Rows[rowno]["isAppApproved"].ToString();
                    string IsIssueLetter = ds.Tables[0].Rows[rowno]["IssueLetter"].ToString();
                    log.Error(wfStatus.ToString());

                    #region Process flow change
                    if (wfStatus >= 1 && wfStatus <= 4)
                    {

                        btnEdit.Enabled = true;
                        btnPay.Enabled = true;
                        btnDownload.Enabled = false;
                        btnRecDownload.Enabled = false;
                        //btnUploadForm.Enabled = false;
                        btnCommFees.Enabled = false;
                        btnCommittmentDownload.Enabled = false;
                        //btnViewDoc.Enabled = false;
                        btnDownloadGC.Enabled = false;
                    }
                    if (wfStatus == 5)
                    {

                        btnEdit.Enabled = false;
                        btnPay.Enabled = false;
                        btnDownload.Enabled = true;
                        btnRecDownload.Enabled = true;
                        //btnUploadForm.Enabled = true;
                        btnCommFees.Enabled = false;
                        btnCommittmentDownload.Enabled = false;
                        //btnViewDoc.Enabled = true;

                        btnDownloadGC.Enabled = false;
                    }
                    if (wfStatus >= 5 && wfStatus <= 15)
                    {
                        btnEdit.Enabled = false;
                        btnPay.Enabled = false;
                        btnDownload.Enabled = true;
                        btnRecDownload.Enabled = true;
                        //btnUploadForm.Enabled = false;
                        btnCommFees.Enabled = true;
                        btnCommittmentDownload.Enabled = false;
                        //btnViewDoc.Enabled = true;

                        btnDownloadGC.Enabled = false;
                    }
                    if (wfStatus >= 16)
                    {
                        btnEdit.Enabled = false;
                        btnPay.Enabled = false;
                        btnDownload.Enabled = true;
                        btnRecDownload.Enabled = true;
                        //btnUploadForm.Enabled = false;
                        btnCommFees.Enabled = false;
                        btnCommittmentDownload.Enabled = true;
                        btnApply.Enabled = true;
                        //btnViewDoc.Enabled = true;
                        btnDownloadGC.Enabled = true;
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

       
