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
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GGC.WebService;
using GGC.Scheduler;

namespace GGC.UI.FinalGC
{
    public partial class AppComm : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppDetail));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        //string strMEDAPrjID = string.Empty;
        //string strAPPID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["strMEDAPrjID"] = Request.QueryString["projectID"];
                Session["strAPPID"] = Request.QueryString["appId"];
                fillGrid();
                fillGridCertification();
            }
        }
        void fillGrid()
        {
            //string strAppID = "12345";

            string strQuery = "select * from fgc_comm_schedule where MEDAProjectID='" + Session["strMEDAPrjID"].ToString() + "'";
            try
            {
                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GvComm.DataSource = dsResult.Tables[0];
                    GvComm.DataBind();
                }
                else
                {
                    GvComm.DataSource = null;
                    GvComm.DataBind();
                }

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }
        }
        void fillGridCertification()
        {


            string strQuery = "select * from fgc_cert_list";
            try
            {
                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    gvCertification.DataSource = dsResult.Tables[0];
                    gvCertification.DataBind();

                    foreach (GridViewRow row in gvCertification.Rows)
                    {
                        row.Cells[1].Text += "<span style='color: red;'>*</span>";
                    }
                }

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }
        }

       


        protected void Insert(object sender, EventArgs e)
        {
            var errors = ValidateCommissionigScheduleCODUnitFields();
            if (!errors.Any())
            {
                //string strAPPID = "12345";
                //string strMEDAPrjID = "12345";

                string strQuery = "insert into fgc_comm_schedule(APPLICATION_NO, MEDAProjectID, UNIT_NO, UNIT_SIZE, DT_OF_WORK_COMMENCMENT, DT_WORK_COMPLETE, DT_SYNCH, DT_SCH_COD, createBy, createDT)" +
                        " values('" + Session["strAPPID"].ToString() + "','" + Session["strMEDAPrjID"].ToString() + "','" + txtUnitno.Text + "','" + txtUnitSize.Text + "','" + txtDTWorkComm.Text + "','" + txtDTWorkCompletion.Text + "','" + txtDTSynch.Text + "','" + txtDTSCHCOD.Text + "','" + Session["strAPPID"].ToString() + "',now())";
                try
                {

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    fillGrid();

                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    lblResult.Text = "Some problem during registration.Please try again.";
                }
                ResetCommissionigScheduleCODUnitFields();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{errors.First().Value}');", true);
            }

        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GvComm.EditIndex = e.NewEditIndex;
            this.fillGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = GvComm.Rows[e.RowIndex];
            int srno = Convert.ToInt32(GvComm.DataKeys[e.RowIndex].Values[0]);
            string UnitNo = (row.FindControl("txtUnitNo") as TextBox).Text;
            string UNIT_SIZE = (row.FindControl("txtUNIT_SIZE") as TextBox).Text;
            string DT_OF_WORK_COMMENCMENT = (row.FindControl("txtDT_OF_WORK_COMMENCMENT") as TextBox).Text;
            string DT_WORK_COMPLETE = (row.FindControl("txtDT_WORK_COMPLETE") as TextBox).Text;
            string DT_SYNCH = (row.FindControl("txtDT_SYNCH") as TextBox).Text;
            string DT_SCH_COD = (row.FindControl("txtDT_SCH_COD") as TextBox).Text;
            string strQuery = "UPDATE fgc_comm_schedule SET UNIT_NO='" + UnitNo + "',UNIT_SIZE='" + UNIT_SIZE + "',DT_OF_WORK_COMMENCMENT='" + DT_OF_WORK_COMMENCMENT + "',DT_WORK_COMPLETE='" + DT_WORK_COMPLETE + "',DT_SYNCH='" + DT_SYNCH + "',DT_SCH_COD='" + DT_SCH_COD + "' WHERE srno=" + srno + "";
            try
            {

                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                GvComm.EditIndex = -1;
                fillGrid();

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }
        }
        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            GvComm.EditIndex = -1;
            fillGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int srno = Convert.ToInt32(GvComm.DataKeys[e.RowIndex].Values[0]);
            string strQuery = "DELETE FROM fgc_comm_schedule  WHERE srno=" + srno;
            try
            {
                if (SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery) > 0)
                {
                    fillGrid();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }



        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != GvComm.EditIndex)
            {
                (e.Row.Cells[6].Controls[0] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
            }
        }
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            GvComm.PageIndex = e.NewPageIndex;
            fillGrid();
        }

        protected void btnComFinalSave_Click(object sender, EventArgs e)
        {
            if (ValidateConfirmationCertificationConfirmation())
            {
                string strMEDAPrjID = Session["strMEDAPrjID"].ToString();
                string strQuery = "update finalgcapproval set WF_STATUS_CD=2, app_status='APPLICATION COMMISSIONING SCHEDULE RECEIVED',app_status_dt=now() where MEDAProjectID='" + strMEDAPrjID + "'";
                try
                {

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    Session["strMEDAPrjID"] = strMEDAPrjID;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('APPLICATION COMMISSIONING SCHEDULE RECEIVED.');", true);

                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    lblResult.Text = "Some problem during registration.Please try again.";
                }
                ResetCommissionigScheduleCODUnitFields();
            }
        }

        protected void btnSave_Next_Click(object sender, EventArgs e)
        {
            if (ValidateConfirmationCertificationConfirmation())
            {

                //string strAppID = "12345";
                //string strMEDAPrjID = "12345";
                foreach (GridViewRow row in gvCertification.Rows)
                {
                    // Selects the text from the TextBox
                    // which is inside the GridView control
                    Label lblsrno = (Label)row.FindControl("lblsrno");
                    RadioButtonList rbVR = (RadioButtonList)row.FindControl("rbYesNo");

                    string isFileVerified = rbVR.SelectedIndex != -1 ? rbVR.SelectedItem.Value : null;

                    string strQuery = "insert into fgc_cert_data(application_no,MEDAProjectID,cert_type_srno,cert_value,create_dt,create_by) values('" + Session["strAPPID"].ToString() + "','" + Session["strMEDAPrjID"].ToString() + "'," + lblsrno.Text + ",'" + isFileVerified + "',now(),'" + Session["strAPPID"].ToString() + "')";
                    try
                    {

                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);



                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        // Use the exception object to handle all other non-MySql specific errors
                        lblResult.Text = "Some problem during registration.Please try again.";
                    }
                }
                try
                {
                    string strQuery = "update finalgcapproval set WF_STATUS_CD=3, app_status='APPLICATION CERTIFICATION & CONFIRMATION RECEIVED',app_status_dt=now() where MEDAProjectID='" + Session["strMEDAPrjID"].ToString() + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    lblResult.Text = "Some problem during registration.Please try again.";
                }
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('APPLICATION CERTIFICATION & CONFIRMATION RECEIVED.');window.location ='DocUpload.aspx';", true);
                Response.Redirect("~/UI/FinalGC/DocUpload.aspx?application=" + Session["strAPPID"].ToString(), false);
            }
        }

        private bool ValidateConfirmationCertificationConfirmation()
        {
            string validationMessage = null;

            if (!(GvComm.Rows.Count > 0))
            {
                validationMessage = "Please Add at least one COMMISSIONIG SCHEDULE & COD of units";
            }
            else if (!gvCertification.Rows.Cast<GridViewRow>().All(row => (row.FindControl("rbYesNo") as RadioButtonList)?.SelectedIndex != -1))
            {
                validationMessage = "Please select an Yes/No option for all CERTIFICATION & CONFIRMATION.";
            }

            if (!string.IsNullOrEmpty(validationMessage))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('{validationMessage}');", true);
                return false;
            }

            return true;
        }

        private Dictionary<string, string> ValidateCommissionigScheduleCODUnitFields()
        {

            var invalidFields = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(txtUnitno.Text))
            {
                invalidFields.Add("UnitNo", "Required Unit No");
            }

            if (string.IsNullOrEmpty(txtUnitSize.Text))
            {
                invalidFields.Add("UnitSize", "Required Unit Size (MW)");
            }

            DateTime dtWorkComm = default, dtWorkCompletion = default, dtSynch = default, dtSchCod = default;
            bool validDates = true;

            if (string.IsNullOrEmpty(txtDTWorkComm.Text))
            {
                invalidFields.Add("DateOfWorkCommencement", "Required Date of Work Commencement");
                validDates = false;
            }
            else if (!DateTime.TryParse(txtDTWorkComm.Text, out dtWorkComm))
            {
                invalidFields.Add("DateOfWorkCommencement", "Invalid Date of Work Commencement");
                validDates = false;
            }


            if (string.IsNullOrEmpty(txtDTWorkCompletion.Text))
            {
                invalidFields.Add("WorkCompletionDate", "Required Work Completion Date");
                validDates = false;
            }
            else if (!DateTime.TryParse(txtDTWorkCompletion.Text, out dtWorkCompletion))
            {
                invalidFields.Add("WorkCompletionDate", "Invalid Work Completion Date");
                validDates = false;
            }

            if (string.IsNullOrEmpty(txtDTSynch.Text))
            {
                invalidFields.Add("DateSynchronizationScheduled)", "Required Date of Synchronization (Scheduled)");
                validDates = false;
            }
            else if (!DateTime.TryParse(txtDTSynch.Text, out dtSynch))
            {
                invalidFields.Add("DateSynchronizationScheduled)", "Invalid Date of Synchronization (Scheduled)");
                validDates = false;
            }

            if (string.IsNullOrEmpty(txtDTSCHCOD.Text))
            {
                invalidFields.Add("ScheduledCommercialOperationDate", "Required Scheduled Commercial Operation Date(COD)");
                validDates = false;
            }
            else if (!DateTime.TryParse(txtDTSCHCOD.Text, out dtSchCod))
            {
                invalidFields.Add("ScheduledCommercialOperationDate", "Invalid Scheduled Commercial Operation Date(COD)");
                validDates = false;
            }

            if (validDates)
            {
                if (dtWorkComm >= dtWorkCompletion)
                {
                    invalidFields.Add("DateOfWorkCommencement", "The Date of Work Commencement must be before the Work Completion Date.");
                }
                if (dtWorkCompletion >= dtSynch)
                {
                    invalidFields.Add("WorkCompletionDate", "The Work Completion Date must be before the Date of Synchronization (Scheduled).");
                }
                if (dtSynch >= dtSchCod)
                {
                    invalidFields.Add("DateSynchronizationScheduled", "The Date of Synchronization (Scheduled) must be before the Scheduled Commercial Operation Date (COD).");
                }
            }

            return invalidFields;
        }

        private void ResetCommissionigScheduleCODUnitFields()
        {
            txtUnitno.Text = string.Empty;
            txtUnitSize.Text = string.Empty;
            txtDTWorkComm.Text = string.Empty;
            txtDTWorkCompletion.Text = string.Empty;
            txtDTSynch.Text = string.Empty;
            txtDTSCHCOD.Text = string.Empty;
        }
    }
}