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
    public partial class MSSPDList : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSSPDList));
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

                strQuery = "select a.* from mskvy_applicantdetails_spd a where a.WF_STATUS_CD_C>=5 order by a.CREATED_DT desc";
                

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
                Response.Redirect("~/UI/Emp/ViewDocSPD.aspx?application=" + applicationId, false);

            }
        }

    }
}