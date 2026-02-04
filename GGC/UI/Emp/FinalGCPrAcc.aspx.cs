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
using log4net;
using System.Reflection;
using System.Globalization;
using Microsoft.Reporting.WebForms;
using GGC.Common;
using System.Text;
using Newtonsoft.Json;
using System.Net.Mail;

namespace GGC.UI.Emp
{
    public partial class FinalGCPrAcc : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(PropasalAcceptance));
        string strAppID = "";
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            strAppID = Request.QueryString["application"];
            Session["AppID"] = Request.QueryString["application"];
            if (!Page.IsPostBack)
            {

                fillData();

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
                strQuery = "select * from finalgcapproval where APPLICATION_NO='" + Session["AppID"].ToString() + "' and roleid='" + rollid + "' and isAppr_Rej_Ret is null order by createDT desc";


                DataSet dsResultStatus = new DataSet();
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                da.Fill(dsResultStatus);
                HDFCount.Value = dsResultStatus.Tables[0].Rows.Count.ToString();

                strQuery = "select * from applicantdetails  where APPLICATION_NO='" + Session["AppID"].ToString() + "'";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    lblApplcationNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                    lblDevName.Text = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                    lblNatOfApp.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                    lblProjectType.Text = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                    lblProjCap.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                    lblProjLoc.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();

                }
                strQuery = "select case when isDeviation='Y' then 'Yes' else 'No' end isDeviation,Deviation_Remark from finalgcapproval  where APPLICATION_NO='" + Session["AppID"].ToString() + "'";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                dsResult = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);
                if (dsResult.Tables[0].Rows.Count > 0)
                {

                    lblIsDeviation.Text = dsResult.Tables[0].Rows[0]["isDeviation"].ToString();
                    lblDevRemark.Text = dsResult.Tables[0].Rows[0]["Deviation_Remark"].ToString();

                }


                strQuery = "select distinct a.APPLICATION_NO, case when a.isAppr_Rej_Ret='Y' then 'Verified' else 'Return' end as isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from finalgcapprovaltxn a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.srno desc";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                dsResult = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResult.Tables[0];
                    GVApplications.DataBind();
                }
            }
            //}


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
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            string strIsDeviation = lblIsDeviation.Text;
            int maxRoleId = 55;
            int roleID = int.Parse(Session["EmpRole"].ToString());
            string strQuery = string.Empty;
            strAppID = lblApplcationNo.Text;
            float projCapacity = 0.0f;
            projCapacity = float.Parse(lblProjCap.Text);
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            try
            {
                mySqlConnection.Open();

                if (projCapacity > 20 && strIsDeviation == "Yes")
                {
                    maxRoleId = 55;
                }

                else if (projCapacity < 20 && (strIsDeviation == "No" || strIsDeviation == "Yes"))
                {
                    maxRoleId = 54;
                }
                else
                {
                    maxRoleId = 54;
                }
                if (roleID == maxRoleId)
                {

                    strQuery = "update finalgcapproval set isAppr_Rej_Ret='Y', remark='" + txtRemark.Text + "', Aprove_Reject_Return_by= '" + Session["SAPID"].ToString() + "', roleid=2 where APPLICATION_NO='" + strAppID + "'";
                    //strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','51'," + strAppID + ")";
                }
                else
                {
                    strQuery = "update finalgcapproval set isAppr_Rej_Ret='Y', remark='" + txtRemark.Text + "', Aprove_Reject_Return_by= '" + Session["SAPID"].ToString() + "', roleid='" + (roleID + 1) + "' where APPLICATION_NO='" + strAppID + "'";
                    //if(strIsDeviation=="Y")
                    //    strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,isDeviation,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "','" + rbDev.SelectedItem.Value + "','" + (roleID + 1) + "','" + Session["SAPID"].ToString() + "')";
                    //else
                    //    strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "','" + (roleID + 1) + "','" + Session["SAPID"].ToString() + "')";

                }
                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();

                strQuery = "insert into finalgcapprovaltxn(APPLICATION_NO,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,roleid,createby) values('" + strAppID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "'," + roleID + ",'" + Session["SAPID"].ToString() + "')";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                if (roleID == maxRoleId)
                {
                    strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=27, app_status='FINAL GRID CONNECTIVITY PROPOSAL APPROVED.',APP_STATUS_DT=NOW() where APPLICATION_NO='" + strAppID + "'";
                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                    cmd.ExecuteNonQuery();

                    string strQueryGetDetails = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + Session["AppID"].ToString() + "'";
                    MySqlCommand cmdQueryGetDetails = new MySqlCommand(strQueryGetDetails, mySqlConnection);

                    DataSet dsResultGetDetails = new DataSet();
                    MySqlDataAdapter daQueryGetDetails = new MySqlDataAdapter(cmdQueryGetDetails);
                    daQueryGetDetails.Fill(dsResultGetDetails);

                    string strEmail = dsResultGetDetails.Tables[0].Rows[0]["Zone_email"].ToString();
                    if (dsResultGetDetails.Tables[0].Rows.Count > 0)
                    {
                        #region Send Mail
                        //sendMailOTP(strRegistrationno, strEmailID);
                        string strBody = string.Empty;

                        strBody += "Dear Sir/Madam" + ",<br/>";
                        strBody += "Regarding your request for Final Grid Connectivity to " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed by M/s. " + dsResultGetDetails.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Tal.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + ", on scrutiny of documents All documents have been verified. ";

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
                            Msg.To.Add(new MailAddress(dsResultGetDetails.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                            Msg.CC.Add(new MailAddress(dsResultGetDetails.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                            Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMEmailID"].ToString()));
                            //  Msg.To.Add(new MailAddress(toAddress));

                            //Msg.Subject = "SLD document rejected in Online Grid connectivity application.";
                            Msg.Subject = "Regarding Final Grid Connectivity for " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed by M/s. " + dsResultGetDetails.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Tal.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "";
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

                }
                strQuery = "select distinct a.APPLICATION_NO, case when a.isAppr_Rej_Ret='Y' then 'Verified' else 'Return' end as isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from finalgcapprovaltxn a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.srno desc";


                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResult.Tables[0];
                    GVApplications.DataBind();
                }
                lblApplcationNo.Text = "";
                lblDevName.Text = "";
                lblNatOfApp.Text = "";
                lblProjectType.Text = "";
                lblProjCap.Text = "";
                lblProjLoc.Text = "";
                lblIsDeviation.Text = "";
                lblDevRemark.Text = "";
                lblResult.Text = "Submitted Successfully";
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
            string strAppID = Session["APPID"].ToString();
            string strMEDAID = Session["MEDAID"].ToString();
            int roleID = int.Parse(Session["EmpRole"].ToString());

            MySqlConnection mySqlConnection1 = new MySqlConnection();

            try
            {

                mySqlConnection1.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection1.Open();
                int maxRoleId = 0;
                string strIsDev = Session["isDeviation"].ToString();
                switch (mySqlConnection1.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;

                        if (roleID == 51)
                        {

                            strQuery = "update finalgcapproval set isAppr_Rej_Ret='N', remark='" + txtRemark.Text + "', Aprove_Reject_Return_by= '" + Session["SAPID"].ToString() + "', roleid='2' where APPLICATION_NO='" + strAppID + "'";
                            //strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','51'," + strAppID + ")";
                        }
                        else
                        {
                            strQuery = "update finalgcapproval set isAppr_Rej_Ret='N', remark='" + txtRemark.Text + "', Aprove_Reject_Return_by= '" + Session["SAPID"].ToString() + "', roleid='" + (roleID - 1) + "' where APPLICATION_NO='" + strAppID + "'";
                            //if(strIsDeviation=="Y")
                            //    strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,isDeviation,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "','" + rbDev.SelectedItem.Value + "','" + (roleID + 1) + "','" + Session["SAPID"].ToString() + "')";
                            //else
                            //    strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "','" + (roleID + 1) + "','" + Session["SAPID"].ToString() + "')";

                        }
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection1);
                        cmd.ExecuteNonQuery();

                        strQuery = "insert into finalgcapprovaltxn(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "'," + roleID + ",'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection1);
                        cmd.ExecuteNonQuery();

                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('Returned Succsefully.')</script>");

                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:
                        break;

                }
            }
            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Submission Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Submission Failed!!";
            }

            finally
            {
                if (mySqlConnection1.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection1.Close();
                }

            }
        }

        protected void btnViewPropsal_Click(object sender, EventArgs e)
        {
            string applicationId = lblApplcationNo.Text;
            //    Response.Redirect("~/UI/Emp/FinalGCDoc.aspx?application=" + applicationId, false);

            MySqlConnection mySqlConnection1 = new MySqlConnection();

            try
            {

                mySqlConnection1.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection1.Open();
                //int maxRoleId = 0;
                //string strIsDev = Session["isDeviation"].ToString();
                switch (mySqlConnection1.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        string strQueryGetDetails = "select FGCDraftPrFileName from finalgcapproval where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmdQueryGetDetails = new MySqlCommand(strQueryGetDetails, mySqlConnection1);

                        DataSet dsResultGetDetails = new DataSet();
                        MySqlDataAdapter daQueryGetDetails = new MySqlDataAdapter(cmdQueryGetDetails);
                        daQueryGetDetails.Fill(dsResultGetDetails);
                        string fileName = dsResultGetDetails.Tables[0].Rows[0]["FGCDraftPrFileName"].ToString();

                        //Path of the File to be downloaded.
                        string filePath = Server.MapPath(string.Format("~/Files/FinalGC/" + applicationId + "/{0}", fileName));

                        //Content Type and Header.
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);

                        //Writing the File to Response Stream.
                        Response.WriteFile(filePath);

                        //Flushing the Response.
                        Response.Flush();
                        Response.End();

                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:
                        break;

                }
            }
            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Submission Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Submission Failed!!";
            }

            finally
            {
                if (mySqlConnection1.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection1.Close();
                }

            }

        }

        protected void btnViewLetter_Click(object sender, EventArgs e)
        {
            string applicationId = lblApplcationNo.Text;
            //    Response.Redirect("~/UI/Emp/FinalGCDoc.aspx?application=" + applicationId, false);

            MySqlConnection mySqlConnection1 = new MySqlConnection();

            try
            {

                mySqlConnection1.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection1.Open();
                //int maxRoleId = 0;
                //string strIsDev = Session["isDeviation"].ToString();
                switch (mySqlConnection1.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        string strQueryGetDetails = "select FGCDraftLetterFileName from finalgcapproval where APPLICATION_NO='" + applicationId + "'";
                        MySqlCommand cmdQueryGetDetails = new MySqlCommand(strQueryGetDetails, mySqlConnection1);

                        DataSet dsResultGetDetails = new DataSet();
                        MySqlDataAdapter daQueryGetDetails = new MySqlDataAdapter(cmdQueryGetDetails);
                        daQueryGetDetails.Fill(dsResultGetDetails);
                        string fileName = dsResultGetDetails.Tables[0].Rows[0]["FGCDraftLetterFileName"].ToString();

                        //Path of the File to be downloaded.
                        // string filePath = Server.MapPath(string.Format("~/Files/{0}", fileName));
                        string filePath = Server.MapPath(string.Format("~/Files/FinalGC/" + applicationId + "/{0}", fileName));

                        //Content Type and Header.
                        Response.ContentType = "application/pdf";
                        Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);

                        //Writing the File to Response Stream.
                        Response.WriteFile(filePath);

                        //Flushing the Response.
                        Response.Flush();
                        Response.End();

                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:
                        break;

                }
            }
            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Submission Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Submission Failed!!";
            }

            finally
            {
                if (mySqlConnection1.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection1.Close();
                }

            }
        }
    }
}