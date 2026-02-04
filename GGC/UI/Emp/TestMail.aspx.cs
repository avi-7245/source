using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Net.Mail;
using log4net;
using System.Reflection;
using System.Globalization;
using System.Configuration;
using System.IO;
using GGC.WebService;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;
using GGC.Common;

namespace GGC.UI.Emp
{
    public partial class TestMail : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(TestMail));
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    File.Copy(@"\\192.168.10.122\FTP Test Data\22.02.2023\\Transmission_Line_400kV_BABHALESHWAR_LINE__Operator_Report_38246_22_02_2023_03_38_11.pdf", "\\\\192.168.10.123\\E$\\Transmission_Line_400kV_BABHALESHWAR_LINE__Operator_Report_38246_22_02_2023_03_38_11.pdf");
            //}
            //catch (Exception ex)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
            //    log.Error(ErrorMessage);
            //}
            #region Send SMS
            //try
            //{
            //    int OTP=0;
            //    string strmobileno = string.Empty; ;
            //    //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=http://164.52.205.46:6005/api/v2/SendSMS?SenderId=CESTU&Message=Dear%20User%2C%20%20Your%20One%20Time%20Password%20%28OTP%29%20for%20GC%20login%20is%20%7B%23var%23%7D.%20%5CnPlease%20do%20not%20share%20this%20OTP.%20%20%5Cn%5CnRegards%2C%20CE%20STU%2C%20MSETCL&MobileNumbers=9768776677&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
            //    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Is_Unicode=false&Is_Flash=false&Message=Dear%20User%2C%20%5CnYour%20One%20Time%20Password%20%28OTP%29%20for%20GC%20login%20is%20"+OTP+".%5CnPlease%20do%20not%20share%20this%20OTP.%5Cn%5CnRegards%2C%5CnCE%20STU%2C%20MSETCL&MobileNumbers="+strmobileno+"&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
            //    //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Is_Unicode=false&Is_Flash=false&Message=Dear%20User%2C%20%5CnYour%20One%20Time%20Password%20%28OTP%29%20for%20GC%20login%20is%20%7B%23var%23%7D.%5CnPlease%20do%20not%20share%20this%20OTP.%5Cn%5CnRegards%2C%5CnCE%20STU%2C%20MSETCL&MobileNumbers=9768776677%2C8854993588&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
            //    WebRequest request = HttpWebRequest.Create(strURL);
            //    // Get the response back  
            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //    Stream s = (Stream)response.GetResponseStream();
            //    StreamReader readStream = new StreamReader(s);
            //    string dataString = readStream.ReadToEnd();
            //    log.Error(dataString);
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
            //string strBody = string.Empty;

            //strBody += "Respected Sir/Madam" + ",<br/>";
            //strBody += "Test Email" + ",<br/>";
            //log.Error(strBody);
            
            //try
            //{


            //    #region using MailMessage
            //    MailMessage Msg = new MailMessage();
            //    //MailAddress fromMail = new MailAddress("progit4000@mahatransco.in");
            //    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
            //    Msg.From = fromMail;
            //    Msg.IsBodyHtml = true;
            //    //log.Error("from:" + fromAddress);
            //    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
            //    Msg.To.Add(new MailAddress("sait4000@mahatransco.in"));
            //    Msg.To.Add(new MailAddress("ashishbtribhuwan@gmail.com"));
            //    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
            //    Msg.Subject = "Test Email";
            //    Msg.Body = strBody;
            //    SmtpClient a = new SmtpClient();
            //    a.Host = "smtp.office365.com";
            //    a.EnableSsl = true;
            //    NetworkCredential n = new NetworkCredential();
            //    n.UserName = "donotreply@mahatransco.in";
            //    n.Password = "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~";
            //    a.UseDefaultCredentials = false;
            //    a.Credentials = n;
            //    a.Port = 587;
            //    log.Error("Mail send");
            //    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
            //    a.Send(Msg);
            //    log.Error("Mail Ends");
                

                
            //    #endregion
            //}
            //catch (Exception ex)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
            //    log.Error(ErrorMessage);
            //    // throw ex;
            //}
            //saveApp("PSLR20234740116");
        }

        public string saveApp(string projId)
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
                        strQuery = "select * from applicantdetails where MEDAProjectID='PSLR20234740116'";
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
                        objGetStatusDTO.statusId = "19";
                        objGetStatusDTO.OLD_GC_APPROVED_FLAG = dsResult.Tables[0].Rows[0]["IS_ALREADY_APPLIED"].ToString();
                        objGetStatusDTO.DISTANCE_FROM_PLANT = dsResult.Tables[0].Rows[0]["DISTANCE_FROM_PLANT"].ToString();
                        objGetStatusDTO.VOLT_LEVEL_SUBSTATION = dsResult.Tables[0].Rows[0]["VOLT_LEVEL_SUBSTATION"].ToString();

                        //objGetStatusDTO.OLD_GC_APPLICATION_DATE = txtAppliedDt.Text;
                        //objGetStatusDTO.OLD_GC_APPROVED_DATE = txtIssueDt.Text;
                        //objGetStatusDTO.validity = txtValidityDt.Text;
                        //objGetStatusDTO.FIRST_EXTENSION_DATE = txtExt1.Text;
                        //objGetStatusDTO.SECOND_EXTENSION_DATE = txtExt2.Text;

                        string json = JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);



                        APICall apiCall = new APICall();
                        string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=sorigin" + "&projId=PSLP20234790355&entity=MSETCL");

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
                //string userAuthenticationURI = ConfigurationManager.AppSettings["MEDASAVETOMEDAURL"].ToString();
                ////WebRequest req = WebRequest.Create(@userAuthenticationURI);
                //var req = (HttpWebRequest)WebRequest.Create(@userAuthenticationURI);


                //req.Method = "POST";
                //req.ContentType = "application/json";
                ////req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                ////req.Credentials = new NetworkCredential("username", "password");
                //using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                //{
                //    string json = JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);

                //    streamWriter.Write(JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None));
                //    log.Error("Old GC json " + json);
                //}
                //HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                //StreamReader reader = new StreamReader(resp.GetResponseStream());

                //string responseText = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
            }
            //string str = JsonConverter. .Serialize(objGetStatusDTO, Formatting.Indented);
            return "";
            //return  new JavaScriptSerializer().Serialize(objGetStatusDTO);
        }
    }
}