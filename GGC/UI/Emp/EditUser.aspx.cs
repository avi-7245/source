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
using GGC.Scheduler;
namespace GGC.UI.Emp
{
    public partial class EditUser : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(EditUser));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            string UID = Request.QueryString["userId"].ToString();
            if (!Page.IsPostBack)
            {

                fillDesignation();
                fillZone();
                fillEmpData(UID);
            }
        }
        protected void fillEmpData(string uID)
        {
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "Select * from empmaster where sapid='" + uID + "'");
                txtSAPID.Text = dsResult.Tables[0].Rows[0]["sapid"].ToString();
                txtEmpName.Text = dsResult.Tables[0].Rows[0]["EMP_NAME"].ToString();
                ddlDesgntn.ClearSelection();
                ddlDesgntn.Items.FindByText(dsResult.Tables[0].Rows[0]["DESIGNATION"].ToString()).Selected = true;
                ddlDepartment.ClearSelection();
                ddlDepartment.Items.FindByText(dsResult.Tables[0].Rows[0]["DEPARTMENT_NAME"].ToString()).Selected = true;
                ddlZone.ClearSelection();
                ddlZone.Items.FindByText(dsResult.Tables[0].Rows[0]["Zone"].ToString()).Selected = true;
                txtReportingSAPID.Text = dsResult.Tables[0].Rows[0]["ReportingOfficerSAPID"].ToString();
                txtEmail.Text = dsResult.Tables[0].Rows[0]["EmpEmailID"].ToString();
                txtMob.Text = dsResult.Tables[0].Rows[0]["EmpMobile"].ToString();
            }
            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                lblResult.Text = "Some problem.Please try again.";
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
                lblResult.Text = "Some problem.Please try again.";
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

        protected void btnGetDet_Click(object sender, EventArgs e)
        {
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT a* from empmaster where sapid='" + Session["userId"].ToString() + "'");
            if (dsResult.Tables.Count > 0)
            {
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    txtEmpName.Text = dsResult.Tables[0].Rows[0][""].ToString();
                }
            }
        }
    }
}