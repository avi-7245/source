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
using GGC.Scheduler;

namespace GGC.UI.MSKVY
{
    public partial class MSKVYPayRegReceipt : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSKVYPayRegReceipt));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                #region Returned from Payment Gatway 

                string[] keys = Request.Form.AllKeys;
                //for (int i = 0; i < keys.Length; i++)
                //{
                //     Label1.Text += keys[i] + ": " + Request.Form[keys[i]];
                //}
                string[] strParams = Request.Form[keys[0]].Split('|');
                int len = strParams.Length - 1;
                string strBillDesk_Param = string.Empty;
                for (int i = 0; i < len; i++)
                {
                    if (i <= len - 2)
                        strBillDesk_Param = strBillDesk_Param + strParams[i] + "|";
                    else
                        strBillDesk_Param = strBillDesk_Param + strParams[i];

                }
                // strBillDesk_Param.Remove(strBillDesk_Param.LastIndexOf('|'));
                //lblRegID.Text=words[0]
                //lblName.Text = strParams[16];
                lblRegID.Text = strParams[17];
                String data = Request.Form[keys[0]];
                string checkSumKey = ConfigurationManager.AppSettings["ChecksumKey"].ToString();
                PaymentGateway dataprg = new PaymentGateway();
                String hash = String.Empty;
                hash = dataprg.GetHMACSHA256(strBillDesk_Param, checkSumKey).ToUpper();

                #endregion


                //For Test AddToComment
                //string[] strParams = "4MSETCLRC|339|YHMP2233292761|1|NA|NA|NA|INR|NA|R|msetclrc|NA|NA|{DateTime.Today:yyyy-MM-dd}|0300|F|8820700088|8820700088|MSKVYRegistration|NA|NA|NA|NA|https://gctest.mahatransco.in:9999/UI/MSKVY/MSKVYPayRegReceipt.aspx||".Split('|');
                //string hash = "";

                if (hash == strParams[25])
                {
                    if (strParams[14] == "0300")
                    {
                        MySqlConnection mySqlConnection = new MySqlConnection();
                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                        string strPostName = string.Empty;

                        try
                        {

                            mySqlConnection.Open();


                            switch (mySqlConnection.State)
                            {

                                case System.Data.ConnectionState.Open:

                                    //Update BillDesk_TXN
                                    string strQuery = "update BillDesk_TXN set TxnNo= " +
                                            "'" + strParams[2] + "',TxnDate='" + strParams[13] + "',AuthStatus='" + strParams[14] + "',ORIGINALSTATUS='2' ,UpdatedDt='" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "' where ApplicationNo='" + strParams[17] + "' and CustomerID='" + strParams[1] + "'";
                                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);

                                    //Update MSKVY_applicantdetails
                                    cmd.ExecuteNonQuery();
                                    strQuery = "update MSKVY_applicantdetails  set WF_STATUS_CD_C=5,IsPaymentDone='Y', paymentdate=CURDATE(), APP_STATUS_DT=CURDATE() ,app_status='REGISTRATION PAYMENT DONE.PAYMENT APPROVAL PENDING.' ,  CustomerID='" + strParams[1] + "' where Application_No='" + strParams[17] + "'";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();

                                    //Insert APPLICANT_STATUS_TRACKING
                                    #region Application Status tracking date
                                    strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                            " values('" + strParams[17] + "','PAYMENT DONE.DOCUMENT VERIFICATION PENDING.',now(),'" + strParams[17] + "')";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    #endregion

                                    //Set Payment successfull 
                                    lblTransNo.Text = strParams[2];
                                    lblTransDt.Text = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                                    lblStatus.Text = "Payment successfull.";

                                    //strQuery = "select distinct b.email Email from MSKVY_applicantdetails  a, zone_district b where lower(a.PROJECT_DISTRICT)=lower(b.district) and lower(b.DISTRICT)=(select distinct district from MSKVY_applicantdetails  where application_no='" + strParams[17] + "')";
                                    //cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    //DataSet dsResult = new DataSet();
                                    //MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                    //da.Fill(dsResult);

                                    //strQuery = "select distinct a.*,b.*,c.email Zone_email from MSKVY_applicantdetails  a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + strParams[17] + "'";
                                    strQuery = "select distinct a.*  from MSKVY_applicantdetails  a where a.application_no='" + strParams[17] + "'";
                                    MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                                    DataSet dsAppDet = new DataSet();
                                    MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                                    daAppDet.Fill(dsAppDet);
                                    Session["user_name"] = dsAppDet.Tables[0].Rows[0]["user_name"].ToString();
                                    //string strEmail = dsResult.Tables[0].Rows[0]["email"].ToString();
                                    string strMobile = dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();

                                    #region Save To MEDA
                                    saveApp(dsAppDet.Tables[0].Rows[0]["MEDAProjectID"].ToString(), strParams[13]);
                                    #endregion Save To MEDA
                                    //RemoveComment 

                                    //NeedToCheck


                                    var emailIds = new List<string>();
                                    var mobileNumbers = new List<string>();

                                    DataSet dsSNotifyEmp = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"SELECT e.SRNO, e.SAPID, e.EMP_NAME, e.EmpEmailID, e.EmpMobile FROM empmaster e WHERE e.ROLE_ID IN ({RoleConst.STU},{RoleConst.FD});");

                                    mobileNumbers.AddRange(dsSNotifyEmp.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpMobile"].ToString())).Select(a => a["EmpMobile"].ToString().Trim()).Distinct());
                                    emailIds.AddRange(dsSNotifyEmp.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpEmailID"].ToString())).Select(a => a["EmpEmailID"].ToString().Trim().ToLower()).Distinct());


                                    if (dsAppDet.Tables[0].Rows.Count > 0)
                                    {
                                        #region Send Mail
                                        //sendMailOTP(strRegistrationno, strEmailID);
                                        string strBody = string.Empty;

                                        strBody += "Respected Sir/Madam" + ",<br/>";
                                        strBody += "Following developer applied and  paid application Fees for Deemed Grid Connectivity Application." + "<br/>";
                                        strBody += "Application ID for further reference is : " + strParams[17] + "<br/>";
                                        strBody += "Project ID : " + dsAppDet.Tables[0].Rows[0]["MEDAProjectID"].ToString() + "<br/>";
                                        strBody += "Developer / Promoter Name : " + dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "<br/>";
                                        strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                                        strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                                        strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                                        strBody += "Project Location : " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp; " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                                        //strBody += "Please use following information for login for further process. <br/>";
                                        strBody += "Amount paid : Rs." + strParams[4] + "<br/>";
                                        strBody += "Date and Time of Payment : " + strParams[13] + "<br/>";
                                        strBody += "<br/>";
                                        strBody += "Thanks & Regards, " + "<br/>";
                                        strBody += "STU Department" + "<br/>";
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
                                            //Msg.To.Add(new MailAddress(strEmail));

                                            //  Msg.To.Add(new MailAddress(toAddress));
                                            string strTo = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                                            if (strTo != "")
                                            {
                                                string[] splittedCC = strTo.Split(';');
                                                foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                                {
                                                    if (part != "")
                                                        Msg.To.Add(new MailAddress(part.ToString()));
                                                    //Msg.CC.Add(new MailAddress(part.ToString()));
                                                    log.Error("Part " + part);
                                                }
                                            }
                                            //Msg.To.Add(new MailAddress(strFinanceReporting.ToString())); 
                                            //Commented 

                                            emailIds?.ForEach(email =>
                                            {
                                                Msg.To.Add(new MailAddress(email));
                                            });

                                            Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                            //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                            Msg.Subject = "Online Deemed Grid connectivity application fees payment.";
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

                                        mobileNumbers.Add(strMobile);

                                        SMS.Send(message: SMSTemplates.ProcessingFeePaidForDeemGC(applicationNo: strParams[17], spvName: dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), mobileNumbers.JoinStrings(), MethodBase.GetCurrentMethod(), log);

                                        #endregion
                                    }


                                    //#region Send Mail

                                    //DataSet dsResultFinance = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id=31");
                                    //string strFinance, strFinanceReporting;
                                    //strFinance = dsResultFinance.Tables[0].Rows[0]["EmpEmailID"].ToString();
                                    //strFinanceReporting = dsResultFinance.Tables[0].Rows[0]["EmpReportingEmail"].ToString();
                                    //if (dsAppDet.Tables[0].Rows.Count > 0)
                                    //{
                                    //    //sendMailOTP(strRegistrationno, strEmailID);
                                    //    string strBody = string.Empty;

                                    //    strBody += "Respected Sir/Madam" + ",<br/>";
                                    //    strBody += "Following developer applied and  paid application Fees for Grid Connectivity Application." + "<br/>";
                                    //    strBody += "Application ID for further reference is : " + strParams[17] + "<br/>";
                                    //    strBody += "Project ID : " + dsAppDet.Tables[0].Rows[0]["MEDAProjectID"].ToString() + "<br/>";
                                    //    strBody += "Developer / Promoter Name : " + dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "<br/>";
                                    //    strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                                    //    strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                                    //    strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                                    //    strBody += "Project Location : " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp; " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                                    //    //strBody += "Please use following information for login for further process. <br/>";
                                    //    strBody += "Amount paid : Rs." + strParams[4] + "<br/>";
                                    //    strBody += "Date and Time of Payment : " + strParams[13] + "<br/>";
                                    //    strBody += "<br/>";
                                    //    strBody += "Thanks & Regards, " + "<br/>";
                                    //    strBody += "STU Department" + "<br/>";
                                    //    strBody += "MSETCL  " + "<br/>";
                                    //    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                                    //    //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
                                    //    try
                                    //    {


                                    //        #region using MailMessage
                                    //        MailMessage Msg = new MailMessage();
                                    //        MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                                    //        Msg.From = fromMail;
                                    //        Msg.IsBodyHtml = true;
                                    //        //log.Error("from:" + fromAddress);
                                    //        //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                                    //        //Msg.To.Add(new MailAddress(strEmail));

                                    //        //  Msg.To.Add(new MailAddress(toAddress));
                                    //        string strTo = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                                    //        if (strTo != "")
                                    //        {
                                    //            string[] splittedCC = strTo.Split(';');
                                    //            foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                    //            {
                                    //                if (part != "")
                                    //                    Msg.To.Add(new MailAddress(part.ToString()));
                                    //                //Msg.CC.Add(new MailAddress(part.ToString()));
                                    //                log.Error("Part " + part);
                                    //            }
                                    //        }
                                    //        Msg.To.Add(new MailAddress(strFinance.ToString()));

                                    //        //Msg.To.Add(new MailAddress(strFinanceReporting.ToString())); 
                                    //        //Commented 

                                    //        Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                    //        //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                    //        Msg.Subject = "Online Grid connectivity application fees payment.";
                                    //        Msg.Body = strBody;
                                    //        //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                    //        SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                                    //        a.EnableSsl = true;
                                    //        NetworkCredential n = new NetworkCredential();
                                    //        n.UserName = "donotreply@mahatransco.in";
                                    //        n.Password = "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~";
                                    //        a.UseDefaultCredentials = false;
                                    //        a.Credentials = n;
                                    //        a.Port = 587;
                                    //        System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                                    //        a.Send(Msg);

                                    //        Msg = null;
                                    //        fromMail = null;
                                    //        a = null;
                                    //        #endregion
                                    //    }
                                    //    catch (Exception ex)
                                    //    {
                                    //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                    //        log.Error(ErrorMessage);
                                    //        // throw ex;
                                    //    }


                                    //}
                                    //#endregion

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

                        catch (MySql.Data.MySqlClient.MySqlException ex)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                            log.Error(ErrorMessage);
                        }

                        catch (Exception ex)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                            log.Error(ErrorMessage);
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
                    else
                    {
                        lblTransNo.Text = strParams[2];
                        lblTransDt.Text = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                        lblStatus.Text = "Payment failed.";
                    }
                }
            }
        }


        public string saveApp(string projId, string txnDt)
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
                        strQuery = "select * from mskvy_applicantdetails where MEDAProjectID='" + projId + "'";
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
                        objGetStatusDTO.subProjID = dsResult.Tables[0].Rows[0]["sub_project_code"].ToString();
                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.PAYMENT_TYPE_ID = 1;
                        objGetStatusDTO.PAYMENT_TYPE = "MSKVYRegistration";
                        //objGetStatusDTO.Txn_Date = txnDt;
                        objGetStatusDTO.statusId = "4";

                        strQuery = "SELECT TxnNo,TxnAmount,TxnAmount FROM billdesk_txn WHERE MEDAProjectID='" + projId + "' and AuthStatus='0300' and typeofpay='MSKVYRegistration'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        dsResult = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //txtEmail1.Text = dsResult.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
                        objGetStatusDTO.Txn_No = dsResult.Tables[0].Rows[0]["TxnNo"].ToString();
                        objGetStatusDTO.Txn_Amount = dsResult.Tables[0].Rows[0]["TxnAmount"].ToString();
                        objGetStatusDTO.Txn_Date = dsResult.Tables[0].Rows[0]["TxnDate"].ToString();

                        APICall apiCall = new APICall();
                        string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + Session["user_name"].ToString() + "&projId=" + projId + "&entity=MSETCL");

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
    }
}