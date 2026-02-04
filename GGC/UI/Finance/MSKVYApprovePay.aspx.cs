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
using Microsoft.Reporting.WebForms;

namespace GGC.UI.Finance
{
    public partial class MSKVYApprovePay : BasePage
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

                        //NeedToCheck
                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        //last query//strQuery = "SELECT a.*,b.*,c.ORGANIZATION_NAME FROM applicantdetails a, billdesk_txn b,applicant_reg_det c WHERE a.APPLICATION_NO=b.APPLICATIONNO and b.IsPayMentApproveFin is null and b.TxnNo is not null and a.USER_NAME=c.USER_NAME";
                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, billdesk_txn b WHERE a.APPLICATION_NO=b.APPLICATIONNO and b.IsPayMentApproveFin is null and TxnNo is not null ";
                        strQuery = "SELECT a.APPLICATION_NO,a.MEDAProjectID,b.MerchantID,b.CustomerID,b.TxnAmount,b.TxnDate,b.TxnNo,b.typeofpay,c.ORGANIZATION_NAME FROM MSKVY_applicantdetails a, billdesk_txn b,applicant_reg_det c WHERE a.APPLICATION_NO=b.APPLICATIONNO and b.IsPayMentApproveFin is null and b.AuthStatus='0300' and a.USER_NAME=c.USER_NAME and typeofpay='MSKVYRegistration'";

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
                updateApproval(applicationId, projId, custId, typeofpay);
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

                        //billdesk_txn
                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "update billdesk_txn set IsPayMentApproveFin='Y' ,PaymentApproveByFin='" + Session["SAPID"].ToString() + "',FinancePaymentAppr_DT=now() where ApplicationNo='" + applicationId + "' and CustomerID='" + custId + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();


