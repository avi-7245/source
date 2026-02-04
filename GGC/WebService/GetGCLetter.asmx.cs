using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using System.Web.Script.Services;

namespace GGC.WebService
{
    /// <summary>
    /// Summary description for GetGCLetter
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GetGCLetter : System.Web.Services.WebService
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(GetGCLetter));

        //[ScriptMethod(UseHttpGet = false)]
        [WebMethod]
        public string GetPDF(string MEDAProjectID, int doctype,string strKey)
        {
            //var hmac = new HMACSHA256();
            //var key = Convert.ToBase64String(hmac.Key);

            //string path = Server.MapPath("~/Files/FinalGCFiles/CONNECTION AGREEMENT.pdf");
            //byte[] bytes = System.IO.File.ReadAllBytes(path);
            //return Convert.ToBase64String(bytes);
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strData = string.Empty;
            GetPDFDTO objGetPDFDTO = new GetPDFDTO();
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
                                strQuery = "select GCLetterName as docname from AppliedGCDocs where MEDAProjectID='" + MEDAProjectID + "'";
                                //strFolderName = "AppForm";
                                break;

                            case 2:
                                strQuery = "select GCOtherDocName as docname from AppliedGCDocs where MEDAProjectID='" + MEDAProjectID + "'";

                                //strFolderName = "Tech";
                                break;
                            case 3:
                                strQuery = "select GCExt1 as docname from AppliedGCDocs where MEDAProjectID='" + MEDAProjectID + "'";
                                //strFolderName = "LFS";
                                break;
                            case 4:
                                strQuery = "select GCExt2 as docname from AppliedGCDocs where MEDAProjectID='" + MEDAProjectID + "'";
                                //strFolderName = "LFSOther";
                                break;
                            case 5:
                                strQuery = "select IssueLetter as docname from applicantdetails where MEDAProjectID='" + MEDAProjectID + "'";
                                //strFolderName = "LFSOther";
                                break;
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        log.Error("dsResult.Tables[0].Rows[0][0].ToString()" + dsResult.Tables[0].Rows[0][0].ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            if (dsResult.Tables[0].Rows[0][0].ToString() != "")
                            {
                                string path;
                                if(doctype==5)
                                    path = Server.MapPath("~/Files/IssueLetter/" + dsResult.Tables[0].Rows[0][0].ToString());
                                else
                                    path = Server.MapPath("~/Files/GCFiles/" + dsResult.Tables[0].Rows[0][0].ToString());
                                byte[] bytes = System.IO.File.ReadAllBytes(path);
                                objGetPDFDTO.strFile= Convert.ToBase64String(bytes);
                            }
                            else
                            {
                                objGetPDFDTO.strFile = "No file found.";
                            }
                        }
                        else
                        {
                            objGetPDFDTO.strFile = "No file found.";
                        }
                        log.Error("File status");
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
            return JsonConvert.SerializeObject(objGetPDFDTO, Formatting.None);
        }

        //[WebMethod]
        //protected string getDocument(string MEDAProjectID, int doctype)
        //{
           
        //}

        private string ResolveUrl(string p)
        {
            throw new NotImplementedException();
        }
    }
}
