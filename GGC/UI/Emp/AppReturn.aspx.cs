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
using GGC.Scheduler;

namespace GGC.UI.Emp
{
    public partial class AppReturn : BasePage
    {

        protected static readonly ILog log = LogManager.GetLogger(typeof(AppReturn));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblAppNo.Text = Request.QueryString["application"];
                Session["AppId"] = Request.QueryString["application"];
                Session["ProjectType"] = Request.QueryString["ProjectType"];
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);

            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            //DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53)");
            //var mobileNumbers = dsEmailMob.Tables[0].AsEnumerable().Select(a => a["EmpMobile"].ToString()).Distinct();

            var nd = GetNotificationData();

            if (Session["ProjectType"].ToString() == "SPV1")
            {
                #region Send Email

                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update mskvy_applicantdetails set isAppApproved='N',AppApproveComment='" + txtReturn.Text + "',app_status='Application returned.' where application_no=" + Session["AppId"].ToString());
                //RemoveComment
                SendEmail sm = new SendEmail();
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_applicantdetails where application_no =" + Session["AppId"].ToString());

                string strTo = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                string spvMobileNumber = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();

                if (!string.IsNullOrEmpty(spvMobileNumber)) nd.MobileNumbers.Add(spvMobileNumber);

                string strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();

                string strBody = "Dear" + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " <br/>";
                strBody += "Respected Sir" + ",<br/>";
                strBody += "Proposal for the Deemed Grid Connectivity for " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project of M/s." + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " is return with reason : <b>" + txtReturn.Text + "</b><br/>";

                strBody += "<br/>";
                strBody += "<br/>";
                strBody += "<br/>";
                strBody += "Thanks & Regards, " + "<br/>";
                strBody += "Chief Engineer / STU Department" + "<br/>";
                strBody += "MSETCL  " + "<br/>";
                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                strTo += nd.EmailIds.JoinStrings(";");

                sm.Send(strTo, strCC, "MSKVY application returned", strBody);
                //RemoveComment

                #endregion Send Email

                #region Send SMS
                SMS.Send(message: SMSTemplates.DeemedGCReturned(applicationNo: Session["AppId"].ToString(), spvName: dsResult.Tables[0].Rows[0]["SPV_Name"].ToString()), nd.MobileNumbers.JoinStrings(), MethodBase.GetCurrentMethod(), log);
                #endregion Send SMS


                btnReturn.Enabled = false;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "ApplicationConfirmationAlert", $"alert('APPLICATION {lblAppNo.Text} Return successfully.'); window.location = '{ResolveUrl("~/UI/Emp/MSKVYHome.aspx")}';", true);
            }
            if (Session["ProjectType"].ToString() == "SPV2")
            {
                try
                {
                    #region Send Email

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "update mskvy_applicantdetails_spd set isAppApproved='N',AppApproveComment='" + txtReturn.Text + "',app_status='Application returned.' where application_no=" + Session["AppId"].ToString());
                    SendEmail sm = new SendEmail();
                    DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_applicantdetails_spd where application_no =" + Session["AppId"].ToString());
                    string strTo = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                    string strCC = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();

                    string spdMobileNumber = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();

                    if (!string.IsNullOrEmpty(spdMobileNumber)) nd.MobileNumbers.Add(spdMobileNumber);

                    string strBody = "Dear" + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " <br/>";
                    strBody += "Respected Sir" + ",<br/>";
                    strBody += "Proposal for the Grid Connectivity for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project of M/s." + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " is return with reason : <b>" + txtReturn.Text + "</b><br/>";

                    strBody += "<br/>";
                    strBody += "<br/>";
                    strBody += "<br/>";
                    strBody += "Thanks & Regards, " + "<br/>";
                    strBody += "Chief Engineer / STU Department" + "<br/>";
                    strBody += "MSETCL  " + "<br/>";
                    strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                    strCC += nd.EmailIds.JoinStrings(";");

                    sm.Send(strTo, strCC, "MSKVY application retuned", strBody);

                    //Response.Write("<script language='javascript'>alert('Return successfully.');</script>");

                    #endregion Send Email

                    #region Send SMS

                    SMS.Send(message: SMSTemplates.GCReturned(applicationNo: Session["AppId"].ToString(), spvName: dsResult.Tables[0].Rows[0]["SPV_Name"].ToString()), nd.MobileNumbers.JoinStrings(), MethodBase.GetCurrentMethod(), log);

                    #endregion Send SMS

                    btnReturn.Enabled = false;
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ApplicationConfirmationAlert", $"alert('APPLICATION {lblAppNo.Text} Return successfully.'); window.location = '{ResolveUrl("~/UI/Emp/MSSPDDraft.aspx")}';", true);
                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    log.Error(ErrorMessage);

                }
            }
            //Response.Write("<script language='javascript'>alert('Return successfully.');</script>");
        }
    }
}