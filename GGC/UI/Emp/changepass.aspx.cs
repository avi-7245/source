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
using GGC.Scheduler;

namespace GGC.UI.Emp
{
    public partial class changepass : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(changepass));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            int roleid = 0;
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where SAPID='" + Session["SAPID"].ToString() + "'");
            if (dsResult.Tables[0].Rows.Count > 0)
            {
                roleid = int.Parse(dsResult.Tables[0].Rows[0]["ROLE_ID"].ToString());
            }
            if (roleid > 50 && roleid <= 55)
            {
                prList.Visible = true;
            }
            if (roleid > 1 && roleid <= 5)
            {
                home.Visible = true;
            }
            if (roleid >=31 && roleid <= 40)
            {
                payList.Visible = true;
            }

        }

        protected void btnChangePass_Click(object sender, EventArgs e)
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
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        //string strQuery=string.Empty;
                        strQuery = "update empmaster set password='" + txtConfirmPass.Text + "', isFirstLogin='N' where SAPID='" + Session["SAPID"].ToString() + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();

                        
                        
                        lblMessage.Text = "Password changed successfully!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        
                        
                           
                        
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
                lblMessage.Text = "Error occurred during Password changed!";

                // Use the mySqlException object to handle specific MySql errors
                // lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblMessage.Text = "Error occurred during Password changed!";

                // Use the exception object to handle all other non-MySql specific errors
                //lblResult.Text = "Some problem during registration.Please try again.";
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