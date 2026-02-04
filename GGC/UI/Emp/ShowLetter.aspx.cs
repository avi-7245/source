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


namespace GGC.UI.Emp
{
    public partial class ShowLetter : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ShowLetter));
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {
                
                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    string letterName = Request.QueryString["DocName"].ToString();
                    Session["APPID"] = ID;
                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"400px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/SLD/" + letterName));
                    //imgMedaLetter.ImageUrl = "~/Files/MEDA/" + letterName;
                    lblAppNo.Text += Session["APPID"].ToString();
                    if (Session["PROJID"].ToString() != "")
                    {
                        lblAppNo.Text += " And Project ID : " + Session["PROJID"].ToString();
                    }
                }
            }
        }
        protected void fillData(string ID)
        {

        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            string applicationId = Session["APPID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            string letterName = string.Empty;
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        string strQuery1 = string.Empty;
                        string strComments = txtComments.Text;
                        if (rbAddCap.SelectedItem.Value == "2")
                        {
                            strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=8, isMEDAApp='Y' , APP_STATUS_DT=CURDATE() , app_status='DOCUMENT VERIFIED.SITE TECHNICAL FESIBILITY REPORT PENDING.', comments='" + strComments + "' where APPLICATION_NO='" + applicationId + "'";
                            strQuery1 = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','DOCUMENT VERIFIED.SITE TECHNICAL FESIBILITY REPORT PENDING.',now(),'" + Session["SAPID"].ToString() + "')";
                        }
                        else
                        {
                            strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=10, isMEDAApp='Y' , APP_STATUS_DT=CURDATE() , app_status='TECHNICAL FEASIBILITY REPORT VERIFIED', comments='" + strComments + "' ,IsAddCapacity='" + rbAddCap.SelectedItem.Value + "'  where APPLICATION_NO='" + applicationId + "'";
                            strQuery1 = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','TECHNICAL FEASIBILITY REPORT VERIFIED',now(),'" + Session["SAPID"].ToString() + "')";
                        }

                        try
                        {
                            log.Error("proc_5_scheduler");  
                            SQLHelper.ExecuteDataset(mySqlConnection.ConnectionString, CommandType.StoredProcedure, "proc_5_scheduler");
                            log.Error("proc_7_scheduler");
                            SQLHelper.ExecuteDataset(mySqlConnection.ConnectionString, CommandType.StoredProcedure, "proc_7_scheduler");
                            log.Error("proc_16_scheduler");
                            SQLHelper.ExecuteDataset(mySqlConnection.ConnectionString, CommandType.StoredProcedure, "proc_16_scheduler");
                        }
                        catch (Exception exception)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                            log.Error(ErrorMessage);
                            // SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                        }
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                       // fillGrid();
                        #region Application Status tracking date
                        //strQuery1 = "insert into APPLICANT_STATUS_TRACKING " +
                        //        " values('" + applicationId + "','DOCUMENT VERIFIED.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery1, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "8");

                        #endregion
                        //strQuery = "select distinct a.*,b.*,c.email Zone_email,c.mobile from sop_email_mob where zone in (select zone from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        strQuery = " select distinct a.*,c.email Zone_email,c.mobile from APPLICANTDETAILS a,zone_district c where lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);


                        strQuery = " select * from sop_email_mob where district='" + dsAppDet.Tables[0].Rows[0]["project_district"].ToString() + "'";
                        cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsZoneDet = new DataSet();
                        daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsZoneDet);


                        string strEmail = dsZoneDet.Tables[0].Rows[0]["EMAIL_ID"].ToString();
                        string strZone = dsAppDet.Tables[0].Rows[0]["Zone"].ToString();
                        string strMobile = dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        string strCONT_PER_MOBILE_1 = dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        string strQueryMob = string.Empty;
                            string strMobNos = string.Empty;
                            strQueryMob = "select * from empmaster where role_id=53 or ZONE='" + strZone + "'";
                            MySqlCommand cmdMob = new MySqlCommand(strQueryMob, mySqlConnection);
                            DataSet dsMob = new DataSet();
                            MySqlDataAdapter daMob = new MySqlDataAdapter(cmdMob);
                            daMob.Fill(dsMob);

                        
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            if (rbAddCap.SelectedItem.Value == "2")
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
                                strBody += "You are requested to carry out the joint survey along with representative of M/s. " + dsAppDet.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " and inform to this office the various possible evacuation arrangements from the existing MSETCL network.<br/><br/>";
                                strBody += "The Technical Feasibility Report should have information such as:<br/>";
                                strBody += "1.	Nearest MSETCL’s Substation.<br/>";
                                strBody += "2.	Present load and capacity of transformers.<br/>";
                                strBody += "3.	Length of line from solar plant site to the MSETCL’s Substation.<br/>";
                                strBody += "4.	Availability of space at existing MSETCL’s Substation for line bays and transformer bays and possibility of procurement of adjacent land for Bus extension.<br/>";
                                strBody += "5.	Any augmentation in transformation capacity required.<br/>";
                                strBody += "6.	Any EHV Line / Railway / Highway / Forest crossing etc.<br/>";
                                strBody += "7.	Route proposed for Grid Connectivity.<br/>";
                                strBody += "8.	Any ROW problems.<br/>";
                                strBody += "<b>While issuing technical feasibility report, please ensure that, the existing & proposed RE power projects in the region are considered, along with the space availability for the proposed network already planned in STU plan. </b><br/>";
                                strBody += "<b>The Technical Feasibility Report should be submitted in the enclosed format for Technical Feasibility Report. </b><br/>";
                                strBody += "<b>The Technical Feasibility Report as per the enclosed format furnishing above- mentioned details should be submitted to this office within 15 days from the date of this letter. 	</b><br/>";
                                strBody += "Thanks & Regards, " + "<br/>";
                                strBody += " Chief Engineer / STU Department" + "<br/>";
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

                                    string strTo = strEmail;

                                    if (strTo != "")
                                    {
                                        string[] splittedCC = strTo.Split(',');
                                        foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                        {
                                            if (part != "")
                                                Msg.To.Add(new MailAddress(part.ToString()));
                                            //Msg.CC.Add(new MailAddress(part.ToString()));
                                            //log.Error("Part " + part);
                                        }
                                    }

                                    string strCC=ConfigurationManager.AppSettings["MMCCEmailID"].ToString();

                                    if (strCC != "")
                                    {
                                        string[] splittedCC = strCC.Split(',');
                                        foreach (var part in strCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                                        {
                                            if (part != "")
                                                Msg.CC.Add(new MailAddress(part.ToString()));
                                            //Msg.CC.Add(new MailAddress(part.ToString()));
                                            //log.Error("Part " + part);
                                        }
                                    }

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
                                //Response.Write("<script language='javascript'>alert('DOCUMENT VERIFIED.');</script>");

                                #endregion
                            }
                        }
                        //log.Error("dsAppDet.Tables[0].Rows.Count " + dsAppDet.Tables[0].Rows.Count);
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            
                            strMobNos = strMobile + ",";
                            //log.Error("strMobile " + strMobile);
                            
                            if (dsMob.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsMob.Tables[0].Rows.Count; i++)
                                {
                                    if (dsMob.Tables[0].Rows[i]["EmpMobile"].ToString()!="")
                                    strMobNos += dsMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                }

                            }
                            //log.Error("strMobNos " + strMobNos);

                            #region Send SMS
                            try
                            {
                                //log.Error("1 ");

                                string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Your%20application%20no." + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20is%20forwarded%20to%20Chief%20Engineer%2C%20" + dsAppDet.Tables[0].Rows[0]["ZONE"].ToString() + "%20for%20Technical%20Feasibility%20Report.%20Please%20visit%20field%20office%20for%20joint%20survey.%5CnRegards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                //log.Error("strURL " + strURL);

                                WebRequest request = HttpWebRequest.Create(strURL);
                                //log.Error("2 ");
                                // Get the response back  
                                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                Stream s = (Stream)response.GetResponseStream();
                                StreamReader readStream = new StreamReader(s);
                                string dataString = readStream.ReadToEnd();
                                log.Error("3 "+dataString);

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

                            //#region Send SMS
                            //try
                            //{

                            //    // use the API URL here  
                            //    //string strUrl = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=%22Your%20application%20no.12345678%20is%20forwarded%20to%20Chief%20Engineer%2C%20NASHIK%20for%20Revised%20Technical%20Feasibility%20Report.%20Please%20visit%20field%20office%20for%20joint%20survey.%5CnRegards%2C%20C.E.%20STU%2C%20MSETCL%22&MobileNumbers=" + strMobile + "," + strCONT_PER_MOBILE_1 + "&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
                            //    //http://164.52.205.46:6005/api/v2/SendSMS?SenderId=CESTU&Message=Due%20to%20non-receipt%20of%20payment%20for%20Commitment%20fee%2C%20your%20application%20no.12345%20for%20Grid%20Connectivity%20is%20cancelled.%20%5Cn%20Regards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=9768776677&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35
                            //    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Due%20to%20non-receipt%20of%20payment%20for%20Commitment%20fee%2C%20your%20application%20no.12345%20for%20Grid%20Connectivity%20is%20cancelled.%20%5Cn%20Regards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobile + "&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
                            //    //string strUrl = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=" + Uri.EscapeDataString("%22Your application no." + applicationId + " is forwarded to Chief Engineer, " + strZone + " for Technical Feasibility Report. Please visit field office for joint survey.\nRegards, C.E. STU, MSETCL%22") + "&MobileNumbers=" + strMobile + "," + strCONT_PER_MOBILE_1 + "&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
                            //    //string strUrl = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=" + "Your application no." + applicationId + " is forwarded to Chief Engineer, " + strZone + " for Technical Feasibility Report. Please visit field office for joint survey. \n\n Regards, C.E. STU, MSETCL&MobileNumbers=" + strMobile + "," + strCONT_PER_MOBILE_1 + "&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
                            //    // Create a request object  
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Document Verified.');window.location ='EmpHome.aspx';", true);
            Response.Redirect("~/UI/Emp/EmpHome.aspx", false);
 
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            string applicationId = Session["APPID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            string letterName = string.Empty;
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        string strComments = txtComments.Text;

                        strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=7, isMEDAApp='N' , APP_STATUS_DT=CURDATE() , app_status='SLD not approved.' , comments='" + strComments + "'  where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        
                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                " values('" + applicationId + "','SLD not approved.',now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();

                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "6");
                        Response.Write("<script language='javascript'>alert('Document Not return.');</script>");

                        #endregion
                        strQuery = "select distinct a.*,c.email Zone_email from APPLICANTDETAILS a,zone_district c where lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsAppDet = new DataSet();
                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                        daAppDet.Fill(dsAppDet);

                        string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
                        if (dsAppDet.Tables[0].Rows.Count > 0)
                        {
                            #region Send Mail
                            //sendMailOTP(strRegistrationno, strEmailID);
                            string strBody = string.Empty;

                            strBody += "Respected Sir/Madam" + ",<br/>";
                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["promotor_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            //strBody += "Please use following information for login for further process. <br/>";
                            strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>";
                            strBody += "You are requested to Upload SLD documents again  beacuse of following reason.<br/><b> " + txtComments.Text + " .</b> <br/>  <br/><br/><br/>";
                            strBody += "<a href=https://gctest.mahatransco.in:9999/UI/Cons/UploadReDoc.aspx?appid=" + applicationId + ">Click here</a>";
                            
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

                                Msg.Subject = "SLD document returned in Online Grid connectivity application.";
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Document Rejected.');window.location ='EmpHome.aspx';", true);
            Response.Redirect("~/UI/Emp/EmpHome.aspx", false);

 
        }

    }
}