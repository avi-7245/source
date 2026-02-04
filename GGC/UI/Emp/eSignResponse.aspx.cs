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
using System.Text;
using System.Xml;

namespace GGC.UI.Emp
{
    public partial class eSignResponse : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(eSignResponse));
        string txn = "";
        string reference = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            //{
                if (Request.QueryString["txn"] != null)
                {

                    HDFtxn.Value = Request.QueryString["txn"].ToString();
                    HDFRef.Value = Request.QueryString["reference"].ToString();
                }
            //}
        }

        protected void Button1_Click(object sender, EventArgs e)
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
                                strQuery = "select * from eSignDetails where TxnNo='" + HDFtxn.Value + "' and reference='" + HDFRef.Value + "' ";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                DataSet dsResult = new DataSet();
                                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                da.Fill(dsResult);
                                log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());

                                string url = dsResult.Tables[0].Rows[0]["getsigneddocurl"].ToString();


                                #region Fetch PDF
                                try
                                {
                                    WebRequest request = HttpWebRequest.Create(url);
                                    // Get the response back  
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; 
                                    Stream s = (Stream)response.GetResponseStream();
                                    StreamReader readStream = new StreamReader(s);
                                    string dataString = readStream.ReadToEnd();
                                    //log.Error("Response :" + dataString);
                                    response.Close();
                                    s.Close();
                                    readStream.Close();
                                
                                //WebRequest req = WebRequest.Create(@url);


                                //req.Method="GET";
                                ////req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                                ////req.Credentials = new NetworkCredential("username", "password");
                                //HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                                //StreamReader reader = new StreamReader(resp.GetResponseStream());

                                //string responseText = reader.ReadToEnd();
                                //log.Error(responseText);

                                //var stream = resp.GetResponseStream();
                                //StreamReader respStream = new StreamReader(resp.GetResponseStream(), System.Text.Encoding.Default);
                                //string receivedResponse = respStream.ReadToEnd();

                                //XmlDocument xml = new XmlDocument();
                                //xml.LoadXml(stream);

                                XmlDocument xml1 = new XmlDocument();
                                xml1.LoadXml(dataString);


                                //log.Error(receivedResponse);
                                //respStream.Close();
                                //resp.Close();


                                //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                                ////byte[] requestInFormOfBytes = Encoding.ASCII.GetBytes(strConsDetails.ToString());
                                //request.Method = "GET";
                                //request.ContentType = "text/xml;charset=utf-8";
                                ////request.ContentLength = requestInFormOfBytes.Length;
                                //Stream requestStream = request.GetRequestStream();
                                ////requestStream.Write(requestInFormOfBytes, 0, requestInFormOfBytes.Length);
                                //requestStream.Close();

                                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                                //log.Error(response);
                                //StreamReader respStream = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);
                                //string receivedResponse = respStream.ReadToEnd();




                                string xpath = "response/responsedata/response";
                                var nodes = xml1.SelectNodes(xpath);
                                string pdfurl = string.Empty;
                                string signpdf64 = string.Empty;
                                //string reference = string.Empty;
                                string getsigneddocurl = string.Empty;
                                string txnNo = string.Empty;
                                foreach (XmlNode childrenNode in nodes)
                                {
                                    signpdf64 = childrenNode.SelectSingleNode("//signpdf64").InnerText;
                                    reference = childrenNode.SelectSingleNode("//reference").InnerText;
                                    pdfurl = childrenNode.SelectSingleNode("//pdfurl").InnerText;
                                    txnNo = childrenNode.SelectSingleNode("//txn").InnerText;
                                }

                               
                                #endregion

                                //MySqlConnection mySqlConnection = new MySqlConnection();
                                //mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


                                            strQuery = string.Empty;
                                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                                            //string strQuery=string.Empty;
                                            //strQuery = "update eSignDetails set eSignPDFURL='" + pdfurl + "' ,eSignPDF_Base64='" + signpdf64 + "' where TxnNo='" + txnNo + "' and reference='" + reference + "' ";
                                            strQuery = "update eSignDetails set eSignPDFURL='" + pdfurl + "' where TxnNo='" + txnNo + "' and reference='" + reference + "' ";

                                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                                            cmd.ExecuteNonQuery();

                                //log.Error("PDF URL : " + pdfurl);
                                StringBuilder sb = new StringBuilder();
                                sb.Append("<script type = 'text/javascript'>");
                                sb.Append("window.open('");
                                sb.Append(pdfurl);
                                sb.Append("');");
                                sb.Append("</script>");
                                ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());

                                }
                                catch (Exception ex)
                                {
                                    log.Error("Calling API : " + ex.Message);
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
                        //lblMessage.Text = "Error occurred during Password changed!";

                        // Use the mySqlException object to handle specific MySql errors
                        // lblResult.Text = "Some problem during registration.Please try again.";
                    }

                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        //lblMessage.Text = "Error occurred during Password changed!";

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


                //WebRequest req = WebRequest.Create(@userAuthenticationURI);


                //req.Method = "GET";
                ////req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                ////req.Credentials = new NetworkCredential("username", "password");
                //HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                //StreamReader reader = new StreamReader(resp.GetResponseStream());

                //string responseText = reader.ReadToEnd();

                //var stream = resp.GetResponseStream();
                //XmlDocument xml = new XmlDocument();
                //xml.Load(stream);
            
        
        }
}