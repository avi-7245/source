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
    public partial class LoadFlowStudy : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(LoadFlowStudy));
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
            int rollid=int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";
            try
            {

                mySqlConnection.Open();
                

                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days,concat(PROJECT_LOC,'', PROJECT_TALUKA,' ', PROJECT_DISTRICT) as Location  from APPLICANTDETAILS where  IsPaymentDone='Y' and WF_STATUS_CD_C>=10 order by CREATED_DT desc";
                        
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //Session["AppDet"] = dsResult;
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
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                if (e.CommandName == "UploadLFS")
                {
                    
                    Response.Redirect("~/UI/Emp/UploadLFS.aspx?application=" + applicationId, false);
                }

                if (e.CommandName == "Feasibility")
                {
                    
                    Response.Redirect("~/UI/Emp/Feasibility.aspx?application=" + applicationId, false);
                }
                if (e.CommandName == "FeasibilityConsent")
                {

                    Response.Redirect("~/UI/Emp/FeasibilityConsent.aspx?application=" + applicationId, false);
                }
                if (e.CommandName == "SendProposal")
                {

                    Response.Redirect("~/UI/Emp/FeasibilityConsent.aspx?application=" + applicationId, false);
                }
                if (e.CommandName == "ViewAllDoc")
                {

                    Response.Redirect("~/UI/Emp/ViewAllDocs.aspx?application=" + applicationId, false);
                }
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
                            
                            
                            Button btnView = (Button)e.Row.Cells[11].FindControl("btnView");

                            Button btnFeasibility = (Button)e.Row.Cells[12].FindControl("btnFeasibility");
                            Button btnFeasibilityConsent = (Button)e.Row.Cells[13].FindControl("btnFeasibilityConsent");

                            // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                            string app = e.Row.Cells[0].Text;
                            int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());
                            string IsIssueLetter = ds.Tables[0].Rows[rowno]["IssueLetter"].ToString();
                            log.Error(wfStatus.ToString());

                            string hex = "#008CBA";
                            System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

                            btnView.BackColor = System.Drawing.Color.Red;

                            btnFeasibility.BackColor = System.Drawing.Color.Red;

                            btnFeasibilityConsent.BackColor = System.Drawing.Color.Red;

                            if (wfStatus == 10)
                            {

                                
                                
                                btnView.Enabled = true;
                                btnView.BackColor = _color;
                                btnFeasibility.Enabled = false;
                                btnFeasibility.BackColor = System.Drawing.Color.Red;
                                btnFeasibilityConsent.Enabled = false;
                                btnFeasibilityConsent.BackColor = System.Drawing.Color.Red;
                            }
                            if (wfStatus == 11)
                            {
                                
                                btnView.Enabled = false;
                                btnView.BackColor = System.Drawing.Color.Red;
                                btnFeasibility.Enabled = true;
                                btnFeasibility.BackColor = _color;
                                btnFeasibilityConsent.Enabled = true ;
                                btnFeasibilityConsent.BackColor = _color;

                            }
                            if (wfStatus == 12)
                            {

                                btnView.Enabled = false;
                                btnView.BackColor = System.Drawing.Color.Red;
                                btnFeasibility.Enabled = false;
                                btnView.BackColor = System.Drawing.Color.Red;
                                btnFeasibilityConsent.Enabled = true;
                                btnFeasibilityConsent.BackColor = _color;

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