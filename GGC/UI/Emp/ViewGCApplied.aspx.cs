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
    public partial class ViewGCApplied : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ViewGCApplied));
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["application"] != null)
                {
                    string ID = Request.QueryString["application"].ToString();
                    lblAppNo.Text = ID;
                    Session["APPID"] = ID;

                    getDocument(ID, 1);
                    //getDetails(ID);
                    if (Session["WF_STATUS_CD_C"].ToString() == "26")
                    {
                        btnApproved.Enabled = true;
                        btnReject.Enabled = true;
                    }
                }
            }
        }
        protected void getDetails(string applicationId)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strQuery = string.Empty;
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        strQuery = "select srno from appliedgcdocs where APPLICATION_NO='" + applicationId + "' and UpdateDt isnull";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            Session["srno"] = dsResult.Tables[0].Rows[0]["srno"].ToString();
                            
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
                                strQuery = "select AppFormDocName as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
                                strFolderName = "AppForm";
                                break;
                            case 2:
                                strQuery = "select GCLetterName as docname from appliedgcdocs where APPLICATION_NO='" + applicationId + "' and UpdateDt isnull";
                                strFolderName = "GCFiles";
                                break;
                            case 3:
                                strQuery = "select GCOtherDocName as docname from appliedgcdocs where APPLICATION_NO='" + applicationId + "' and UpdateDt isnull";
                                strFolderName = "GCFiles";
                                break;
                            case 4:
                                strQuery = "select GCExt1 as docname from appliedgcdocs where APPLICATION_NO='" + applicationId + "' and UpdateDt isnull";
                                strFolderName = "GCFiles";
                                break;
                            case 5:
                                strQuery = "select GCExt2 as docname from appliedgcdocs where APPLICATION_NO='" + applicationId + "' and UpdateDt isnull";
                                strFolderName = "GCFiles";
                                break;
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            Session["WF_STATUS_CD_C"] = dsResult.Tables[0].Rows[0]["WF_STATUS_CD_C"].ToString();
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
        protected void btnApplication_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 1);
        }

        protected void btnGCLetter_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 2);
        }

        protected void btnOther_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 3);
        }

        protected void btnExtension1_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 4);
        }

        protected void btnExtension2_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 5);

        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            //Save the File to the Directory (Folder).
            try
            {


                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:

                        string strQuery = "update AppliedGCDocs set remark'" + txtRemark.Text + "', UpdateDt=now() where application_no='" + Session["APPID"].ToString() + " and UpdateDt isnull";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();

                        strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=27,app_status='DOCUMENTS APPROVED.' where APPLICATION_NO='" + Session["APPID"].ToString() + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        //saveApp(Session["ProjectID"].ToString());
                        //Response.Write("<script language='javascript'>window.alert('Documents Uploaded');window.location='UploadGCLetter.aspx';</script>");
                        //lblGCLetter1.Text = "SLD Uploaded Successfully!!";
                        //Response.Redirect("~/UI/Cons/AppHome.aspx", false);

                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + Session["APPID"].ToString() + "','Already GC Documents approved.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        Response.Write("<script language='javascript'>alert('Application verified.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + Session["APPID"].ToString() + "'";
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
                            strBody += "Your documents have been Approved.<br/><br/>";
                            
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
                                Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));
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
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "27");
                        Response.Write("<script language='javascript'>window.alert('Approved Successfully.');window.location='AppHome.aspx';</script>");
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
                lblMessage.Text = "Update Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblMessage.Text = "Update Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            //Save the File to the Directory (Folder).
            try
            {


                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:

                        string strQuery = "update AppliedGCDocs set remark'" + txtRemark.Text + "' where application_no='" + Session["APPID"].ToString() + " and UpdateDt isnull";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();

                        strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=25,app_status='DOCUMENTS REJECTED.' where APPLICATION_NO='" + Session["APPID"].ToString() + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        //saveApp(Session["ProjectID"].ToString());
                        //Response.Write("<script language='javascript'>window.alert('Documents Uploaded');window.location='UploadGCLetter.aspx';</script>");
                        //lblGCLetter1.Text = "SLD Uploaded Successfully!!";
                        //Response.Redirect("~/UI/Cons/AppHome.aspx", false);

                         #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + Session["APPID"].ToString() + "','Already GC Documents Rejected.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        Response.Write("<script language='javascript'>alert('Application Rejected.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + Session["APPID"].ToString() + "'";
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
                            strBody += "Your documents have been Rejected.Kindly upload again.<br/><br/>";
                            
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
                                Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));
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
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "25");

                        Response.Write("<script language='javascript'>window.alert('Approved Successfully.');window.location='AppHome.aspx';</script>");
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
                lblMessage.Text = "Update Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblMessage.Text = "Update Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }
        }
    }
}