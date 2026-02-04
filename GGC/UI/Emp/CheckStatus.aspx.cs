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

namespace GGC.UI.Emp
{
    public partial class CheckStatus : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(CheckStatus));
        protected void Page_Load(object sender, EventArgs e)
        {
            
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int role = int.Parse(Session["EmpRole"].ToString());
            if (role > 0 && role <= 20)
            {
                ProposalList.Visible = false;
                Home.Visible = true;
            }
            if (role > 50 && role <= 60)
            {
                Home.Visible = false;
                ProposalList.Visible = true;
            }
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
                            strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days ,b.ORGANIZATION_NAME  from APPLICANTDETAILS a,applicant_reg_det b where a.USER_NAME=b.USER_NAME and IsPaymentDone='Y' and WF_STATUS_CD_C < 19 order by a.CREATED_DT desc";
                        }
                        else
                        {
                            if (rollid > 10 && rollid<=20)
                            {
                                //strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from APPLICANTDETAILS where ZONE='" + Session["EmpZone"].ToString() + "' AND IsPaymentDone='Y'";
                                strQuery = "select * ,b.ORGANIZATION_NAME  from APPLICANTDETAILS a,applicant_reg_det b where a.USER_NAME=b.USER_NAME and IsPaymentDone='Y' and ZONE='" + Session["EmpZone"].ToString() + "' AND isMEDAApp='Y' and WF_STATUS_CD_C < 19 order by a.CREATED_DT desc";
                            }
                            if (rollid > 50 && rollid <= 60)
                            {
                                //strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from APPLICANTDETAILS where ZONE='" + Session["EmpZone"].ToString() + "' AND IsPaymentDone='Y'";
                                strQuery = "select * ,b.ORGANIZATION_NAME  from APPLICANTDETAILS a,applicant_reg_det b where a.USER_NAME=b.USER_NAME and IsPaymentDone='Y'  and WF_STATUS_CD_C < 19 order by a.CREATED_DT desc";
                            }
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        Session["AppDet"] = dsResult;
                        //if (dsResult.Tables[0].Rows.Count > 0)
                        //{
                            GVApplications.DataSource = dsResult.Tables[0];
                            GVApplications.DataBind();

                        //}
                        //else
                        //{
                        //    //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
                        //}

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

        protected void GVApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GVApplications.PageIndex = e.NewPageIndex;
            fillGrid();
        }
    }
}