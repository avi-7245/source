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

namespace GGC.UI.Emp
{
    public partial class MSKVYHome : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSKVYHome));
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int role = int.Parse(Session["EmpRole"].ToString());

            if (!Page.IsPostBack)
            {

                fillGrid();

            }
        }
        protected void fillGrid()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        if (rollid <= 10)
                        {
                            strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from MSKVY_applicantdetails  a where IsPaymentDone='Y' and WF_STATUS_CD_C between 6 and 13 order by a.CREATED_DT desc";
                        }
                        else
                        {
                            //strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from MSKVY_applicantdetails  where ZONE='" + Session["EmpZone"].ToString() + "' AND IsPaymentDone='Y'";
                            strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from MSKVY_applicantdetails  a where  IsPaymentDone='Y' and ZONE='" + Session["EmpZone"].ToString() + "' and WF_STATUS_CD_C between 6 and 13 order by a.CREATED_DT desc";
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;

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

        protected void GVApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["AppDet"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        int rowno = e.Row.RowIndex;
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{
                        Button btnDownloadApp = (Button)e.Row.Cells[6].FindControl("btnDownloadApp");
                        Button btnUpload = (Button)e.Row.Cells[7].FindControl("btnUpload");
                        Button btnFeasibility = (Button)e.Row.Cells[8].FindControl("btnFeasibility");
                        Button btnFeasibilityConsent = (Button)e.Row.Cells[9].FindControl("btnFeasibilityConsent");
                        Button btnReturn = (Button)e.Row.FindControl("btnReturn");

                        //Button btnUpload = (Button)e.Row.Cells[13].FindControl("btnUpload");
                        //Button btnViewTech = (Button)e.Row.Cells[14].FindControl("btnViewTech");

                        // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                        string app = e.Row.Cells[0].Text;
                        int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());
                        string IsIssueLetter = ds.Tables[0].Rows[rowno]["IssueLetter"].ToString();
                        string appno = ds.Tables[0].Rows[rowno]["APPLICATION_NO"].ToString();
                        string isAppApproved = ds.Tables[0].Rows[rowno]["isAppApproved"].ToString();

                        log.Error("AppNo " + appno + " status " + wfStatus.ToString());

                        string hex = "#008CBA";
                        System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

                        if (wfStatus == 6)
                        {

                            btnDownloadApp.Enabled = true;
                            btnDownloadApp.BackColor = _color;

                            //btnUpload.Enabled = false;
                            //btnUpload.BackColor = System.Drawing.Color.Red;

                            btnUpload.Enabled = true;
                            btnUpload.BackColor = _color;

                        }
                        if (wfStatus == 10)
                        {
                            btnDownloadApp.Enabled = true;
                            btnDownloadApp.BackColor = _color;



                            btnUpload.Enabled = true;
                            btnUpload.BackColor = _color;

                        }
                        if (wfStatus == 11)
                        {
                            btnDownloadApp.Enabled = true;
                            btnDownloadApp.BackColor = _color;
                            
                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;
                            
                            btnFeasibility.Enabled = true;
                            btnFeasibility.BackColor = _color;
                            
                            btnFeasibilityConsent.Enabled = false;
                            btnFeasibilityConsent.BackColor = System.Drawing.Color.Red; ;

                            btnReturn.Enabled = false;
                            btnReturn.BackColor = System.Drawing.Color.Red;
                        }

                        if (wfStatus == 12)
                        {
                            btnDownloadApp.Enabled = true;
                            btnDownloadApp.BackColor = _color;

                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;

                            btnFeasibility.Enabled = false;
                            btnFeasibility.BackColor = System.Drawing.Color.Red;
                            
                            btnFeasibilityConsent.Enabled = true;
                            btnFeasibilityConsent.BackColor = _color;

                            btnReturn.Enabled = false;
                            btnReturn.BackColor = System.Drawing.Color.Red;
                        }
                        if (wfStatus == 13)
                        {
                            btnDownloadApp.Enabled = true;
                            btnDownloadApp.BackColor = _color;

                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;

                            btnFeasibility.Enabled = false;
                            btnFeasibility.BackColor = System.Drawing.Color.Red;
                            btnFeasibilityConsent.Enabled = false;
                            btnFeasibilityConsent.BackColor = System.Drawing.Color.Red;
                        }

                        if (isAppApproved == "N")
                        {
                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;

                            btnReturn.Enabled = false;
                            btnReturn.BackColor = System.Drawing.Color.Red;
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

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadApp")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];


                string applicationId = row.Cells[0].Text;
                //string GSTIN = row.Cells[1].Text;
                string strName = row.Cells[4].Text;


                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                string strPostName = string.Empty;
                string strUserName = string.Empty;
                try
                {

                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F  from MSKVY_applicantdetails  where APPLICATION_NO='" + applicationId + "'";
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);

                            strUserName = dsResult.Tables[0].Rows[0]["USER_NAME"].ToString();
                            cmd = new MySqlCommand("select * from applicant_reg_det where USER_NAME='" + strUserName + "'", mySqlConnection);
                            DataSet dsRegDet = new DataSet();
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dsRegDet);
                            dsRegDet.Tables[0].TableName = "AppRegDet";
                            dsResult.Tables.Add(dsRegDet.Tables[0].Copy());

                            strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and AuthStatus='0300'";
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
                                string serverFilePath = Server.MapPath("~/Reports/") + FileName;
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

            if (e.CommandName == "UploadLFS")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/UploadMSKVYLFS.aspx?application=" + applicationId, false);

            }

            if (e.CommandName == "Feasibility")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/MSKVYFeasibility.aspx?application=" + applicationId, false);
            }

            if (e.CommandName == "Return")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/AppReturn.aspx?application=" + applicationId + "&ProjectType=SPV1", false);
            }

            if (e.CommandName == "FeasibilityConsent")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/MSKVYFeasibilityConsent.aspx?application=" + applicationId, false);
            }
        }

        protected void GVApplications_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int roll_Id;
            roll_Id = int.Parse(Session["EmpRole"].ToString());


            //if (roll_Id <= 10)
            //{
            //    ((DataControlField)GVApplications.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Document verification")
            //    .SingleOrDefault()).Visible = true;


            //    ((DataControlField)GVApplications.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Application verification")
            //    .SingleOrDefault()).Visible = true;

            //    ((DataControlField)GVApplications.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "View Application")
            //    .SingleOrDefault()).Visible = false;

            //    // ((DataControlField)GVApplications.Columns
            //    //.Cast<DataControlField>()
            //    //.Where(fld => fld.HeaderText == "Approve MEDA Report")
            //    //.SingleOrDefault()).Visible = true;

            //    ((DataControlField)GVApplications.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "Technical Feasibility")
            //    .SingleOrDefault()).Visible = false;

            //    ((DataControlField)GVApplications.Columns
            //   .Cast<DataControlField>()
            //   .Where(fld => fld.HeaderText == "View Feasibility")
            //   .SingleOrDefault()).Visible = true;
            //}
            //else
            //{
            //    {
            //        ((DataControlField)GVApplications.Columns
            //        .Cast<DataControlField>()
            //        .Where(fld => fld.HeaderText == "Document verification")
            //        .SingleOrDefault()).Visible = false;

            //        ((DataControlField)GVApplications.Columns
            //        .Cast<DataControlField>()
            //        .Where(fld => fld.HeaderText == "Application verification")
            //        .SingleOrDefault()).Visible = false;

            //        ((DataControlField)GVApplications.Columns
            //    .Cast<DataControlField>()
            //    .Where(fld => fld.HeaderText == "View Application")
            //    .SingleOrDefault()).Visible = true;

            //        // ((DataControlField)GVApplications.Columns
            //        //.Cast<DataControlField>()
            //        // .Where(fld => fld.HeaderText == "Approved by commitee")
            //        //.SingleOrDefault()).Visible = false;

            //        ((DataControlField)GVApplications.Columns
            //        .Cast<DataControlField>()
            //        .Where(fld => fld.HeaderText == "Technical Feasibility")
            //        .SingleOrDefault()).Visible = true;

            //        ((DataControlField)GVApplications.Columns
            //       .Cast<DataControlField>()
            //       .Where(fld => fld.HeaderText == "View Feasibility")
            //       .SingleOrDefault()).Visible = false;
            //    }

            //}
        }
    }
}