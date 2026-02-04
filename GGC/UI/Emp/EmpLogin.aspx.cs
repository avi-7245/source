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
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using GGC.Common;

namespace GGC.UI.Emp
{
    public class SMSDTO
    {
        public string a1 { get; set; }
        public string a2 { get; set; }
        public string a3 { get; set; }
    }
    public partial class EmpLogin : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(EmpLogin));

        public void SendSMS()
        {
            #region Variables
            string userId = "msetcl";
            string pwd = "Ashish@1234";
            string senderId = "CENGPZ";
            string Message = "Important Letter\nPlease find letter no. {#var#}, dtd. {#var#} linked below. \n{#var#}\nSubmitted for information & necessary action.\nRegards, C.E., MSETCL Nagpur";
            string mobile = "9768776677";
            string ApiKey = "J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D";
            string postURL = "http://164.52.205.46:6005/api/v2/SendSMS";
            string ClientId = "57b6632f-ac04-41cf-8907-2ba861acfc35";
            StringBuilder postData = new StringBuilder();
            string responseMessage = string.Empty;
            HttpWebRequest request = null;
            #endregion

            try
            {
                // Prepare POST data 
                //postData.Append("action=send");
                //postData.Append("&username=" + userId);
                //postData.Append("&passphrase=" + pwd);
                postData.Append("&SenderId=" + senderId);
                postData.Append("&Message=" + Message);
                postData.Append("&MobileNumbers=" + mobile);
                postData.Append("&ApiKey=" + ApiKey);
                postData.Append("&ClientId=" + ClientId);
                byte[] data = new System.Text.ASCIIEncoding().GetBytes(postData.ToString());

                // Prepare web request
                request = (HttpWebRequest)WebRequest.Create(postURL);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                // Write data to stream
                using (Stream newStream = request.GetRequestStream())
                {
                    newStream.Write(data, 0, data.Length);
                }

                // Send the request and get a response
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Read the response
                    using (StreamReader srResponse = new StreamReader(response.GetResponseStream()))
                    {
                        responseMessage = srResponse.ReadToEnd();
                    }

                    // Logic to interpret response from your gateway goes here
                    Response.Write(String.Format("Response from gateway: {0}", responseMessage));
                }
            }
            catch (Exception objException)
            {
                Response.Write(objException.ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            //   SendSMS();

            //try
            //{
            //    //                                string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projid=" + txtProjCode + "&entity=MSETCL&secKey=" + str;
            //    string userAuthenticationURI = "https://regrid.mahadiscom.in/reGrid/saveToMedaGcAppln";

            //    //WebRequest req = WebRequest.Create(@userAuthenticationURI);
            //    var req = (HttpWebRequest)WebRequest.Create(@userAuthenticationURI);


            //    req.Method = "POST";
            //    req.ContentType = "application/json";
            //    //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
            //    req.Credentials = new NetworkCredential("MSETCL", "Ashish@1234");
            //    using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            //    {
            //        string json = JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);

            //        streamWriter.Write(JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None));
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



            //var client = new RestClient("http://164.52.205.46:6005/api/v2/SendSMS?SenderId=CENGPZ&Message=Important Letter\nPlease find letter no. {#var#}, dtd. {#var#} linked below. \n{#var#}\nSubmitted for information & necessary action.\nRegards, C.E., MSETCL Nagpur&MobileNumbers=9768776677&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35");
            //var request = new RestRequest(Method.GET);
            //request.AddHeader("postman-token", "e2ea21f9-46cb-9a26-df07-80e13310ef5f");
            //request.AddHeader("cache-control", "no-cache");
            //IRestResponse response = client.Execute(request);

            if (IsPostBack)
            {
                if (!(String.IsNullOrEmpty(txtPass.Text.Trim())))
                {
                    txtPass.Attributes["value"] = txtPass.Text;
                    //or txtPwd.Attributes.Add("value",txtPwd.Text);
                }
            }
        }
        protected void btnSendOTP_Click(object sender, EventArgs e)
        {
            if (txtSAPID.Text != "")
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
                            string strMobNo = string.Empty;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            //string strQuery=string.Empty;
                            string strSAPID = txtSAPID.Text;
                            strQuery = "select *  from empmaster where SAPID='" + strSAPID + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);

                            //log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                            strMobNo = dsResult.Tables[0].Rows[0]["EmpMobile"].ToString();

                            if (dsResult.Tables[0].Rows[0]["Password"].ToString() == txtPass.Text)  
                            {
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    #region Send SMS
                                    try
                                    {
                                        //int OTP = 0;
                                        string OTP = String.Empty;
                                        string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                                        string sTempChars = String.Empty;

                                        Random rand = new Random();

                                        for (int i = 0; i < 4; i++)
                                        {

                                            int p = rand.Next(0, saAllowedCharacters.Length);

                                            sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                                            OTP += sTempChars;

                                        }
                                        string strmobileno = string.Empty; ;
                                        //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=http://164.52.205.46:6005/api/v2/SendSMS?SenderId=CESTU&Message=Dear%20User%2C%20%20Your%20One%20Time%20Password%20%28OTP%29%20for%20GC%20login%20is%20%7B%23var%23%7D.%20%5CnPlease%20do%20not%20share%20this%20OTP.%20%20%5Cn%5CnRegards%2C%20CE%20STU%2C%20MSETCL&MobileNumbers=9768776677&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
                                        //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Is_Unicode=false&Is_Flash=false&Message=Dear%20User%2C%20%5CnYour%20One%20Time%20Password%20%28OTP%29%20for%20GC%20login%20is%20" + OTP + ".%5CnPlease%20do%20not%20share%20this%20OTP.%5Cn%5CnRegards%2C%5CnCE%20STU%2C%20MSETCL&MobileNumbers=" + strMobNo + "&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
                                        //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Is_Unicode=false&Is_Flash=false&Message=Dear%20User%2C%20%5CnYour%20One%20Time%20Password%20%28OTP%29%20for%20GC%20login%20is%20%7B%23var%23%7D.%5CnPlease%20do%20not%20share%20this%20OTP.%5Cn%5CnRegards%2C%5CnCE%20STU%2C%20MSETCL&MobileNumbers=9768776677%2C8854993588&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";

                                        //WebRequest request = HttpWebRequest.Create(strURL);
                                        ////// Get the response back  
                                        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                        //Stream s = (Stream)response.GetResponseStream();
                                        //StreamReader readStream = new StreamReader(s);
                                        //string dataString = readStream.ReadToEnd();
                                        //log.Error(dataString);
                                        //response.Close();
                                        //s.Close();
                                        //readStream.Close();

                                        //SendSMS.
                                        SMS.Send(message: SMSTemplates.SmsOtp(OTP), strMobNo, MethodBase.GetCurrentMethod(), log);

                                        Session["OTP"] = OTP.ToString();
                                        btnSendOTP.Text = "Resend OTP";

                                        //txtOTP.TextMode = TextBoxMode.SingleLine;
                                        //txtOTP.Text = OTP;
                                        //AddComment 216 to 217

                                        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "disableButton();", true);

                                        //btnSendOTP.Enabled = false;
                                    }
                                    catch (Exception ex)
                                    {
                                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                        log.Error(ErrorMessage);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                Response.Write("<script language='javascript'>alert('User Name or Password does not match.');</script>");
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
                    // lblResult.Text = "Some problem during registration.Please try again.";
                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    lblResult.Text = "Invalid credential";
                    // Use the exception object to handle all other non-MySql specific errors
                    //lblResult.Text = "Some problem during registration.Please try again.";
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
                #region Send SMS
                //try
                //{
                //    string strMobNos = "9768776677";
                //    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Payment%20of%20processing%20fee%20towards%20Grid%20Connectivity%20application%20no." + "1234567" + "%20is%20received.%5CnRegards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
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
            else
            {
                Response.Write("<script language='javascript'>alert('Enter User Name & Password.');</script>");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string strSAPID, strPass;
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$";
            strSAPID = txtSAPID.Text;
            strPass = txtPass.Text;
            bool isAllValues = true;
            if (strSAPID == "" || strPass == "" || txtOTP.Text == "")
            {
                isAllValues = false;
            }
            Regex rg = new Regex(pattern);
            if (isAllValues)
            {
                if (Session["OTP"].ToString() == txtOTP.Text)
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
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                //string strQuery=string.Empty;
                                strQuery = "select *  from empmaster where SAPID='" + strSAPID + "' and PASSWORD='" + strPass + "' and isActive='Y'";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                DataSet dsResult = new DataSet();
                                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                da.Fill(dsResult);
                                log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                                if (dsResult.Tables[0].Rows.Count > 0)
                                {
                                    int Role_ID = int.Parse(dsResult.Tables[0].Rows[0]["ROLE_ID"].ToString());
                                    Session[SessionConst.EmpRole] = dsResult.Tables[0].Rows[0]["ROLE_ID"].ToString();
                                    Session[SessionConst.SAPID] = dsResult.Tables[0].Rows[0]["SAPID"].ToString();
                                    Session[SessionConst.ReportingSAPID] = dsResult.Tables[0].Rows[0]["ReportingOfficerSAPID"].ToString();
                                    Session[SessionConst.EmpZone] = dsResult.Tables[0].Rows[0]["ZONE"].ToString();
                                    Session[SessionConst.EmpDesignation] = dsResult.Tables[0].Rows[0]["Designation"].ToString();
                                    Session[SessionConst.EmpName] = dsResult.Tables[0].Rows[0]["Emp_Name"].ToString();
                                    Session[SessionConst.EKYC_ID] = dsResult.Tables[0].Rows[0]["ekycid"].ToString();
                                    string strIsFirstLogin = dsResult.Tables[0].Rows[0]["isFirstLogin"].ToString();
                                    if (strIsFirstLogin == "Y")
                                    {
                                        Response.Redirect("~/UI/Emp/changepass.aspx", false);
                                    }
                                    else
                                    {
                                        if (Role_ID > 30 && Role_ID <= 40)
                                        {
                                            Response.Redirect("~/UI/Finance/ApprovePayment.aspx", false);

                                        }
                                        else
                                        {
                                            if (Role_ID > 50 && Role_ID <= 60)
                                            {
                                                Response.Redirect("~/UI/Emp/ProposalApprovalList.aspx", false);

                                            }
                                            else
                                            {
                                                Response.Redirect("~/UI/Emp/EmpHome.aspx", false);
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    Response.Write("<script language='javascript'>alert('Login UnSuccessfull.');</script>");
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
                        // lblResult.Text = "Some problem during registration.Please try again.";
                    }

                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        // Use the exception object to handle all other non-MySql specific errors
                        //lblResult.Text = "Some problem during registration.Please try again.";
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
                    //Response.Write("<script language='javascript'>alert('OTP does not match.');</script>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('OTP does not match.');window.location ='EmpLogin.aspx';", true);
                }
            }
            else
            {
                //Response.Write("<script language='javascript'>alert('Kindly enter all details.');</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Kindly enter all details.');window.location ='EmpLogin.aspx';", true);

            }
        }

        protected void btnForgot_Click(object sender, EventArgs e)
        {
        }
    }
}