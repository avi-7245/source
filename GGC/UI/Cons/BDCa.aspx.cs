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
using Newtonsoft.Json;
namespace GGC.UI.Cons
{
    public partial class BDCa : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(PayConfirm));

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Send Mail
            //EmailModel em = new EmailModel();
            //em.MailTo = "progit4000@mahatransco.in;";
            //em.MailCC = "sait4000@mahatransco.in;";
            //em.MailSubject = "Testing Email";
            //em.MailBody = "Testing";
            //em.MailAttachment = "";

            //try
            //{


            //    #region using MailMessage
            //    MailMessage Msg = new MailMessage();
            //    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
            //    Msg.From = fromMail;
            //    Msg.IsBodyHtml = true;
            //    //log.Error("from:" + fromAddress);
            //    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
            //    Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
            //    Msg.CC.Add(new MailAddress("sait4000@mahatransco.in"));
            //    //Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
            //    //Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));

            //    //  Msg.To.Add(new MailAddress(toAddress));

            //    Msg.Subject = "Testing";
            //    Msg.Body = "Testing";
            //    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
            //    SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
            //    a.Port = 587;
            //    log.Error("Before send mail");
            //    a.Send(Msg);
            //    log.Error("After send mail");
            //    Msg = null;
            //    fromMail = null;
            //    a = null;
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
            //    log.Error(ErrorMessage);
            //    // throw ex;
            //}

            //try
            //{
            //    //                                string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projid=" + txtProjCode + "&entity=MSETCL&secKey=" + str;
            //    //string userAuthenticationURI = "https://regrid.mahadiscom.in/reGrid/saveToMedaGcAppln";
            //    string userAuthenticationURI = "http://localhost:51233/api/values/SendMail";

            //    //WebRequest req = WebRequest.Create(@userAuthenticationURI);
            //    var req = (HttpWebRequest)WebRequest.Create(@userAuthenticationURI);


            //    req.Method = "POST";
            //    req.ContentType = "application/json";
            //    //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
            //    //req.Credentials = new NetworkCredential("username", "password");
            //    using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            //    {
            //        string json = JsonConvert.SerializeObject(em, Formatting.None);

            //        streamWriter.Write(JsonConvert.SerializeObject(em, Formatting.None));
            //    }
            //    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            //    StreamReader reader = new StreamReader(resp.GetResponseStream());

            //    string responseText = reader.ReadToEnd();
            //}
            //catch (Exception ex)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
            //    log.Error(ErrorMessage);
            //}
            #endregion
            #region TestBilldesk
            string applicationId = "";
            string strProjectID = string.Empty;
            //string PAN = ViewState["payPAN"].ToString();
            //string strName = ViewState["payName"].ToString();

            string strMerchantCode = ConfigurationManager.AppSettings["MerchantID"].ToString();
            string strCustID = "";
            //string strCustomerID = "1";
            long strSrno = 0;
            string strPayAmt;
            string strCurrencyType = ConfigurationManager.AppSettings["CurrencyType"].ToString();
            string strSecurityID = ConfigurationManager.AppSettings["SecurityID"].ToString();
            string checkSumKey = ConfigurationManager.AppSettings["ChecksumKey"].ToString();
            string strPGURL = ConfigurationManager.AppSettings["PGURL"].ToString();
            string strRU = ConfigurationManager.AppSettings["RU"].ToString();
            string strCon = ConfigurationManager.AppSettings["ConventionalFees"].ToString();
            string strNonCon = ConfigurationManager.AppSettings["NonConventionalFees"].ToString();
            //log.Error("Session[AppType] " + Session["AppType"].ToString());


            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strPostName = string.Empty;

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        System.Globalization.DateTimeStyles style = DateTimeStyles.None;
                        DateTime date1;
                        DateTime.TryParseExact(DateTime.Now.ToString(), "yyyy-MM-dd", provider, style, out date1);
                        //date1 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                        string yyyy, mm, dd;
                        yyyy = DateTime.Now.AddDays(-20).Year.ToString();
                        mm = DateTime.Now.AddDays(-20).Month.ToString();
                        dd = DateTime.Now.AddDays(-20).Day.ToString();
                        MySqlCommand cmd = new MySqlCommand("select * from BillDesk_TXN where CreatedDt<=DATE_SUB('" + yyyy + "-" + mm + "-" + dd + "',INTERVAL 10 day) and AuthStatus is not null and typeofpay='Registration'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        applicationId = dsResult.Tables[0].Rows[10]["ApplicationNo"].ToString();

                        cmd = new MySqlCommand("select * from applicantdetails where Application_No='" + applicationId + "'", mySqlConnection);
                        DataSet dsResultCommFees = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResultCommFees);
                        string strPROJECT_TYPE = dsResultCommFees.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                        strProjectID = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                        //if (strPROJECT_TYPE == "Solar Project" || strPROJECT_TYPE == "Wind" || strPROJECT_TYPE == "Solar Park" || strPROJECT_TYPE == "Hybrid")
                        //{
                        strPayAmt = dsResult.Tables[0].Rows[0]["TxnAmount"].ToString();
                        //}
                        int cnt = dsResult.Tables[0].Rows.Count;
                        if (cnt >= 1)
                        {

                            strCustID = dsResult.Tables[0].Rows[0]["CustomerID"].ToString();
                            log.Error("strCustID - " + strCustID);
                            //string strQuery = "insert into BillDesk_TXN(ApplicationNo,MEDAProjectID, MerchantID, TxnAmount,ORIGINALSTATUS,  CreatedDt,typeofpay) " +
                            //    "values('" + applicationId + "','" + strProjectID + "','" + strMerchantCode + "','" + strPayAmt + "','" + "1','" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "','Registration')";
                            //string strQuery = "insert into BillDesk_TXN_billdesk_txn_query(ApplicationNo,MEDAProjectID, MerchantID,CustomerID, TxnAmount,ORIGINALSTATUS,  CreatedDt,typeofpay) " +
                            //    "values('" + applicationId + "','" + strProjectID + "','" + strMerchantCode + "','" + strCustID + "','" + strPayAmt + "','" + "1',now(),'Registration')";

                            //cmd = new MySqlCommand(strQuery, mySqlConnection);
                            //cmd.ExecuteNonQuery();
                            //log.Error(strQuery);
                            ////    strSrno=cmd.LastInsertedId;
                            //cmd = new MySqlCommand("select SRNO  from BillDesk_TXN_query where APPLICATIONNO='" + applicationId + "'  order by srno desc", mySqlConnection);
                            //dsResult = new DataSet();
                            //da = new MySqlDataAdapter(cmd);
                            //da.Fill(dsResult);
                            //strSrno = long.Parse(dsResult.Tables[0].Rows[0]["SRNO"].ToString());

                            //log.Error(strQuery + " SRNO : " + strSrno);
                            //strQuery = "update BillDesk_TXN set CustomerID= " +
                            //    "'" + strSrno + "' where SRNO='" + strSrno + "' and APPLICATIONNO='" + applicationId + "'";
                            //log.Error(strQuery + " SRNO : " + strSrno);
                            //cmd = new MySqlCommand(strQuery, mySqlConnection);
                            //cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            //lblMessage.Text = "Payment already done.";
                            break;
                        }
                        String data = "0122" + "|" + strMerchantCode + "|" + strCustID + "|" + DateTime.Now.ToString("yyyyMMddHHmmss");
                        PaymentGateway dataprg = new PaymentGateway();
                        String hash = String.Empty;
                        hash = dataprg.GetHMACSHA256(data, checkSumKey).ToUpper();
                        data += "|" + hash;
                        try
                        {
                            //string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getUsrDtls?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;
                            //string userAuthenticationURI = "https://regridmeda.mahadiscom.in/swPortal/getUsrDtls?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;
                            string userAuthenticationURI = ConfigurationManager.AppSettings["PGQUERYURL"].ToString() + "?msg=" + data;
                            log.Error("userAuthenticationURI - " + userAuthenticationURI);
                            WebRequest req = WebRequest.Create(@userAuthenticationURI);


                            req.Method = "POST";
                            //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                            //req.Credentials = new NetworkCredential("username", "password");
                            HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                            StreamReader reader = new StreamReader(resp.GetResponseStream());

                            string responseText = reader.ReadToEnd();
                            log.Error("responseText : " + responseText);
                        }
                        catch (Exception ex)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                            log.Error(ErrorMessage);
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
                //  log.Error(strQuery);

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
    #endregion
        }
    }
}