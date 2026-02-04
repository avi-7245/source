using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using log4net;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;
using GGC.Common;
using GGC.WebService;

namespace GGC.UI.Cons
{
    public partial class UploadForm : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadForm));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                getDocDetails();
            }
        }

       
        protected void getDocDetails()
        {
            string strAppID = Session["APPID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();
            //Save the File to the Directory (Folder).
            try
            {
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection.Open();
                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = "select count(AppFormDocName) AppFormDocName from APPLICANTDETAILS where APPLICATION_NO='" + strAppID + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (dsResult.Tables[0].Rows[0]["AppFormDocName"].ToString() != "0")
                        {
                            lblResult.Text = "Application Form already uploaded";
                        }
                        
                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:
                        break;

                }
            }
            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Letter Uploaded Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Letter Uploaded Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }

        }
        protected void UploadAppForm(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/AppForm/");
            string strAppID = Session["APPID"].ToString();
            string newFileName = "";
            if (Session["IsValid"].ToString() != "false")
            {
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUAppForm.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        FUAppForm.SaveAs(folderPath + Path.GetFileName(FUAppForm.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUAppForm.FileName;
                        System.IO.File.Move(folderPath + FUAppForm.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=4, AppFormDocName='" + newFileName + "',app_status='APPLICATION FORM UPLOADED.PAYMENT PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblResult.Text = "Application form uploaded Successfully. Kindly make payment from Home page.";
                                lblResult.ForeColor = System.Drawing.Color.Green;

                                saveApp(Session["ProjectID"].ToString());
                                break;

                            case System.Data.ConnectionState.Closed:

                                // Connection could not be made, throw an error

                                throw new Exception("The database connection state is Closed");

                                break;

                            default:
                                break;

                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                        log.Error(ErrorMessage);
                        lblResult.Text = "Letter Uploaded Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblResult.Text = "Letter Uploaded Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }

        protected void ValidateMEDAFileSize(object sender, ServerValidateEventArgs e)
        {
            //System.Drawing.Image img = System.Drawing.Image.FromStream(FUMEDADoc.PostedFile.InputStream);
            //int height = img.Height;
            //int width = img.Width;
            decimal sizeFUDoc1 = 0;
            sizeFUDoc1 = Math.Round(((decimal)FUAppForm.PostedFile.ContentLength / (decimal)1024), 2);

            if (Session["IsValid"] != "false")
            {
                e.IsValid = true;
                Session["IsValid"] = "true";
            }


        }

        public string saveApp(string projId)
        {
            GetStatusDTO objGetStatusDTO = new GetStatusDTO();
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        strQuery = "select * from applicantdetails where MEDAProjectID='" + projId + "'";
                        MySqlCommand cmd;
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //txtEmail1.Text = dsResult.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
                        objGetStatusDTO.projID = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                        objGetStatusDTO.status = dsResult.Tables[0].Rows[0]["app_status"].ToString();
                        //objGetStatusDTO.statusDT = dsResult.Tables[0].Rows[0]["APP_STATUS_DT"].ToString();
                        string strStatusDate = dsResult.Tables[0].Rows[0]["APP_STATUS_DT"].ToString();
                        objGetStatusDTO.statusDT = strStatusDate; //DateTime.ParseExact(strStatusDate, "dd-MM-yyyy", null);
                        objGetStatusDTO.APPID = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                        objGetStatusDTO.Contact_Person = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString();
                        objGetStatusDTO.Contact_Person_No = dsResult.Tables[0].Rows[0]["CONT_PER_PHONE_1"].ToString();
                        objGetStatusDTO.Contact_Person_MobNo = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        objGetStatusDTO.Contact_Person_Email = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                        objGetStatusDTO.TotalLandRequired = dsResult.Tables[0].Rows[0]["TOTAL_REQUIRED_LAND"].ToString();
                        objGetStatusDTO.ProjecType = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                        objGetStatusDTO.ProjCapacityMW = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                        objGetStatusDTO.GenerationVoltage = dsResult.Tables[0].Rows[0]["GENERATION_VOLTAGE"].ToString();
                        objGetStatusDTO.PointOfInjection = dsResult.Tables[0].Rows[0]["POINT_OF_INJECTION"].ToString();
                        objGetStatusDTO.InjectionVoltage = dsResult.Tables[0].Rows[0]["INJECTION_VOLTAGE"].ToString();
                        objGetStatusDTO.GENERATOR_NAME = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                        objGetStatusDTO.TYPE_OF_GENERATION = dsResult.Tables[0].Rows[0]["TYPE_OF_GENERATION"].ToString();
                        objGetStatusDTO.NAME_OF_SUBSTATION = dsResult.Tables[0].Rows[0]["POINT_OF_INJECTION"].ToString();
                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.statusId = "6";
                        APICall apiCall = new APICall();
                        string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + Session["user_name"].ToString() + "&projId=" + Session["ProjectID"].ToString() + "&entity=MSETCL");

                        objGetStatusDTO.secKey = str;
                        objGetStatusDTO.entity = "MSETCL";



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
                //lblResult.Text = "Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                //lblResult.Text = "Please try again.";
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



            try
            {
                //                                string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projid=" + txtProjCode + "&entity=MSETCL&secKey=" + str;
                //string userAuthenticationURI = "https://regrid.mahadiscom.in/reGrid/saveToMedaGcAppln";
                string userAuthenticationURI = ConfigurationManager.AppSettings["MEDASAVETOMEDAURL"].ToString();
                //WebRequest req = WebRequest.Create(@userAuthenticationURI);
                var req = (HttpWebRequest)WebRequest.Create(@userAuthenticationURI);


                req.Method = "POST";
                req.ContentType = "application/json";
                //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                //req.Credentials = new NetworkCredential("username", "password");
                using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);

                    streamWriter.Write(JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None));
                }
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseText = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
            }
            //string str = JsonConverter. .Serialize(objGetStatusDTO, Formatting.Indented);
            return JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);
            //return  new JavaScriptSerializer().Serialize(objGetStatusDTO);
        }
    }
}