                        #region PDF creation
                        strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F  from mskvy_applicantdetails where APPLICATION_NO='" + applicationId + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        cmd = new MySqlCommand("select * from applicant_reg_det where USER_NAME='" + dsResult.Tables[0].Rows[0]["USER_NAME"].ToString() + "'", mySqlConnection);
                        DataSet dsRegDet = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsRegDet);
                        dsRegDet.Tables[0].TableName = "AppRegDet";
                        dsResult.Tables.Add(dsRegDet.Tables[0].Copy());

                        strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and AuthStatus='0300' and typeofpay='MSKVYRegistration'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsReceipt = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsReceipt);
                        dsReceipt.Tables[0].TableName = "TableReceipt";
                        dsResult.Tables.Add(dsReceipt.Tables[0].Copy());


                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                            string FileName = "ApplicationForm_" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                            string serverFilePath;
                            if (!Directory.Exists(Server.MapPath("~/Files/MSKVY/" + applicationId + "/")))
                            {
                                //If Directory (Folder) does not exists Create it.
                                Directory.CreateDirectory(Server.MapPath("~/Files/MSKVY/" + applicationId + "/"));
                            }
                            try
                            {
                                serverFilePath = Server.MapPath("~/Files/MSKVY/" + applicationId + "/") + FileName;
                                LocalReport report = new LocalReport();
                                report.ReportPath = Server.MapPath("~/PDFReport/") + "Application_MSKVY.rdlc";

                                ReportDataSource rds = new ReportDataSource();
                                rds.Name = "dsApplication_AppDet";//This refers to the dataset name in the RDLC file
                                rds.Value = dsResult.Tables[0];
                                report.DataSources.Add(rds);

                                ReportDataSource rds2 = new ReportDataSource();
                                rds2.Name = "dsApplication_AppRegDet";//This refers to the dataset name in the RDLC file
                                rds2.Value = dsResult.Tables["AppRegDet"];
                                report.DataSources.Add(rds2);

                                ReportDataSource rds3 = new ReportDataSource();
                                rds3.Name = "DataSetReceipt";//This refers to the dataset name in the RDLC file
                                rds3.Value = dsResult.Tables["TableReceipt"];
                                report.DataSources.Add(rds3);

                                //ReportDataSource rds3 = new ReportDataSource();
                                //rds3.Name = "DataSet3";//This refers to the dataset name in the RDLC file
                                //rds3.Value = dsResult.Tables["Table3"];
                                //report.DataSources.Add(rds3);

                                //ReportDataSource rds4 = new ReportDataSource();
                                //rds4.Name = "DataSet4";//This refers to the dataset name in the RDLC file
                                //rds4.Value = dsResult.Tables["Table4"];
                                //report.DataSources.Add(rds4);

                                ReportViewer reportViewer = new ReportViewer();
                                reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "Application_MSKVY.rdlc";
                                reportViewer.LocalReport.DataSources.Clear();

                                reportViewer.LocalReport.DataSources.Add(rds);
                                reportViewer.LocalReport.DataSources.Add(rds2);
                                reportViewer.LocalReport.DataSources.Add(rds3);

                                // reportViewer.LocalReport.DataSources.Add(rds3);


                                reportViewer.LocalReport.Refresh();


                                string mimeType, encoding, extension, deviceInfo;
                                string[] streamids;
                                Warning[] warnings;
                                string format = "PDF";

                                deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                                byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                                FileStream fs1 = new FileStream(serverFilePath, FileMode.Create);
                                fs1.Write(bytes, 0, bytes.Length);
                                fs1.Close();

                                //Save File Name mskvy_upload_doc_spd
                                strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + applicationId + "', 1, '" + FileName + "',  '" + applicationId + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();


                            }
                            catch (Exception ex)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                log.Error(ErrorMessage);

                            }
                        }
                        #endregion


                        #region Save To MEDA
                        if (typeofpay == "MSKVYRegistration")
                        {
                            strQuery = "Update MSKVY_applicantdetails  set  WF_STATUS_CD_C=10, app_status='REGISTRATION FEES APPROVED BY FINANCE. LFS Pending.' , APP_STATUS_DT=CURDATE(),IsPaymentDone='Y' where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            strQuery = "insert into APPLICANT_STATUS_TRACKING(APPLICATION_NO,STATUS,STATUS_DT,Created_By) values('" + applicationId + "','REGISTRATION FEES APPROVED BY FINANCE.UPLOAD LFS Pending.',now(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            objSaveToMEDA.saveApp(projId, "6");
                            //RemoveComment
                        }
                        else
                        {
                            strQuery = "Update MSKVY_applicantdetails  set  WF_STATUS_CD_C=17, app_status='COMMITTMENT FEES APPROVED BY FINANCE.' , APP_STATUS_DT=CURDATE(),IsPaymentDone='Y' where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            strQuery = "insert into APPLICANT_STATUS_TRACKING(APPLICATION_NO,STATUS,STATUS_DT,Created_By) values('" + applicationId + "','COMMITTMENT FEES APPROVED BY FINANCE.',now(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            objSaveToMEDA.saveApp(projId, "17");
                            //RemoveComment
                        }
                        #endregion Save To MEDA


                        #region Send Notification

                        strQuery = "select distinct a.* from MSKVY_applicantdetails  a where a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();

                        var notify = GetNotificationData();

                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW<br/>";
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


                                notify.EmailIds.ForEach(email =>
                                {
                                    Msg.To.Add(new MailAddress(email));
                                });


                                //  Msg.To.Add(new MailAddress(toAddress));

                                Msg.Subject = "Payment approved in Online Deemed Grid connectivity application.";
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

                            #region Send SMS


                            string stuMobileNumber =  dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                            try
                            {
                                if (!string.IsNullOrEmpty(stuMobileNumber))
                                {
                                    notify.MobileNumbers.Add(stuMobileNumber);
                                }

                                SMS.Send(message: SMSTemplates.ProcessingApprovedForDeemGC(dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), notify.MobileNumbers.JoinStrings(), MethodBase.GetCurrentMethod(), log);

                                //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Load%20Flow%20studies%20are%20completed%20for%20your%20application%20no." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".%20The%20proposal%20for%20Grid%20Connectivity%20is%20under%20approval.%20Regards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Payment%20towards%20Commitment%20fee%20has%20been%20approved%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20Project%20proposed%20by%20M%2Fs." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                //AddedComment 345

                                //log.Error("strURL " + strURL);
                                //WebRequest request = HttpWebRequest.Create(strURL);
                                //log.Error("2 ");
                                // Get the response back  
                                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                //Stream s = (Stream)response.GetResponseStream();
                                //StreamReader readStream = new StreamReader(s);
                                //string dataString = readStream.ReadToEnd();
                                //log.Error("8 " + dataString);

                                //response.Close();
                                //s.Close();
                                //readStream.Close();
                            }
                            catch (Exception ex)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                log.Error(ErrorMessage);
                            }
                            #endregion

                        }

                        #endregion Send Notification


                        fillGrid();

                        Response.Write("<script language='javascript'>alert('Payment Approved.');</script>");


                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

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

                        //strQuery = "SELECT a.*,b.* FROM MSKVY_applicantdetails  a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "update billdesk_txn set IsPayMentApproveFin='N' ,PaymentApproveByFin='" + Session["SAPID"].ToString() + "' where ApplicationNo='" + applicationId + "' and CustomerID='" + custId + "'";


                        cmd = new MySqlCommand(strQuery, mySqlConnection);

                        cmd.ExecuteNonQuery();

                        //Save To MEDA
                        if (typeofpay == "Registration")
                        {
                            strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=4, app_status='Payment declined by finance.' , APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            objSaveToMEDA.saveApp(projId, "4");
                            //RemoveComment
                        }
                        else
                        {
                            strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=15, app_status='Payment declined by finance.' , APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            objSaveToMEDA.saveApp(projId, "15");
                            //RemoveComment
                        }



                        strQuery = "select distinct a.* from MSKVY_applicantdetails a  where a.application_no='" + applicationId + "'";

                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        var notify = GetNotificationData(false);

                        

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


                                notify.EmailIds?.ForEach(email =>
                                {
                                    Msg.To.Add(new MailAddress(email));
                                });


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