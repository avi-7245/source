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
using GGC.Common;

namespace GGC.UI.Emp
{
    public partial class MSKVYFeasibility : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSKVYFeasibility));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();

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
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();





                string strQuery = string.Empty;
                MySqlCommand cmd;

                strQuery = "insert into LFSFeasibilityStatus (APPLICATION_NO,FeasibilityApprStatus,interconnectionarrangementremark,Remarks,StatusDt) values ('" + strAppID + "','Y','" + txtIAR.Text + "', '" + txtRemark.Text + "',now())";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();

                strQuery = "insert into proposalapproval(APPLICATION_NO , roleid ,remark, createDT ,createBy) values ('" + strAppID + "','51','" + txtRemark.Text + "', now() , '" + Session["SAPID"].ToString() + "')";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();

                strQuery = "insert into proposalapprovaltxn(APPLICATION_NO , isAppr_Rej_Ret,remark, Aprove_Reject_Return_by,roleid,createDT ,createBy) values ('" + strAppID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "', '51', now() , '" + Session["SAPID"].ToString() + "')";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();

                strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=12, app_status='PROPOSAL FEASIBILITY VERIFIED.' ,APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + strAppID + "'";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "12");
                #region Application Status tracking date
                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                        " values('" + strAppID + "','Proposal feasibility approved.',now(),'" + Session["SAPID"].ToString() + "')";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                #endregion
                lblResult.Text = "Proposal feasibility approved.";
                lblResult.ForeColor = System.Drawing.Color.Green;

                lblApplcationNo.Text = "";
                lblNatOfApp.Text = "";
                lblProjCap.Text = "";
                lblProjectType.Text = "";
                lblProjLoc.Text = "";
                txtRemark.Text = "";
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

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();





                string strQuery = string.Empty;
                MySqlCommand cmd;

                strQuery = "insert into LFSFeasibilityStatus(APPLICATION_NO,FeasibilityApprStatus,interconnectionarrangementremark,Remarks,StatusDt) values ('" + strAppID + "','R','" + txtIAR.Text + "', '" + txtRemark.Text + "',now())";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=11,  app_status='Proposal feasibility  rejected.' ,APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + strAppID + "'";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "11");

                #region Application Status tracking date
                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                        " values('" + strAppID + "','Proposal feasibility rejected.',now(),'" + Session["SAPID"].ToString() + "')";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                #endregion



                lblResult.Text = "Proposal feasibility rejected.";
                lblApplcationNo.Text = "";
                lblNatOfApp.Text = "";
                lblProjCap.Text = "";
                lblProjectType.Text = "";
                lblProjLoc.Text = "";
                txtRemark.Text = "";
                strQuery = "select a.*,b.* from MSKVY_applicantdetails  a, applicant_reg_det b where a.APPLICATION_NO='" + strAppID + "' and a.PAN_TAN_NO=b.PAN_TAN_NO ";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);

                strQuery = "select * from LFSFeasibilityStatus where a.APPLICATION_NO='" + strAppID + "'";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResultFeasibility = new DataSet();
                MySqlDataAdapter daResultFeasibility = new MySqlDataAdapter(cmd);
                daResultFeasibility.Fill(dsResultFeasibility);
                #region Send Mail
                //sendMailOTP(strRegistrationno, strEmailID);
                string strBody = string.Empty;


                //strBody += "Respected Sir/Madam" + ",<br/>";
                strBody = "With reference to above this office is in receipt of your application for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() +
                    " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + "Power Project at Site:" + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + " Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + ". C. E., _______ vide email Dt. ______ has submitted the Technical Feasibility Report for said project " +
                    " As per the technical feasibility report it is proposed to interconnect the said project " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + "<br/>" +
                    " Load Flow studies have been carried out and it is observed that the grid connectivity to said project is <b> not feasible</b>. <br/>" +
                    " However, the grid connectivity can be feasible if " + dsResultFeasibility.Tables[0].Rows[0]["interconnectionarrangementremark"].ToString() + "<br/>" +
                    " Thus, the tentative scope of works to be executed by you at your own cost for interconnection is as follows: <br/>" + dsResultFeasibility.Tables[0].Rows[0]["Remarks"].ToString() + "<br/>" +
                    " Please note that, this is tentative scope of works and detailed scope of works will be mentioned in Grid Connectivity Letter.<br/>" +
                    " You are therefore requested to submit your consent with undertaking regarding readiness for carrying out above-mention work at your own cost for interconnection of said RE Project within 15 days from date of issue of this letter. <br/>" +
                    " <b>If no communication is received in 15 days it will be presumed that you are not ready/interested in carrying out the work and said application will be cancelled.</b><br/><br/><br/><br/>";
                strBody += "Thanks & Regards, " + "<br/>";
                //  strBody += " Chief Engineer" + "<br/>";
                strBody += "State Transmission Utility (STU)" + "<br/>";
                strBody += "MSETCL  " + "<br/>";
                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
                try
                {


                    #region using MailMessage
                    string mailTo = string.Empty;
                    MailMessage Msg = new MailMessage();
                    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                    Msg.From = fromMail;
                    Msg.IsBodyHtml = true;
                    //log.Error("from:" + fromAddress);
                    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                    Msg.To.Add(new MailAddress(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                    Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));
                    lblApplcationNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                    lblNatOfApp.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                    lblProjectType.Text = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                    lblProjCap.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                    lblProjLoc.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();

                    Msg.Subject = "Submission of Consent towards Scope of Works to be carried out for Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Power Project proposed by M/s." + dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + " , Tal.: " + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + " Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + " vide Application No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                    Msg.Body = strBody;
                    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                    //SmtpClient a = new SmtpClient("23.103.140.170");
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
        void fillData()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();





                string strQuery = string.Empty;
                MySqlCommand cmd;

                strQuery = "select * from LFSFeasibilityStatus where APPLICATION_NO='" + strAppID + "' order by StatusDt desc";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResultStatus = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResultStatus);
                if (dsResultStatus.Tables[0].Rows.Count > 0 && dsResultStatus.Tables[0].Rows[0]["FeasibilityApprStatus"].ToString() == "Y")
                {
                    lblResult.Text = "Already Feasibility Approved.";
                    btnApproved.Enabled = false;
                }
                else
                {
                    strQuery = "select * from MSKVY_applicantdetails  where APPLICATION_NO='" + strAppID + "' and docLFS is not null";


                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                    DataSet dsResult = new DataSet();
                    da = new MySqlDataAdapter(cmd);
                    da.Fill(dsResult);
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lblApplcationNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                        lblNatOfApp.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                        lblProjectType.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                        lblProjCap.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                        lblProjLoc.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();

                    }
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
    }
}