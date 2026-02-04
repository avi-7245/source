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
    public partial class GCApplied : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(GCApplied));
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int roleId = int.Parse(Session["EmpRole"].ToString());
            if (roleId > 50 && roleId<56)
            {
                mgtHome.Visible = true;
                empHome.Visible = false;
            }
            if (roleId >= 1 && roleId < 20)
            {
                empHome.Visible = true;
                mgtHome.Visible = false;
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
            int roleid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        if (roleid <= 10)
                        {
                            strQuery = "select * from APPLICANTDETAILS a where  WF_STATUS_CD_C >=4 order by CREATED_DT desc";
                        }
                        if (roleid > 50 && roleid < 56)
                        {
                            strQuery = "select * from APPLICANTDETAILS a where  WF_STATUS_CD_C >=4 order by CREATED_DT desc";
                        }
                        if (roleid > 10 && roleid < 20)
                        {
                            strQuery = "select * from APPLICANTDETAILS a where  WF_STATUS_CD_C >=4  and zone='" + Session["EmpZone"] .ToString()+ "' order by CREATED_DT desc";
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
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
                
                Session["APPID"] = row.Cells[0].Text;
                Response.Redirect("~/UI/emp/ViewFullDocs.aspx?application=" + applicationId, false);


            }
        }
    }
}