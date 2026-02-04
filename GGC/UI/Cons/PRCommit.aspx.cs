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

namespace GGC.UI.Cons
{
    public partial class PRCommit : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(PRCommit));


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
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

                                    string strQuery = "update BillDesk_TXN set TxnNo= " +
                                            "'" + strParams[2] + "',TxnDate='" + strParams[13] + "',AuthStatus='" + strParams[14] + "',ORIGINALSTATUS='2' ,UpdatedDt='" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "' where ApplicationNo='" + strParams[17] + "' and CustomerID='" + strParams[1] + "'";
                                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    strQuery = "update APPLICANTDETAILS set WF_STATUS_CD_C=16,IsPaymentDone='Y', paymentdate=CURDATE(), APP_STATUS_DT=CURDATE() ,app_status='COMMITTMENT FEES PAID.PAYMENT APPROVAL PENDING.' ,  CustomerID='" + strParams[1] + "' where Application_No='" + strParams[17] + "'";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();

                                    
                                    #region Application Status tracking date
                                    strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                            " values('" + strParams[17] + "','COMMITTMENT FEES PAID.PAYMENT APPROVAL PENDING.',CURDATE(),'" + strParams[17] + "')";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    #endregion

                                    lblTransNo.Text = strParams[2];
                                    lblTransDt.Text = DateTime.Now.ToString("yyyy'-'MM'-'dd' 'HH':'mm':'ss");
                                    lblStatus.Text = "Payment successfull.";
                                    strQuery = "select * from empmaster where role_id in (2,51,52,31)";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    DataSet dsResult = new DataSet();
                                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                    da.Fill(dsResult);

                                    //strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + strParams[17] + "'";
                                    strQuery = "select distinct a.*,b.*  from APPLICANTDETAILS a,APPLICANT_REG_DET b where a.GSTIN_NO=b.GSTIN_NO and a.application_no='" + strParams[17] + "'";
                                    MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                                    DataSet dsAppDet = new DataSet();
                                    MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                                    daAppDet.Fill(dsAppDet);
                                    Session["user_name"] = dsAppDet.Tables[0].Rows[0]["user_name"].ToString();
                                    string strEmail = dsResult.Tables[0].Rows[0]["EmpEmailID"].ToString();
                                    saveApp(dsAppDet.Tables[0].Rows[0]["MEDAProjectID"].ToString(), strParams[13]);
                                    if (dsAppDet.Tables[0].Rows.Count > 0)
                                    {
                                        #region Send Mail
                                        //sendMailOTP(strRegistrationno, strEmailID);
                                        string strBody = string.Empty;

                                        strBody += "Respected Sir/Madam" + ",<br/>";
                                        strBody += "Following Developer paid Committment Fees for Grid Connectivity Application." + "<br/>";
                                        strBody += "Application ID for further reference is : " + strParams[17] + "<br/>";
                                        strBody += "Project ID : " + dsAppDet.Tables[0].Rows[0]["MEDAProjectID"].ToString() + "<br/>";
                                        strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                                        strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                                        strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                                        strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                                        strBody += "Project Location : " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp; " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                                        //strBody += "Please use following information for login for further process. <br/>";
                                        strBody += "Amount paid : Rs." + strParams[4] + "<br/>";
                                        strBody += "Date and Time of Payment : " + strParams[13] + "<br/>";
                                        strBody += "<br/>";
                                        strBody += "Thanks & Regards, " + "<br/>";
                                        strBody += "Chief Engineer / STU Department" + "<br/>";
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
                                        string strMobNos = string.Empty;
                                        strMobNos = dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + ",";
                                        try
                                        {
                                            //log.Error("1 ");
                                            if (dsResult.Tables[0].Rows.Count > 0)
                                            {



                                                if (dsResult.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                                                    {
                                                        if (dsResult.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                                                            strMobNos += dsResult.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                                    }

                                                }
                                            }
                                            string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Payment%20towards%20Commitment%20fee%20has%20been%20received%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20Project%20proposed%20by%20M%2Fs." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
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

        public string saveApp(string projId,string txnDate)
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
                        strQuery = "select * from applicantdetails where MEDAProjectID='" + projId + "'";
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
                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.PAYMENT_TYPE_ID = 2;
                        objGetStatusDTO.PAYMENT_TYPE = "Committment";
                        objGetStatusDTO.Txn_Date = txnDate;
                        objGetStatusDTO.statusId = "16";
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