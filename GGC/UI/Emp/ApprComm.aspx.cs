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

namespace GGC.UI.Emp
{
    public partial class ApprComm : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ApprComm));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                lblApplcationNo.Text= Session["APPID"].ToString();
            }
        }

        protected void btnUploadMEDADoc_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/APPROVE/");
            string strAppID = Session["APPID"].ToString();
            string newFileName = "";
            if (Session["IsValid"] != "false")
            {
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUApproval.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        FUApproval.SaveAs(folderPath + Path.GetFileName(FUApproval.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUApproval.FileName;
                        System.IO.File.Move(folderPath + FUApproval.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                string strIsApprove ;
                                if (RBApprove.SelectedItem.Value == "Y")
                                {
                                    strIsApprove = "Approved By commitee";
                                }
                                else
                                {
                                    strIsApprove = "Rejected By commitee";
                                }

                                string strQuery = "Update APPLICANTDETAILS set COMMITEE_LETTER='" + newFileName + "' , APP_STATUS_DT=CURDATE() , app_status='"+strIsApprove+"' where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();

                                #region Application Status tracking date
                            strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                    " values('" + strAppID + "','Approved by commitee.',CURDATE(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            Response.Write("<script language='javascript'>alert('Approved by commitee.');</script>");

                            #endregion
                            strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + strAppID + "'";
                            MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsAppDet = new DataSet();
                            MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                            daAppDet.Fill(dsAppDet);

                            string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
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
                                
                                strBody += "Commitee recommended grid connectivity for your project. The letter is being send to MEDA for Grid connectivity recommendation."+"<br/><br/>";

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
                                    Msg.To.Add(new MailAddress(strEmail));
                                    Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                    Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                    //  Msg.To.Add(new MailAddress(toAddress));

                                    Msg.Subject = "Your Online Grid connectivity application. is Approved by commitee";
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

                                lblResult.Text = "Letter Uploaded Successfully!!";
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