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
using System.Text;
using System.Xml;
using GGC.Scheduler;
using GGC.Common;
using Microsoft.Reporting.WebForms;

namespace GGC.UI.Emp
{
    public partial class MSSPDPRAccept : BasePage
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSSPDPRAccept));
        string strAppID = "";
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();


        void fillData()
        {
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());

            try
            {

                string strQuery = string.Empty;
                strQuery = "select * from proposalapproval_SPD where APPLICATION_NO='" + strAppID + "' and roleid='" + rollid + "' and isAppr_Rej_Ret is null order by createDT desc";


                DataSet dsResultStatus = new DataSet();
                dsResultStatus = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                HDFCount.Value = dsResultStatus.Tables[0].Rows.Count.ToString();

                strQuery = "select * from mskvy_applicantdetails_spd  where APPLICATION_NO='" + strAppID + "'";
                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    lblApplcationNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                    lblDevName.Text = dsResult.Tables[0].Rows[0]["NAME_OF_SPD"].ToString();
                    //lblNatOfApp.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                    //lblProjectType.Text = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                    lblProjCap.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                    lblProjLoc.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();
                    lblInter.Text = dsResult.Tables[0].Rows[0]["INTERCONNECTION_AT"].ToString();
                }
                strQuery = "select distinct a.APPLICATION_NO, case when a.isAppr_Rej_Ret='Y' then 'Verified' else 'Returned' end as isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from proposalapprovaltxn_spd a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.createDT desc";

                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResult.Tables[0];
                    GVApplications.DataBind();
                }

                //}
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Submission Failed!!";
            }
        }

        void sendMailApproval(string strAppID, string isApproved)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rowCount = 0;

            int rollid = int.Parse(Session["EmpRole"].ToString());

            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();

                string strQuery = "SELECT a.*,b.*,concat(a.PROJECT_LOC,' ', a.PROJECT_TALUKA,' ', a.PROJECT_DISTRICT) as Location FROM MSKVY_applicantdetails  a, applicant_reg_det b WHERE a.USER_NAME=b.USER_NAME and a.APPLICATION_NO='" + strAppID + "'";
                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);

                strQuery = "select * from empmaster where sapid='" + Session["SAPID"].ToString() + "'";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResultEmail = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dsResultEmail);


                var notify = GetNotificationData();



                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    #region Send Mail
                    //sendMailOTP(strRegistrationno, strEmailID);
                    string strBody = string.Empty;



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
                        if (isApproved == "N")
                        {
                            Msg.To.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["EmpReportingEmail"].ToString()));
                            strBody += "Respected Sir" + ",<br/>";
                            strBody += "Grid Connectivity Proposal against Project ID No " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + " , Online Application No " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + " for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " , " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[0]["Location"].ToString() + " (location) by M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has been forworded for Approval/Recommendation.";
                            //                    strBody += "Grid Connectivity Proposal for the Grid Connectivity for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project of M/s." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " is submitted for approval/recommendation.<br/>";
                            //strBody += "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            //strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            //strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            ////strBody += "Please use following information for login for further process. <br/>";
                            //strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".The copy of application mentioning details of project and applicant is attached herewith.<br/>";
                            //strBody += "<b>Your payment has been approved.</b> <br/>  ";
                            //strBody += "<b>Note : Kindly upload scan copy of your application form with duly sign and stamp.</b> <br/>  <br/><br/><br/>";
                            strBody += "<a href='" + ConfigurationManager.AppSettings["ClickHereURL"].ToString() + ">Click here </a> to login.<br/>  <br/><br/><br/>";

                            strBody += "Thanks & Regards, " + "<br/>";
                            //strBody += " Chief Engineer" + "<br/>";
                            strBody += "State Transmission Utility" + "<br/>";
                            strBody += "MSETCL" + "<br/>";
                            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                        }
                        else
                        {
                            Msg.To.Add(new MailAddress(ConfigurationManager.AppSettings["CESTU"].ToString()));
                            strBody += "Respected Sir" + ",<br/>";
                            strBody += "MSKVY Grid Connectivity Proposal against Project ID No " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + " , Online Application No " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + " for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " , " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[0]["Location"].ToString() + " (location) by M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has been Approved." + ",<br/>";
                            //strBody += "SPV can download GC letter from MSETCL Grid connectivity portal.";
                            strBody += "SPD can download GC letter from MEDA portal.";
                            //                    strBody += "Grid Connectivity Proposal for the Grid Connectivity for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project of M/s." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " is submitted for approval/recommendation.<br/>";
                            //strBody += "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                            //strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            //strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                            ////strBody += "Please use following information for login for further process. <br/>";
                            //strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".The copy of application mentioning details of project and applicant is attached herewith.<br/>";
                            //strBody += "<b>Your payment has been approved.</b> <br/>  ";
                            //strBody += "<b>Note : Kindly upload scan copy of your application form with duly sign and stamp.</b> <br/>  <br/><br/><br/>";
                            //strBody += "<a href='" + ConfigurationManager.AppSettings["ClickHereURL"].ToString() + ">Click here </a> to login.<br/>  <br/><br/><br/>";

                            strBody += "Thanks & Regards, " + "<br/>";
                            //strBody += " Chief Engineer" + "<br/>";
                            strBody += "State Transmission Utility" + "<br/>";
                            strBody += "MSETCL" + "<br/>";
                            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                        }
                        //Msg.CC.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                        string strCC = ConfigurationManager.AppSettings["MMEmailID"].ToString();


                        notify.EmailIds.ForEach(email =>
                        {
                            Msg.CC.Add(new MailAddress(email));
                        });

                        if (strCC != "")
                        {
                            string[] splittedCC = strCC.Split(',');
                            foreach (var part in strCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (part != "")
                                    Msg.CC.Add(new MailAddress(part.ToString()));
                                //Msg.CC.Add(new MailAddress(part.ToString()));
                                log.Error("Part " + part);
                            }
                        }

                        strCC = ConfigurationManager.AppSettings["MMCCEmailID"].ToString();
                        if (strCC != "")
                        {
                            string[] splittedCC = strCC.Split(',');
                            foreach (var part in strCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (part != "")
                                    Msg.CC.Add(new MailAddress(part.ToString()));
                                //Msg.CC.Add(new MailAddress(part.ToString()));
                                log.Error("Part " + part);
                            }
                        }



                        Msg.Subject = "Proposal for Grid connectivity application from " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
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

                    string spdMobileNumber = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();

                    if (!string.IsNullOrEmpty(spdMobileNumber)) notify.MobileNumbers.Add(spdMobileNumber);

                    SMS.Send(message: SMSTemplates.GCApproved(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), notify.MobileNumbers.JoinStrings(), MethodBase.GetCurrentMethod(), log);
                    #endregion Send SMS

                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Submission Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }

        }

        void sendMailReturn(string strAppID, string isApproved)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rowCount = 0;

            int rollid = int.Parse(Session["EmpRole"].ToString());

            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();

                string strQuery = "SELECT a.*,b.* FROM MSKVY_applicantdetails  a, applicant_reg_det b WHERE a.USER_NAME=b.USER_NAME and a.APPLICATION_NO='" + strAppID + "'";
                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);

                //strQuery = "select * from empmaster where role_id=(select role_id from empmaster where sapid='" + Session["SAPID"].ToString() + "')-1";
                //cmd = new MySqlCommand(strQuery, mySqlConnection);
                //DataSet dsResultEmail = new DataSet();
                //da = new MySqlDataAdapter(cmd);
                //da.Fill(dsResultEmail);

                var notify = GetNotificationData();


                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    #region Send Mail
                    //sendMailOTP(strRegistrationno, strEmailID);
                    string strBody = string.Empty;

                    strBody += "Respected Sir" + ",<br/>";
                    strBody += "Proposal for the Grid Connectivity for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project of M/s." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " is return with reason : <b>" + txtRemark.Text + "</b><br/>";
                    //strBody += "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                    //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                    //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                    //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                    //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                    //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                    //strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                    //strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                    ////strBody += "Please use following information for login for further process. <br/>";
                    //strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".The copy of application mentioning details of project and applicant is attached herewith.<br/>";
                    //strBody += "<b>Your payment has been approved.</b> <br/>  ";
                    //strBody += "<b>Note : Kindly upload scan copy of your application form with duly sign and stamp.</b> <br/>  <br/><br/><br/>";
                    strBody += "<a href='https://grid.mahatransco.in/UI/emp/emplogin.aspx'>Click here </a> to login.<br/>  <br/><br/><br/>";

                    strBody += "Thanks & Regards, " + "<br/>";
                    //strBody += " Chief Engineer" + "<br/>";
                    strBody += "State Transmission Utility" + "<br/>";
                    strBody += "MSETCL" + "<br/>";
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
                        //if (rollid != 51)
                        //{
                        //    Msg.To.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["EmpEmailID"].ToString()));
                        //}
                        //else
                        //{
                        //    Msg.To.Add(new MailAddress(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                        //}
                        //Msg.CC.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));

                        notify.EmailIds.ForEach(email =>
                        {
                            Msg.To.Add(new MailAddress(email));
                        });


                        Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMEmailID"].ToString()));

                        Msg.Subject = "Proposal for Grid connectivity application from " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
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
                    SMS.Send(message: SMSTemplates.GCReturned(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), notify.MobileNumbers.JoinStrings(), MethodBase.GetCurrentMethod(), log);
                    #endregion Send SMS
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Submission Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            strAppID = Request.QueryString["application"];
            if (!Page.IsPostBack)
            {

                fillData();

            }

        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            string strGSTIN = string.Empty;
            string isApproved = "N";
            int rowCount = 0;

            int rollid = int.Parse(Session["EmpRole"].ToString());
            if (rollid == 53)
            {
                rollid = 51;

            }
            else
            {
                rollid++;
            }
            //Session["EmpZone"] = "Thane";
            try
            {

                string strQuery = string.Empty;

                //rowCount=Int32.Parse(HDFCount.Value.ToString());

                //if (rowCount == 1)
                //{
                //strQuery = "update proposalapproval_SPD set isAppr_Rej_Ret='Y', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='" + rollid + "', createDT=now() where APPLICATION_NO='" + strAppID + "'";
                strQuery = "update proposalapproval_SPD set  isAppr_Rej_Ret='Y', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='" + rollid + "', createDT=now() where APPLICATION_NO='" + strAppID + "'";
                //}

                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                //RemoveComment

                /*Insert into Transaction Table */
                strQuery = "insert into proposalapprovaltxn_spd(APPLICATION_NO , isAppr_Rej_Ret,remark,Aprove_Reject_Return_by, roleid , createDT ,createBy) values ('" + strAppID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "', '" + rollid + "', now() , '" + Session["SAPID"].ToString() + "')";
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                //RemoveComment

                float projectCapacity = lblProjCap.Text == "" ? 0 : float.Parse(lblProjCap.Text);

                if (Session["EmpRole"].ToInt() == RoleConst.CE)
                {
                    strQuery = "update MSKVY_applicantdetails_spd  set  WF_STATUS_CD_C=19, app_status='PROPOSAL APPROVED.',app_status_dt=now() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = "update proposalapproval_SPD set  roleid='2' where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    isApproved = "Y";

                    #region Save To MEDA
                    objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "19");
                    //RemoveComment
                    #endregion Save To MEDA

                    sendMailApproval(strAppID, isApproved);
                }



                //#region Send SMS
                //DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53,54)");
                //string strMobNos = string.Empty;

                //try
                //{
                //    //log.Error("1 ");
                //    if (dsEmailMob.Tables[0].Rows.Count > 0)
                //    {



                //        if (dsEmailMob.Tables[0].Rows.Count > 0)
                //        {
                //            for (int i = 0; i < dsEmailMob.Tables[0].Rows.Count; i++)
                //            {
                //                if (dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                //                    strMobNos += dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                //            }

                //        }
                //    }
                //    string strURL = string.Empty;
                //    if (Session["EmpRole"].ToString() == "57")
                //    {
                //        SMS.Send(message: SMSTemplates.GCApproved(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), strMobNos.Remove(strMobNos.Length - 1, 1), MethodBase.GetCurrentMethod(), log);


                //        //strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20proposal%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20by%20M%2Fs.%20" + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20forwarded%20by%20CE%2C%20STU%20for%20Approval%2FRecommendation%20of%20Director%20%28O%29.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                //        //WebRequest request = HttpWebRequest.Create(strURL);
                //        ////log.Error("2 ");
                //        //// Get the response back  
                //        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //        //Stream s = (Stream)response.GetResponseStream();
                //        //StreamReader readStream = new StreamReader(s);
                //        //string dataString = readStream.ReadToEnd();
                //        //log.Error("8 " + dataString);

                //        //response.Close();
                //        //s.Close();
                //        //readStream.Close();
                //        ////log.Error("strURL " + strURL);
                //    }


                //}
                //catch (Exception ex)
                //{
                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                //    log.Error(ErrorMessage);
                //}
                //#endregion



                //strQuery = "select a.APPLICATION_NO,a.isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from proposalapproval_SPDtxn a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.srno desc";
                //cmd = new MySqlCommand(strQuery, mySqlConnection);
                //DataSet dsResultStatus = new DataSet();
                //MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                //da.Fill(dsResultStatus);
                //if (dsResultStatus.Tables[0].Rows.Count > 0)
                //{
                //    GVApplications.DataSource = dsResultStatus.Tables[0];
                //    GVApplications.DataBind();
                //}
                //sendMailApproval(strAppID, isApproved);
                lblResult.Text = "Verified Successfully";
                lblResult.ForeColor = System.Drawing.Color.Green;
                fillData();
                lblApplcationNo.Text = "";
                //lblNatOfApp.Text = "";
                lblProjCap.Text = "";
                //lblProjectType.Text = "";
                lblProjLoc.Text = "";
                txtRemark.Text = "";
                btnApproved.Visible = false;
                btnReturn.Visible = false;
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Submission Failed!!";
            }

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            string strGSTIN = string.Empty;
            //int rowCount = 0;

            int rollid = int.Parse(Session["EmpRole"].ToString());


            if (rollid == 51)
            {
                rollid = 51;
            }
            else
            {
                rollid--;
            }

            //Session["EmpZone"] = "Thane";
            try
            {

                string strQuery = string.Empty;
                //rowCount = Int32.Parse(HDFCount.Value.ToString());

                if (Session["EmpRole"].ToString() == "51")
                {
                    strQuery = "update proposalapproval_SPD set isAppr_Rej_Ret='R', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='2', createDT=now() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = "update mskvy_applicantdetails_spd set WF_STATUS_CD_C=6, app_status='Proposal returned', app_status_dt=now() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment
                }
                else
                {
                    strQuery = "update proposalapproval_SPD set isAppr_Rej_Ret='R', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='" + rollid + "', createDT=now() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment
                }


                strQuery = "insert into proposalapprovaltxn_spd(APPLICATION_NO ,isAppr_Rej_Ret, remark,Aprove_Reject_Return_by, roleid , createDT ,createBy) values ('" + strAppID + "','R','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "', '" + rollid + "', now() , '" + Session["SAPID"].ToString() + "')";
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                //RemoveComment

                strQuery = "select a.APPLICATION_NO,a.isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from proposalapprovaltxn_spd a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.srno desc";

                DataSet dsResultStatus = new DataSet();
                dsResultStatus = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                if (dsResultStatus.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResultStatus.Tables[0];
                    GVApplications.DataBind();
                }

                //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from MSKVY_applicantdetails_SPD  where application_no='" + strAppID + "'");
                //DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53)");

                //#region Send SMS
                //string strMobNos = string.Empty;

                //try
                //{
                //log.Error("1 ");
                //if (dsEmailMob.Tables[0].Rows.Count > 0)
                //{



                //    if (dsEmailMob.Tables[0].Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dsEmailMob.Tables[0].Rows.Count; i++)
                //        {
                //            if (dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                //                strMobNos += dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                //        }

                //    }
                //}

                //var mobileNumbers = dsEmailMob.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpMobile"].ToString())).Select(a => a["EmpMobile"].ToString()).Distinct();

                //SMS.Send(message: SMSTemplates.GCReturned(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["SPV_Name"].ToString()), string.Join(",", mobileNumbers), MethodBase.GetCurrentMethod(), log);

                //string strURL = string.Empty;
                //if (Session["EmpRole"].ToString() == "54")
                //{
                //    SMS.Send(message: SMSTemplates.GridConnectivityProposalCompliance(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), string.Join(",", mobileNumbers), MethodBase.GetCurrentMethod(), log);
                //    //strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20proposal%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20by%20M%2Fs.%20" + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20returned%20by%20Director%20%28O%29%20for%20compliance.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();


                //}
                //if (Session["EmpRole"].ToString() == "55")
                //{

                //    SMS.Send(message: SMSTemplates.GridConnectivityProposalCompliance(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString()), string.Join(",", mobileNumbers), MethodBase.GetCurrentMethod(), log);

                //    //strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20proposal%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20by%20M%2Fs.%20" + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20returned%20by%20Hon.%20CMD%20for%20compliance.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();

                //}

                //log.Error("strURL " + strURL);

                //WebRequest request = HttpWebRequest.Create(strURL);
                ////log.Error("2 ");
                //// Get the response back  
                //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Stream s = (Stream)response.GetResponseStream();
                //StreamReader readStream = new StreamReader(s);
                //string dataString = readStream.ReadToEnd();
                //log.Error("8 " + dataString);

                //response.Close();
                //s.Close();
                //readStream.Close();
                //}
                //catch (Exception ex)
                //{
                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                //    log.Error(ErrorMessage);
                //}
                //#endregion

                sendMailReturn(strAppID, "N");
                lblResult.Text = "Proposal Returned Successfully.";
                lblApplcationNo.Text = "";
                //lblNatOfApp.Text = "";
                lblProjCap.Text = "";
                //lblProjectType.Text = "";
                lblProjLoc.Text = "";
                txtRemark.Text = "";
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Submission Failed!!";
            }

        }

        protected void lnkAppForm_Click(object sender, EventArgs e)
        {

            string applicationId = lblApplcationNo.Text;


            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strPostName = string.Empty;
            string strUserName = string.Empty;
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F  from MSKVY_applicantdetails  where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        strUserName = dsResult.Tables[0].Rows[0]["USER_NAME"].ToString();
                        cmd = new MySqlCommand("select * from applicant_reg_det where USER_NAME='" + strUserName + "'", mySqlConnection);
                        DataSet dsRegDet = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsRegDet);
                        dsRegDet.Tables[0].TableName = "AppRegDet";
                        dsResult.Tables.Add(dsRegDet.Tables[0].Copy());

                        strQuery = "select * from billdesk_txn where ApplicationNo='" + applicationId + "' and AuthStatus='0300'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsReceipt = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsReceipt);
                        dsReceipt.Tables[0].TableName = "TableReceipt";
                        dsResult.Tables.Add(dsReceipt.Tables[0].Copy());

                        //cmd = new MySqlCommand("select * from PostQualExperience where RegistrationId='" + strRegistrationNo + "' order by srno", mySqlConnection);
                        //DataSet dsExperience = new DataSet();
                        //da = new MySqlDataAdapter(cmd);
                        //da.Fill(dsExperience);
                        //dsExperience.Tables[0].TableName = "Table3";
                        //dsResult.Tables.Add(dsExperience.Tables[0].Copy());

                        //cmd = new MySqlCommand("select * from BillDesk_TXN where RegistrationId='" + strRegistrationNo + "' order by srno", mySqlConnection);
                        //DataSet dsReceipt = new DataSet();
                        //da = new MySqlDataAdapter(cmd);
                        //da.Fill(dsReceipt);
                        //dsReceipt.Tables[0].TableName = "Table4";
                        //dsResult.Tables.Add(dsReceipt.Tables[0].Copy());

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                            string FileName = "ApplicationForm_" + strUserName + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                            string serverFilePath = Server.MapPath("~/Reports/") + FileName;
                            try
                            {

                                LocalReport report = new LocalReport();
                                report.ReportPath = Server.MapPath("~/PDFReport/") + "Application_MSSPD.rdlc";

                                ReportDataSource rds = new ReportDataSource();
                                rds.Name = "dsApplication_AppDet";//This refers to the dataset name in the RDLC file
                                rds.Value = dsResult.Tables[0];
                                report.DataSources.Add(rds);

                                ReportDataSource rds2 = new ReportDataSource();
                                rds2.Name = "dsApplication_AppRegDet";//This refers to the dataset name in the RDLC file
                                rds2.Value = dsResult.Tables["AppRegDet"];
                                report.DataSources.Add(rds2);

                                ReportDataSource rds3 = new ReportDataSource();
                                rds3.Name = "DataSetReceipt";//This refers to the dataset name in the RDLC file
                                rds3.Value = dsResult.Tables["TableReceipt"];
                                report.DataSources.Add(rds3);

                                //ReportDataSource rds3 = new ReportDataSource();
                                //rds3.Name = "DataSet3";//This refers to the dataset name in the RDLC file
                                //rds3.Value = dsResult.Tables["Table3"];
                                //report.DataSources.Add(rds3);

                                //ReportDataSource rds4 = new ReportDataSource();
                                //rds4.Name = "DataSet4";//This refers to the dataset name in the RDLC file
                                //rds4.Value = dsResult.Tables["Table4"];
                                //report.DataSources.Add(rds4);

                                ReportViewer reportViewer = new ReportViewer();
                                reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "Application_MSSPD.rdlc";
                                reportViewer.LocalReport.DataSources.Clear();

                                reportViewer.LocalReport.DataSources.Add(rds);
                                reportViewer.LocalReport.DataSources.Add(rds2);
                                reportViewer.LocalReport.DataSources.Add(rds3);

                                // reportViewer.LocalReport.DataSources.Add(rds3);


                                reportViewer.LocalReport.Refresh();


                                string mimeType, encoding, extension, deviceInfo;
                                string[] streamids;
                                Warning[] warnings;
                                string format = "PDF";

                                deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                                byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                                FileStream fs1 = new FileStream(serverFilePath, FileMode.Create);
                                fs1.Write(bytes, 0, bytes.Length);
                                fs1.Close();
                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                                Response.ContentType = "PDF";
                                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                                Response.BinaryWrite(bytes);
                                Response.Flush();
                                Response.End();

                            }
                            catch (Exception ex)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                log.Error(ErrorMessage);

                            }
                        }
                        else
                        {
                            //lblResult.Text = "Error!!";

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

            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
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

        protected void lnkGC_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/MSKVY/" + lblApplcationNo.Text + "/");
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_upload_doc_spd where Application_no='" + lblApplcationNo.Text + "' and FileType=7");
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=MSKVY_GC_" + lblApplcationNo.Text + ".pdf");
            Response.TransmitFile(folderPath + dsResult.Tables[0].Rows[0]["file_name"].ToString());
            Response.End();
        }

    }
}