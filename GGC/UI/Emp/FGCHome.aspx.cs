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
using Microsoft.Reporting.WebForms;
using GGC.Scheduler;

namespace GGC.UI.Emp
{
    public partial class FGCHome : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FGCHome));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int role = int.Parse(Session["EmpRole"].ToString());

            if (!Page.IsPostBack)
            {

                fillGrid();

            }

        }

        protected void fillGrid()
        {
            string strGSTIN = string.Empty;
            int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";

            try
            {

                string strQuery = string.Empty;
                MySqlCommand cmd;
                if (rollid <= 10)
                {
                    strQuery = "SELECT a.APPLICATION_NO, a.medaprojectid, b.project_loc, b.NAME_OF_SPD, b.SPV_Name, b.PROJECT_CAPACITY_MW, a.app_status, a.WF_STATUS_CD_C, DATE_FORMAT(a.APP_STATUS_DT, '%Y-%m-%d') AS APP_STATUS_DT FROM mskvy_applicantdetails a JOIN mskvy_applicantdetails_spd b ON a.APPLICATION_NO = b.APPLICATION_NO ORDER BY a.APP_STATUS_DT DESC ";
                }
                if (rollid >= 51 && rollid <= 55)
                {
                    strQuery = "select a.* ,date_format(a.APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,c.PROJECT_LOC, c.NAME_OF_SPD,c.SPV_Name,c.PROJECT_CAPACITY_MW  from finalgcapproval a,proposalapproval_fgc b,mskvy_applicantdetails_spd c where a.WF_STATUS_CD=13 and b.roleId=" + rollid + " and a.APPLICATION_NO=b.APPLICATION_NO and b.APPLICATION_NO=c.APPLICATION_NO and a.APPLICATION_NO=c.APPLICATION_NO order by a.CREATEDT desc";
                }
                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
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
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
            }


        }

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDoc")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/ViewDocFGC.aspx?application=" + applicationId, false);

            }
            if (e.CommandName == "isDev")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FGCDeviation.aspx?application=" + applicationId, false);

            }
            if (e.CommandName == "UploadDraft")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FGCUpDraft.aspx?application=" + applicationId, false);

            }
            if (e.CommandName == "AppFGC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FGCPRAccept.aspx?application=" + applicationId, false);

            }
            //if (e.CommandName == "AppFGC")
            //{
            //    int rowIndex = Convert.ToInt32(e.CommandArgument);

            //    //Reference the GridView Row.
            //    GridViewRow row = GVApplications.Rows[rowIndex];

            //    //Fetch value of Name.
            //    //string name = (row.FindControl("txtName") as TextBox).Text;

            //    //Fetch value of Country
            //    string applicationId = row.Cells[0].Text;
            //    Session["APPID"] = row.Cells[0].Text; ;
            //    Session["PROJID"] = row.Cells[1].Text;
            //    Response.Redirect("~/UI/Emp/FGCDeviation.aspx?application=" + applicationId, false);

            //}
            if (e.CommandName == "UploadGC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FGCUpFin.aspx?application=" + applicationId, false);

            }
        }

        protected void GVApplications_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int roll_Id;
            roll_Id = int.Parse(Session["EmpRole"].ToString());


            if (roll_Id <= 10)
            {
                

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "View FGC")
                .SingleOrDefault()).Visible = true;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Any Deviation required")
                .SingleOrDefault()).Visible = true;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Approve/Return Proposal")
                .SingleOrDefault()).Visible = false;

            }
            if (roll_Id > 50)
            {


                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "View FGC")
                .SingleOrDefault()).Visible = false;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Any Deviation required")
                .SingleOrDefault()).Visible = false;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Approve/Return Proposal")
                .SingleOrDefault()).Visible = true;

            }
        }
    }
}