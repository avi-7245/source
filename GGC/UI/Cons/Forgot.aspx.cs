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
using System.Text;

namespace GGC.UI.Cons
{
    public partial class Forgot : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Forgot));
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            string strPan = txtUID.Text;
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;

                        strQuery = "select ORG_MOB,ORG_EMAIL from APPLICANT_REG_DET where PAN_TAN_NO='" + txtUID.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        string strMob= dsResult.Tables[0].Rows[0]["ORG_MOB"].ToString();
                        string strEmail = dsResult.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
                         Random rnd = new Random();
            string otp = string.Empty;
            
            otp += rnd.Next(100000, 999999).ToString();
            Session["OTP"] = otp.ToString();
            
                        sendOTP(strMob,otp);
                        sendEmailOTP(strEmail,otp);

                       
                        txtUID.Enabled = false;
                        trSubmitOTP.Visible = true;            
                        
                        
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
        }

        protected void sendEmailOTP(string strEmail, string otp)
        {
            #region Send Mail
            //sendMailOTP(strRegistrationno, strEmailID);
            string strBody = string.Empty;
           

            strBody += "Respected Sir/Madam" + ",<br/>";
            strBody = "Your OTP is " + otp + " for forgot password in Grid connectivity website,MSETCL."+"<br/>";
            
            strBody += "Thanks & Regards, " + "<br/>";
            //  strBody += " Chief Engineer" + "<br/>";
            strBody += "State Transmission Utility (STU)" + "<br/>";
            strBody += "MSETCL  " + "<br/>";
            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

            //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
            try
            {


                #region using MailMessage
                MailMessage Msg = new MailMessage();
                MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                Msg.From = fromMail;
                Msg.IsBodyHtml = true;
                //log.Error("from:" + fromAddress);
                //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                Msg.To.Add(new MailAddress(strEmail));

                Msg.Subject = "Otp for forgot password.";
                Msg.Body = strBody;
                //SmtpClient a = new SmtpClient("mail.mahatransco.in");
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
        }

        private void sendOTP(string strMob, string otp)
        {
            
            string strMessage = string.Empty;
            
            
            strMessage = "Your OTP is " + otp + " for forgot password to login into Grid connectivity website,MSETCL.";
            string sUserID = "MSETCL";
            string sPwd = "MSETCL@@";
            string sNumber = strMob;
            string sSID = "MSETCL";
            string sMessage = strMessage;
            string sURL = "http://sms.360marketings.in/vendorsms/pushsms.aspx?user=" + sUserID + "&password=" + sPwd + "&msisdn=" + sNumber + "&sid=" + sSID + "&msg=" + sMessage
            + "&dc=8&fl=0&gwid=2";
            string sResponse = GetResponse(sURL);
            Response.Write(sResponse);
            log.Error("Mob : " + strMob + " Response : " + sResponse);
        }

        public static string GetResponse(string sURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sURL);
            request.MaximumAutomaticRedirections = 4;
            request.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string sResponse = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                return sResponse;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtOTP.Text == Session["OTP"].ToString())
            {
                tblNewPass.Visible = true;
                btnSubmit.Enabled = false;
                txtOTP.Enabled = false;
                txtOTP.Text = "";
            }
            else
            {
                tblNewPass.Visible = false;
                btnSubmit.Enabled = true;
                txtOTP.Enabled = true;
                txtOTP.Text = "";
            }
        }

        protected void btnUpdatePass_Click(object sender, EventArgs e)
        {
           // string strPan = lblPAN.Text;
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
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        
        
                            strQuery = "Update applicant_login_master set PASSWORD='"+ txtNewPassword.Text +"' where USER_NAME='" + txtUID.Text + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            txtNewPassword.Text = "";
                            txtConfirmPass.Text = "";
                            txtOTP.Text = "";
                            Response.Write("<script language='javascript'>alert('Password updated successfully.');</script>");


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

        
        }

        

       
    }
}