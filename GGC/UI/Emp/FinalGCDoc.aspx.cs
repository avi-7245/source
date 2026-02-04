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
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using Org.BouncyCastle.Crypto.Engines;

namespace GGC.UI.Emp
{
    public partial class FinalGCDoc : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FinalGCDoc));
        private readonly string connectionStrings = ConfigurationManager.AppSettings["DevpGridConnectivity"];

        bool isApplicationApproved = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";

            if (!IsPostBack)
            {
                if (Request.QueryString["appid"] != null)
                {
                    string appId = Request.QueryString["appid"].ToString();
                    Session[SessionConst.ApplicationNo] = appId.ToString();

                    isApplicationApproved = IsApplicationApproved();
                    fillGrid(appId);
                    FillGVDeviationHistory();
                }

            }
        }
        protected void GetDocuments(string ITCode)
        {
            #region Display Files name directly from directory
            //string[] filePaths = Directory.GetFiles((Server.MapPath("~/Files/FinalGC/" + ITCode + "/")));
            //List<ListItem> files =new List<ListItem>();
            //foreach (string filePath in filePaths)
            //{
            //    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
            //}
            //grvDocuments.DataSource = files;
            //grvDocuments.DataBind();
            #endregion



        }
        protected void fillGrid(string appId)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            //string strUserName = Session["user_name"].ToString(); ;
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        //                        strQuery = "select a.*, b.DocName from finalgridcondocs a, finalgcdocmaster b where a.FileSrNo=b.srno and a.APPLICATION_NO='" + Session["AppID"] .ToString()+ "' and (a.isVerified='Y' or a.isVerified is null) order by a.CreateDt";
                        strQuery = "select a.*, b.DocName from finalgridcondocs a, finalgcdocmaster b where a.FileSrNo=b.srno and a.APPLICATION_NO='" + Session["AppID"].ToString() + "'  order by b.srno";
                        //strQuery = "select *  from finalgridcondocs where APPLICATION_NO='" + appId + "' and (isVerified='Y' or isVerified is null) order by CreateDt";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            Session["documents"] = dsResult;

                            grvDocuments.DataSource = dsResult.Tables[0];
                            grvDocuments.DataBind();
                        }

                        strQuery = "select *  from finalgcapproval where APPLICATION_NO='" + appId + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);

                        dsResult = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (dsResult.Tables[0].Rows[0]["isDeviation"].ToString() != null)
                            Session["isDeviation"] = dsResult.Tables[0].Rows[0]["isDeviation"].ToString();

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
                string ErrorMessage = "Sql ExceptionMethod Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "FilData Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
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

        protected void grvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "AddRemark")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = grvDocuments.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = Session["AppID"].ToString();

                string strDocName = row.Cells[1].Text;






            }
            if (e.CommandName == "View")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = grvDocuments.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = Session["AppID"].ToString();
                HiddenField btnChange = row.FindControl("HdfFileName") as HiddenField;

                string strDocName = row.Cells[1].Text;
                log.Error("File Name : " + "~/UI/Emp/ViewFinalGCDoc.aspx?appid=" + applicationId + "&docname=" + btnChange.Value.ToString() + "");
                Response.Redirect("~/UI/Emp/ViewFinalGCDoc.aspx?appid=" + applicationId + "&docname=" + btnChange.Value.ToString() + "", false);


            }
            if (e.CommandName == "Change")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = grvDocuments.Rows[rowIndex];

                //Button btnChange = row.FindControl("btnChange") as Button;
                RadioButtonList rbVR = row.FindControl("rbVR") as RadioButtonList;
                rbVR.Items.FindByValue("Y").Enabled = true;
                rbVR.Items.FindByValue("N").Enabled = true;

            }
        }

        protected void btnVerified_Click(object sender, EventArgs e)
        {
            lblResult.Text = "";
            lblResult.ForeColor = System.Drawing.Color.Green;

            string strAppID = Session["APPID"].ToString();
            string strMEDAID = Session["MEDAID"].ToString();
            log.Error("1");
            int roleID = int.Parse(Session["EmpRole"].ToString());
            string strIsDeviation = rbDev.SelectedItem?.Value;

            if (string.IsNullOrEmpty(strIsDeviation))
            {
                lblResult.Text = "Select Deviation";
                lblResult.ForeColor = System.Drawing.Color.Red;
                return;
            };


            #region Update finalgridcondocs
            log.Error("2");
            string strAllVerified = "";
            foreach (GridViewRow row in grvDocuments.Rows)
            {
                // Selects the text from the TextBox
                // which is inside the GridView control
                string hdnSRno = ((HiddenField)row.FindControl("HFSrno")).Value;
                string hdnFileSRno = ((HiddenField)row.FindControl("HFFilesrno")).Value;
                RadioButtonList rbVR = (RadioButtonList)row.FindControl("rbVR");
                TextBox txtRemark = (TextBox)row.FindControl("txtRemark");

                string isFileVerified = rbVR.SelectedIndex != -1 ? rbVR.SelectedItem.Value : null;
                //int rowIndex = Convert.ToInt32(e.CommandArgument);   
                MySqlConnection mySqlConnection = new MySqlConnection();

                //Save the File to the Directory (Folder).
                try
                {
                    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                    mySqlConnection.Open();


                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            //string strQuery = "insert into finalgcapprovaldoctxn(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,table_docs_srNo,table_docs_File_srNo,createby) values('" + strAppID + "','" + strMEDAID + "','" + isFileVerified + "','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "','" + hdnSRno + "','" + hdnFileSRno + "','" + Session["SAPID"].ToString() + "')";
                            //MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            //cmd.ExecuteNonQuery();

                            string strQuery = "update finalgridcondocs set isVerified='" + isFileVerified + "', remark='" + txtRemark.Text + "' where SrNo='" + hdnSRno + "' and FileSrNo='" + hdnFileSRno + "' and APPLICATION_NO=" + strAppID;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();


                            //btnFinalSubmit.Enabled = true;
                            //btnFinalSubmit.Visible = true;
                            btnVerified.Enabled = false;

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
                    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                    {
                        mySqlConnection.Close();
                    }

                }

            }
            #endregion Update finalgridcondocs

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

                        string strQuery = "select a.* from finalgridcondocs a where  a.APPLICATION_NO='" + Session["AppID"].ToString() + "' order by FileSrNo";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection1);

                        DataSet dsResultFindAll = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResultFindAll);
                        strAllVerified = "Y";

                        strQuery = "update finalgcapproval set isDeviation= '" + strIsDeviation + "', Deviation_Remark='" + txtRemark.Text + "'  where APPLICATION_NO='" + strAppID + "'";
                        cmd = new MySqlCommand(strQuery, mySqlConnection1);
                        cmd.ExecuteNonQuery();

                        lblResult.Text = "Submitted successfully";


                        #region Is Deviation ?
                        if (rbDev.SelectedItem.Value == "N")
                        {
                            strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=27, app_status='FGC Application Verified.',APP_STATUS_DT=NOW() where APPLICATION_NO='" + strAppID + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection1);
                            cmd.ExecuteNonQuery();

                            #region Application Status tracking date
                            strQuery = "insert into APPLICANT_STATUS_TRACKING (APPLICATION_NO,STATUS,STATUS_DT,Created_By) " +
                                    " values('" + strAppID + "','FGC Application Verified.',now(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection1);
                            cmd.ExecuteNonQuery();
                            #endregion

                            #region Save To MEDA
                            SaveToMEDA objSaveToMEDA = new SaveToMEDA();
                            objSaveToMEDA.saveApp(Session["MEDAID"].ToString(), "27");
                            #endregion Save To MEDA
                        }
                        else if (rbDev.SelectedItem.Value == "Y")
                        {
                            strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=26, app_status='In FGC documents deviation found.Kindly reupload the document.',APP_STATUS_DT=NOW() where APPLICATION_NO='" + strAppID + "'";
                            cmd = new MySqlCommand(strQuery, mySqlConnection1);
                            cmd.ExecuteNonQuery();

                            #region Application Status tracking date
                            strQuery = "insert into APPLICANT_STATUS_TRACKING (APPLICATION_NO,STATUS,STATUS_DT,Created_By) " +
                                    " values('" + strAppID + "','FGC documents deviation found.Kindly reupload the document.',now(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection1);
                            cmd.ExecuteNonQuery();
                            #endregion

                            #region Save To MEDA
                            SaveToMEDA objSaveToMEDA = new SaveToMEDA();
                            objSaveToMEDA.saveApp(Session["MEDAID"].ToString(), "26");
                            #endregion Save To MEDA
                        }
                        #endregion Is Deviation ?


                        #region Verify docs
                        for (int i = 0; i < dsResultFindAll.Tables[0].Rows.Count; i++)
                        {
                            if (dsResultFindAll.Tables[0].Rows[i]["isVerified"].ToString() == "N" || dsResultFindAll.Tables[0].Rows[i]["isVerified"].ToString() == "")
                            {
                                strAllVerified = "N";
                                break;
                            }
                        }

                        //Send mail if all documnet is not verified
                        if (strAllVerified == "N")
                        {
                            string strQueryGetDetails = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + Session["AppID"].ToString() + "'";
                            MySqlCommand cmdQueryGetDetails = new MySqlCommand(strQueryGetDetails, mySqlConnection1);

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
                                strBody += "Regarding your request for Final Grid Connectivity to " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed by M/s. " + dsResultGetDetails.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Tal.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + ", on scrutiny of documents following discrepancies have been observed: ";
                                strBody += txtRemark.Text + "<br/><br/><br/><br/>";
                                strBody += "You are requested to submit the compliance of above and relevant documents so as to enable this office to take further action in this regard.  <br/>  <br/><br/><br/>";

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
                                    Msg.Subject = "Submission of compliance regarding Final Grid Connectivity for " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() + " Project proposed by M/s. " + dsResultGetDetails.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " at Village: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Tal.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + " ";
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

                        else
                        {
                            //Response.Write("<script language='javascript'>window.alert('Submitted Successfully.');window.location='FinalGC.aspx';</script>");
                            Response.Write("<script language='javascript'>window.alert('Submitted Successfully.');</script>");
                        }
                        #endregion Verify docs

                        break;

                    case System.Data.ConnectionState.Closed:

                        // Connection could not be made, throw an error

                        throw new Exception("The database connection state is Closed");

                        break;

                    default:
                        break;

                }
                //Response.Write("<script language='javascript'>window.alert('Submitted Successfully.');window.location('FinalGC.aspx')</script>");
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Submitted Successfully.);", true);
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

            //}
            //else
            //{
            //    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('You have selected one or more return file.Couldnot submit.You have to click on return.')</script>");
            //}

        }

        protected void grvDocuments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataSet ds = new DataSet();
            ds = (DataSet)Session["documents"];
            if (ds != null)
            {
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {

                        int rowno = e.Row.RowIndex;

                        RadioButtonList rbVR = e.Row.Cells[3].FindControl("rbVR") as RadioButtonList;
                        TextBox txtRemark = e.Row.Cells[4].FindControl("txtRemark") as TextBox;
                        Button btnChange = e.Row.Cells[5].FindControl("btnChange") as Button;

                        //int selectValue = (int)DataBinder.Eval(e.Row.DataItem, "isVerified");  

                        string strIsVerified = ds.Tables[0].Rows[rowno]["isVerified"].ToString();

                        bool? isVerified = string.IsNullOrEmpty(strIsVerified) ? (bool?)null : (strIsVerified == "Y");

                        if (isVerified != null)
                        {
                            var item = rbVR.Items.FindByValue(strIsVerified);
                            item.Selected = true;

                            rbVR.Items.Cast<ListItem>().ToList().ForEach(li =>
                            {
                                li.Enabled = false;
                            });
                        }

                        if (isApplicationApproved)
                        {
                            rbVR.Enabled = false;
                            txtRemark.Enabled = false;
                            btnChange.Enabled = false;
                            btnChange.BackColor = System.Drawing.Color.Red;
                        }
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

        bool IsApplicationApproved()
        {
            using (var connection = new MySqlConnection(connectionStrings))
            {
                connection.Open();

                string query = "SELECT a.APPLICATION_NO" +
                    ", f.isAppr_Rej_Ret" +
                    ", a.WF_STATUS_CD_C" +
                    ", f.isDeviation" +
                    ", f.Remark" +
                    " FROM  applicantdetails a " +
                    " JOIN finalgcapproval f ON f.APPLICATION_NO=a.APPLICATION_NO" +
                    " WHERE a.APPLICATION_NO=@ApplicationNo" +
                    " AND f.isAppr_Rej_Ret='Y'" +
                    " AND a.WF_STATUS_CD_C='27';";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationNo", Session[SessionConst.ApplicationNo]);

                    var sde = new MySqlDataAdapter(command);
                    DataSet ds = new DataSet();
                    sde.Fill(ds);

                    if ((ds.Tables[0].Rows.Count > 0) && ds.Tables[0].Rows[0] is DataRow data)
                    {

                        txtRemark.Text = data["Remark"].ToString();
                        txtRemark.Enabled = false;

                        var item = rbDev.Items.FindByValue(data["isDeviation"].ToString());
                        item.Selected = true;
                        rbDev.Enabled = false;

                        btnVerified.BackColor = System.Drawing.Color.Red;
                        btnVerified.Enabled = false;

                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
            }
        }

        void FillGVDeviationHistory()
        {
            using (var connection = new MySqlConnection(connectionStrings))
            {
                connection.Open();

                string query = "SELECT DISTINCT a.application_no" +
                    ", CASE WHEN a.isappr_rej_ret = 'Y' THEN 'Verified' ELSE 'Return' END AS isAppr_Rej_Ret" +
                    ", a.remark" +
                    ", Concat(b.designation, Concat(' (', Concat(a.aprove_reject_return_by, ')'))) Aprove_Reject_Return_by" +
                    ", a.createdt" +
                    " FROM finalgcapprovaltxn a, empmaster b" +
                    " WHERE a.application_no =@ApplicationNo" +
                    " AND a.isappr_rej_ret IS NOT NULL " +
                    " AND b.sapid = a.aprove_reject_return_by" +
                    " ORDER BY a.srno DESC";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationNo", Session[SessionConst.ApplicationNo]);
                    DataSet dataSet = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(command);
                    da.Fill(dataSet);

                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        GVDeviationHistory.DataSource = dataSet.Tables[0];
                        GVDeviationHistory.DataBind();
                    }
                }
            }
        }

        #region Not Used As Per Current Flow

        protected void btnFinalSubmit_Click(object sender, EventArgs e)
        {
            string strAppID = Session["APPID"].ToString();
            string strMEDAID = Session["MEDAID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();

            //Save the File to the Directory (Folder).
            try
            {
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:

                        string strQuery = "update finalgcapproval set WF_STATUS_CD= 26 , isDeviation='" + rbDev.SelectedItem.Value + "' ,Deviation_Remark='" + txtRemark.Text + "', roleid=51, APP_STATUS='Proposal send for approval.', APP_STATUS_DT=now() where APPLICATION_NO='" + strAppID + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();

                        strQuery = "INSERT INTO finalgcapprovaltxn(APPLICATION_NO, MEDAProjectID, isAppr_Rej_Ret,Aprove_Reject_Return_by, roleid, createDT, createBy) " +
                                    "VALUES ('" + strAppID + "','" + strMEDAID + "','Y','" + Session["SAPID"].ToString() + "',51,now(),'" + Session["SAPID"].ToString() + "')";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();
                        lblMessage.Text = "Final submit successfull";

                        btnFinalSubmit.Enabled = false;
                        btnVerified.Enabled = false;
                        Response.Write("<script language='javascript'>window.alert('Submitted Successfully.');</script>");
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
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }

        }

        protected void btnReturned_Click(object sender, EventArgs e)
        {

            string strAppID = Session["APPID"].ToString();
            string strMEDAID = Session["MEDAID"].ToString();
            int roleID = int.Parse(Session["EmpRole"].ToString());
            string strIsDeviation = trDeviation.Visible == true ? rbDev.SelectedItem.Value : "N";
            int AllVerified = 0;
            foreach (GridViewRow row in grvDocuments.Rows)
            {
                string isFileVerified = ((RadioButtonList)row.FindControl("rbVR")).SelectedItem.Value;
                if (isFileVerified == "Y")
                {
                    AllVerified += 1;
                }
            }
            if (AllVerified != grvDocuments.Rows.Count)
            {
                foreach (GridViewRow row in grvDocuments.Rows)
                {
                    // Selects the text from the TextBox
                    // which is inside the GridView control
                    string hdnSRno = ((HiddenField)row.FindControl("HFSrno")).Value;
                    string hdnFileSRno = ((HiddenField)row.FindControl("HFFilesrno")).Value;
                    string isFileVerified = ((RadioButtonList)row.FindControl("rbVR")).SelectedItem.Value;
                    //int rowIndex = Convert.ToInt32(e.CommandArgument);   
                    MySqlConnection mySqlConnection = new MySqlConnection();

                    //Save the File to the Directory (Folder).
                    try
                    {
                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                string strQuery = "insert into finalgcapprovaldoctxn(APPLICATION_NO,MEDAProjectID,isAppr_Rej_Ret,remark,Aprove_Reject_Return_by,table_docs_srNo,table_docs_File_srNo,createby) values('" + strAppID + "','" + strMEDAID + "','" + isFileVerified + "','" + txtRemark.Text + "','" + Session["SAPID"].ToString() + "','" + hdnSRno + "','" + hdnFileSRno + "','" + Session["SAPID"].ToString() + "')";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();

                                strQuery = "update finalgridcondocs set isVerified='" + isFileVerified + "' where SrNo='" + hdnSRno + "' and FileSrNo='" + hdnFileSRno + "'";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();


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
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }
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

                                strQuery = "update finalgcapproval set isAppr_Rej_Ret='N', remark='" + txtRemark.Text + "', Aprove_Reject_Return_by= '" + Session["SAPID"].ToString() + "', roleid='51' where APPLICATION_NO='" + strAppID + "'";
                                //strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,roleid,createby) values('" + strAppID + "','" + strMEDAID + "','51'," + strAppID + ")";
                            }
                            else
                            {
                                strQuery = "update finalgcapproval set isAppr_Rej_Ret='N', remark='" + txtRemark.Text + "', Aprove_Reject_Return_by= '" + Session["SAPID"].ToString() + "', roleid='" + (roleID - 1) + "', isDeviation='" + strIsDeviation + "' where APPLICATION_NO='" + strAppID + "'";
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

                            btnVerified.Enabled = false;
                            btnVerified.BackColor = System.Drawing.Color.Red;
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('Submitted Succsefully.')</script>");

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
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('You have selected all verified.Couldnot submit.You have to click on verify button.')</script>");
            }
        }

        #endregion Not Used As Per Current Flow

        protected void grvDocuments_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}