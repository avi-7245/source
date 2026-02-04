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
//using SendGrid;
namespace GGC.UI.Emp
{
    public partial class StatusReport : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(StatusReport));
        protected void Page_Load(object sender, EventArgs e)
        {
            //var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
            
            //var client = new SendGridClient(apiKey);
            //var from = new EmailAddress("test@example.com", "Example User");
            //var subject = "Sending with SendGrid is Fun";
            //var to = new EmailAddress("test@example.com", "Example User");
            //var plainTextContent = "and easy to do anywhere, even with C#";
            //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //var response = await client.SendEmailAsync(msg);
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            
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
                            strQuery = "SELECT a.APPLICATION_NO,a.PROJECT_TYPE,'MSETCL', a.PROMOTOR_NAME,a.MEDAProjectID,'',PROJECT_CAPACITY_MW,A.PROJECT_DISTRICT,A.PROJECT_TALUKA,A.PROJECT_LOC,"
                                        + "date_format(a.CREATED_DT,'%Y-%m-%d') as CREATED_DT,a.CONT_PER_NAME_1,a.CONT_PER_PHONE_1,a.CONT_PER_MOBILE_1,a.DISTANCE_FROM_PLANT,b.typeofpay,b.TxnNo,b.TxnAmount, b.TxnDate,a.app_status,date_format(a.APP_STATUS_DT,'%Y-%m-%d') as APP_STATUS_DT, date_format(a.IssueLetterDt,'%Y-%m-%d') as IssueLetterDt,CONT_PER_EMAIL_1 as Email, CONT_PER_EMAIL_2 as Email  FROM applicantdetails a , billdesk_txn b WHERE   a.APPLICATION_NO=b.ApplicationNo and b.AuthStatus='0300' and WF_STATUS_CD_C < 19 order by a.CREATED_DT desc";
                        }
                        else
                        {
                            //strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from APPLICANTDETAILS where ZONE='" + Session["EmpZone"].ToString() + "' AND IsPaymentDone='Y'";
                            strQuery = "SELECT a.APPLICATION_NO,a.PROJECT_TYPE,'MSETCL', a.PROMOTOR_NAME,a.MEDAProjectID,'',PROJECT_CAPACITY_MW,A.PROJECT_DISTRICT,A.PROJECT_TALUKA,A.PROJECT_LOC,"
                                        + "date_format(a.CREATED_DT,'%Y-%m-%d') as CREATED_DT,a.CONT_PER_NAME_1,a.CONT_PER_PHONE_1,a.CONT_PER_MOBILE_1,a.DISTANCE_FROM_PLANT,b.typeofpay,b.TxnNo,b.TxnAmount, b.TxnDate,a.app_status,date_format(a.APP_STATUS_DT,'%Y-%m-%d') as APP_STATUS_DT, date_format(a.IssueLetterDt,'%Y-%m-%d') as IssueLetterDt,CONT_PER_EMAIL_1 as Email , CONT_PER_EMAIL_2 as Email FROM applicantdetails a , billdesk_txn b WHERE   a.APPLICATION_NO=b.ApplicationNo and b.AuthStatus='0300' and WF_STATUS_CD_C < 19 and ZONE='" + Session["EmpZone"].ToString() + "' order by a.CREATED_DT desc ";


                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        //log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["AppDet"];
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                string filename = "GCExport.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }

        }

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadActivity")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[1].Text;
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                string letterName = string.Empty;
                try
                {

                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd = new MySqlCommand("select *  from applicant_status_tracking where APPLICATION_NO='" + applicationId + "' group by application_no,status order by STATUS_DT", mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {

                                DataTable dt = dsResult.Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    string filename = "GCActivity_"+applicationId+".xls";
                                    System.IO.StringWriter tw = new System.IO.StringWriter();
                                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                                    DataGrid dgGrid = new DataGrid();
                                    dgGrid.DataSource = dt;
                                    dgGrid.DataBind();

                                    //Get the HTML for the control.
                                    dgGrid.RenderControl(hw);
                                    //Write the HTML back to the browser.
                                    //Response.ContentType = application/vnd.ms-excel;
                                    Response.ContentType = "application/vnd.ms-excel";
                                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                                    this.EnableViewState = false;
                                    Response.Write(tw.ToString());
                                    Response.End();
                                }

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

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors

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
        }

    }
}