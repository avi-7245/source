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

namespace GGC.UI.Cons
{
    public partial class Register : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Register));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["isLoginValid"] = "true";
            }
        }
        protected void btnReg_Click(object sender, EventArgs e)
        {
            if (Session["IsValid"] == "true")  
            {
                string strGISTN_No, strOrgName, strAdd1, strAdd2, strAdd3, strState, strCountry, strPhone, strMob, strEmail, strFAX, strPass, strCPass,strPAN,strProvisionalGSTIN,strPIN;
                strGISTN_No = txtGSTIN.Text;
                strOrgName = txtOrgName.Text;
                strAdd1 = txtAddress1.Text;
                strAdd2 = txtAddress2.Text;
                strAdd3 = txtAddress3.Text;
                strState = ddlState.SelectedItem.Text;
                strCountry = ddlCountry.SelectedItem.Text;
                strPhone = txtPhone.Text;
                strMob = txtMob.Text;
                strEmail = txtEmail.Text;
                strFAX = txtFax.Text;
                strPass = txtPass.Text;
                strCPass = txtCPass.Text;
                strPAN = txtPAN.Text;
                strProvisionalGSTIN = txtProvGSTIN.Text;
                strPIN = txtPin.Text;
                string isGSTIN = rbISGSTIN.SelectedItem.Value;
                
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            string strUname = txtPAN.Text;
                            int cnt=0;
                        MySqlCommand cmd;
                        strQuery = "select count(1)  from APPLICANT_LOGIN_MASTER where USER_NAME='" + strUname + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        cnt = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                        if (cnt > 0)
                        {
                            Session["isLoginValid"] = "false";
                            lblResult.Text = "Already register with same PAN.";
                        }
                        else
                        {
                            //  Session["isLoginValid"] = "true";
                            //    lblLoginStatus.Text = "User Name availaible.";
                            //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();


                           // cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd = new MySqlCommand("select *  from APPLICANT_REG_DET where USER_NAME='" + strUname + "'", mySqlConnection);
                            dsResult = new DataSet();
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                            if (dsResult.Tables[0].Rows.Count < 1)
                            {
                                strQuery = "insert into APPLICANT_REG_DET (GSTIN_NO, ORGANIZATION_NAME, ADDRESS1, ADDRESS2, ADDRESS3, ORG_PHONE, ORG_MOB, ORG_EMAIL, ORG_FAX, STATE, COUNTRY, CREATED_DT,PAN_TAN_NO,IsGSTIN,Provisional_GSTIN,USER_NAME,isRegApprove,PINcode) " +
                                    " values('" + strGISTN_No + "','" + strOrgName + "','" + strAdd1 + "','" + strAdd2 + "','" + strAdd3 + "','" + strPhone + "','" + strMob + "','" + strEmail + "','" + strFAX + "','" + strState + "','" + strCountry + "',CURDATE(),'"+strPAN+"','"+isGSTIN+"','"+strProvisionalGSTIN+"','"+strPAN+"','N','"+strPIN+"')";
                                log.Error("Insert " + strQuery);

                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();


                                strQuery = "insert into APPLICANT_LOGIN_MASTER(GSTIN_NO, PASSWORD, CREATED_DT,USER_NAME) values('" + strGISTN_No + "','" + strPass + "',CURDATE(),'" + strUname + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                log.Error("Insert login" + strQuery);

                                cmd.ExecuteNonQuery();

                                //Response.Write("<script language='javascript'>alert('Login UnSuccessfull.');</script>");


                                #region Send Mail
                                //sendMailOTP(strRegistrationno, strEmailID);
                                string strBody = string.Empty;

                                strBody += "Dear " + strOrgName + ",<br/>";
                                strBody += "You registered to MSETCL online Grid Connectivity" + "<br/>";
                                strBody += "Please use following information for login for further process. <br/>";
                                strBody += "Kindly wait 24Hrs to 48 Hrs for login. <br/>";
                                strBody += "User Name : " + strUname + "<br/>";
                                strBody += "Password : " + strPass + "<br/>";
                                strBody += "<br/>";
                                strBody += "Please wait for 24Hrs to 48 Hrs for login.<br/><br/>";
                                strBody += "<a href='http://gridconn.mahatransco.in/UI/Cons/Home.aspx'>Click here</a> for login.<br/><br/>";
                                strBody += "Thanks & Regards, " + "<br/>";
                                strBody += "STU Department" + "<br/>";
                                strBody += "MSETCL  " + "<br/>";
                                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
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


                                    //  Msg.To.Add(new MailAddress(toAddress));

                                    Msg.Subject = "Online Grid Connectivity Registration MSETCL";
                                    Msg.Body = strBody;
                                    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                    SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
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


                                #region Send Mail to MM
                                //sendMailOTP(strRegistrationno, strEmailID);
                                string strMMMailID = ConfigurationManager.AppSettings["MMEmailID"].ToString();
                                string strMMCCMailID = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                                strBody = string.Empty;

                                strBody += "Dear Team,<br/>";
                                strBody += strOrgName +" registered to MSETCL for online Grid Connectivity" + "<br/>";
                                strBody += "Please use following information to create or update Vendor ID. <br/>";
                                strBody += "Organization Name : " + strOrgName + "<br/>";
                                strBody += "Organization GSTIN/Provisional : " + strGISTN_No + " "+ strProvisionalGSTIN + "<br/>";
                                strBody += "Organization PAN : " + strPAN + "<br/>";
                                strBody += "Organization Address : " + strAdd1 + " " + strAdd2 + " " + strAdd3 + "<br/>";
                                strBody += "Organization State : " + strState + " <br/>Country : " + strCountry + "<br/>";
                                strBody += "Organization Pincode : " + strPIN + "<br/>";
                                strBody += "Organization Phone : " + strPhone + "<br/>";
                                strBody += "Organization Mobile : " + strMob+ "<br/>";
                                strBody += "Organization FAX : " + strFAX + "<br/>";
                                strBody += "Organization Email ID : " + strEmail + "<br/>";

                                strBody += "<br/>";
                                strBody += "Thanks & Regards, " + "<br/>";
                                strBody += "STU Department" + "<br/>";
                                strBody += "MSETCL  " + "<br/>";
                                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                                try
                                {


                                    #region using MailMessage
                                    MailMessage Msg = new MailMessage();
                                    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                                    Msg.From = fromMail;
                                    Msg.IsBodyHtml = true;
                                    //log.Error("from:" + fromAddress);
                                    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                                    Msg.To.Add(new MailAddress(strMMMailID));
                                    Msg.To.Add(new MailAddress(strMMCCMailID));


                                    //  Msg.To.Add(new MailAddress(toAddress));

                                    Msg.Subject = "Online Grid Connectivity Registration MSETCL";
                                    Msg.Body = strBody;
                                    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                    SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
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

                                #region Upload GSTIN
                                string folderPath = Server.MapPath("~/Files/GSTIN/");
                               
                                string newFileName = "";
                                if (Session["IsValid"] != "false")
                                {
                                    //Check whether Directory (Folder) exists.
                                    if (!Directory.Exists(folderPath))
                                    {
                                        //If Directory (Folder) does not exists Create it.
                                        Directory.CreateDirectory(folderPath);
                                    }
                                    if (FUGSTIN.HasFile)
                                    {
                                        
                                        //Save the File to the Directory (Folder).
                                        try
                                        {

                                            FUGSTIN.SaveAs(folderPath + Path.GetFileName(FUGSTIN.FileName));
                                            newFileName = strUname + "_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUGSTIN.FileName;
                                            System.IO.File.Move(folderPath + FUGSTIN.FileName, folderPath + newFileName);
                                            strQuery = "Update APPLICANT_REG_DET set GSTIN_CERTIFICATE='" + newFileName + "' where USER_NAME='" + strUname+ "'";
                                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                                    cmd.ExecuteNonQuery();
                                                    //lblGSTINStatus.Text = "Uploaded Successfully!!";
                                            

                                            
                                        }
                                        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                                        {
                                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                                            log.Error(ErrorMessage);
                                            lblResult.Text = "Letter Uploaded Failed!!";

                                        }
                                        catch (Exception exception)
                                        {
                                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                            log.Error(ErrorMessage);
                                            lblResult.Text = "Letter Uploaded Failed!!";
                                        }

                                       

                                    }



                                    //Display the Picture in Image control.
                                    //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
                                }

                                #endregion

                                #region Upload PAN
                                folderPath = Server.MapPath("~/Files/PAN/");

                                newFileName = "";
                                if (Session["IsValid"] != "false")
                                {
                                    //Check whether Directory (Folder) exists.
                                    if (!Directory.Exists(folderPath))
                                    {
                                        //If Directory (Folder) does not exists Create it.
                                        Directory.CreateDirectory(folderPath);
                                    }
                                    if (FUPAN.HasFile)
                                    {

                                        //Save the File to the Directory (Folder).
                                        try
                                        {

                                            FUPAN.SaveAs(folderPath + Path.GetFileName(FUPAN.FileName));
                                            newFileName = strUname + "_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUPAN.FileName;
                                            System.IO.File.Move(folderPath + FUPAN.FileName, folderPath + newFileName);
                                            strQuery = "Update APPLICANT_REG_DET set PAN_DOC='" + newFileName + "' where USER_NAME='" + strUname + "'";
                                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                                            cmd.ExecuteNonQuery();
                                            //lblGSTINStatus.Text = "Uploaded Successfully!!";



                                        }
                                        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                                        {
                                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                                            log.Error(ErrorMessage);
                                            lblResult.Text = "Letter Uploaded Failed!!";

                                        }
                                        catch (Exception exception)
                                        {
                                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                            log.Error(ErrorMessage);
                                            lblResult.Text = "Letter Uploaded Failed!!";
                                        }

                                        finally
                                        {
                                            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                                            {
                                                mySqlConnection.Close();
                                            }

                                        }

                                    }



                                    //Display the Picture in Image control.
                                    //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
                                }

                                #endregion

                                //divModal.Visible = true;
                                //string scriptText = "alert('Registration successfully done.You will be able to login after 1 HRS. '); window.location='" + "ConsumerDetail.aspx';";
                                //Response.Write("<script language='javascript'>alert('Registration done successfully. Kindly wait 24Hrs to 48 Hrs for login.');</script>");

                                //      ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
                                //    lblResult.Text = "Registration No is : " + strRegistrationno;
                                //ClientScript.RegisterStartupScript(this.GetType(), "alert", scriptText , true);
                                //Response.Write("<script language='javascript'>window.alert('" + scriptText + "');window.location='ConsumerDetail.aspx';</script>");
                                string message = "alert('Registration successfully done.You will be able to login after 1HR.')";
                                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "alert", message, true);
                                //Response.Redirect("~/UI/Cons/Home.aspx", false);
                                clearcontrol();
                            }
                            else
                            {
                                lblResult.Text = "Already Register";
                            }
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
                    lblResult.Text = "Some problem during registration.Please try again.";
                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    lblResult.Text = "Some problem during registration.Please try again.";
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
                lblResult.Text = "Please Upload documents correctly.";
            }
        }

        void clearcontrol()
        {
             txtGSTIN.Text="";
             txtOrgName.Text="";
             txtAddress1.Text="";
             txtAddress2.Text="";
             txtAddress3.Text="";
             txtPhone.Text="";
             txtMob.Text="";
             txtEmail.Text="";
             txtFax.Text="";
             txtPass.Text="";
             txtCPass.Text="";
             txtPAN.Text="";
             txtProvGSTIN.Text="";


        }
        protected void rbISGSTIN_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbISGSTIN.SelectedItem.Value == "Y")
            {
                dvProvGSTIN.Visible = false;
                txtGSTIN.Enabled = true;
                RequiredFieldValidator15.Enabled = false;
            }
            else
            {
                dvProvGSTIN.Visible = true;
                txtGSTIN.Enabled = false;
                RequiredFieldValidator15.Enabled = true;
            }
        }

        protected void lnkChkUName_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strUname = string.Empty;
            
            strUname=txtPAN.Text;
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        int cnt=0;
                        MySqlCommand cmd;
                        strQuery = "select count(1)  from APPLICANT_LOGIN_MASTER where USER_NAME='" + strUname + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        cnt = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());
                        if (cnt > 0)
                        {
                            Session["isLoginValid"] = "false";
                            lblLoginStatus.Text = "User Name already present.";
                        }
                        else
                        {
                            Session["isLoginValid"] = "true";
                            lblLoginStatus.Text = "User Name availaible.";
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



        protected void ValidateFileSize(object sender, ServerValidateEventArgs e)
        {
            Session["IsValid"] = "true";
            decimal sizeFUDoc1 = 0;
            decimal sizeFUDoc2 = 0;
            if (FUGSTIN.Visible == true) sizeFUDoc1 = Math.Round(((decimal)FUGSTIN.PostedFile.ContentLength / (decimal)1024), 2);
            if (FUPAN.Visible == true) sizeFUDoc2 = Math.Round(((decimal)FUPAN.PostedFile.ContentLength / (decimal)1024), 2);
            //if (FUDoc3.Visible == true) sizeFUDoc3 = Math.Round(((decimal)FUDoc3.PostedFile.ContentLength / (decimal)1024), 2);
            //if (FUDoc4.Visible == true) sizeFUDoc4 = Math.Round(((decimal)FUDoc4.PostedFile.ContentLength / (decimal)1024), 2);
            //if (FUDoc5.Visible == true) sizeFUDoc5 = Math.Round(((decimal)FUDoc5.PostedFile.ContentLength / (decimal)1024), 2);
            //if (FUDoc6.Visible == true) sizeFUDoc6 = Math.Round(((decimal)FUDoc6.PostedFile.ContentLength / (decimal)1024), 2);
            //if (FUDoc7.Visible == true) sizeFUDoc7 = Math.Round(((decimal)FUDoc7.PostedFile.ContentLength / (decimal)1024), 2);
            //if (FUDoc8.Visible == true) sizeFUDoc8 = Math.Round(((decimal)FUDoc8.PostedFile.ContentLength / (decimal)1024), 2);
            if (sizeFUDoc1 > 2048)
            {
                //customGSTINValidate.ErrorMessage = "File size must not exceed 2 MB.";
                e.IsValid = false;
                Session["IsValid"] = "false";
            }
            if (sizeFUDoc2 > 2048)
            {
                customPANValidate.ErrorMessage = "File size must not exceed 2 MB.";
                e.IsValid = false;
                Session["IsValid"] = "false";
            }
            
            if (Session["IsValid"] != "false")
            {
                e.IsValid = true;
                Session["IsValid"] = "true";
            }
         
        }
    }
}