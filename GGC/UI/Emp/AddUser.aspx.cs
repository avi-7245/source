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
using GGC.Common;
namespace GGC.UI.Emp
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AddUser));

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int role = int.Parse(Session["EmpRole"].ToString());
            if (role > 10 && role <= 20)
            {
                
            }
            if (role > 50 && role <= 60)
            {
                
            }
            if (!Page.IsPostBack)
            {

                fillDesignation();
                fillZone();
            }
        }
        protected void fillDesignation()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select * from rolemaster  order by 2", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        ddlDesgntn.DataSource = dsResult.Tables[0];
                        ddlDesgntn.DataValueField = "ROLLID";
                        ddlDesgntn.DataTextField = "ROLLNAME";
                        ddlDesgntn.DataBind();
                        ddlDesgntn.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlDesgntn.ClearSelection();
                        ddlDesgntn.SelectedIndex = 0;

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
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
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
        protected void fillZone()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("SELECT distinct zone FROM zone_district order by 1", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        ddlZone.DataSource = dsResult.Tables[0];
                        ddlZone.DataValueField = "zone";
                        ddlZone.DataTextField = "zone";
                        ddlZone.DataBind();
                        ddlZone.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlZone.Items.Insert(1, new System.Web.UI.WebControls.ListItem("C.O.", "C.O."));
                        ddlZone.ClearSelection();
                        ddlZone.SelectedIndex = 0;

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
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
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

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;

                        //strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from APPLICANTDETAILS where ZONE='" + Session["EmpZone"].ToString() + "' AND IsPaymentDone='Y'";
                        strQuery = "INSERT INTO  empmaster  ( SAPID ,  PASSWORD ,  EMP_NAME ,  DESIGNATION ,  DEPARTMENT_NAME ,  ROLE_ID ,  ReportingOfficerSAPID ,  ZONE , CREATED_DT ,  CREATED_BY ,  isFirstLogin ,  isActive ,  EmpEmailID ,  EmpMobile ) VALUES ( '" + txtSAPID.Text + "','Test@123','" + txtEmpName.Text + "','" + ddlDesgntn.SelectedItem.Text + "','" + ddlDepartment.SelectedItem.Value + "','" + ddlDesgntn.SelectedItem.Value + "','" + txtReportingSAPID.Text + "','" + ddlZone.SelectedItem.Value + "',now(),'" + Session["SAPID"].ToString()+ "','Y','Y','"+txtEmail.Text+"','"+txtMob.Text+"')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();



                        //HiddenField hdnEmpEmailID = (HiddenField)GVUsers.Rows[rowIndex].Cells[0].FindControl("hdnEmpEmailID");
                        string strBody = string.Empty;

                        strBody += "Respected Sir/Madam" + ",<br/>";
                        strBody += "Your Login ID is '"+txtSAPID.Text+"' and password for Grid connectivity application is Test@123." + "<br/>";
                        strBody += "Kindly change the password after login." + "<br/><br/><br/><br/><br/>";
                        strBody += "Thanks & Regards, " + "<br/>";
                        strBody += "(State Transmission Utility)" + "<br/>";
                        strBody += "MSETCL  " + "<br/>";
                        strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                        SendEmail objSM = new SendEmail();
                        objSM.Send(txtEmail.Text, "", "Grid connectivity application User created.", strBody);
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
    }
}