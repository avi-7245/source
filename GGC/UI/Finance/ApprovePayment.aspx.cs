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
using GGC.Scheduler;

namespace GGC.UI.Finance
{

    public partial class ApprovePayment : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ApprovePayment));
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {

                fillGrid();

            }
        }

        protected void fillGrid()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;

                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        //last query//strQuery = "SELECT a.*,b.*,c.ORGANIZATION_NAME FROM applicantdetails a, billdesk_txn b,applicant_reg_det c WHERE a.APPLICATION_NO=b.APPLICATIONNO and b.IsPayMentApproveFin is null and b.TxnNo is not null and a.USER_NAME=c.USER_NAME";
                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, billdesk_txn b WHERE a.APPLICATION_NO=b.APPLICATIONNO and b.IsPayMentApproveFin is null and TxnNo is not null ";
                        strQuery = "SELECT a.APPLICATION_NO,a.MEDAProjectID,b.MerchantID,b.CustomerID,b.TxnAmount,b.TxnDate,b.TxnNo,b.typeofpay,c.ORGANIZATION_NAME FROM applicantdetails a, billdesk_txn b,applicant_reg_det c WHERE a.APPLICATION_NO=b.APPLICATIONNO and b.IsPayMentApproveFin is null and b.TxnNo is not null and a.USER_NAME=c.USER_NAME";

                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //log.Error("dsResult.Tables[0].Rows[0][custId] " + dsResult.Tables[0].Rows[0]["CustomerID"].ToString());
                        //log.Error("dsResult.Tables[0].Rows[0][custId] " + dsResult.Tables[0].Rows[1]["CustomerID"].ToString());

                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            log.Error("GVPayments 1");
                            GVPayments.DataSource = dsResult.Tables[0];
                            GVPayments.DataBind();
                            log.Error("GVPayments 2");
                        }
                        else
                        {
                            GVPayments.DataSource = null;
                            GVPayments.DataBind();
                            //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
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
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
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

        protected void GVPayments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPayments.Rows[rowIndex];

            string applicationId = row.Cells[0].Text;
            string projId = row.Cells[1].Text;
            string custId = row.Cells[3].Text;
            string typeofpay = row.Cells[8].Text;
            log.Error("typeofpay" + typeofpay);
            if (e.CommandName == "Approve")
            {
                updateApproval(applicationId,projId, custId, typeofpay);
            }
            if (e.CommandName == "Decline")
            {
                updateDecline(applicationId, projId, custId, typeofpay);
            }
        }

        protected void updateApproval(string applicationId, string projId, string custId, string typeofpay)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            
            
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;

                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "update billdesk_txn set IsPayMentApproveFin='Y' ,PaymentApproveByFin='" + Session["SAPID"].ToString() + "', FinancePaymentAppr_DT=now() where ApplicationNo='" + applicationId + "' and CustomerID='" + custId + "'";


                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        
                        cmd.ExecuteNonQuery();

                        if (typeofpay == "Registration")
                        {
                            strQuery = "Update APPLICANTDETAILS set  WF_STATUS_CD_C=6, app_status='REGISTRATION FEES APPROVED BY FINANCE. FORM VERIFICATION PENDING.' , APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            strQuery = "insert into APPLICANT_STATUS_TRACKING values('" + applicationId + "','REGISTRATION FEES APPROVED BY FINANCE. FORM VERIFICATION PENDING.',now(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            objSaveToMEDA.saveApp(projId, "5");
                            fillGrid();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Payment approved.');window.location ='ApprovePayment.aspx';", true);

                        }
                        else
                        {
                            strQuery = "Update APPLICANTDETAILS set  WF_STATUS_CD_C=17, app_status='COMMITTMENT FEES APPROVED BY FINANCE.' , APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            strQuery = "insert into APPLICANT_STATUS_TRACKING values('" + applicationId + "','COMMITTMENT FEES APPROVED BY FINANCE.',now(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            objSaveToMEDA.saveApp(projId, "17");
                            fillGrid();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Payment approved.');window.location ='ApprovePayment.aspx';", true);
                            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no='" + applicationId + "'");
                            DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,31)");
                            #region Send SMS
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
                                //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Load%20Flow%20studies%20are%20completed%20for%20your%20application%20no." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".%20The%20proposal%20for%20Grid%20Connectivity%20is%20under%20approval.%20Regards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Payment%20towards%20Commitment%20fee%20has%20been%20approved%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20Project%20proposed%20by%20M%2Fs." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
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
                        }

                        
                        strQuery = "select distinct a.* from APPLICANTDETAILS a where a.application_no='" + applicationId + "'";

                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
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
                            strBody += "<b>Your payment has been approved.</b> <br/>  <br/><br/><br/>";
                            //strBody += "<b>Note : Kindly upload scan copy of your application form with duly sign and stamp.</b> <br/>  <br/><br/><br/>";
                            
                            strBody += "Thanks & Regards, " + "<br/>";
                            //strBody += " Chief Engineer" + "<br/>";
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
                                //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                string strCC = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                                if (strCC != "")
                                {
                                    string[] splittedCC = strCC.Split(';');
                                    foreach (var part in strCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        if (part != "")
                                            Msg.CC.Add(new MailAddress(part.ToString()));
                                        //Msg.CC.Add(new MailAddress(part.ToString()));
                                        log.Error("Part " + part);
                                    }
                                }

                                //  Msg.To.Add(new MailAddress(toAddress));

                                Msg.Subject = "Payment approved in Online Grid connectivity application.";
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

                        fillGrid();

                        Response.Write("<script language='javascript'>alert('Payment Approved.');</script>");


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
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
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

        protected void updateDecline(string applicationId, string projId, string custId, string typeofpay)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;

                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "update billdesk_txn set IsPayMentApproveFin='N' ,PaymentApproveByFin='" + Session["SAPID"].ToString() + "' where ApplicationNo='" + applicationId + "' and CustomerID='" + custId + "'";


                        cmd = new MySqlCommand(strQuery, mySqlConnection);

                        cmd.ExecuteNonQuery();

                        if (typeofpay == "Registration")
                        {
                            strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=4, app_status='Payment declined by finance.' , APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            objSaveToMEDA.saveApp(projId, "4");
                            fillGrid();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Payment rejected.');window.location ='ApprovePayment.aspx';", true);
                        }
                        else
                        {
                            strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=15, app_status='Payment declined by finance.' , APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            objSaveToMEDA.saveApp(projId, "15");
                            fillGrid();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Payment rejected.');window.location ='ApprovePayment.aspx';", true);
                        }

                        

                        strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";

                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
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
                            strBody += "Your payment has been declined.</b> <br/>  <br/><br/><br/>";

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
                                //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                //  Msg.To.Add(new MailAddress(toAddress));

                                Msg.Subject = "Payment declined in Online Grid connectivity application.";
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

                        fillGrid();


                        Response.Write("<script language='javascript'>alert('Payment Rejected.');</script>");
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
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
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