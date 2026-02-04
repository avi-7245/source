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
using GGC.WebService;
namespace GGC.UI.Cons
{

    public partial class ConsumerDetail : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ConsumerDetail));



        protected void Page_Load(object sender, EventArgs e)
        {
            //txtLetterDt.Attributes.Add("readonly", "readonly");
            txtSchCommDt.Attributes.Add("readonly", "readonly");
            //CompareScheduledCommissioningValidator.ValueToCompare = DateTime.Now.ToShortDateString();
            CalendarExtender2.StartDate = DateTime.Now;
            if (!Page.IsPostBack)
            {
                Session["isValiMEDAID"] = "false";

                fillDistrict();
                fillAppType();
                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    //fillDistrict();

                    //string gstin = Request.QueryString["GSTIN"].ToString();
                    Session["APPID"] = ID;
                    Session["isValiMEDAID"] = "true";
                    fillData(ID);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["isValiMEDAID"].ToString() == "true")
            {
                string strCONT_PER_NAME_1, strCONT_PER_DESIG_1, strCONT_PER_PHONE_1, strCONT_PER_MOBILE_1, strCONT_PER_FAX_1, IsAddCapacity;
                string strCONT_PER_EMAIL_1, strCONT_PER_NAME_2, strCONT_PER_DESIG_2, strCONT_PER_PHONE_2, strCONT_PER_MOBILE_2;
                string strCONT_PER_FAX_2, strCONT_PER_EMAIL_2, strMEDA_REC_LETTER_NO, strMEDA_REC_LETTER_DT, strNATURE_OF_APP;
                string strPROJECT_TYPE, strPROJECT_CAPACITY_MW, strPROJECT_LOC, strPROJECT_TALUKA, strPROJECT_DISTRICT, strSCHEDULED_COMM_DATE;
                string ApplicationID = string.Empty;
                string stradd_corr = string.Empty;
                string strProjectID = string.Empty;
                string strAlreadyApplied = string.Empty;
                string strTYPE_OF_GENERATION, strQuantum_power_injected_MW, strPROMOTOR_NAME, strLatitude, strLongitude, strTYPE_OF_FUEL, strSTEP_UP_GEN_VOLT = "", strIS_CAPTIVE_POWER_PLANT, strNAME_INJECTING_PARTY, strNAME_OWNER_OF_SS, strINJECTED_EXISTING_PROPOSED_NW, strPPA_POWER_TOBE_INJECTED, strAGGREEMENT_WITH_TRADER, strSTATUS_FUEL_LINKAGE, strSTATUS_OF_WATER_SUPPLY, strTYPE_OF_LOAD, strQUANTUM_POWER_DRAWN, strDRAWAL_VOLTAGE_LEVEL, strREACTIVE_POWER_REQ;
                string schDD, schMM, schYYYY;
                strProjectID = txtProjCode.Text;
                strCONT_PER_NAME_1 = txtContPer1.Text;
                strCONT_PER_DESIG_1 = txtDesign1.Text;
                strCONT_PER_PHONE_1 = txtPhone1.Text;
                strCONT_PER_MOBILE_1 = txtMob1.Text;
                strCONT_PER_FAX_1 = txtFax1.Text;
                strCONT_PER_EMAIL_1 = txtEmail1.Text;
                strCONT_PER_NAME_2 = txtContPer2.Text;
                strCONT_PER_DESIG_2 = txtDesign2.Text;
                strCONT_PER_PHONE_2 = txtPhone2.Text;
                strCONT_PER_MOBILE_2 = txtMob2.Text;
                strCONT_PER_FAX_2 = txtFax2.Text;
                strCONT_PER_EMAIL_2 = txtEmail2.Text;
                stradd_corr = txtCorAdd.Text;
                //strMEDA_REC_LETTER_NO = txtLetterNo.Text;
                //if(txtLetterDt.Text!="")
                //    strMEDA_REC_LETTER_DT = txtLetterDt.Text;
                //else
                //    strMEDA_REC_LETTER_DT = "1900-01-01";

                strNATURE_OF_APP = ddlNatureOfApp.SelectedItem.Text;
                Session["apptype"] = strNATURE_OF_APP;
                Session["isAppliedMEDAID"] = rbOldYesNo.SelectedItem.Value;
                //if (ddlProjectType.SelectedItem.Value != "-1")
                if (hdnProjectType.Value != "-1")
                {
                    strPROJECT_TYPE = ddlProjectType.SelectedItem.Text;
                }
                else
                {
                    strPROJECT_TYPE = "Not Applicable";

                }
                strPROJECT_CAPACITY_MW = txtProjectCapacity.Text;
                strPROJECT_LOC = txtProjectLocation.Text;
                strPROJECT_TALUKA = txtTaluka.Text;
                strPROJECT_DISTRICT = ddlDistrict.SelectedItem.Text;
                schDD = txtSchCommDt.Text.Substring(0, 2);
                schMM = txtSchCommDt.Text.Substring(3, 2);
                schYYYY = txtSchCommDt.Text.Substring(6, 4);
                strSCHEDULED_COMM_DATE = schYYYY + "-" + schMM + "-" + schDD;

                strTYPE_OF_GENERATION = txtTypeOfGen.Text;
                if (txtQuantumeOfPower.Text != "")
                    strQuantum_power_injected_MW = txtQuantumeOfPower.Text;
                else
                    strQuantum_power_injected_MW = "0";
                strPROMOTOR_NAME = txtNameOfPromoter.Text;
                strLatitude = txtLatitude.Text.Replace("\"", "*").Replace("\'", "$");     //This is for replacing Doublequote("") with * and single quote(') $ for Lat aong format
                strLongitude = txtLongitude.Text.Replace("\"", "*").Replace("\'", "$");
                //strTYPE_OF_FUEL = ddlTypeOfFuel.SelectedItem.Text;
                strTYPE_OF_FUEL = "";
                //strSTEP_UP_GEN_VOLT = txtStepUpGen.Text;
                strIS_CAPTIVE_POWER_PLANT = RbCaptivePower.SelectedItem.Value;

                strTYPE_OF_LOAD = ddlConsTypeOfLoad.Text;
                strQUANTUM_POWER_DRAWN = txtConsQuantumPowerDrawn.Text;
                if (txtDrawalVoltLevel.Text != "")
                    strDRAWAL_VOLTAGE_LEVEL = txtDrawalVoltLevel.Text;
                else
                    strDRAWAL_VOLTAGE_LEVEL = "0";
                strREACTIVE_POWER_REQ = txtReactivePower.Text;
                IsAddCapacity = rbAddCap.SelectedItem.Value;
                string IssueLetterDt = txtGCIssueDt.Text;
                /*strNAME_INJECTING_PARTY = txt
                strNAME_OWNER_OF_SS = txt
                strINJECTED_EXISTING_PROPOSED_NW = txt
                strPPA_POWER_TOBE_INJECTED = txt
                strAGGREEMENT_WITH_TRADER = txt
                strSTATUS_FUEL_LINKAGE = txt
                strSTATUS_OF_WATER_SUPPLY = txt
            
                */

                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                string strGSTIN = string.Empty;
                string strUname = Session["user_name"].ToString();


                if (Session["GSTIN"] != null)
                    strGSTIN = Session["GSTIN"].ToString();

                try
                {

                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            MySqlCommand cmd;
                            cmd = new MySqlCommand("select * from APPLICANTDETAILS where MEDAProjectID='" + txtProjCode.Text + "'", mySqlConnection);
                            DataSet dsResult = new DataSet();
                            DataSet dsResultProjID = new DataSet();
                            MySqlDataAdapter daResult = new MySqlDataAdapter(cmd);
                            daResult.Fill(dsResultProjID);
                            //if (dsResultProjID.Tables[0].Rows.Count > 0)
                            //{
                            //    if (dsResultProjID.Tables[0].Rows[0]["IS_ALREADY_APPLIED"].ToString() == "Y")
                            //    {
                            //        Session["isAppliedMEDAID"] = "true";
                            //    }
                            //    else
                            //    {
                            //        Session["isAppliedMEDAID"] = "false";
                            //    }
                            //}
                            //if (dsResult.Tables[0].Rows.Count <= 0 && Session["NewApplication"].ToString() == "true")
                            //{
                            cmd = new MySqlCommand("select SRNO  from APPLICANTDETAILS where USER_NAME='" + strUname + "' order by srno desc", mySqlConnection);
                            dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            int maxSRNO = 0;
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {
                                maxSRNO = int.Parse(dsResult.Tables[0].Rows[0]["SRNO"].ToString());
                            }
                            if (dsResult.Tables[0].Rows.Count < 1 || Session["NewApplication"].ToString() == "true")
                            {
                                if (dsResultProjID.Tables[0].Rows.Count < 1)
                                {
                                    MySqlCommand cmdPAN = new MySqlCommand("select * from applicant_reg_det where USER_NAME='" + strUname + "'", mySqlConnection);
                                    DataSet dsResultPAN = new DataSet();
                                    MySqlDataAdapter daResultPAN = new MySqlDataAdapter(cmdPAN);
                                    daResultPAN.Fill(dsResultPAN);

                                    strQuery = "insert into APPLICANTDETAILS  (MEDAProjectID,GSTIN_NO,CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_FAX_1, CONT_PER_EMAIL_1, CONT_PER_NAME_2, CONT_PER_DESIG_2, CONT_PER_PHONE_2, CONT_PER_MOBILE_2, CONT_PER_FAX_2, CONT_PER_EMAIL_2, NATURE_OF_APP, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_TALUKA, PROJECT_DISTRICT, SCHEDULED_COMM_DATE,CREATED_DT,TYPE_OF_GENERATION ,Quantum_power_injected_MW ,PROMOTOR_NAME ,Latitude ,Longitude ,TYPE_OF_FUEL ,STEP_UP_GEN_VOLT ,IS_CAPTIVE_POWER_PLANT, TYPE_OF_LOAD ,QUANTUM_POWER_DRAWN ,DRAWAL_VOLTAGE_LEVEL ,REACTIVE_POWER_REQ,app_status,WF_STATUS_CD_C,USER_NAME,PAN_TAN_NO,IS_ALREADY_APPLIED, IsAddCapacity,GCApprLetterNo_Date,Address_correspondence) " +
                                            " values('" + strProjectID + "','" + strGSTIN + "','" + strCONT_PER_NAME_1 + "','" + strCONT_PER_DESIG_1 + "','" + strCONT_PER_PHONE_1 + "','" + strCONT_PER_MOBILE_1 + "','" + strCONT_PER_FAX_1 + "','" + strCONT_PER_EMAIL_1 + "','" + strCONT_PER_NAME_2 + "','" + strCONT_PER_DESIG_2 + "','" + strCONT_PER_PHONE_2 + "','" + strCONT_PER_MOBILE_2 + "','" + strCONT_PER_FAX_2 + "','" + strCONT_PER_EMAIL_2 + "','" + strNATURE_OF_APP + "','" + strPROJECT_TYPE + "','" + strPROJECT_CAPACITY_MW + "','" + strPROJECT_LOC + "','" + strPROJECT_TALUKA + "','" + strPROJECT_DISTRICT + "','" + strSCHEDULED_COMM_DATE + "',CURDATE(),'" + strTYPE_OF_GENERATION + "','" + strQuantum_power_injected_MW + "','" + strPROMOTOR_NAME + "','" + strLatitude + "','" + strLongitude + "','" + strTYPE_OF_FUEL + "','" + strSTEP_UP_GEN_VOLT + "','" + strIS_CAPTIVE_POWER_PLANT + "','" + strTYPE_OF_LOAD + "','" + strQUANTUM_POWER_DRAWN + "','" + strDRAWAL_VOLTAGE_LEVEL + "','" + strREACTIVE_POWER_REQ + "','APPLICATION INCOMPLETE','1','" + strUname + "','" + dsResultPAN.Tables[0].Rows[0]["PAN_TAN_NO"].ToString() + "','" + rbOldYesNo.SelectedItem.Value + "','" + rbAddCap.SelectedItem.Value + "','" + IssueLetterDt + "','" + stradd_corr + "')";

                                    //strQuery = "insert into APPLICANTDETAILS  (MEDAProjectID,GSTIN_NO,CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_FAX_1, CONT_PER_EMAIL_1, CONT_PER_NAME_2, CONT_PER_DESIG_2, CONT_PER_PHONE_2, CONT_PER_MOBILE_2, CONT_PER_FAX_2, CONT_PER_EMAIL_2, NATURE_OF_APP, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_TALUKA, PROJECT_DISTRICT, SCHEDULED_COMM_DATE,CREATED_DT,TYPE_OF_GENERATION ,Quantum_power_injected_MW ,PROMOTOR_NAME ,Latitude ,Longitude ,TYPE_OF_FUEL ,STEP_UP_GEN_VOLT ,IS_CAPTIVE_POWER_PLANT, TYPE_OF_LOAD ,QUANTUM_POWER_DRAWN ,DRAWAL_VOLTAGE_LEVEL ,REACTIVE_POWER_REQ,app_status,WF_STATUS_CD_C,USER_NAME,PAN_TAN_NO) " +
                                    //        " values('" + strProjectID + "','" + strGSTIN + "','" + strCONT_PER_NAME_1 + "','" + strCONT_PER_DESIG_1 + "','" + strCONT_PER_PHONE_1 + "','" + strCONT_PER_MOBILE_1 + "','" + strCONT_PER_FAX_1 + "','" + strCONT_PER_EMAIL_1 + "','" + strCONT_PER_NAME_2 + "','" + strCONT_PER_DESIG_2 + "','" + strCONT_PER_PHONE_2 + "','" + strCONT_PER_MOBILE_2 + "','" + strCONT_PER_FAX_2 + "','" + strCONT_PER_EMAIL_2 + "','" + strNATURE_OF_APP + "','" + strPROJECT_TYPE + "','" + strPROJECT_CAPACITY_MW + "','" + strPROJECT_LOC + "','" + strPROJECT_TALUKA + "','" + strPROJECT_DISTRICT + "','" + strSCHEDULED_COMM_DATE + "',CURDATE(),'" + strTYPE_OF_GENERATION + "','" + strQuantum_power_injected_MW + "','" + strPROMOTOR_NAME + "','" + strLatitude + "','" + strLongitude + "','" + strTYPE_OF_FUEL + "','" + strSTEP_UP_GEN_VOLT + "','" + strIS_CAPTIVE_POWER_PLANT + "','" + strTYPE_OF_LOAD + "','" + strQUANTUM_POWER_DRAWN + "','" + strDRAWAL_VOLTAGE_LEVEL + "','" + strREACTIVE_POWER_REQ + "','APPLICATION INCOMPLETE','1','" + strUname + "','" + dsResultPAN.Tables[0].Rows[0]["PAN_TAN_NO"].ToString() + "')";

                                    log.Error("Insert " + strQuery);
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    cmd = new MySqlCommand("select SRNO  from APPLICANTDETAILS where USER_NAME='" + strUname + "' order by srno desc", mySqlConnection);
                                    dsResult = new DataSet();
                                    da = new MySqlDataAdapter(cmd);
                                    da.Fill(dsResult);
                                    maxSRNO = 0;
                                    if (dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        maxSRNO = int.Parse(dsResult.Tables[0].Rows[0]["SRNO"].ToString());
                                    }
                                    ApplicationID = ddlNatureOfApp.SelectedValue;
                                    string strProjectTypeVal = string.Empty;
                                    strProjectTypeVal = ddlProjectType.SelectedValue;
                                    //(A) Having Project type in application id 13.09.2021 and added column project_type_value  in applicant_project_type table
                                    //if (ddlNatureOfApp.SelectedValue == "1" || ddlNatureOfApp.SelectedValue == "2")
                                    //{
                                    //ApplicationID += ddlNatureOfApp.SelectedValue.PadLeft(2, '0'); // 13.09.2021
                                    ApplicationID += strProjectTypeVal.PadLeft(2, '0'); // 13.09.2021
                                    //}
                                    //else  //For EHV Consumer
                                    //{
                                    //    ApplicationID += "00";
                                    //}
                                    // End (A)
                                    ApplicationID += strPROJECT_CAPACITY_MW.Replace('.', '0').PadLeft(4, '0');
                                    ApplicationID += maxSRNO.ToString().PadLeft(7, '0');
                                    strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=1,APPLICATION_NO= " + ApplicationID + " , APP_STATUS_DT=CURDATE() where USER_NAME='" + strUname + "' and srno=" + maxSRNO;
                                    log.Error("Update application no " + strQuery);

                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();

                                    #region Application Status tracking date
                                    strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                            " values('" + ApplicationID + "','Application Incomplete',CURDATE(),'" + strUname + "')";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    #endregion

                                    cmd = new MySqlCommand("select distinct zone from zone_district where district =(select PROJECT_DISTRICT from applicantdetails where application_no='" + ApplicationID + "')", mySqlConnection);
                                    dsResult = new DataSet();
                                    da = new MySqlDataAdapter(cmd);
                                    da.Fill(dsResult);
                                    string strZone = dsResult.Tables[0].Rows[0][0].ToString();
                                    strQuery = "Update APPLICANTDETAILS set zone= '" + strZone + "' where USER_NAME='" + strUname + "' and srno=" + maxSRNO;
                                    log.Error("Update application no " + strQuery);

                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    Session["NatureOfApp"] = strNATURE_OF_APP;
                                    Session["ProjectType"] = strPROJECT_TYPE;
                                    Session["APPID"] = ApplicationID;
                                    Session["ProjectID"] = strProjectID;
                                    #region Send Mail
                                    //sendMailOTP(strRegistrationno, strEmailID);
                                    string strBody = string.Empty;

                                    strBody += "Dear User" + ",<br/>";
                                    strBody += "Thank you for applying Grid Connectivity Application." + "<br/>";
                                    strBody += "Your Application ID for further reference is : " + ApplicationID + "<br/>";
                                    //strBody += "Full Name " + "<br/>";
                                    //strBody += "Advertisement No : " + strAdvDesc + "\n";
                                    //strBody += "Please use following information for login for further process. <br/>";
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
                                        Msg.To.Add(new MailAddress(strCONT_PER_EMAIL_1));
                                        //Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["EmailServer"].ToString()));


                                        //  Msg.To.Add(new MailAddress(toAddress));

                                        Msg.Subject = "Online Grid connectivity application.";
                                        Msg.Body = strBody;
                                        //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                        SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                                        a.Host = "smtp.office365.com";
                                        a.EnableSsl = true;
                                        NetworkCredential n = new NetworkCredential();
                                        n.UserName = "donotreply@mahatransco.in";
                                        n.Password = "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~";
                                        a.UseDefaultCredentials = false;
                                        a.Credentials = n;
                                        a.Port = 587;
                                        System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                                        a.Send(Msg);

                                        // a.Send(Msg);

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
                                    //try
                                    //{

                                    //    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Payment%20of%20processing%20fee%20towards%20Grid%20Connectivity%20application%20no.1234%20is%20received.%5CnRegards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobile + "&ApiKey=J3OFLVtym38L6HF7LXZlTFlaEIUvvmyDP90r2DasRqI%3D&ClientId=57b6632f-ac04-41cf-8907-2ba861acfc35";
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
                                    Session["NatureOfApp"] = strNATURE_OF_APP;
                                    Session["ProjectType"] = strPROJECT_TYPE;
                                    Session["APPID"] = ApplicationID;
                                    Session["ProjectID"] = strProjectID;
                                    //saveApp(strProjectID);
                                    Response.Redirect("~/UI/Cons/ProjectDetail.aspx", false);
                                }
                                else
                                {
                                    lblProjIDResult.Text = "Project ID already Exists.";
                                    lblProjIDResult.ForeColor = System.Drawing.Color.Red;
                                }
                            }
                            else
                            {
                                //strQuery = "UPDATE APPLICANTDETAILS SET CONT_PER_NAME_1 ='"+ strCONT_PER_NAME_1+"',CONT_PER_DESIG_1 ='"+ strCONT_PER_DESIG_1+"', CONT_PER_PHONE_1 = '"+ strCONT_PER_PHONE_1+"', CONT_PER_MOBILE_1 ='"+ strCONT_PER_MOBILE_1+"', CONT_PER_FAX_1 ='"+ strCONT_PER_FAX_1+"', 	CONT_PER_EMAIL_1 ='"+ strCONT_PER_EMAIL_1+"', 	CONT_PER_NAME_2 = '"+strCONT_PER_NAME_2+"',	CONT_PER_DESIG_2 = '"+strCONT_PER_DESIG_2+"', CONT_PER_PHONE_2 ='"+ strCONT_PER_PHONE_2+"',	CONT_PER_MOBILE_2 = '"+strCONT_PER_MOBILE_2+"', CONT_PER_FAX_2 ='"+ strCONT_PER_FAX_2+"', CONT_PER_EMAIL_2 ='"+ strCONT_PER_EMAIL_2+"', MEDA_REC_LETTER_NO ='"+ strMEDA_REC_LETTER_NO+"', MEDA_REC_LETTER_DT ='"+ strMEDA_REC_LETTER_DT+"', NATURE_OF_APP ='"+ strNATURE_OF_APP+"', PROJECT_TYPE ='"+ strPROJECT_TYPE+"',PROJECT_CAPACITY_MW ='"+ strPROJECT_CAPACITY_MW+"', PROJECT_LOC = '"+strPROJECT_LOC+"', PROJECT_TALUKA = '"+strPROJECT_TALUKA+"', PROJECT_DISTRICT = '"+strPROJECT_DISTRICT+"', SCHEDULED_COMM_DATE ='"+ strSCHEDULED_COMM_DATE+"', SCHEDULED_COMM_DATE ='"+ strSCHEDULED_COMM_DATE+"' WHERE GSTIN_NO='" + strGSTIN + "' AND SRNO="+maxSRNO;
                                strQuery = "UPDATE APPLICANTDETAILS SET CONT_PER_NAME_1 ='" + strCONT_PER_NAME_1 + "',CONT_PER_DESIG_1 ='" + strCONT_PER_DESIG_1 + "', CONT_PER_PHONE_1 = '" + strCONT_PER_PHONE_1 + "', CONT_PER_MOBILE_1 ='" + strCONT_PER_MOBILE_1 + "', CONT_PER_FAX_1 ='" + strCONT_PER_FAX_1 + "', 	CONT_PER_EMAIL_1 ='" + strCONT_PER_EMAIL_1 + "', 	CONT_PER_NAME_2 = '" + strCONT_PER_NAME_2 + "',	CONT_PER_DESIG_2 = '" + strCONT_PER_DESIG_2 + "', CONT_PER_PHONE_2 ='" + strCONT_PER_PHONE_2 + "',	CONT_PER_MOBILE_2 = '" + strCONT_PER_MOBILE_2 + "', CONT_PER_FAX_2 ='" + strCONT_PER_FAX_2 + "', CONT_PER_EMAIL_2 ='" + strCONT_PER_EMAIL_2 + "',  NATURE_OF_APP ='" + strNATURE_OF_APP + "', PROJECT_TYPE ='" + strPROJECT_TYPE + "',PROJECT_CAPACITY_MW ='" + strPROJECT_CAPACITY_MW + "', PROJECT_LOC = '" + strPROJECT_LOC + "', PROJECT_TALUKA = '" + strPROJECT_TALUKA + "', PROJECT_DISTRICT = '" + strPROJECT_DISTRICT + "', SCHEDULED_COMM_DATE ='" + strSCHEDULED_COMM_DATE + "', TYPE_OF_GENERATION ='" + strTYPE_OF_GENERATION + "',Quantum_power_injected_MW =" + strQuantum_power_injected_MW + ",PROMOTOR_NAME ='" + strPROMOTOR_NAME + "', Latitude ='" + strLatitude + "', Longitude ='" + strLongitude + "', TYPE_OF_FUEL ='" + strTYPE_OF_FUEL + "', STEP_UP_GEN_VOLT ='" + strSTEP_UP_GEN_VOLT + "', IS_CAPTIVE_POWER_PLANT ='" + strIS_CAPTIVE_POWER_PLANT + "',TYPE_OF_LOAD ='" + strTYPE_OF_LOAD + "',QUANTUM_POWER_DRAWN  ='" + strQUANTUM_POWER_DRAWN + "',DRAWAL_VOLTAGE_LEVEL  =" + strDRAWAL_VOLTAGE_LEVEL + ",REACTIVE_POWER_REQ ='" + strREACTIVE_POWER_REQ + "',IS_ALREADY_APPLIED='" + rbOldYesNo.SelectedItem.Value + "',IsAddCapacity='" + rbAddCap.SelectedItem.Value + "',GCApprLetterNo_Date='" + IssueLetterDt + "',Address_correspondence='" + stradd_corr + "' WHERE USER_NAME='" + strUname + "' AND APPLICATION_NO='" + Session["APPID"].ToString() + "'";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Session["ProjectID"] = strProjectID;
                                Response.Redirect("~/UI/Cons/ProjectDetail.aspx", false);
                            }





                            //   string scriptText = "alert('Registration successfully done. '); window.location='" + "ConsumerDetail.aspx';";

                            //      ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
                            //    lblResult.Text = "Registration No is : " + strRegistrationno;
                            //ClientScript.RegisterStartupScript(this.GetType(), "alert", scriptText , true);
                            // Response.Write("<script language='javascript'>window.alert('" + scriptText + "');window.location='ConsumerDetail.aspx';</script>");




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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Not a valid Project ID.');window.location ='ConsumerDetail.aspx';", true);
                //   lblResult.Text = "Not a valid Project ID.";
            }
        }

        protected void fillDistrict()
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
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select distinct district district from zone_district order by 1", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        ddlDistrict.DataSource = dsResult.Tables[0];
                        ddlDistrict.DataValueField = "district";
                        ddlDistrict.DataTextField = "district";
                        ddlDistrict.DataBind();
                        ddlDistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlDistrict.ClearSelection();
                        ddlDistrict.SelectedIndex = 0;
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
        protected void fillData(string appId)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            txtProjCode.Enabled = false;
            ddlNatureOfApp.Enabled = false;
            ddlProjectType.Enabled = false;
            //strGSTIN = Session["GSTIN"].ToString();
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select *,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F   from APPLICANTDETAILS where APPLICATION_NO='" + appId + "'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\GrantGridConnectivity\GrantGridConnectivity_Sln\GGC\XMLSCHEMA\ConsumerDet.xsd");
                        if (dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() != "")
                        {
                            txtProjCode.Text = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                        }
                        txtContPer1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString();
                        txtDesign1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString();
                        txtPhone1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_PHONE_1"].ToString();
                        txtMob1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        txtFax1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_FAX_1"].ToString();
                        txtEmail1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                        txtContPer2.Text = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_2"].ToString();
                        txtDesign2.Text = dsResult.Tables[0].Rows[0]["CONT_PER_DESIG_2"].ToString();
                        txtPhone2.Text = dsResult.Tables[0].Rows[0]["CONT_PER_PHONE_2"].ToString();
                        txtMob2.Text = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_2"].ToString();
                        txtFax2.Text = dsResult.Tables[0].Rows[0]["CONT_PER_FAX_2"].ToString();
                        txtEmail2.Text = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_2"].ToString();
                        if (dsResult.Tables[0].Rows[0]["IS_ALREADY_APPLIED"].ToString() == "Y")
                            rbOldYesNo.Items.FindByValue("Y").Selected = true;
                        else
                            rbOldYesNo.Items.FindByValue("N").Selected = true;

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            rbOldYesNo.Enabled = false;
                        }
                        //txtLetterNo.Text = dsResult.Tables[0].Rows[0]["MEDA_REC_LETTER_NO"].ToString();
                        //txtLetterDt.Text = dsResult.Tables[0].Rows[0]["MEDADateF"].ToString();
                        // ddlNatureOfApp.SelectedItem.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                        log.Error("1");
                        //ddlNatureOfApp.Items.FindByText(dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString()).Selected = true;
                        //ddlProjectType.SelectedItem.Text = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                        log.Error("2");
                        //if (dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString() == "EHV Consumer")
                        //{
                        //    ddlProjectType.SelectedIndex = 0;
                        //}
                        //else
                        //{
                        ddlNatureOfApp.ClearSelection();
                        ddlNatureOfApp.SelectedIndex = ddlNatureOfApp.Items.IndexOf(ddlNatureOfApp.Items.FindByText(dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString()));
                        ddlNatureOfApp_SelectedIndexChanged(this, EventArgs.Empty);
                        ddlProjectType.ClearSelection();
                        ddlProjectType.SelectedIndex = ddlProjectType.Items.IndexOf(ddlProjectType.Items.FindByText(dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString().Trim().ToUpper()));
                        //}
                        log.Error("3");
                        txtProjectCapacity.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                        txtProjectLocation.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();
                        txtTaluka.Text = dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString();
                        //ddlDistrict.SelectedValue = dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString();
                        //fillDistrict();
                        ddlDistrict.ClearSelection();
                        ddlDistrict.SelectedIndex = ddlDistrict.Items.IndexOf(ddlDistrict.Items.FindByText(dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString()));
                        log.Error("4");

                        string strSCHEDULED_COMM_DATE, schDD, schMM, schYYYY;
                        strSCHEDULED_COMM_DATE = dsResult.Tables[0].Rows[0]["SCHEDULED_COMM_DATE_F"].ToString();

                        schDD = strSCHEDULED_COMM_DATE.Substring(8, 2);
                        schMM = strSCHEDULED_COMM_DATE.Substring(5, 2);
                        schYYYY = strSCHEDULED_COMM_DATE.Substring(0, 4);
                        txtSchCommDt.Text = schDD + "-" + schMM + "-" + schYYYY;
                        txtTypeOfGen.Text = dsResult.Tables[0].Rows[0]["TYPE_OF_GENERATION"].ToString();
                        txtQuantumeOfPower.Text = dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString();
                        txtNameOfPromoter.Text = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                        txtLatitude.Text = dsResult.Tables[0].Rows[0]["Latitude"].ToString().Replace("*", "\"").Replace("$", "\'");
                        txtLongitude.Text = dsResult.Tables[0].Rows[0]["Longitude"].ToString().Replace("*", "\"").Replace("$", "\'");
                        if (dsResult.Tables[0].Rows[0]["IsAddCapacity"].ToString() == "Y")
                            rbAddCap.Items.FindByValue("Y").Selected = true;
                        else
                            rbAddCap.Items.FindByValue("N").Selected = true;
                        txtGCIssueDt.Text = dsResult.Tables[0].Rows[0]["GCApprLetterNo_Date"].ToString();

                        //        log.Error("5");
                        //ddlTypeOfFuel.ClearSelection();
                        //ddlTypeOfFuel.Items.FindByText(dsResult.Tables[0].Rows[0]["TYPE_OF_FUEL"].ToString()).Selected = true;
                        log.Error("6");
                        //txtStepUpGen.Text = dsResult.Tables[0].Rows[0]["STEP_UP_GEN_VOLT"].ToString();
                        RbCaptivePower.SelectedValue = dsResult.Tables[0].Rows[0]["IS_CAPTIVE_POWER_PLANT"].ToString();
                        //RbCaptivePower.Items.FindByText(dsResult.Tables[0].Rows[0]["IS_CAPTIVE_POWER_PLANT"].ToString()).Selected = true;
                        if (dsResult.Tables[0].Rows[0]["TYPE_OF_LOAD"].ToString() != "-1")
                        {
                            ddlConsTypeOfLoad.ClearSelection();
                            ddlConsTypeOfLoad.SelectedIndex = ddlConsTypeOfLoad.Items.IndexOf(ddlConsTypeOfLoad.Items.FindByText(dsResult.Tables[0].Rows[0]["TYPE_OF_LOAD"].ToString()));
                        }
                        txtConsQuantumPowerDrawn.Text = dsResult.Tables[0].Rows[0]["QUANTUM_POWER_DRAWN"].ToString();
                        txtDrawalVoltLevel.Text = dsResult.Tables[0].Rows[0]["DRAWAL_VOLTAGE_LEVEL"].ToString();
                        txtReactivePower.Text = dsResult.Tables[0].Rows[0]["REACTIVE_POWER_REQ"].ToString();
                        log.Error("7");

                        if (dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString().Contains("Consumer") || dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString() == "Conventional Generator")
                        {
                            //ddlProjectType.Enabled = false;
                            //RQFVProjectType.Enabled = false;
                            //trMEDA.Visible = false;
                            trConsTypeOfLoad.Visible = true;
                            trConsLoadDemand.Visible = true;
                            //   trCoOrdinates.Visible = false;
                        }
                        else
                        {
                            //ddlProjectType.Enabled = true;
                            //RQFVProjectType.Enabled = true;
                            //trMEDA.Visible = true;
                            trConsTypeOfLoad.Visible = false;
                            trConsLoadDemand.Visible = false;
                            // trCoOrdinates.Visible = false;
                        }
                        log.Error("8");

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
        protected void fillAppType()
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
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select distinct app_type ,app_type_no from applicant_project_type where app_type like 'NonConventional Generator'  order by 2", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        ddlNatureOfApp.DataSource = dsResult.Tables[0];
                        ddlNatureOfApp.DataValueField = "app_type_no";
                        ddlNatureOfApp.DataTextField = "app_type";
                        ddlNatureOfApp.DataBind();
                        //ddlNatureOfApp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                        ddlNatureOfApp.ClearSelection();
                        ddlNatureOfApp.SelectedIndex = 0;
                        ddlNatureOfApp_SelectedIndexChanged(this, EventArgs.Empty);
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
                        objGetStatusDTO.statusId = "1";
                        APICall apiCall = new APICall();
                        string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + Session["user_name"].ToString() + "&projId=" + txtProjCode.Text + "&entity=MSETCL");

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
        protected void ddlNatureOfApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlNatureOfApp.SelectedItem.Value == "3" || ddlNatureOfApp.SelectedItem.Value == "1")
            {
                //  ddlProjectType.Enabled = false;
                //RQFVProjectType.Enabled = false;
                //trMEDA.Visible = false;
                trConsTypeOfLoad.Visible = true;
                trConsLoadDemand.Visible = true;
                //trCoOrdinates.Visible = false;
            }
            else
            {
                //ddlProjectType.Enabled = true;
                //RQFVProjectType.Enabled = true;
                //trMEDA.Visible = true;
                trConsTypeOfLoad.Visible = false;
                trConsLoadDemand.Visible = false;
                //trCoOrdinates.Visible = false;  
            }

            //MySqlConnection mySqlConnection = new MySqlConnection();
            //mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //int apptypeno = int.Parse(ddlNatureOfApp.SelectedItem.Value);

            //try
            //{

            //    mySqlConnection.Open();
            //    

            //    switch (mySqlConnection.State)
            //    {

            //        case System.Data.ConnectionState.Open:
            //            string strQuery = string.Empty;
            //            MySqlCommand cmd;
            //            cmd = new MySqlCommand("select project_type,project_type_value from applicant_project_type  where app_type_no =" + apptypeno + " order by 2", mySqlConnection);
            //            DataSet dsResult = new DataSet();
            //            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            //            da.Fill(dsResult);
            //            ddlProjectType.DataSource = dsResult.Tables[0];
            //            ddlProjectType.DataValueField = "project_type_value";
            //            ddlProjectType.DataTextField = "project_type";
            //            ddlProjectType.DataBind();
            //            ddlProjectType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            //            ddlProjectType.ClearSelection();
            //            ddlProjectType.SelectedIndex = 0;
            //            break;

            //        case System.Data.ConnectionState.Closed:

            //            // Connection could not be made, throw an error

            //            throw new Exception("The database connection state is Closed");

            //            break;

            //        default:

            //            // Connection is actively doing something else

            //            break;

            //    }


            //    // Place Your Code Here to Process Data //

            //}

            //catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
            //    log.Error(ErrorMessage);
            //    // Use the mySqlException object to handle specific MySql errors
            //    lblResult.Text = "Some problem during registration.Please try again.";
            //}

            //catch (Exception exception)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
            //    log.Error(ErrorMessage);
            //    // Use the exception object to handle all other non-MySql specific errors
            //    lblResult.Text = "Some problem during registration.Please try again.";
            //}

            //finally
            //{

            //    // Make sure to only close connections that are not in a closed state

            //    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
            //    {

            //        // Close the connection as a good Garbage Collecting practice

            //        mySqlConnection.Close();

            //    }

            //}

        }

        protected void ddlProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectType.SelectedItem.Value == "1" || ddlProjectType.SelectedItem.Value == "2" || ddlProjectType.SelectedItem.Value == "3" || ddlProjectType.SelectedItem.Value == "4")
            {
                trStatusOfApp.Visible = true;
            }
            else
            {
                trStatusOfApp.Visible = false;

            }
        }
        private string BuildUserAuthenticationURI(string projId, string entity, string securityKey)
        {
            // Base URL from configuration
            string baseUrl = ConfigurationManager.AppSettings["MEDAGETPRJDETURL"].ToString();

            // Use a StringBuilder or UrlEncoder for safer URL creation
            var queryParameters = $"?projId={HttpUtility.UrlEncode(projId)}&entity={HttpUtility.UrlEncode(entity)}&secKey={HttpUtility.UrlEncode(securityKey)}";

            return $"{baseUrl}{queryParameters}";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            APICall apiCall = new APICall();
            string securityKey = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?projId=" + txtProjCode.Text + "&entity=MSETCL");

            try
            {
                string userAuthenticationURI = BuildUserAuthenticationURI(txtProjCode.Text, "MSETCL", securityKey);

                WebRequest req = WebRequest.Create(userAuthenticationURI);
                req.Method = "GET";

                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                StreamReader reader = new StreamReader(resp.GetResponseStream());
                string responseText = reader.ReadToEnd();

                GetProjectDetails objProjDet = JsonConvert.DeserializeObject<GetProjectDetails>(responseText);
                if (Session["generatorId"].ToString() == objProjDet.createdBy)
                {
                    if (objProjDet.createdDt != null)
                    {
                        DateTime dt = DateTime.ParseExact(objProjDet.createdDt, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        txtProjectCapacity.Text = objProjDet.projectCapacityMw;
                        txtProjectLocation.Text = objProjDet.gutNo + " " + objProjDet.villageName;
                        txtTaluka.Text = objProjDet.talukaName;
                        txtProjectCapacity.Enabled = false;
                        txtProjectLocation.Enabled = false;
                        txtTaluka.Enabled = false;

                        ddlProjectType.ClearSelection();
                        string projectTypeName = objProjDet.projectTypeName.ToUpper().Trim();
                        if (projectTypeName == "HYBRID")
                        {
                            ddlProjectType.SelectedIndex = ddlProjectType.Items.IndexOf(ddlProjectType.Items.FindByText("HYBRID (Colocated)"));
                        }
                        else
                        {
                            ddlProjectType.SelectedIndex = ddlProjectType.Items.IndexOf(ddlProjectType.Items.FindByText(projectTypeName));
                        }

                        hdnProjectType.Value = ddlProjectType.SelectedItem.Value;
                        ddlNatureOfApp.Enabled = false;
                        ddlProjectType.Enabled = false;
                        fetchRegDet();
                        Session["isValiMEDAID"] = "true";
                    }
                    else
                    {
                        Session["isValiMEDAID"] = "false";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{objProjDet.response}');window.location ='ConsumerDetail.aspx';", true);
                        txtProjCode.Text = string.Empty;
                    }
                }
               else
              {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Project ID does not belong to current user!');window.location ='ConsumerDetail.aspx';", true);
                    txtProjCode.Text = string.Empty;
              }
            }
            catch (Exception ex)
            {
                string errorMessage = $"Method Name: {MethodBase.GetCurrentMethod().Name} | Description: {ex.Message} {ex.InnerException}";
                log.Error(errorMessage);
                Response.Write("<script language='javascript'>alert('Kindly try after some time.');</script>");
            }
        }

        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    APICall apiCall = new APICall();
        //    string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?projId=" + txtProjCode.Text + "&entity=MSETCL");

        //    try
        //    {
        //        //                                string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projid=" + txtProjCode + "&entity=MSETCL&secKey=" + str;
        //        // string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projId=" + txtProjCode.Text + "&entity=MSETCL&secKey=" + str;
        //        string userAuthenticationURI = ConfigurationManager.AppSettings["MEDAGETPRJDETURL"].ToString() + "?projId=" + txtProjCode.Text + "&entity=MSETCL&secKey=" + str;

        //        WebRequest req = WebRequest.Create(@userAuthenticationURI);


        //        req.Method = "GET";
        //        //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
        //        //req.Credentials = new NetworkCredential("username", "password");
        //        HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

        //        StreamReader reader = new StreamReader(resp.GetResponseStream());

        //        string responseText = reader.ReadToEnd();

        //        //if your response is in json format just uncomment below line  

        //        //Response.AddHeader("Content-type", "text/json");  

        //        //Response.Write(responseText);

        //        GetProjectDetails objProjDet = JsonConvert.DeserializeObject<GetProjectDetails>(responseText);
        //        if (Session["generatorId"].ToString() == objProjDet.createdBy)
        //        {
        //            if (objProjDet.createdDt != null)
        //            {
        //                DateTime dt = DateTime.ParseExact(objProjDet.createdDt, "dd-MM-yyyy",
        //                  CultureInfo.InvariantCulture);
        //                txtProjectCapacity.Text = objProjDet.projectCapacityMw;
        //                txtProjectLocation.Text = objProjDet.gutNo + " " + objProjDet.villageName;
        //                txtTaluka.Text = objProjDet.talukaName;
        //                txtProjectCapacity.Enabled = false;
        //                txtProjectLocation.Enabled = false;
        //                txtTaluka.Enabled = false;
        //                //ddlProjectType.SelectedItem.Text = objProjDet.projectTypeName;
        //                if (objProjDet.projectTypeName.ToUpper() == "HYBRID" || objProjDet.projectTypeName.ToUpper() == "Hybrid")
        //                {
        //                    ddlProjectType.ClearSelection();
        //                    ddlProjectType.SelectedIndex = ddlProjectType.Items.IndexOf(ddlProjectType.Items.FindByText("HYBRID (Colocated)"));
        //                }
        //                else
        //                {
        //                    ddlProjectType.ClearSelection();
        //                    ddlProjectType.SelectedIndex = ddlProjectType.Items.IndexOf(ddlProjectType.Items.FindByText(objProjDet.projectTypeName.ToUpper().Trim()));
        //                }
        //                hdnProjectType.Value = ddlProjectType.SelectedItem.Value;
        //                ddlNatureOfApp.Enabled = false;
        //                ddlProjectType.Enabled = false;
        //                fetchRegDet();
        //                //Session["MEDA_PRJ_ID"] = txtProjCode.Text;
        //                Session["isValiMEDAID"] = "true";

        //            }
        //            else
        //            {
        //                Session["isValiMEDAID"] = "false";


        //                //       Response.Write("<script language='javascript'>alert('" + responseText + "');location='~/UI/Cons/ConsumerDetail.aspx'</script>");
        //                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + objProjDet.response + "');window.location ='ConsumerDetail.aspx';", true);
        //                //Response.Redirect("~/UI/Cons/AppHome.aspx", false);
        //                txtProjCode.Text = "";

        //            }
        //        }
        //        else
        //        {
        //            //       Response.Write("<script language='javascript'>alert('" + responseText + "');location='~/UI/Cons/ConsumerDetail.aspx'</script>");
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Project ID does not belong to current user!');window.location ='ConsumerDetail.aspx';", true);
        //            //Response.Redirect("~/UI/Cons/AppHome.aspx", false);
        //            txtProjCode.Text = "";

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
        //        log.Error(ErrorMessage);
        //        Response.Write("<script language='javascript'>alert('Kindly try after some time.');</script>");
        //    }
        //}

        void fetchRegDet()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            int apptypeno = int.Parse(ddlNatureOfApp.SelectedItem.Value);

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        strQuery = "select * from applicant_reg_det where USER_NAME='" + Session["user_name"].ToString() + "'";
                        MySqlCommand cmd;
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        txtContPer1.Text = dsResult.Tables[0].Rows[0]["ContactPerson"].ToString();
                        txtPhone1.Text = dsResult.Tables[0].Rows[0]["ORG_PHONE"].ToString();
                        txtMob1.Text = dsResult.Tables[0].Rows[0]["ORG_MOB"].ToString();
                        txtEmail1.Text = dsResult.Tables[0].Rows[0]["ORG_EMAIL"].ToString();

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
                lblResult.Text = "Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Please try again.";
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