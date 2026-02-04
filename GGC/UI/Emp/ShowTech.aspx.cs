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
using GGC.Common;

namespace GGC.UI.Emp
{
    public partial class ShowTech : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ShowTech));
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    string docName = Request.QueryString["docName"].ToString();
                    Session["APPID"] = ID;
                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"1000px\" height=\"500px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/Tech/" + docName));
                    //imgMedaLetter.ImageUrl = "~/Files/MEDA/" + letterName;
                    //lblAppNo.Text = ID;

                    lblAppNo.Text += Session["APPID"].ToString();

                    if (Session["PROJID"] != null)
                    {
                        lblAppNo.Text += " And Project ID : " + Session["PROJID"].ToString();
                    }
                }
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            string applicationId = Session["APPID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            string letterName = string.Empty;
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        string strComments=txtComments.Text;
                        strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=10, APP_STATUS_DT=CURDATE() , app_status='TECHNICAL FEASIBILITY REPORT VERIFIED.', comments='" + strComments + "' where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "10");
                        // fillGrid();
                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','Technical feasibility report approved.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        Response.Write("<script language='javascript'>alert('Technical feasibility report Approved.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,c.email Zone_email from APPLICANTDETAILS a,zone_district c where lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
                        string strZone = dsAppDet.Tables[0].Rows[0]["Zone"].ToString();
                        string strMobile = dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            //strBody += "Please use following information for login for further process. <br/>";
                            strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>";
                            strBody += "Your Technical feasibility report is Approved and now under Load Flow Study.<br/><br/>";
                            
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
                                //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                //  Msg.To.Add(new MailAddress(toAddress));
                                string strTo = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                                if (strTo != "")
                                {
                                    string[] splittedCC = strTo.Split(';');
                                    foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        if (part != "")
                                            Msg.CC.Add(new MailAddress(part.ToString()));
                                        //Msg.CC.Add(new MailAddress(part.ToString()));
                                        log.Error("Part " + part);
                                    }
                                }
                                Msg.Subject = "Technical feasibility report is approved for Online Grid connectivity application.";
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
                        btnApprove.Visible = false;
                        log.Error("strMobile" + strMobile);
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            string strQueryMob = string.Empty;
                            string strMobNos = string.Empty;
                            strQueryMob = "select * from empmaster where role_id in(53,52,51) or ZONE='" + strZone + "'";
                            MySqlCommand cmdMob = new MySqlCommand(strQueryMob, mySqlConnection);
                            DataSet dsMob = new DataSet();
                            MySqlDataAdapter daMob = new MySqlDataAdapter(cmdMob);
                            daMob.Fill(dsMob);
                            strMobNos = strMobile + ",";
                            if (dsMob.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMob.Tables[0].Rows.Count; i++)
                                {
                                    strMobNos += dsMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                }

                            }
                            log.Error("strMobNos" + strMobNos);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Technical feasibility report verifed.');window.location ='EmpHome.aspx';", true);
                            Response.Redirect("~/UI/Emp/EmpHome.aspx", false);
                            #region Send SMS
                            //try
                            //{

                            //    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Your%20application%20no." + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20is%20forwarded%20to%20Chief%20Engineer%2C%20" + dsAppDet.Tables[0].Rows[0]["ZONE"].ToString() + "%20for%20Technical%20Feasibility%20Report.%20Please%20visit%20field%20office%20for%20joint%20survey.%5CnRegards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                            //    log.Error("strURL" + strURL);
                                
                            //    WebRequest request = HttpWebRequest.Create(strURL);
                            //    // Get the response back  
                            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            //    Stream s = (Stream)response.GetResponseStream();
                            //    StreamReader readStream = new StreamReader(s);
                            //    string dataString = readStream.ReadToEnd();
                            //    response.Close();
                            //    s.Close();
                            //    readStream.Close();
                            //}
                            //catch (Exception ex)
                            //{
                            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                            //    log.Error(ErrorMessage);
                            //}
                            #endregion
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
            
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string applicationId = Session["APPID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            string letterName = string.Empty;
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        string strComments = txtComments.Text;

                        strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=8, APP_STATUS_DT=CURDATE() , app_status='Technical feasibility report Not approved.', comments='" + strComments + "' where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "8");
                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','Technical feasibility report Not approved.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        //Response.Write("<script language='javascript'>alert('Technical feasibility report Not Approved.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,c.email Zone_email from APPLICANTDETAILS a,zone_district c where lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
                        string strZone = dsAppDet.Tables[0].Rows[0]["Zone"].ToString();
                        string strMobile = dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            //strBody += "Please use following information for login for further process. <br/>";
                            strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".The copy of application mentioning details of project and applicant is attached herewith.<br/>";
                            strBody += "Your Technical feasibility report is not approved beacuse of following reason.<br/><b> " + txtComments.Text + " .</b> <br/> Please reupload correct Technical feasibility report.<br/><br/><br/><br/>";

                            strBody += "Thanks & Regards, " + "<br/>";
                            strBody += " Chief Engineer" + "<br/>";
                            strBody += "(State Transmission Utility)" + "<br/>";
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
                                Msg.To.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                Msg.CC.Add(new MailAddress(strEmail));
                                //  Msg.To.Add(new MailAddress(toAddress));
                                string strTo = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                                if (strTo != "")
                                {
                                    string[] splittedCC = strTo.Split(';');
                                    foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        if (part != "")
                                            Msg.CC.Add(new MailAddress(part.ToString()));
                                        //Msg.CC.Add(new MailAddress(part.ToString()));
                                        log.Error("Part " + part);
                                    }
                                }
                                Msg.Subject = "Technical feasibility report rejected in Online Grid connectivity application.";
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

                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            string strQueryMob = string.Empty;
                            string strMobNos = string.Empty;
                            strQueryMob = "select * from empmaster where role_id in(53,52,51) or ZONE='" + strZone + "'";
                            MySqlCommand cmdMob = new MySqlCommand(strQueryMob, mySqlConnection);
                            DataSet dsMob = new DataSet();
                            MySqlDataAdapter daMob = new MySqlDataAdapter(cmdMob);
                            daMob.Fill(dsMob);
                            strMobNos = strMobile + ",";
                            log.Error("strMobNos" + strMobNos);
                            log.Error("strMobile" + strMobile);
                            if (dsMob.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMob.Tables[0].Rows.Count; i++)
                                {
                                    strMobNos += dsMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                }

                            }
                            log.Error("strMobNos" + strMobNos);

                            #region Send SMS
                            try
                            {

                                string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Your%20application%20no." + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20is%20forwarded%20to%20Chief%20Engineer%2C%20" + dsAppDet.Tables[0].Rows[0]["ZONE"].ToString() + "%20for%20Revised%20Technical%20Feasibility%20Report.%20Please%20visit%20field%20office%20for%20joint%20survey.%5CnRegards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                log.Error("strURL" + strURL);
                                WebRequest request = HttpWebRequest.Create(strURL);
                                // Get the response back  
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                Stream s = (Stream)response.GetResponseStream();
                                StreamReader readStream = new StreamReader(s);
                                string dataString = readStream.ReadToEnd();
                                log.Error("dataString" + dataString);
                                response.Close();
                                s.Close();
                                readStream.Close();
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Technical feasibility report not approved.');window.location ='EmpHome.aspx';", true);
                                //Response.Redirect("~/UI/Emp/EmpHome.aspx", false);
                            }
                            catch (Exception ex)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                log.Error(ErrorMessage);
                            }
                            #endregion
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

            

        }
    }
}