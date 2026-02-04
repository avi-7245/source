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
    public partial class CreateLogin : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(CreateLogin));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillDDL();
            }
        }
        protected void fillDDL()
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
                        cmd = new MySqlCommand("select distinct rollname , rollid from rolemaster order by 2", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        ddlRole.DataSource = dsResult.Tables[0];
                        ddlRole.DataValueField = "rollid";
                        ddlRole.DataTextField = "rollname";
                        ddlRole.DataBind();
                        ddlRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlRole.SelectedIndex = 0;


                        cmd = new MySqlCommand("select distinct Zone from zone_district order by 1", mySqlConnection);
                        DataSet dsZone = new DataSet();
                        MySqlDataAdapter daZone  = new MySqlDataAdapter(cmd);
                        daZone.Fill(dsZone);
                        ddlZone.DataSource = dsZone.Tables[0];
                        ddlZone.DataValueField = "Zone";
                        ddlZone.DataTextField = "Zone";
                        ddlZone.DataBind();
                        ddlZone.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlZone.SelectedIndex = 0;

                        cmd = new MySqlCommand("select distinct Circle from zone_district order by 1", mySqlConnection);
                        DataSet dsCircle = new DataSet();
                        MySqlDataAdapter daCircle = new MySqlDataAdapter(cmd);
                        daCircle.Fill(dsCircle);
                        ddlCircle.DataSource = dsCircle.Tables[0];
                        ddlCircle.DataValueField = "Circle";
                        ddlCircle.DataTextField = "Circle";
                        ddlCircle.DataBind();
                        ddlCircle.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlCircle.SelectedIndex = 0;
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strSAPID, strPASSWORD, strEMP_NAME, strDESIGNATION, strDEPARTMENT_NAME, strZONE, strCIRCLE;
            int rollID;

            strSAPID = txtSAPID.Text;
            strPASSWORD = txtPass.Text;
            strEMP_NAME = txtEmpName.Text;
            strDESIGNATION = ddlDesignation.SelectedItem.Text;
            strDEPARTMENT_NAME = ddlDesignation.SelectedItem.Text;
            rollID = int.Parse(ddlRole.SelectedItem.Value);
            strZONE = ddlZone.SelectedItem.Text;
            strCIRCLE = ddlCircle.SelectedItem.Text;

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
                        strQuery = "insert into empmaster (SAPID , PASSWORD , EMP_NAME , DESIGNATION , DEPARTMENT_NAME , ROLE_ID , ZONE , CIRCLE, CREATED_DT , CREATED_BY) " +
                                " values('" + strSAPID + "','" + strPASSWORD + "','" + strEMP_NAME + "','" + strDESIGNATION + "','" + strDEPARTMENT_NAME + "'," + rollID + ",'" + strZONE + "','" + strCIRCLE + "',curdate(),'ADMIN')";
                            log.Error("Insert " + strQuery);

                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();


                            
                           // string scriptText = "alert('Registration successfully done. '); window.location='" + "ConsumerDetail.aspx';";

                            //      ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
                            //    lblResult.Text = "Registration No is : " + strRegistrationno;
                            //ClientScript.RegisterStartupScript(this.GetType(), "alert", scriptText , true);
                         //   Response.Write("<script language='javascript'>window.alert('" + scriptText + "');window.location='ConsumerDetail.aspx';</script>");

                            //RegisterStartupScript("save", "&lt;script type=\"text/javascript\"&gt; alert('Save successfully');  window.location.href = CreateLogin.aspx; &lt;/script&gt;");
                            Response.Write("<script language='javascript'>alert('Login created Successfully');</script>");

                            Server.Transfer ("~/UI/Emp/CreateLogin.aspx", false);
                //            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Login created Successfully')", true);
                  //          lblResult.Text = "Login created successfully.";
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
    }
}