using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using System.Data;
using MySql.Data.MySqlClient;

using System.Configuration;
using System.Net;
using System.IO;
using log4net;
using System.Reflection;
using System.Globalization;


/// <summary>
/// Summary description for GGCLogin
/// </summary>
[WebService(Namespace = "http://gridconn.mahatransco.in/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class GGCLogin : System.Web.Services.WebService {
    protected static readonly ILog log = LogManager.GetLogger(typeof(GGCLogin));

    public GGCLogin () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public bool Validate(string uName,string Pass) {

        int count = isValid(uName,Pass);
        if (count > 0)
            return true;
        else
            return false;
    }
    public int isValid(string uName, string Pass)
    {
        MySqlConnection mySqlConnection = new MySqlConnection();
        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        int count = -1;

        try
        {

            mySqlConnection.Open();
            

            switch (mySqlConnection.State)
            {

                case System.Data.ConnectionState.Open:
                    string strQuery = string.Empty;
                    MySqlCommand cmd;
                    cmd = new MySqlCommand("select count(1) from applicant_login_master where USER_NAME='" + uName + "' and PASSWORD='" + Pass + "'", mySqlConnection);
                    log.Error("query :" + cmd.CommandText.ToString());

                    DataSet dsResult = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                    da.Fill(dsResult);
                    count = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                    log.Error("count :" + count);

                    //if (count > 0)
                    //{
                    //    return "Yes";
                    //}
                    //else
                    //{
                    //    return "No";
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
        return count;
    }
}
