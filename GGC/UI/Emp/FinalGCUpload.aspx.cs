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

namespace GGC.UI.Emp
{
    public partial class FinalGCUpload : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadIssueLetter));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            strAppID = Request.QueryString["appid"];

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

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {
            string strAppID = Session["APPID"].ToString();
            string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");

            string newFileName = "";
            //if (Session["IsValid"] != "false")
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
                                string strQuery = "UPDATE finalgcapproval SET FinalGCLetterFN = @FinalGCLetterFN WHERE APPLICATION_NO = @ApplicationNo";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.Parameters.AddWithValue("@FinalGCLetterFN", newFileName);
                                cmd.Parameters.AddWithValue("@ApplicationNo", strAppID);

                                int rowsAffected = cmd.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    lblResult.Text = "Final Grid Connectivity Issued Letter Uploaded Successfully!!";
                                }
                                else
                                {
                                    lblResult.Text = "No records were updated. Please check the APPLICATION_NO.";
                                }


                                strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=27, app_status='FINAL GRID CONNECTIVITY LETTER ISSUED.' where APPLICATION_NO='" + strAppID + "'";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();

                                objSaveToMEDA.saveApp(Session["MEDAID"].ToString(), "27");

                                #region Application Status tracking date
                                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                        " values('" + strAppID + "','Final Grid Connectivity Letter Issued Uploaded.',now(),'" + Session["SAPID"].ToString() + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();

                                strQuery = "select a.*,b.* from APPLICANTDETAILS a, applicant_reg_det b where a.APPLICATION_NO='" + strAppID + "' and a.USER_NAME =b.USER_NAME ";


                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                DataSet dsResult = new DataSet();
                                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                da.Fill(dsResult);


                                #region Send Mail
                                //sendMailOTP(strRegistrationno, strEmailID);
                                string strBody = string.Empty;


                                //strBody += "Respected Sir/Madam" + ",<br/>";
                                strBody = "M/s. " + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " , <br/>" +
                                    "Your Final Grid connectivity Letter is issued for application no " + strAppID + ". Kindly download attachement. <br/>";
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
                                    Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));


                                    Msg.Subject = "Final Grid connectivity Letter is issued ";
                                    Msg.Body = strBody;
                                    System.Net.Mail.Attachment attachment;
                                    attachment = new System.Net.Mail.Attachment(Server.MapPath("~/Files/FinalGC/" + newFileName));
                                    Msg.Attachments.Add(attachment);
                                    //Msg.Attachments.Add(new Attachment(new MemoryStream(bytes),newFileName);
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
                                #endregion
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
    }
}