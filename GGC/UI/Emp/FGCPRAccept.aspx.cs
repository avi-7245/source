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
    public partial class FGCPRAccept : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FGCPRAccept));
        string strAppID = "";
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            
            if (!Page.IsPostBack)
            {
                Session["application"] = Request.QueryString["application"];
                strAppID = Session["application"].ToString();
                fillData();

            }
        }
        void fillData()
        {
            string strGSTIN = string.Empty;
            strAppID = Session["application"].ToString();
            int rollid = int.Parse(Session["EmpRole"].ToString());

            try
            {

                string strQuery = string.Empty;
                strQuery = " select a.*, c.NAME_OF_SPD,c.SPV_Name,c.PROJECT_CAPACITY_MW,c.project_loc  from proposalapproval_fgc a,mskvy_applicantdetails_spd c where a.APPLICATION_NO=c.APPLICATION_NO and a.APPLICATION_NO='" + strAppID + "' and roleid='" + rollid + "' and isAppr_Rej_Ret is null order by createDT desc";


                DataSet dsResultStatus = new DataSet();
                dsResultStatus = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                HDFCount.Value = dsResultStatus.Tables[0].Rows.Count.ToString();

                strQuery = "select a.*, c.NAME_OF_SPD,c.SPV_Name,c.PROJECT_CAPACITY_MW,c.project_loc,c.INTERCONNECTION_AT  from proposalapproval_fgc a,mskvy_applicantdetails_spd c where a.APPLICATION_NO=c.APPLICATION_NO and a.APPLICATION_NO='" + strAppID + "' and roleid='" + rollid + "' order by createDT desc";
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
                strQuery = "select distinct a.APPLICATION_NO, case when a.isAppr_Rej_Ret='Y' then 'Verified' else 'Return' end as isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from proposalapprovaltxn_fgc a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by b.srno desc";

                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResult.Tables[0];
                    GVApplications.DataBind();
                }

                strQuery = "SELECT if(isDeviation=0,'No','Yes') as Deviation,Deviation_Remark FROM finalgcapproval WHERE APPLICATION_NO='" + strAppID + "'";

                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                lblIsDev.Text = dsResult.Tables[0].Rows[0]["Deviation"].ToString();
                lblDevRemark.Text = dsResult.Tables[0].Rows[0]["Deviation_Remark"].ToString();
                //}
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Submission Failed!!";
            }
        }

        protected void btnApproved_Click(object sender, EventArgs e)
        {
            string strGSTIN = string.Empty;
            string isApproved = "N";
            int rowCount = 0;
            strAppID = Session["application"].ToString();
            int rollid = int.Parse(Session["EmpRole"].ToString());
            if (rollid == 53)
            {
                if (lblIsDev.Text == "No")
                {
                    rollid = 2;
                }
                else
                {
                    rollid = 54;
                }
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
                //strQuery = "update proposalapproval_fgc set isAppr_Rej_Ret='Y', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='" + rollid + "', createDT=now() where APPLICATION_NO='" + strAppID + "'";
                strQuery = "update proposalapproval_fgc set  isAppr_Rej_Ret='Y', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='" + rollid + "', createDT=now() where APPLICATION_NO='" + strAppID + "'";
                //}

                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                /*Insert into Transaction Table */
                strQuery = "insert into proposalapprovaltxn_fgc(APPLICATION_NO , isAppr_Rej_Ret,remark,Aprove_Reject_Return_by, roleid , createDT ,createBy) values ('" + Session["application"].ToString() + "','Y','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "', '" + rollid + "', now() , '" + Session["SAPID"].ToString() + "')";
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);


                float projectCapacity = lblProjCap.Text == "" ? 0 : float.Parse(lblProjCap.Text);
                if (Session["EmpRole"].ToString() == "53" && lblIsDev.Text=="No")
                {
                    strQuery = "update finalgcapproval  set  WF_STATUS_CD=18, app_status='PROPOSAL APPROVED.', app_status_dt=now() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);


                    strQuery = "update proposalapproval_fgc set  roleid='2' where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);



                    isApproved = "Y";
                    objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "18");
                    sendMailApproval(strAppID, isApproved);
                }

                if (Session["EmpRole"].ToString() == "54")
                {
                    strQuery = "update finalgcapproval  set  WF_STATUS_CD=18, app_status='PROPOSAL APPROVED.', app_status_dt=now() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);


                    strQuery = "update proposalapproval_FGC set  roleid='2' where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);



                    isApproved = "Y";
                    objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "18");
                    sendMailApproval(strAppID, isApproved);
                }


                //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from MSKVY_applicantdetails  where application_no='" + strAppID + "'");
                //DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53,54)");




                #region Send SMS
                string strMobNos = string.Empty;

                try
                {
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
                    //string strURL = string.Empty;
                    //if (Session["EmpRole"].ToString() == "54")
                    //{

                    //    strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20proposal%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20by%20M%2Fs.%20" + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20forwarded%20by%20CE%2C%20STU%20for%20Approval%2FRecommendation%20of%20Director%20%28O%29.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                    //    WebRequest request = HttpWebRequest.Create(strURL);
                    //    //log.Error("2 ");
                    //    // Get the response back  
                    //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //    Stream s = (Stream)response.GetResponseStream();
                    //    StreamReader readStream = new StreamReader(s);
                    //    string dataString = readStream.ReadToEnd();
                    //    log.Error("8 " + dataString);

                    //    response.Close();
                    //    s.Close();
                    //    readStream.Close();
                    //    //log.Error("strURL " + strURL);
                    //}


                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    log.Error(ErrorMessage);
                }
                #endregion



                //strQuery = "select a.APPLICATION_NO,a.isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from proposalapproval_fgctxn a, empmaster b where a.APPLICATION_NO='" + strAppID + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.srno desc";
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
            int rowCount = 0;

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
                rowCount = Int32.Parse(HDFCount.Value.ToString());

                if (Session["EmpRole"].ToString() == "51")
                {
                    strQuery = "update proposalapproval_fgc set isAppr_Rej_Ret='R', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='2', createDT=now() where APPLICATION_NO='" + Session["application"].ToString() + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //strQuery = "update finalgcapproval set WF_STATUS_CD_C=6, app_status='Proposal returned', app_status_dt=now() where APPLICATION_NO='" + strAppID + "'";
                    strQuery = "update finalgcapproval set WF_STATUS_CD=6, app_status='Proposal returned', app_status_dt=now() where APPLICATION_NO='" + Session["application"].ToString() + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                }
                else
                {
                    strQuery = "update proposalapproval_fgc set isAppr_Rej_Ret='R', remark='" + txtRemark.Text + "',Aprove_Reject_Return_by='" + Session["SAPID"].ToString() + "', roleid='" + rollid + "', createDT=now() where APPLICATION_NO='" + Session["application"].ToString()  + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                }


                strQuery = "insert into proposalapprovaltxn_fgc(APPLICATION_NO ,isAppr_Rej_Ret, remark,Aprove_Reject_Return_by, roleid , createDT ,createBy) values ('" + Session["application"].ToString()  + "','R','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "', '" + rollid + "', now() , '" + Session["SAPID"].ToString() + "')";
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                strQuery = "select a.APPLICATION_NO,a.isAppr_Rej_Ret,a.remark,concat(b.DESIGNATION,concat(' (',concat(a.Aprove_Reject_Return_by,')'))) Aprove_Reject_Return_by ,a.createDT from proposalapprovaltxn_fgc a, empmaster b where a.APPLICATION_NO='" + Session["application"].ToString() + "' and a.isAppr_Rej_Ret is not null and b.SAPID=a.Aprove_Reject_Return_by order by a.srno desc";

                DataSet dsResultStatus = new DataSet();
                dsResultStatus = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                if (dsResultStatus.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResultStatus.Tables[0];
                    GVApplications.DataBind();
                }
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from finalgcapproval  where application_no='" + strAppID + "'");
                DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53)");

                #region Send SMS
                string strMobNos = string.Empty;

                try
                {
                    //log.Error("1 ");
                    if (dsEmailMob.Tables[0].Rows.Count > 0)
                    {



                        if (dsEmailMob.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsEmailMob.Tables[0].Rows.Count; i++)
                            {
                                if (dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                                    strMobNos += dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                            }

                        }
                    }
                    string strURL = string.Empty;
                    if (Session["EmpRole"].ToString() == "54")
                    {
                        strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20proposal%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20by%20M%2Fs.%20" + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20returned%20by%20Director%20%28O%29%20for%20compliance.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                    }
                    if (Session["EmpRole"].ToString() == "55")
                    {
                        strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Grid%20Connectivity%20proposal%20against%20Online%20Application%20No." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20by%20M%2Fs.%20" + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%20has%20been%20returned%20by%20Hon.%20CMD%20for%20compliance.%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                    }
                    //log.Error("strURL " + strURL);

                    WebRequest request = HttpWebRequest.Create(strURL);
                    //log.Error("2 ");
                    // Get the response back  
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream s = (Stream)response.GetResponseStream();
                    StreamReader readStream = new StreamReader(s);
                    string dataString = readStream.ReadToEnd();
                    log.Error("8 " + dataString);

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

                string strQuery = "SELECT a.*,b.*,concat(a.PROJECT_LOC,' ', a.PROJECT_TALUKA,' ', a.PROJECT_DISTRICT) as Location FROM MSKVY_applicantdetails_SPD  a, applicant_reg_det b WHERE a.USER_NAME=b.USER_NAME and a.APPLICATION_NO='" + strAppID + "'";
                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);

                strQuery = "select * from empmaster where sapid='" + Session["SAPID"].ToString() + "'";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResultEmail = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dsResultEmail);

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
                            strBody += "Grid Connectivity Proposal against Project ID No " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + " , Online Application No " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + " for " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " , " + dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed at " + dsResult.Tables[0].Rows[0]["Location"].ToString() + " (location) by M/s. " + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " has been forworded for Approved.";
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
                        //Msg.CC.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                        string strCC = ConfigurationManager.AppSettings["MMEmailID"].ToString();
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

                strQuery = "select * from empmaster where role_id=(select role_id from empmaster where sapid='" + Session["SAPID"].ToString() + "')-1";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResultEmail = new DataSet();
                da = new MySqlDataAdapter(cmd);
                da.Fill(dsResultEmail);

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
                        if (rollid != 51)
                        {
                            Msg.To.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["EmpEmailID"].ToString()));
                        }
                        else
                        {
                            Msg.To.Add(new MailAddress(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                        }
                        //Msg.CC.Add(new MailAddress(dsResultEmail.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));

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