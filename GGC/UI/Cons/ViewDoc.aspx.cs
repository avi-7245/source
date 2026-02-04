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

namespace GGC.UI.Cons
{
    public partial class ViewDoc : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ViewDoc));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    lblAppNo.Text = ID;
                    Session["APPID"] = ID;
                    getDocument(ID, 2);
                }
            }
        }
        protected string getDocument(string applicationId, int doctype)
        {
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strFolderName = string.Empty;

            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        switch (doctype)
                        {
                            case 1:
                                strQuery = "select AppFormDocName as docname from applicantdetails where APPLICATION_NO='" + applicationId + "'";
                                strFolderName = "AppForm";
                                break;
                            case 2:
                                strQuery = "select SLDDocName as docname from applicantdetails where APPLICATION_NO='" + applicationId + "'";
                                strFolderName = "SLD";
                                break;
                            case 3:
                                strQuery = "select docTech as docname from applicantdetails where APPLICATION_NO='" + applicationId + "'";
                                strFolderName = "Tech";
                                break;
                            case 4:
                                strQuery = "select IssueLetter as docname from applicantdetails where APPLICATION_NO='" + applicationId + "'";
                                strFolderName = "IssueLetter";
                                break;
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString() != "")
                            {
                                strFileName = dsResult.Tables[0].Rows[0]["docname"].ToString();
                                string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                                embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                                embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                                embed += "</object>";
                                ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/" + strFolderName + "/" + strFileName));
                            }
                            else
                            {
                                Response.Write("<script language='javascript'>alert('No document found.');</script>");
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
            return strFileName;
        }

        protected void btnSLD_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 2);
        }

        protected void btnTech_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 3);
        }

        protected void btnGCLetter_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 4);

        }
    }
}