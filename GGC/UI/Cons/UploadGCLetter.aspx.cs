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
    public partial class UploadGCLetter : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadGCLetter));
        string strMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["APPID"] = "1";
            //Session["ProjectID"] = "2";
            //Session["user_name"] = "ashisht";
            txtAppliedDt.Attributes.Add("readonly", "readonly");
            txtIssueDt.Attributes.Add("readonly", "readonly");
            txtExt1.Attributes.Add("readonly", "readonly");
            txtExt2.Attributes.Add("readonly", "readonly");
            txtValidityDt.Attributes.Add("readonly", "readonly");

            CalendarExtender2.EndDate = DateTime.Today.AddDays(-1);
            CalendarExtender1.EndDate = DateTime.Today.AddDays(-1);
            
            if (!Page.IsPostBack)
            {
                getDocDetails();
            }
        }

        protected string checkDates()
        {
            log.Error("Before checking dates");
            DateTime dtApplyDt = DateTime.ParseExact(txtAppliedDt.Text, "dd-MM-yyyy",CultureInfo.InvariantCulture);
            DateTime dtIssueDt = DateTime.ParseExact(txtIssueDt.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime dtValidityDt = DateTime.ParseExact(txtValidityDt.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            DateTime dtExt1Dt = txtExt1.Text!=""? DateTime.ParseExact(txtExt1.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture): DateTime.Today;
            DateTime dtExt2Dt = txtExt2.Text!=""? DateTime.ParseExact(txtExt2.Text, "dd-MM-yyyy", CultureInfo.InvariantCulture):DateTime.Today;
            
           
            log.Error("dtApplyDt " + dtApplyDt);
            log.Error("dtIssueDt " + dtIssueDt);
            string isValid = "true";
            int issue_Apply = dtIssueDt.CompareTo(dtApplyDt);
            if (issue_Apply <= 0)
            {
                strMessage = "Issue date must be greater than apply date!!<br/>";
                isValid = "false";
            }
            int Val_Issue = dtValidityDt.CompareTo(dtIssueDt);
            if (Val_Issue <= 0)
            {
                strMessage += "Issue date must be less than Validity date!!<br/>";
                isValid = "false";
            }
            if (txtExt1.Text != "")
            {
                int Ext1_Val = dtExt1Dt.CompareTo(dtValidityDt);
                if (Ext1_Val <= 0)
                {
                    strMessage += "Validity date must be less than First Extension date!!<br/>";
                    isValid = "false";
                }
            }
            if (txtExt2.Text != "")
            {
                int Ext2_Ext1 = dtExt2Dt.CompareTo(dtExt1Dt);
                if (Ext2_Ext1 <= 0)
                {
                    strMessage += "First Extension date must be less than Second Extension date!!<br/>";
                    isValid = "false";
                }
                int Ext2_Val = dtExt2Dt.CompareTo(dtValidityDt);
                if (Ext2_Val <= 0)
                {
                    strMessage += "Validity date must be less than Second Extension date!!<br/>";
                    isValid = "false";
                }
            }
            return isValid;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string isValid=checkDates();
            if (isValid == "true")
            {
                string folderPath = Server.MapPath("~/Files/GCFiles/");
                string strAppID = Session["APPID"].ToString();
                string strMEDAPrjID = Session["ProjectID"].ToString();
                string GCFileName = "";
                string OtherFileName = "";
                string Ext1FileName = "";
                string Ext2FileName = "";
                string appDD = txtAppliedDt.Text.Substring(0, 2);
                string appMM = txtAppliedDt.Text.Substring(3, 2);
                string appYYYY = txtAppliedDt.Text.Substring(6, 4);
                string strapp_DATE = appYYYY + "-" + appMM + "-" + appDD;

                //string issueDD = txtAppliedDt.Text.Substring(0, 2);
                //string issueMM = txtAppliedDt.Text.Substring(3, 2);
                //string issueYYYY = txtAppliedDt.Text.Substring(6, 4);
                string strissue_DATE = txtIssueDt.Text.Substring(6, 4) + "-" + txtIssueDt.Text.Substring(3, 2) + "-" + txtIssueDt.Text.Substring(0, 2);
                string strExt1_DATE = "";
                string strExt2_DATE = "";
                if (txtExt1.Text != "")
                    strExt1_DATE = txtExt1.Text.Substring(6, 4) + "-" + txtExt1.Text.Substring(3, 2) + "-" + txtExt1.Text.Substring(0, 2);
                if (txtExt2.Text != "")
                    strExt2_DATE = txtExt2.Text.Substring(6, 4) + "-" + txtExt2.Text.Substring(3, 2) + "-" + txtExt2.Text.Substring(0, 2);
                try
                {

                    //Check whether Directory (Folder) exists.
                    if (!Directory.Exists(folderPath))
                    {
                        //If Directory (Folder) does not exists Create it.
                        Directory.CreateDirectory(folderPath);
                    }
                    if (FUGCLetter.HasFile)
                    {
                        FUGCLetter.SaveAs(folderPath + Path.GetFileName(FUGCLetter.FileName));
                        GCFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUGCLetter.FileName;
                        System.IO.File.Move(folderPath + FUGCLetter.FileName, folderPath + GCFileName);
                    }
                    if (FUOther.HasFile)
                    {
                        FUOther.SaveAs(folderPath + Path.GetFileName(FUOther.FileName));
                        OtherFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUOther.FileName;
                        System.IO.File.Move(folderPath + FUOther.FileName, folderPath + OtherFileName);
                    }
                    if (FUExt1.HasFile)
                    {
                        FUExt1.SaveAs(folderPath + Path.GetFileName(FUExt1.FileName));
                        Ext1FileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUExt1.FileName;
                        System.IO.File.Move(folderPath + FUExt1.FileName, folderPath + Ext1FileName);
                    }
                    if (FUExt2.HasFile)
                    {
                        FUExt2.SaveAs(folderPath + Path.GetFileName(FUExt2.FileName));
                        Ext2FileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUExt2.FileName;
                        System.IO.File.Move(folderPath + FUExt2.FileName, folderPath + Ext2FileName);
                    }
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {


                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:

                                string strQuery = "";
                                if (strExt1_DATE == "" && strExt2_DATE == "")
                                {
                                    strQuery = "insert into AppliedGCDocs (APPLICATION_NO , MEDAProjectID , AppliedDt , GCLetterName , GCOtherDocName , GCExt1 , GCExt2,GCIssuedDt) values('" + strAppID + "','" + strMEDAPrjID + "','" + strapp_DATE + "','" + GCFileName + "','" + OtherFileName + "','" + Ext1FileName + "','" + Ext2FileName + "','" + strissue_DATE + "')";
                                    log.Error(strQuery);
                                }
                                if (strExt1_DATE != "" && strExt2_DATE == "")
                                {
                                    strQuery = "insert into AppliedGCDocs (APPLICATION_NO , MEDAProjectID , AppliedDt , GCLetterName , GCOtherDocName , GCExt1 , GCExt2,GCIssuedDt,GCExt1Dt) values('" + strAppID + "','" + strMEDAPrjID + "','" + strapp_DATE + "','" + GCFileName + "','" + OtherFileName + "','" + Ext1FileName + "','" + Ext2FileName + "','" + strissue_DATE + "','" + strExt1_DATE + "')";
                                    log.Error(strQuery);
                                }
                                if (strExt1_DATE != "" && strExt2_DATE != "")
                                {
                                    strQuery = "insert into AppliedGCDocs (APPLICATION_NO , MEDAProjectID , AppliedDt , GCLetterName , GCOtherDocName , GCExt1 , GCExt2,GCIssuedDt,GCExt1Dt,GCExt2Dt) values('" + strAppID + "','" + strMEDAPrjID + "','" + strapp_DATE + "','" + GCFileName + "','" + OtherFileName + "','" + Ext1FileName + "','" + Ext2FileName + "','" + strissue_DATE + "','" + strExt1_DATE + "','" + strExt2_DATE + "')";
                                    log.Error(strQuery);
                                }
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                log.Error("Before Update");
                                strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=19,app_status='OLD GC APPLICATION FORWORDED FOR APPROVAL.' where APPLICATION_NO='" + strAppID + "'";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                log.Error("After Update");
                                saveApp(Session["ProjectID"].ToString());
                                try
                                {
                                    SendEmail mail = new SendEmail();
                                    strQuery = "select * from APPLICANTDETAILS where APPLICATION_NO='" + strAppID + "'";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    DataSet dsResult = new DataSet();
                                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                    da.Fill(dsResult);

                                    string strMailTo = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString() + ";";
                                    //string strCC = ConfigurationManager.AppSettings["MEDAEmail"].ToString() + ";";
                                    string strBody = string.Empty;

                                    strBody += "Dear User" + ",<br/>";
                                    strBody += "Thank you for submitting Letters and details of old Grid connectivity application having project ID :" + strMEDAPrjID + "<br/>";
                                    strBody += "Your application will be approved/verified by MEDA. <br/>";
                                    //strBody += "Full Name " + "<br/>";
                                    //strBody += "Advertisement No : " + strAdvDesc + "\n";
                                    //strBody += "Please use following information for login for further process. <br/>";
                                    strBody += "<br/>";
                                    strBody += "Thanks & Regards, " + "<br/>";
                                    strBody += "Chief Engineer / STU Department" + "<br/>";
                                    strBody += "MSETCL  " + "<br/>";
                                    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                                    mail.Send(strMailTo, "", "Online Grid connectivity application.", strBody);
                                }
                                catch (Exception ex)
                                {
                                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                    log.Error(ErrorMessage);
                                }
                                
                                //Response.Write("<script language='javascript'>window.alert('Documents Uploaded');window.location='UploadGCLetter.aspx';</script>");
                                //lblGCLetter1.Text = "SLD Uploaded Successfully!!";
                                //Response.Redirect("~/UI/Cons/AppHome.aspx", false);
                                Response.Write("<script language='javascript'>window.alert('Documents Uploaded.');window.location='AppHome.aspx';</script>");
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
                        lblMessage.Text = "Submission Failed, Uploaded files might be having problem!! ";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblMessage.Text = "Submission Failed. Check weather all files selected should not be the same.";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    lblMessage.Text = "Submitted Failed!!";
                }

            }
            else
            {
                lblMessage.Text = strMessage;
            }

                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            
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
                        objGetStatusDTO.statusId = "19";
                        objGetStatusDTO.OLD_GC_APPROVED_FLAG = dsResult.Tables[0].Rows[0]["IS_ALREADY_APPLIED"].ToString();
                        objGetStatusDTO.DISTANCE_FROM_PLANT = dsResult.Tables[0].Rows[0]["DISTANCE_FROM_PLANT"].ToString();
                        objGetStatusDTO.VOLT_LEVEL_SUBSTATION = dsResult.Tables[0].Rows[0]["VOLT_LEVEL_SUBSTATION"].ToString();

                        objGetStatusDTO.OLD_GC_APPLICATION_DATE = txtAppliedDt.Text;
                        objGetStatusDTO.OLD_GC_APPROVED_DATE = txtIssueDt.Text;
                        objGetStatusDTO.validity = txtValidityDt.Text;
                        objGetStatusDTO.FIRST_EXTENSION_DATE = txtExt1.Text;
                        objGetStatusDTO.SECOND_EXTENSION_DATE = txtExt2.Text;

                        


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
                    log.Error("Old GC json " + json);
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
                        string strQuery = "select * from APPLICANTDETAILS where APPLICATION_NO='" + strAppID + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["ProjectID"] = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                        if (dsResult.Tables[0].Rows[0]["WF_STATUS_CD_C"].ToString() == "2")
                        {
                            btnSubmit.Enabled = true;
                        }
                        else
                        {
                            btnSubmit.Enabled = false;
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
                //lblResult.Text = "Letter Uploaded Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Letter Uploaded Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }
        
        }
        protected void ValidateMEDAFileSize(object sender, ServerValidateEventArgs e)
        {
            //System.Drawing.Image img = System.Drawing.Image.FromStream(FUMEDADoc.PostedFile.InputStream);
            //int height = img.Height;
            //int width = img.Width;
            decimal sizeFUDoc1 = 0;
            sizeFUDoc1 = Math.Round(((decimal)FUOther.PostedFile.ContentLength / (decimal)1024), 2);

            if (Session["IsValid"] != "false")
            {
                e.IsValid = true;
                Session["IsValid"] = "true";
            }


        }

    }
}