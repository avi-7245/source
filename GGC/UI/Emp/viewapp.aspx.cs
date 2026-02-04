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
    public partial class viewapp : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(viewapp));
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    string letterName = Request.QueryString["DocName"].ToString();
                    Session["APPID"] = ID;
                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"400px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/AppForm/" + letterName));
                    //imgMedaLetter.ImageUrl = "~/Files/MEDA/" + letterName;
                    lblAppNo.Text = "Application ID : " + Session["APPID"].ToString();

                    if (Session["PROJID"].ToString() != "")
                    {
                        lblAppNo.Text += " And Project ID : " + Session["PROJID"].ToString();
                    }

                    int roll_Id = int.Parse(Session["EmpRole"].ToString());
                    if (roll_Id >= 11 && roll_Id <= 20)
                    {
                        tbApprRet.Visible = false;

                    }
                    else
                    {
                        tbApprRet.Visible = true;
                        
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
                        string strComments = txtComments.Text;
                        strQuery = "Update APPLICANTDETAILS set  WF_STATUS_CD_C=7 ,isAppApproved='Y' , APP_STATUS_DT=CURDATE() , app_status='APPLICATION VERIFIED.', AppApproveComment='" + strComments + "'  where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        

                        // fillGrid();
                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','Application verified.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        Response.Write("<script language='javascript'>alert('Application verified.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                        string strCC = dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString();

                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            //strBody += "Please use following information for login for further process. <br/>";
                            strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>";
                            strBody += "Your application has been verified..<br/><br/>";
                            
                            strBody += "Thanks & Regards, " + "<br/>";
                            strBody += " Chief Engineer / STU Department" + "<br/>";
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
                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                //  Msg.To.Add(new MailAddress(toAddress));

                                Msg.Subject = "Online Grid connectivity application.";
                                Msg.Body = strBody;
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
                        }
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "7");
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

            Response.Redirect("~/UI/Emp/EmpHome.aspx", false);
 
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
                        strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=5 ,isAppApproved='N' , APP_STATUS_DT=CURDATE() , app_status='Application Returned.', AppApproveComment='" + strComments + "'  where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        // fillGrid();
                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','Application Returned.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        Response.Write("<script language='javascript'>alert('Application Returned.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                        string strCC = dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            //strBody += "Please use following information for login for further process. <br/>";
                            strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>";
                            strBody += "Your application has been Returned due to reason <b>" + txtComments.Text + ".</b><br/><br/>";

                            strBody += "Thanks & Regards, " + "<br/>";
                            strBody += " Chief Engineer / STU Department" + "<br/>";
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
                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                //  Msg.To.Add(new MailAddress(toAddress));

                                Msg.Subject = "Online Grid connectivity application.";
                                Msg.Body = strBody;
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
                        }
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "6");
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

            Response.Redirect("~/UI/Emp/EmpHome.aspx", false);
        }
        
    }
}