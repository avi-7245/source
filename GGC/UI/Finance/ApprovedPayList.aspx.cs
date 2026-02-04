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

namespace GGC.UI.Finance
{
    public partial class ApprovedPayList : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ApprovedPayList));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT a.Application_No,a.PROMOTOR_NAME,b.TxnAmount,b.TxnDate,b.typeofpay,b.TxnNo , b.FinancePaymentAppr_DT as approvedDt,PaymentApproveByFin FROM applicantdetails a,  billdesk_txn b where a.APPLICATION_NO=b.ApplicationNo and b.AuthStatus='0300' and b.IsPayMentApproveFin='Y'");
                if (dsResult.Tables.Count > 0)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        Session["AppDet"] = dsResult;
                        GVPayments.DataSource = dsResult.Tables[0];
                        GVPayments.DataBind();
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
             
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            DataSet ds = (DataSet)Session["AppDet"];
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                string filename = "PayListExport.xls";
                System.IO.StringWriter tw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
                DataGrid dgGrid = new DataGrid();
                dgGrid.DataSource = dt;
                dgGrid.DataBind();

                //Get the HTML for the control.
                dgGrid.RenderControl(hw);
                //Write the HTML back to the browser.
                //Response.ContentType = application/vnd.ms-excel;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename + "");
                this.EnableViewState = false;
                Response.Write(tw.ToString());
                Response.End();
            }
        }
    }
}