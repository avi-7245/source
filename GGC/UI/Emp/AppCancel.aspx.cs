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

namespace GGC.UI.Emp
{
    public partial class AppCancel : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppCancel));
        string strAppID = "";
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            strAppID = Request.QueryString["application"];
            if (!Page.IsPostBack)
            {

                fillData(strAppID);

            }
        }
        void fillData(string strAppID)
        {
                string strQuery = string.Empty;

                strQuery = "select a.APPLICATION_NO, a.MEDAProjectID ,a.PROMOTOR_NAME,a.PROJECT_LOC,a.NATURE_OF_APP,a.PROJECT_TYPE,a.PROJECT_CAPACITY_MW,a.app_status,a.APP_STATUS_DT,concat(a.PROJECT_LOC,' ', a.PROJECT_TALUKA,' ', a.PROJECT_DISTRICT) as Location from applicantdetails a where a.application_no = '" + strAppID + "'";
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                try
                {


                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lblApplcationNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                        lblDevName.Text = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                        lblNatOfApp.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                        lblProjectType.Text = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                        lblProjCap.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                        lblProjLoc.Text = dsResult.Tables[0].Rows[0]["Location"].ToString();
                        Session["PROJID"] = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                    }
                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                }
                

                //}
            
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/CancelApplication/");
            string strAppID = lblApplcationNo.Text;
            string newFileName = "";
            
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
            if (FUCancel.HasFile)
            {
                try
                {

                    FUCancel.SaveAs(folderPath + Path.GetFileName(FUCancel.FileName));
                    newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUCancel.FileName;
                    System.IO.File.Move(folderPath + FUCancel.FileName, folderPath + newFileName);

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into CancelApplications (APPLICATION_NO,CancelRemark,CancelFileName,CancelBy) values('" + strAppID + "','" + txtRemark.Text + "','" + newFileName + "','" + Session["SAPID"].ToString() + "')");

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update applicantdetails set  WF_STATUS_CD_C=21,app_status='APPLICATION CANCELLED.',app_status_dt=curdate() where APPLICATION_NO='" + strAppID + "'");
                    btnCancel.Enabled = false;

                    lblResult.Text = "Application cancelled.";
                    objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "21");

                    DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no='"+strAppID+"'");
                    string strTo = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString() + ";";
                    DataSet dsEmail = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from sop_email_mob where zone='" + dsResult.Tables[0].Rows[0]["zone"].ToString() + "' and district='" + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "'");
                    strTo += dsEmail.Tables[0].Rows[0]["email_id"].ToString();
                    string strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
                    strCC += ConfigurationManager.AppSettings["DOP"].ToString();
                    string strSub = "Cancellation of Application for Grid Connectivity against Project ID No. " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() +
                        " for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW," + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal. " + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + " by M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                    string strBody = "With reference to above subject, M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has proposed to setup " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + "MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + " vide against Project ID No. " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>" +
                                    
                                    "<b>M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "‘s Application to setup " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW," + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Power Project at Village: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal.: " + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + " against Project ID No. " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online Application No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + " is hereby cancelled due to reason '"+txtRemark.Text+"' and no further correspondence will be entertained in this matter.</b></br>" +
                                    "If required, you may apply afresh through Single Window Portal as per the provisions of the prevailing GoM RE Policy however you shall not have any precedence over other interested applicants at the same place.";

                    strBody += "<br/>";
                    strBody += "Thanks & Regards, " + "<br/>";
                    strBody += "Chief Engineer / STU Department" + "<br/>";
                    strBody += "MSETCL  " + "<br/>";
                    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                    try
                    {
                        SendEmail objSe = new SendEmail();
                        objSe.SendAttachment(strTo, strCC, strSub, strBody, folderPath + newFileName);
                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',16,'N')");
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Application cancelled.');window.location ='EmpHome.aspx';", true);


                }
                catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                    log.Error(ErrorMessage);
                    lblResult.Text = "Cancellation failed.";

                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    lblResult.Text="Cancellation failed.";
                }
            }
        }
    }
}