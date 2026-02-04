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
using GGC.Scheduler;
namespace GGC.UI.Emp
{
    public partial class AppCancelList : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppCancelList));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            int role = int.Parse(Session["EmpRole"].ToString());

            if (role > 51 && role <= 60)
            {
                if (role == 53)
                {
                    //comFees.Visible = true;
                    //comFeesApr.Visible = true;
                }
                else
                {
                    //comFees.Visible = false;
                    //comFeesApr.Visible = false;
                }

            }
            if (!Page.IsPostBack)
            {

                fillGrid();

            }
        }
        protected void fillGrid()
        {
            int rollid = int.Parse(Session["EmpRole"].ToString());
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select a.APPLICATION_NO, a.MEDAProjectID ,a.PROMOTOR_NAME,a.PROJECT_LOC,a.NATURE_OF_APP,a.PROJECT_TYPE,a.PROJECT_CAPACITY_MW,a.app_status,a.APP_STATUS_DT,concat(a.PROJECT_LOC,' ', a.PROJECT_TALUKA,' ', a.PROJECT_DISTRICT) as Location from applicantdetails a where a.WF_STATUS_CD_C=21");
            if (dsResult.Tables[0].Rows.Count > 0)
            {
                GVApplications.DataSource = dsResult.Tables[0];
                GVApplications.DataBind();
            }
        }

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVApplications.Rows[rowIndex];

            string applicationId = row.Cells[0].Text;
            Session["APPID"] = row.Cells[0].Text;
            Session["PROJID"] = row.Cells[1].Text;

            if (e.CommandName == "CancelApp")
            {

                Response.Redirect("~/UI/Emp/AppCancel.aspx?application=" + applicationId, false);
            }

            
        }
    }
}
