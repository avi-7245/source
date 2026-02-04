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
using GGC.Common;
using GGC.Scheduler;
using MySqlX.XDevAPI.Relational;
using MySqlX.XDevAPI;

namespace GGC.UI.Emp
{
    public partial class UploadPPA : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadPPA));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            strAppID = Request.QueryString["application"];
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/PPALetter/");
            //string strAppID = Session["APPID"].ToString();
            string newFileName = "";
            if (Session["IsValid"] != "false")
            {
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUDoc.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        FUDoc.SaveAs(folderPath + Path.GetFileName(FUDoc.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUDoc.FileName;
                        System.IO.File.Move(folderPath + FUDoc.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=14, PPALetter='" + newFileName + "' ,app_status='PPA letter uploaded.' where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblResult.Text = "PPA Letter Uploaded Successfully!!";
                                lblResult.ForeColor = System.Drawing.Color.Green;
                                objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "14");

                                #region Application Status tracking date
                                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                        " values('" + strAppID + "','PPA letter uploaded.',now(),'" + Session["SAPID"].ToString() + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();

                                strQuery = "select a.* from APPLICANTDETAILS a where a.APPLICATION_NO='" + strAppID + "'";


                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                DataSet dsResult = new DataSet();
                                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                da.Fill(dsResult);


                              /*  #region Send Mail
                                //sendMailOTP(strRegistrationno, strEmailID);
                                string strBody = string.Empty;


                                //strBody += "Respected Sir/Madam" + ",<br/>";
                                strBody = "M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " , <br/>" +
                                    "Your PPA letter uploaded for application no " + strAppID + ". Kindly download attachement. <br/>";
                                //  strBody += " Chief Engineer" + "<br/>";
                                strBody += "State Transmission Utility (STU)" + "<br/>";
                                strBody += "MSETCL  " + "<br/>";
                                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                                //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
                                try
                                {


                                    #region using MailMessage
                                    string mailTo = string.Empty;
                                    MailMessage Msg = new MailMessage();
                                    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                                    Msg.From = fromMail;
                                    Msg.IsBodyHtml = true;
                                    //log.Error("from:" + fromAddress);
                                    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                                    Msg.To.Add(new MailAddress(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                    string[] splittedCC = ConfigurationManager.AppSettings["MMCCEmailID"].ToString().Split(';');
                                    foreach (var part in splittedCC)
                                    {
                                        if (part != "")
                                            Msg.CC.Add(new MailAddress(part));
                                    }
                                    Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["CESTU"].ToString()));
                                    //Msg.CC.Add(new MailAddress("cepd@mahatransco.in"));


                                    Msg.Subject = "Grid connectivity Letter is issued ";
                                    Msg.Body = strBody;
                                    System.Net.Mail.Attachment attachment;

                                    attachment = new System.Net.Mail.Attachment(Server.MapPath("~/Files/IssueLetter/" + newFileName));
                                    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                    //SmtpClient a = new SmtpClient("23.103.140.170");
                                    SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                                    a.EnableSsl = true;
                                    NetworkCredential n = new NetworkCredential();
                                    n.UserName = "donotreply@mahatransco.in";
                                    n.Password = "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~";
                                    a.UseDefaultCredentials = false;
                                    a.Credentials = n;
                                    a.Port = 587;
                                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                                    a.Send(Msg);

                                    Msg = null;
                                    fromMail = null;
                                    a = null;
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                    log.Error(ErrorMessage);
                                    // throw ex;
                                }
                                #endregion*/
                              //  DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,53,54,55) union select * from empmaster where zone='" + dsResult.Tables[0].Rows[0]["ZONE"].ToString() + "'");
                             /*   #region Send SMS
                                string strMobNos = string.Empty;
                                strMobNos = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + ",";
                                try
                                {
                                    //log.Error("1 ");
                                    if (dsEmailMob.Tables[0].Rows.Count > 0)
                                    {



                                        if (dsEmailMob.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dsEmailMob.Tables[0].Rows.Count; i++)
                                            {
                                                if (dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                                                    strMobNos += dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                            }

                                        }
                                    }
                                    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20Project%20proposed%20by%20M%2Fs." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20approved.%20Grid%20Connectivity%20letter%20uploaded.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                    //log.Error("strURL " + strURL);

                                    WebRequest request = HttpWebRequest.Create(strURL);
                                    //log.Error("2 ");
                                    // Get the response back  
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    Stream s = (Stream)response.GetResponseStream();
                                    StreamReader readStream = new StreamReader(s);
                                    string dataString = readStream.ReadToEnd();
                                    log.Error("8 " + dataString);

                                    response.Close();
                                    s.Close();
                                    readStream.Close();
                                }
                                catch (Exception ex)
                                {
                                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                    log.Error(ErrorMessage);
                                }
                                #endregion

                              */ 
                             #endregion
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

        protected void ValidateFileSize(object sender, ServerValidateEventArgs e)
        {
            //System.Drawing.Image img = System.Drawing.Image.FromStream(FUDoc.PostedFile.InputStream);
            //int height = img.Height;
            //int width = img.Width;
            decimal sizeFUDoc1 = 0;
            sizeFUDoc1 = Math.Round(((decimal)FUDoc.PostedFile.ContentLength / (decimal)1024), 2);

            if (Session["IsValid"] != "false")
            {
                e.IsValid = true;
                Session["IsValid"] = "true";
            }


        }
    }
}