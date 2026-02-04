using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;
using log4net;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;
using System.Net.Mail;

namespace GGC.UI.Emp
{
    public partial class DashboardNew : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(DashboardNew));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bindchart();

            }  
        }
        private void Bindchart()
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
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        //string strQuery=string.Empty;
                        strQuery = "select a.stages,IFNULL(sum(b.cnt),0) as cnt from appstatusmaster a left join (SELECT a.app_status, count(1) as cnt FROM applicantdetails a group by a.app_Status) b on a.app_status=b.app_status group by a.stages order by AppStatusID";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            GVApplications.DataSource = dsResult.Tables[0];
                            GVApplications.DataBind();
                            DataTable ChartData = dsResult.Tables[0];
                            Chart1.DataSource = ChartData;
                            Chart1.DataBind();
                            #region Chart1
                            ////storing total rows count to loop on each Record  
                            //string[] XPointMember = new string[ChartData.Rows.Count];
                            //int[] YPointMember = new int[ChartData.Rows.Count];

                            //for (int count = 0; count < ChartData.Rows.Count; count++)
                            //{
                            //    //storing Values for X axis  
                            //    XPointMember[count] = ChartData.Rows[count]["stages"].ToString();
                            //    //storing values for Y Axis  
                            //    YPointMember[count] = Convert.ToInt32(ChartData.Rows[count]["cnt"]);

                            //}
                            ////binding chart control  
                            //Chart1.Series[0].Points.DataBindXY(XPointMember, YPointMember);

                            ////Setting width of line  
                            //Chart1.Series[0].BorderWidth = 10;
                            ////setting Chart type   
                            //Chart1.Series[0].ChartType = SeriesChartType.Bar;


                            foreach (Series charts in Chart1.Series)
                            {
                                foreach (DataPoint point in charts.Points)
                                {
                                    switch (point.AxisLabel)
                                    {
                                        case "APPLICATION RECEIVED": point.Color = Color.RoyalBlue; break;
                                        case "PAYMENT DONE": point.Color = Color.SaddleBrown; break;
                                        case "PROPOSAL FEASIBILITY": point.Color = Color.SpringGreen; break;
                                        case "TECHNICAL FESIBILITY REPORT": point.Color = Color.Aqua; break;
                                        //case "COMMITTMENT FEES": point.Color = Color.BurlyWood; break;
                                        //case "GRID CONNECTIVITY LETTER ISSUED": point.Color = Color.DarkOrchid; break;
                                    }
                                    //point.Label = string.Format("{0:0} - {1}", point.YValues[0], point.AxisLabel);

                                }
                            }
                            //Enabled 3D  
                            //  Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;  
                            #endregion


                        }
                        else
                        {
                            Response.Write("<script language='javascript'>alert('Login UnSuccessfull.');</script>");
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
                // lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                //lblResult.Text = "Some problem during registration.Please try again.";
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
    }
}