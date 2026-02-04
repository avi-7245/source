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
    public partial class MSKVYCommList : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSKVYCommList));
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {

                fillGrid();

            }
        }

        protected void fillGrid()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;

                        //strQuery = "SELECT a.*,b.* FROM MSKVY_applicantdetails  a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "SELECT distinct a.*,b.* FROM MSKVY_applicantdetails  a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and b.roleid='53' and isProposalpprove='Y' and WF_STATUS_CD_C between 14 and 18";


                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        Session["AppDet"] = dsResult;
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            GVApplications.DataSource = dsResult.Tables[0];
                            GVApplications.DataBind();

                        }
                        else
                        {
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
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
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

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Determine the RowIndex of the Row whose Button was clicked.
            int rowIndex = Convert.ToInt32(e.CommandArgument);

            //Reference the GridView Row.
            GridViewRow row = GVApplications.Rows[rowIndex];

            //Fetch value of Name.
            //string name = (row.FindControl("txtName") as TextBox).Text;

            //Fetch value of Country
            string applicationId = row.Cells[0].Text;
            Session["PROJID"] = row.Cells[1].Text;
            //Session["APPID"] = row.Cells[0].Text; ;
            if (e.CommandName == "ApproveReturn")
            {

                Response.Redirect("~/UI/Emp/PropasalAcceptance.aspx?application=" + applicationId, false);
            }
            if (e.CommandName == "UploadLetter")
            {
                Response.Redirect("~/UI/Emp/MSKVYUploadIssueLetter.aspx?application=" + applicationId, false);
            }
            if (e.CommandName == "IssueDemandNote")
            {
                //MySqlConnection mySqlConnection = new MySqlConnection();
                //mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                //string strGSTIN = string.Empty;
                //int rowCount = 0;

                //int rollid = int.Parse(Session["EmpRole"].ToString());
                //rollid++;
                ////Session["EmpZone"] = "Thane";
                //try
                //{

                //    mySqlConnection.Open();
                //    




                //    string strQuery = string.Empty;
                //    MySqlCommand cmd;

                //    strQuery = "update MSKVY_applicantdetails  set  CommittmentFees=, IsCommittmentFeesPaid='Y' where APPLICATION_NO='" + applicationId + "'";
                //    cmd = new MySqlCommand(strQuery, mySqlConnection);
                //    cmd.ExecuteNonQuery();

                //    strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                //                           " values('" + applicationId + "','Demand Note Issued.',CURDATE())";
                //    cmd = new MySqlCommand(strQuery, mySqlConnection);
                //    cmd.ExecuteNonQuery();

                //}
                //catch (Exception exception)
                //{
                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                //    log.Error(ErrorMessage);

                //}

                //finally
                //{
                //    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                //    {
                //        mySqlConnection.Close();
                //    }

                //}
                sendEmail(applicationId);
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('Demand note Issued.')</script>");
            }

        }
        protected void sendEmail(string strAppID)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strQuery = string.Empty;
            double projectCapacityMW = 0;
            double projectCapacityAmount = 0;

            try
            {

                mySqlConnection.Open();



                strQuery = "select a.*,b.* from MSKVY_applicantdetails  a, applicant_reg_det b where a.APPLICATION_NO='" + strAppID + "' and a.USER_NAME=b.USER_NAME";
                log.Error("select query- " + strQuery);
                MySqlCommand cmd;
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);

                string strBody = string.Empty;
                string strSubject = string.Empty;
                projectCapacityMW = float.Parse(dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString());
                projectCapacityAmount = projectCapacityMW * 100000;

                strQuery = "update MSKVY_applicantdetails  set WF_STATUS_CD_C=15, CommittmentFees='" + projectCapacityAmount + "',app_status='DEMAND NOTE ISSUED.',IssueLetter='Y'  where APPLICATION_NO='" + strAppID + "'";
                log.Error("Update query - " + strQuery);
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();

                objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "15");

                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                       " values('" + strAppID + "','Demand Note Issued.',CURDATE(),'" + Session["SAPID"].ToString() + "')";
                cmd = new MySqlCommand(strQuery, mySqlConnection);
                cmd.ExecuteNonQuery();
                //strQuery = "select * from LFSFeasibilityStatus where a.APPLICATION_NO='" + strAppID + "'";
                //cmd = new MySqlCommand(strQuery, mySqlConnection);
                //DataSet dsResultFeasibility = new DataSet();
                //MySqlDataAdapter daResultFeasibility = new MySqlDataAdapter(cmd);
                //daResultFeasibility.Fill(dsResultFeasibility);

                strSubject = "Payment of Commitment fee of Rs." + projectCapacityAmount +
                    " towards Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Project proposed by M/s." +
                    dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village :" + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + " ,Tal.: " +
                    dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + " Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + ".";

                strBody += "Respected Sir/Madam" + ",<br/>";
                strBody = "With reference to above subject, as per Part 1, Clause No. 1 of the GoM GR Dt. 11/05/2022 towards Methodology for establishment of RE Projects under GOM RE Policy 2020, you will have to furnish a Commitment fee of Rs.1.0 Lakh per MW to MSETCL, for timely completion of evacuation arrangement." +
                    " Hence, you are requested to furnish a Commitment fee of <b>Rs." + projectCapacityAmount + " </b>" + "to MSETCL. The commitment fee should be paid online through web portal " +
                    " As per the Part 1, Clause No. 1 of the GoM GR Dt. 11/05/2022 towards Methodology for establishment of RE Projects under GOM RE Policy 2020, the said commitment fee will be refunded to you without any interest after completion of evacuation of arrangement within time limit prescribed by MSETCL in the Grid Connectivity letter. However, in the event of non-completion of evacuation arrangement within time limit, the grid connectivity will be cancelled and the said commitment fee will be forfeited by MSETCL." +
                    " Also, only two extensions of six months each shall be granted after assessment of work progress. If Project is not completed within time period of 2nd time extension, the Grid Connectivity will be cancelled and commitment fee for the same will be forfeited. " +
                    " It is requested to furnish the above mentioned Commitment fee within 15 days (Fifteen days) from the date of this letter. Grid Connectivity for the said Project will be issued only after receipt of Commitment fee within 15 days. If you fail to submit the Commitment fee your application for Grid Connectivity shall be treated as cancelled. No further correspondence will be made in this regards. If required, you will have to apply afresh for grid connectivity and will not have any precedence over other interested applicants in that area. </b><br/><br/><br/><br/>";
                strBody += "Thanks & Regards, " + "<br/>";
                //  strBody += " Chief Engineer" + "<br/>";
                strBody += "State Transmission Utility (STU)" + "<br/>";
                strBody += "MSETCL  " + "<br/>";
                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                string mailTo = string.Empty;
                MailMessage Msg = new MailMessage();
                MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                Msg.From = fromMail;
                Msg.IsBodyHtml = true;
                //log.Error("from:" + fromAddress);
                //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                Msg.To.Add(new MailAddress(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));

                foreach (var address in ConfigurationManager.AppSettings["MMCCEmailID"].ToString().Split(';'))
                {
                    //mailMessagePlainText.To.Add(new MailAddress(address.Trim(), ""));
                    Msg.CC.Add(new MailAddress(address));
                }

                //Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));
                Msg.Subject = strSubject;
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


                DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53)");


                #region Send SMS
                string strMobNos = string.Empty;
                strMobNos = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + ",";
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
                    string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Developer%20is%20requested%20to%20pay%20commitment%20fee%20against%20Online%20Application%20No.%20" + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "%20proposed%20by%20M%2Fs." + dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + "%5CnRegards%2C%20STU%20%28MSETCL%29&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
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

            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
                // throw ex;
            }
            fillGrid();
        }

        protected void GVApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["AppDet"];
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        int rowno = e.Row.RowIndex;
                        //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        //{


                        Button btnIssueDemandNote = (Button)e.Row.Cells[9].FindControl("btnIssueDemandNote");
                        Button btnUploadGCLetter = (Button)e.Row.Cells[9].FindControl("btnUploadGCLetter");


                        // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                        string app = e.Row.Cells[0].Text;
                        string Project_type = e.Row.Cells[4].Text;
                        int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());

                        log.Error(wfStatus.ToString());

                        string hex = "#008CBA";
                        System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

                        //btnView.BackColor = System.Drawing.Color.Red;

                        btnIssueDemandNote.BackColor = System.Drawing.Color.Red;



                        if (wfStatus == 14)
                        {

                            //btnIssueDemandNote.Enabled = true;
                            //btnIssueDemandNote.BackColor = _color;
                            btnUploadGCLetter.Enabled = true;
                            btnUploadGCLetter.BackColor = _color;

                            //if (Project_type.ToUpper() == "Solar".ToUpper() || Project_type.ToUpper() == "Wind".ToUpper() || Project_type.ToUpper() == "Hybrid (Colocated)".ToUpper() || Project_type.ToUpper() == "Hybrid".ToUpper() || Project_type.ToUpper() == "Solar Park".ToUpper())
                            //{
                            //    btnIssueDemandNote.Enabled = true;
                            //    btnIssueDemandNote.BackColor = _color;
                            //    btnUploadGCLetter.Enabled = false;
                            //    btnUploadGCLetter.BackColor = System.Drawing.Color.Red;
                            //}
                            //else
                            //{
                            //    btnIssueDemandNote.Enabled = false;
                            //    btnIssueDemandNote.BackColor = System.Drawing.Color.Red;
                            //    btnUploadGCLetter.Enabled = true;
                            //    btnUploadGCLetter.BackColor = _color;
                            //}

                        }


                        //}
                    }
                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Row Bound Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    //lblResult.Text = "Some problem during registration.Please try again.";
                }
            }
        }
    }
}