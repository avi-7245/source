using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
using log4net;
using System.Reflection;
using System.Configuration;
using GGC.Common;
namespace GGC.Scheduler
{
    public partial class _5Days : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(_5Days));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        void FifthDay()
        {
            string Application_no=string.Empty;
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT Application_No FROM email_scheduler_update_dates where FifthDay=CURDATE()) and wf_status_cd_c<=8");
                //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT Application_No FROM email_scheduler_update_dates where FifthDay='2023-08-29')");
                

                SendEmail objSe = new SendEmail();
                string strTo, strCC, strSub, strBody;

                int i = 0;
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                        {
                            try{
                            
                            //foreach (var part in dsResult.Tables[0].Rows[i]["TxnDate"].ToString().Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries))
                            //{
                            //    if (part != "")
                                    
                            //    //Msg.CC.Add(new MailAddress(part.ToString()));
                            //    log.Error("Part " + part);
                            //}

                            strTo = dsResult.Tables[0].Rows[i]["CONT_PER_EMAIL_1"].ToString() + ";";

                            //DateTime date = DateTime.ParseExact(dsResult.Tables[0].Rows[i]["TxnDate"].ToString(), "d-m-yyyy h:m:s tt", System.Globalization.CultureInfo.InvariantCulture);
                            //string formattedDate = date.ToString("yyyy-MM-dd");

                            Application_no = dsResult.Tables[0].Rows[i]["Application_no"].ToString();
                            DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[i]["zone"].ToString() + "' and district='" + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + "'");
                            DataSet dsSentToTFS = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_status_tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and STATUS='DOCUMENT VERIFIED.SITE TECHNICAL FESIBILITY REPORT PENDING.'");

                            strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString()+";";
                            strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
                            strSub = "Reminder 1: Submission of Technical Feasibility Report for Grid Connectivity application against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() +
                                " for " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString();
                            strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + ".<br/>" +
                                    "This office on Dt. " + dsSentToTFS.Tables[0].Rows[0]["STATUS_DT"].ToString() + " has requested M/s." + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " and C. E. " + dsResult.Tables[0].Rows[i]["Zone"].ToString() + " to carry out " +
                                    "the joint survey and submit the Technical Feasibility Report for said project.<br/>Please Note that, TFR needs to be submitted to C. O. as per the timeline defined in SoP. Thus, it is once again requested to carry out joint survey and forward the Technical " +
                                    "feasibility report at the earliest for taking further necessary action at our end.";
                            strBody += "<br/>";
                            strBody += "Thanks & Regards, " + "<br/>";
                            strBody += "Chief Engineer / STU Department" + "<br/>";
                            strBody += "MSETCL  " + "<br/>";
                            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                            objSe.Send(strTo, strCC, strSub, strBody);

                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'Y')");
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'N')");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'N')");
            }
        }
        void SeventthDay()
        {
            string Application_no = string.Empty;
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT Application_No FROM email_scheduler_update_dates where SeventhDay=CURDATE()) and wf_status_cd_c<=8");
                //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT Application_No FROM email_scheduler_update_dates where SeventhDay='2023-08-31')");

                SendEmail objSe = new SendEmail();
                string strTo, strCC, strSub, strBody;
                int i = 0;
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                        {
                            try{
                            Application_no = dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString();
                            strTo = dsResult.Tables[0].Rows[i]["CONT_PER_EMAIL_1"].ToString() + ";";
                            DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[i]["zone"].ToString() + "' and district='" + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + "'");
                            DataSet dsSentToTFS = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_status_tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and STATUS='DOCUMENT VERIFIED.SITE TECHNICAL FESIBILITY REPORT PENDING.'");
                            DataSet dsTFSReminder = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM SOP_Email_Sched_Tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and ReminderDay=5");
                            DateTime strRem1 = DateTime.Parse(dsTFSReminder.Tables[0].Rows[0]["ReminderSentDate"].ToString());
                            strRem1.ToString("dd-MMM-yyyy");
                            strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString();
                            strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
                            strSub = "Reminder 2: Submission of Technical Feasibility Report for Grid Connectivity application against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() +
                                " for " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString();
                            strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + ".<br/>" +
                                    "This office on Dt. " + dsSentToTFS.Tables[0].Rows[0]["STATUS_DT"].ToString() + " and Reminder-1 Dt." + strRem1 + " has requested M/s." + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " and C. E. " + dsResult.Tables[0].Rows[i]["Zone"].ToString() + " to carry out " +
                                    "the joint survey and submit the Technical Feasibility Report for said project.<br/>Please Note that, TFR needs to be submitted to C. O. as per the timeline defined in SoP. Thus, it is once again requested to carry out joint survey and forward the Technical " +
                                    "feasibility report at the earliest for taking further necessary action at our end.<br/><b>Please Note that, in case TFR is not received within 7 Days from date of this email," +
                                    "action for Cancellation of Grid Connectivity Application will be initiated.</b>";
                            strBody += "<br/>";
                            strBody += "Thanks & Regards, " + "<br/>";
                            strBody += "Chief Engineer / STU Department" + "<br/>";
                            strBody += "MSETCL  " + "<br/>";
                            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                            objSe.Send(strTo, strCC, strSub, strBody);
                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'Y')");
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'N')");
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'N')");
            }
        }

        void Cancellation()
        {
            string Application_no = string.Empty;
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT Application_No FROM email_scheduler_update_dates where CancellDate=CURDATE()) and wf_status_cd_c<=8");
                //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT Application_No FROM email_scheduler_update_dates where CancellDate='2023-09-18')");
                
                SendEmail objSe = new SendEmail();
                string strTo, strCC, strSub, strBody;
                int i = 0;
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        for (i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                        {
                            try{
                            Application_no = dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString();
                            strTo = dsResult.Tables[0].Rows[i]["CONT_PER_EMAIL_1"].ToString() + ";";
                            DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[i]["zone"].ToString() + "' and district='" + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + "'");
                            DataSet dsSentToTFS = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_status_tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and STATUS='DOCUMENT VERIFIED.SITE TECHNICAL FESIBILITY REPORT PENDING.'");
                            DataSet dsTFSReminder = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM SOP_Email_Sched_Tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and ReminderDay in (5,7)");
                            try
                            {
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update applicantdetails set WF_STATUS_CD_C=21, app_status='APPLICATION CANCELLED.', APP_STATUS_DT=CURDATE() where Application_no='" + Application_no + "'");
                              //  SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into CancelApplications (APPLICATION_NO,CancelRemark) values('" + Application_no + "','CANCELLED BY SOP SCHEDULER')");
                               // SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into APPLICANT_STATUS_TRACKING values('" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "','APPLICATION CANCELLED.',now(),'AUTO')");
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                log.Error(" Error while updating applicantdetails : insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                            }
                            try
                            {
                                //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update applicantdetails set WF_STATUS_CD_C=21, app_status='APPLICATION CANCELLED.', APP_STATUS_DT=CURDATE() where Application_no='" + Application_no + "'");
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into CancelApplications (APPLICATION_NO,CancelRemark) values('" + Application_no + "','CANCELLED BY SOP SCHEDULER')");

                                objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "21");
                                //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into APPLICANT_STATUS_TRACKING values('" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "','APPLICATION CANCELLED.',now(),'AUTO')");
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                log.Error(" Error while updating applicantdetails : insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                            }
                            try
                            {
                                //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update applicantdetails set WF_STATUS_CD_C=21, app_status='APPLICATION CANCELLED.', APP_STATUS_DT=CURDATE() where Application_no='" + Application_no + "'");
                                //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into CancelApplications (APPLICATION_NO,CancelRemark) values('" + Application_no + "','CANCELLED BY SOP SCHEDULER')");
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into APPLICANT_STATUS_TRACKING values('" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "','APPLICATION CANCELLED.',now(),'AUTO')");
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                log.Error(" Error while updating applicantdetails : insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                            }
                            DateTime strRem1=DateTime.Today;
                            DateTime strRem2 = DateTime.Today;
                            foreach (DataRow dr in dsTFSReminder.Tables[0].Rows)
                            {
                                if(dr["ReminderDay"].ToString()=="5")
                                {
                                    strRem1 = DateTime.Parse(dr["ReminderSentDate"].ToString());
                                    strRem1.ToString("dd-MMM-yyyy");
                                    log.Error("strRem1" + strRem1);

                                }
                                if (dr["ReminderDay"].ToString() == "7")
                                {
                                    strRem2 = DateTime.Parse(dr["ReminderSentDate"].ToString());
                                    strRem2.ToString("dd-MMM-yyyy");
                                    log.Error("strRem2" + strRem2);
                                }
                            }
                            strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString();
                            strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
                            strCC += ConfigurationManager.AppSettings["DOP"].ToString();
                            strSub = "Cancellation of Application for Grid Connectivity against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() +
                                " for " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + " MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString();
                            strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + ".<br/>" +
                                    "This office on Dt. " + dsSentToTFS.Tables[0].Rows[0]["STATUS_DT"].ToString() + ", Reminder-1 Dt." + strRem1.ToShortDateString() + " and and Reminder- 2 Dt. " + strRem2.ToShortDateString() + " has requested to carry out " +
                                    "the joint survey and submit the Technical Feasibility Report for said project.<br/> However, till date this office has not received technical feasibility report for the said project even after lapse of stipulated time." +
                                    "<b>As stipulated time has been elapsed and TFR is not received, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + "‘s Application to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + " MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "is hereby cancelled and no further correspondence will be entertained in this matter.</b></br>"+
                                    "If required, you may apply afresh through Single Window Portal as per the provisions of the prevailing GoM RE Policy however you shall not have any precedence over other interested applicants at the same place.";
                                    
                            strBody += "<br/>";
                            strBody += "Thanks & Regards, " + "<br/>";
                            strBody += "Chief Engineer / STU Department" + "<br/>";
                            strBody += "MSETCL  " + "<br/>";
                            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                            try{
                            objSe.Send(strTo, strCC, strSub, strBody);
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                            }
                            }
                            catch (Exception exception)
                            {
                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                                log.Error(ErrorMessage);
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                            }
                            //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'Y')");
                            //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update applicantdetails set WF_STATUS_CD_C=20, app_status='APPLICATION CANCELLED.',APP_STATUS_DT=now() where application_no ='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "'");
                            //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into APPLICANT_STATUS_TRACKING values('" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "','APPLICATION CANCELLED.',now(),'AUTO')");
                            
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                log.Error("proc_5_scheduler");
                SQLHelper.ExecuteDataset(conString, CommandType.StoredProcedure, "proc_5_scheduler");
                log.Error("proc_7_scheduler");
                SQLHelper.ExecuteDataset(conString, CommandType.StoredProcedure, "proc_7_scheduler");
                log.Error("proc_16_scheduler");
                SQLHelper.ExecuteDataset(conString, CommandType.StoredProcedure, "proc_16_scheduler");
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
               // SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
            }
            FifthDay();
            SeventthDay();
            Cancellation();
        //    string Application_no = string.Empty;
        //    DateTime dt = new DateTime(2023, 7, 12);
        //    DateTime dt1 = new DateTime(2023, 7, 14);
        //    DateTime dtstart = new DateTime(2023, 6, 23);
        //    DateTime dtend = DateTime.Today;
        //    DateTime[] bankHolidays = { dt, dt1 };
        //    int x = Calculate.BusinessDaysUntil(dtstart, dtend, bankHolidays);
        //    #region 5Days Reminder
        //    try
        //    {
        //        DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no in (SELECT ApplicationNo FROM billdesk_txn where AuthStatus='0300' and IsPayMentApproveFin='Y' and dateDiff(curdate(),UpdatedDt)=5)");
        //        SendEmail objSe = new SendEmail();
        //        string strTo, strCC, strSub, strBody;
        //        int i = 0;
        //        if (dsResult.Tables.Count > 0)
        //        {
        //            if (dsResult.Tables[0].Rows.Count > 0)
        //            {
        //                for (i = 0; i < dsResult.Tables[0].Rows.Count; i++)
        //                {
        //                    strTo = dsResult.Tables[0].Rows[i]["CONT_PER_EMAIL_1"].ToString() + ";";
        //                    Application_no = dsResult.Tables[0].Rows[i]["Application_no"].ToString();
        //                    DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[i]["zone"].ToString() + "'");
        //                    DataSet dsSentToTFS = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_status_tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and STATUS='REGISTRATION FEES APPROVED BY FINANCE.UPLOAD APPLICATION FORM PENDING.'");

        //                    strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString();
        //                    strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
        //                    strSub = "(Testing) Reminder 1: Submission of Technical Feasibility Report for Grid Connectivity application against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() +
        //                        " for " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString();
        //                    strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + ".<br/>" +
        //                            "This office on Dt. " + dsSentToTFS.Tables[0].Rows[i]["STATUS_DT"].ToString() + " has requested M/s." + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " and C. E. " + dsResult.Tables[0].Rows[i]["Zone"].ToString() + " to carry out " +
        //                            "the joint survey and submit the Technical Feasibility Report for said project.<br/>Please Note that, TFR needs to be submitted to C. O. as per the timeline defined in SoP. Thus, it is once again requested to carry out joint survey and forward the Technical " +
        //                            "feasibility report at the earliest for taking further necessary action at our end.";
        //                    strBody += "<br/>";
        //                    strBody += "Thanks & Regards, " + "<br/>";
        //                    strBody += "Chief Engineer / STU Department" + "<br/>";
        //                    strBody += "MSETCL  " + "<br/>";
        //                    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
        //                    objSe.Send(strTo, strCC, strSub, strBody);

        //                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'Y')");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //        log.Error(ErrorMessage);
        //        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'N')");
        //    }
        //    #endregion

        //    #region 7Days Reminder
        //    try
        //    {
        //        DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select *,DATE_SUB(CURDATE(), INTERVAL 2 DAY) as 5thDay  from applicantdetails where application_no in (SELECT ApplicationNo FROM billdesk_txn where AuthStatus='0300' and IsPayMentApproveFin='Y' and dateDiff(curdate(),UpdatedDt)=7)");

        //        SendEmail objSe = new SendEmail();
        //        string strTo, strCC, strSub, strBody;
        //        int i = 0;
        //        if (dsResult.Tables.Count > 0)
        //        {
        //            if (dsResult.Tables[0].Rows.Count > 0)
        //            {
        //                for (i = 0; i < dsResult.Tables[0].Rows.Count; i++)
        //                {
        //                    Application_no = dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString();
        //                    strTo = dsResult.Tables[0].Rows[i]["CONT_PER_EMAIL_1"].ToString() + ";";
        //                    DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[i]["zone"].ToString() + "'");
        //                    DataSet dsSentToTFS = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_status_tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and STATUS='REGISTRATION FEES APPROVED BY FINANCE.UPLOAD APPLICATION FORM PENDING.'");

        //                    strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString();
        //                    strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
        //                    strSub = "(Testing) Reminder 2: Submission of Technical Feasibility Report for Grid Connectivity application against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() +
        //                        " for " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString();
        //                    strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + ".<br/>" +
        //                            "This office on Dt. " + dsSentToTFS.Tables[0].Rows[i]["STATUS_DT"].ToString() + " and Reminder-1 Dt." + dsResult.Tables[0].Rows[i]["5thDay"].ToString() + " has requested M/s." + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " and C. E. " + dsResult.Tables[0].Rows[i]["Zone"].ToString() + " to carry out " +
        //                            "the joint survey and submit the Technical Feasibility Report for said project.<br/>Please Note that, TFR needs to be submitted to C. O. as per the timeline defined in SoP. Thus, it is once again requested to carry out joint survey and forward the Technical " +
        //                            "feasibility report at the earliest for taking further necessary action at our end.<br/><b>Please Note that, in case TFR is not received within 7 Days from date of this email," +
        //                            "action for Cancellation of Grid Connectivity Application will be initiated.</b>";
        //                    strBody += "<br/>";
        //                    strBody += "Thanks & Regards, " + "<br/>";
        //                    strBody += "Chief Engineer / STU Department" + "<br/>";
        //                    strBody += "MSETCL  " + "<br/>";
        //                    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
        //                    objSe.Send(strTo, strCC, strSub, strBody);
        //                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'Y')");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //        log.Error(ErrorMessage);
        //        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'N')");
        //    }
        //    #endregion

        //    #region 13Days Notice
        //    try
        //    {
        //        DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select *,DATE_SUB(CURDATE(), INTERVAL 2 DAY) as 5thDay  from applicantdetails where application_no in (SELECT ApplicationNo FROM billdesk_txn where AuthStatus='0300' and IsPayMentApproveFin='Y' and dateDiff(curdate(),UpdatedDt)=13)");

        //        SendEmail objSe = new SendEmail();
        //        string strTo, strCC, strSub, strBody;
        //        int i = 0;
        //        if (dsResult.Tables.Count > 0)
        //        {
        //            if (dsResult.Tables[0].Rows.Count > 0)
        //            {
        //                for (i = 0; i < dsResult.Tables[0].Rows.Count; i++)
        //                {
        //                    Application_no = dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString();
        //                    strTo = dsResult.Tables[0].Rows[i]["CONT_PER_EMAIL_1"].ToString() + ";";
        //                    DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[i]["zone"].ToString() + "'");
        //                    DataSet dsSentToTFS = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_status_tracking where APPLICATION_NO='" + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + "' and STATUS='REGISTRATION FEES APPROVED BY FINANCE.UPLOAD APPLICATION FORM PENDING.'");

        //                    strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString();
        //                    strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
        //                    strSub = "(Testing) Notice for Cancellation of Application for Grid Connectivity against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() +
        //                        " for " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW," + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString();
        //                    strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[i]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[i]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[i]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[i]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[i]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[i]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[i]["APPLICATION_NO"].ToString() + ".<br/>" +
        //                            "This office on Dt. " + dsSentToTFS.Tables[0].Rows[i]["STATUS_DT"].ToString() + ", Reminder-1 Dt." + dsResult.Tables[0].Rows[i]["5thDay"].ToString() + " and Reminder- 2 Dt." + dsResult.Tables[0].Rows[i]["5thDay"].ToString() + " has requested M/s." + dsResult.Tables[0].Rows[i]["PROMOTOR_NAME"].ToString() + " and C. E. " + dsResult.Tables[0].Rows[i]["Zone"].ToString() + " to carry out " +
        //                            "the joint survey and submit the Technical Feasibility Report for said project.<br/>Please Note that, TFR needs to be submitted to C. O. as per the timeline defined in SoP. Thus, it is once again requested to carry out joint survey and forward the Technical " +
        //                            "feasibility report at the earliest for taking further necessary action at our end.<br/><b>Please Note that, in case TFR is not received within 7 Days from date of this email," +
        //                            "action for Cancellation of Grid Connectivity Application will be initiated.</b>";
        //                    strBody += "<br/>";
        //                    strBody += "Thanks & Regards, " + "<br/>";
        //                    strBody += "Chief Engineer / STU Department" + "<br/>";
        //                    strBody += "MSETCL  " + "<br/>";
        //                    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
        //                    objSe.Send(strTo, strCC, strSub, strBody);
        //                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'Y')");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //        log.Error(ErrorMessage);
        //        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',7,'N')");
        //    }
        //    #endregion

            
        }

        
    }

    public static class Calculate
    {
        public static int BusinessDaysUntil(this DateTime firstDay, DateTime lastDay, DateTime[] bankHolidays)
        {
            firstDay = firstDay.Date;
            lastDay = lastDay.Date;
            if (firstDay > lastDay)
                throw new ArgumentException("Incorrect last day " + lastDay);

            TimeSpan span = lastDay - firstDay;
            int businessDays = span.Days + 1;
            int fullWeekCount = businessDays / 7;
            // find out if there are weekends during the time exceedng the full weeks
            if (businessDays > fullWeekCount * 7)
            {
                // we are here to find out if there is a 1-day or 2-days weekend
                // in the time interval remaining after subtracting the complete weeks
                int firstDayOfWeek = (int)firstDay.DayOfWeek;
                int lastDayOfWeek = (int)lastDay.DayOfWeek;
                if (lastDayOfWeek < firstDayOfWeek)
                    lastDayOfWeek += 7;
                if (firstDayOfWeek <= 6)
                {
                    if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                        businessDays -= 2;
                    else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                        businessDays -= 1;
                }
                else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                    businessDays -= 1;
            }

            // subtract the weekends during the full weeks in the interval
            businessDays -= fullWeekCount + fullWeekCount;

            // subtract the number of bank holidays during the time interval
            foreach (DateTime bankHoliday in bankHolidays)
            {
                DateTime bh = bankHoliday.Date;
                if (firstDay <= bh && bh <= lastDay)
                    --businessDays;
            }

            return businessDays;
        }
    }
}