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


namespace GGC.UI.Finance
{
    public partial class UpdateVID : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UpdateVID));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string strPAN = Request.QueryString["pan"].ToString();
                fillData(strPAN);

            }
        }
        protected void fillData(string strPAN)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //string strGSTIN = string.Empty;
            //if (Session["GSTIN"] != null)
            //    strGSTIN = Session["GSTIN"].ToString();

            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        strQuery = "select * from APPLICANT_REG_DET where isRegApprove='N' and PAN_TAN_NO='"+strPAN+"'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            lblPAN.Text = dsResult.Tables[0].Rows[0]["PAN_TAN_NO"].ToString();
                            lblGSTIN.Text = dsResult.Tables[0].Rows[0]["GSTIN_NO"].ToString();
                            lblOrgName.Text = dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString();
                            lblAdd.Text = dsResult.Tables[0].Rows[0]["ADDRESS1"].ToString() + " " + dsResult.Tables[0].Rows[0]["ADDRESS2"].ToString() + " " + dsResult.Tables[0].Rows[0]["ADDRESS3"].ToString();
                            lblState.Text = dsResult.Tables[0].Rows[0]["STATE"].ToString();
                            lblCountry.Text = dsResult.Tables[0].Rows[0]["COUNTRY"].ToString();
                            //lblAdd.Text = dsResult.Tables[0].Rows[0]["COUNTRY"].ToString();
                            lblMobNo.Text = dsResult.Tables[0].Rows[0]["ORG_MOB"].ToString();
                            lblEmail.Text = dsResult.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
                            ViewState["GSTINCert"] = dsResult.Tables[0].Rows[0]["GSTIN_CERTIFICATE"].ToString();
                            ViewState["PANDOC"] = dsResult.Tables[0].Rows[0]["PAN_DOC"].ToString();
                        }
                        else
                        {
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
                string ErrorMessage = "Sql ExceptionMethod Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "FilData Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
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

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string strPan = lblPAN.Text;
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;

                        strQuery = "select count(1) from APPLICANT_REG_DET where vendorID='" + txtVendorID.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        int count=int.Parse(dsResult.Tables[0].Rows[0][0].ToString());

                        if (count <= 0)
                        {
                            string VendorID = txtVendorID.Text;
                            strQuery = "Update APPLICANT_REG_DET set isRegApprove='Y', vendorIDCreateDt=CURDATE() , vendorID='" + VendorID + "' where PAN_TAN_NO='" + lblPAN.Text + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();

                            strQuery = "Update applicant_login_master set isActive='Y' where USER_NAME='" + lblPAN.Text + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            txtVendorID.Text = "";

                            Response.Write("<script language='javascript'>alert('Vendor ID updated successfully.');</script>");


                            string strEmail = lblEmail.Text;
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "Your login is unlock now." + "<br/>";
                            strBody += "Kindly used credentials given in last mail for login. " + "<br/>";
                            strBody += "Thanks & Regards, " + "<br/>";
                          //  strBody += " Chief Engineer" + "<br/>";
                            strBody += "State Transmission Utility (STU)" + "<br/>";
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

                                Msg.Subject = "Login Unlock.";
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

                            Response.Redirect("~/UI/Finance/VendorRegister.aspx", false);
                        }
                        else
                        {
                            Response.Write("<script language='javascript'>alert('Vendor ID already exist.');</script>");

                            
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

        protected void btnViewGSTIN_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/Finance/ShowGSTIN.aspx?GSTIN=" + lblGSTIN.Text + "&docName=" + ViewState["GSTINCert"].ToString()+"&PAN="+lblPAN.Text, false);
        }

        protected void btnViewPAN_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/Finance/ShowPan.aspx?PAN=" + lblPAN.Text + "&docName=" + ViewState["PANDOC"].ToString(), false);

        }
    }
}