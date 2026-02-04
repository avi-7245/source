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
    public partial class GCIssued : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(GCIssued));
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
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where WF_STATUS_CD_C=19");
                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource=dsResult.Tables[0];
                    GVApplications.DataBind();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
             
            }
        }

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadGC")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                HiddenField fileName = (HiddenField) row.Cells[9].FindControl("HiddenField1");
                Response.Clear();
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("Content-Disposition", "filename=" + fileName.Value);
                Response.TransmitFile(Server.MapPath("~/Files/IssueLetter/") + fileName.Value);
                Response.End(); 
            }
        }
    }
}