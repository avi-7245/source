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

namespace GGC.UI.Emp
{
    public partial class ProposalApprovalList : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ProposalApprovalList));
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int role = int.Parse(Session["EmpRole"].ToString());

            if (role > 51 && role <= 60)
            {
                if (role == 53)
                {
                    
                    comFees.Visible = true;
                    comFeesApr.Visible = true;
                }
                else
                {
                    comFees.Visible = false;
                    comFeesApr.Visible = false;
                }

            }
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

                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
//                        strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and b.roleid='" + Session["EmpRole"].ToString() + "' ";
                        strQuery = "SELECT distinct a.APPLICATION_NO, a.MEDAProjectID ,a.PROMOTOR_NAME,a.PROJECT_LOC,a.NATURE_OF_APP,a.PROJECT_TYPE,a.PROJECT_CAPACITY_MW,a.app_status,a.APP_STATUS_DT,concat(a.PROJECT_LOC,' ', a.PROJECT_TALUKA,' ', a.PROJECT_DISTRICT) as Location FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and b.roleid='" + Session["EmpRole"].ToString() + "' and WF_STATUS_CD_C<14";

                        
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        //if (dsResult.Tables[0].Rows.Count > 0)
                        //{
                            GVApplications.DataSource = dsResult.Tables[0];
                            GVApplications.DataBind();
                        //}
                        //else
                        //{
                        //    //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
                        //}

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
            
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVApplications.Rows[rowIndex];

            string applicationId = row.Cells[0].Text;
            Session["APPID"] = row.Cells[0].Text;
            Session["PROJID"] = row.Cells[1].Text;

            if (e.CommandName == "ApproveReturn")
            {

                Response.Redirect("~/UI/Emp/PropasalAcceptance.aspx?application=" + applicationId, false);
            }

            if (e.CommandName == "IssueDemandNote")
            {
                sendEmail(applicationId);
            }
            if (e.CommandName == "ViewAllDoc")
            {

                Response.Redirect("~/UI/Emp/ViewAllDocs.aspx?application=" + applicationId, false);
            }
        }
        protected void sendEmail(string strAppID)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strQuery = string.Empty;
            float projectCapacityMW = 0;
            float projectCapacityAmount = 0;
            
            try
            {

                mySqlConnection.Open();
                

            strQuery = "select a.*,b.* from APPLICANTDETAILS a, applicant_reg_det b where a.APPLICATION_NO='" + strAppID + "' and a.PAN_TAN_NO=b.PAN_TAN_NO ";
            MySqlCommand cmd;
            cmd = new MySqlCommand(strQuery, mySqlConnection);
            DataSet dsResult = new DataSet();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dsResult);

            strQuery = "select * from LFSFeasibilityStatus where a.APPLICATION_NO='" + strAppID + "'";


            cmd = new MySqlCommand(strQuery, mySqlConnection);
            DataSet dsResultFeasibility = new DataSet();
            MySqlDataAdapter daResultFeasibility = new MySqlDataAdapter(cmd);
            daResultFeasibility.Fill(dsResultFeasibility);
            string strBody = string.Empty;
            string strSubject = string.Empty;
            projectCapacityMW = float.Parse(dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString());
            projectCapacityAmount=projectCapacityMW*100000;
            strSubject = "Payment of Commitment fee of Rs." + projectCapacityAmount +
                " Lakhs towards Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Project proposed by M/s."+
                dsResult.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village :" + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString()+" ,Tal.: " +
                dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + " Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +".";
            
            strBody += "Respected Sir/Madam" + ",<br/>";
            strBody = "With reference to above subject, as per Part 1, Clause No. 1 of the GoM GR Dt. 11/05/2022 towards Methodology for establishment of RE Projects under GOM RE Policy 2020, you will have to furnish a Commitment fee of Rs.1.0 Lakh per MW to MSETCL, for timely completion of evacuation arrangement." +
                " Hence, you are requested to furnish a Commitment fee of <b>Rs."+projectCapacityAmount+ " Lakhs </b>"+"to MSETCL. The commitment fee should be paid in the form of Demand Draft from any nationalized bank, drawn in the name of <b>“Maharashtra State Electricity Transmission Company Ltd.” or “MSETCL”, payable at Mumbai. </b>"+
                " As per the Part 1, Clause No. 1 of the GoM GR Dt. 11/05/2022 towards Methodology for establishment of RE Projects under GOM RE Policy 2020, the said commitment fee will be refunded to you without any interest after completion of evacuation of arrangement within time limit prescribed by MSETCL in the Grid Connectivity letter. However, in the event of non-completion of evacuation arrangement within time limit, the grid connectivity will be cancelled and the said commitment fee will be forfeited by MSETCL."+
                " Also, only two extensions of six months each shall be granted after assessment of work progress. If Project is not completed within time period of 2nd time extension, the Grid Connectivity will be cancelled and commitment fee for the same will be forfeited. "+
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
            Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMCCEmailID"].ToString()));
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
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
                // throw ex;
            }

        }
    }
}