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
using System.Text;
using Newtonsoft.Json;

namespace GGC.UI.MSKVY
{
    public partial class Home : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Home));
        protected string strUserID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["usrName"];
            //strUserID = "sapna";
            if (strUserID != "")
            {
                checkUser();
            }
        }
        protected void checkUser()
        {

            #region


            //if (!string.IsNullOrEmpty(userAuthenticationURI))
            //{
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(userAuthenticationURI);
            //    request.Method = "POST";
            //    request.ContentType = "application/json";
            //    WebResponse response = request.GetResponse();
            //    using (var reader = new StreamReader(response.GetResponseStream()))
            //    {
            //        var ApiStatus = reader.ReadToEnd();
            //        JsonData data = JsonMapper.ToObject(ApiStatus);
            //        string status = data["Status"].ToString();

            //    }
            //}  


            //string sURL = "http://regrid.mahadiscom.in/reGrid/getUsrDtls?usrName=sapna&entity=MSETCL&secKey=127550f7eead313b1df5e02536ec3df6a6cd066016fc2045223dc16c4622e3a3";
            //WebRequest wrGETURL;
            //wrGETURL = WebRequest.Create(sURL);

            //wrGETURL.Method = "POST";
            //wrGETURL.ContentType = @"application/json; charset=utf-8";
            //using (var stream = new StreamWriter(wrGETURL.GetRequestStream()))
            //{
            //    var bodyContent = new
            //    {

            //    }; // This will need to be changed to an actual class after finding what the specification sheet requires.

            //    var json = JsonConvert.SerializeObject(bodyContent);

            //    stream.Write(json);
            //}
            //HttpWebResponse webresponse = wrGETURL.GetResponse() as HttpWebResponse;

            //Encoding enc = System.Text.Encoding.GetEncoding("utf-8");
            //// read response stream from response object
            //StreamReader loResponseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            //// read string from stream data
            //string strResult = loResponseStream.ReadToEnd();
            //// close the stream object
            //loResponseStream.Close();
            //// close the response object
            //webresponse.Close();

            //Response.Write(strResult);


            #endregion


            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            bool isUser = false;

            try
            
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd = new MySqlCommand("select *  from APPLICANT_LOGIN_MASTER where USER_NAME='" + strUserID + "'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            cmd = new MySqlCommand("select *  from MSKVY_applicantdetails  where USER_NAME='" + strUserID + "'", mySqlConnection);
                            DataSet dsResult2 = new DataSet();
                            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd);
                            da2.Fill(dsResult2);
                            if (dsResult2.Tables[0].Rows.Count > 0)
                            {
                                Session["isValid"] = "true";
                                //Session["GSTIN"] = dsResult.Tables[0].Rows[0]["GSTIN_NO"].ToString();
                                Session["user_name"] = strUserID;
                                Session["generatorId"] = dsResult.Tables[0].Rows[0]["generatorId"].ToString();
                                Response.Redirect("~/UI/MSKVY/AppHome.aspx", false);
                            }
                            else
                            {
                                Session["isValid"] = "true";
                                //Session["GSTIN"] = dsResult.Tables[0].Rows[0]["GSTIN_NO"].ToString();
                                Session["user_name"] = strUserID;
                                Session["generatorId"] = dsResult.Tables[0].Rows[0]["generatorId"].ToString();
                                Response.Redirect("~/UI/MSKVY/AppDetail.aspx", false);
                            }
                        }
                        else
                        {

                            APICall apiCall = new APICall();
                            string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + strUserID + "&entity=MSETCL");

                            try
                            {
                                //string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getUsrDtls?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;
                                //string userAuthenticationURI = "https://regridmeda.mahadiscom.in/swPortal/getUsrDtls?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;
                                string userAuthenticationURI = ConfigurationManager.AppSettings["MEDAGETUSRDETURL"].ToString() + "?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;

                                WebRequest req = WebRequest.Create(@userAuthenticationURI);


                                req.Method = "GET";
                                //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                                //req.Credentials = new NetworkCredential("username", "password");
                                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                                StreamReader reader = new StreamReader(resp.GetResponseStream());

                                string responseText = reader.ReadToEnd();

                                //if your response is in json format just uncomment below line  

                                //Response.AddHeader("Content-type", "text/json");  

                                //Response.Write(responseText);

                                UserReg uReg = JsonConvert.DeserializeObject<UserReg>(responseText);
                                DateTime dt = DateTime.ParseExact(uReg.createdDt, "dd-MM-yyyy",
                                  CultureInfo.InvariantCulture);


                                strQuery = "insert into applicant_reg_det(GSTIN_NO,ORGANIZATION_NAME,ContactPerson,address1,address2,ORG_MOB,ORG_EMAIL,CREATED_DT,USER_NAME,PAN_TAN_NO,pincode,createdBy,generatorId)" +
                                    " values('" + uReg.generatorGst + "','" + uReg.generatorName + "','" + uReg.contactPersonName + "','" + uReg.companyAddressLandMark + "','" + uReg.generatorCity + "','" + uReg.generatorMobileNo + "','" + uReg.generatorEmailId + "','" + dt.ToString("yyyy-MM-dd") + "','" + uReg.generatorUserName + "','" + uReg.generatorPan + "','" + uReg.generatorPinCode + "','" + uReg.createdBy + "','" + uReg.generatorId + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();

                                strQuery = "insert into APPLICANT_LOGIN_MASTER(GSTIN_NO, PASSWORD, CREATED_DT,USER_NAME,generatorId) values('" + uReg.generatorGst + "','" + uReg.generatorGst + "',CURDATE(),'" + uReg.generatorUserName + "','" + uReg.generatorId + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                log.Error("Insert login" + strQuery);

                                cmd.ExecuteNonQuery();

                                Session["isValid"] = "true";
                                Session["GSTIN"] = uReg.generatorGst.ToString();
                                Session["user_name"] = uReg.generatorUserName;
                                Session["generatorId"] = uReg.generatorId;
                                //Response.Redirect("~/UI/Cons/ConsumerDetail.aspx", false);
                                Response.Redirect("~/UI/MSKVY/apphome.aspx", false);
                            }
                            catch (Exception ex)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                log.Error(ErrorMessage);
                                Response.Write("<script language='javascript'>alert('User doesnot exist or Intial project registration not done on MEDA portal.');</script>");
                            }

                            //                            Response.Redirect("http://regrid.mahadiscom.in/reGrid/getUsrDtls?usrName='" + strUserID + "'&entity=MSETCL&secKey="+str, false);


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
    }
}