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
using System.Globalization;

namespace GGC.UI.Finance
{
    public partial class UpdatePayment : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UpdatePayment));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCustID_Click(object sender, EventArgs e)
        {
            string Application_no=string.Empty;
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from billdesk_txn where applicationno ='"+txtAppNo.Text+"'");

                ddlCustID.DataSource = dsResult.Tables[0];
                ddlCustID.DataValueField = "CustomerID";
                ddlCustID.DataTextField = "CustomerID";
                ddlCustID.DataBind();
                ddlCustID.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'N')");
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string strQuery = string.Empty;
                strQuery = "update BillDesk_TXN set TxnNo= '"+txtTxnNo.Text.Trim()+"',TxnDate='"+txtDt.Text+"',AuthStatus='0300',ORIGINALSTATUS='2' ,UpdatedDt=now() where ApplicationNo='"+txtAppNo.Text+"' and CustomerID='"+ddlCustID.SelectedItem.Value+"'";
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                if (ddlPayType.SelectedItem.Value == "Registration")
                {
                    DateTime dttxtDate = DateTime.Parse(txtDt.Text);


                    strQuery = "update applicantdetails set WF_STATUS_CD_C=5,IsPaymentDone='Y', paymentdate='" + dttxtDate.ToString("yyyy-MM-dd hh:mm:ss") + "', APP_STATUS_DT=CURDATE() ,app_status='REGISTRATION PAYMENT DONE.PAYMENT APPROVAL PENDING.' ,  CustomerID='" + ddlCustID.SelectedItem.Value + "' where Application_No='" + txtAppNo.Text + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    strQuery = "insert into APPLICANT_STATUS_TRACKING(APPLICATION_NO,STATUS,STATUS_DT,Created_by) values('" + txtAppNo.Text + "','REGISTRATION PAYMENT DONE.PAYMENT APPROVAL PENDING.',CURDATE(),'" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                }
                else
                {
                    DateTime dttxtDate = DateTime.Parse(txtDt.Text);

                    strQuery = "update APPLICANTDETAILS set WF_STATUS_CD_C=16,IsPaymentDone='Y', paymentdate='" + dttxtDate.ToString("yyyy-MM-dd hh:mm:ss") + "', APP_STATUS_DT=CURDATE() ,app_status='COMMITTMENT FEES PAID.PAYMENT APPROVAL PENDING.' ,  CustomerID='" + ddlCustID.SelectedItem.Value + "' where Application_No='" + txtAppNo.Text + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    strQuery = "insert into APPLICANT_STATUS_TRACKING(APPLICATION_NO,STATUS,STATUS_DT,Created_by) values('" + txtAppNo.Text + "','COMMITTMENT FEES PAID.PAYMENT APPROVAL PENDING.',CURDATE(),'" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                }
                txtAppNo.Text = "";
                txtTxnNo.Text = "";
                txtAmt.Text = "";
                txtDt.Text = "";
                ddlCustID.Items.Clear();
                lblResult.Text = "Payment Updated successfully!";
                lblResult.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                lblResult.Text = "Payment not Updated!";
                //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "insert into SOP_Email_Sched_Tracking(Application_no,ReminderDay,isDelivered) values('" + Application_no + "',5,'N')");
            }
        }
    }
}