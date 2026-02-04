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

namespace GGC.UI.Emp
{
    public partial class PaymentApproved : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(PaymentApproved));
        protected string isPaymentAppSTU = string.Empty;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();

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

                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "SELECT a.*,b.*,c.* FROM applicantdetails a, billdesk_txn b, applicant_reg_det c WHERE a.APPLICATION_NO=b.APPLICATIONNO and a.USER_NAME=c.USER_NAME and b.IsPayMentApproveFin='Y' and b.TxnNo is not null and WF_STATUS_CD_C in (17,18) and typeofpay='Committment'";


                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            GVPayments.DataSource = dsResult.Tables[0];
                            GVPayments.DataBind();
                            
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

        protected void GVPayments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVPayments.Rows[rowIndex];

            string applicationId = row.Cells[0].Text;
            string custId = row.Cells[0].Text;
            Session["PROJID"] = row.Cells[1].Text;
            if (e.CommandName == "Approve")
            {
                updateApproval(applicationId, custId);
            }
            if (e.CommandName == "UploadLetter")
            {
                Response.Redirect("~/UI/Emp/UploadIssueLetter.aspx?application=" + applicationId, false);
            }
        }

        protected void updateApproval(string applicationId, string custId)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;

                        //strQuery = "SELECT a.*,b.* FROM applicantdetails a, proposalapproval b  WHERE a.APPLICATION_NO=b.APPLICATION_NO and (b.isAppr_Rej_Ret is null or b.isAppr_Rej_Ret='R') and b.roleid='" + Session["EmpRole"].ToString() + "'";
                        strQuery = "update billdesk_txn set IsPayMentApproveSTU='Y' ,PaymentApproveBySTU='" + Session["SAPID"].ToString() + "' where ApplicationNo='" + applicationId + "' and CustomerID='" + custId + "'";


                       
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        
                        cmd.ExecuteNonQuery();
                        strQuery = "update APPLICANTDETAILS set WF_STATUS_CD_C=18,IsPaymentDone='Y', paymentdate=CURDATE(), APP_STATUS_DT=CURDATE() ,app_status='COMMITTMENT FEES APPROVED BY STU.' ,  CustomerID='" + custId + "' where Application_No='" + applicationId + "'";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                        
                        objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "18");

                        Response.Write("<script language='javascript'>alert('Payment Verified.');</script>");
                        fillGrid();

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

        protected void GVPayments_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        Button btnAppr = (Button)e.Row.Cells[9].FindControl("btnAppr");
                        Button btnRet = (Button)e.Row.Cells[10].FindControl("btnRet");
                        
                        // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                        string app = e.Row.Cells[0].Text;
                        int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());
                        string IsIssueLetter = ds.Tables[0].Rows[rowno]["IssueLetter"].ToString();
                        log.Error(wfStatus.ToString());

                        string hex = "#008CBA";
                        System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

                        btnAppr.BackColor = System.Drawing.Color.Red;

                        btnRet.BackColor = System.Drawing.Color.Red;

                        

                        if (wfStatus == 17)
                        {

                            btnAppr.Enabled = true;
                            btnAppr.BackColor = _color;

                            
                            btnRet.Enabled = false;
                            btnRet.BackColor = System.Drawing.Color.Red;
                            
                        }
                        if (wfStatus == 18)
                        {
                            btnAppr.Enabled = false;
                            btnAppr.BackColor = System.Drawing.Color.Red;


                            btnRet.Enabled = true;
                            btnRet.BackColor = _color;

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
        
    }
}