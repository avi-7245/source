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
using GGC.Scheduler;

namespace GGC.UI.Emp
{
    public partial class Monthwisereport : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(Dashboard));

        string mySqlConnection = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            int roleId = int.Parse(Session["EmpRole"].ToString());
            if (roleId > 50 && roleId < 56)
            {
                mgtHome.Visible = true;
                empHome.Visible = false;
            }
            if (roleId > 1 && roleId <= 3)
            {
                empHome.Visible = true;
                mgtHome.Visible = false;
            }
            if (roleId > 10 && roleId <= 20)
            {
                empHome.Visible = true;
                mgtHome.Visible = false;
            }
            if (!IsPostBack)
            {
                
                
                fillDropdowns();
                Bindchart();
                BindPiechart();
            }  
        }

        private void Bindchart()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            int roleId = int.Parse(Session["EmpRole"].ToString());
            //int roleId = 2;

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        int lstDayMonth = 0;
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        //string strQuery=string.Empty;
                        DateTime mon = DateTime.Parse(ddlMonth.SelectedItem.Value.ToString());
                        if (ddlMonth.SelectedItem.Value != "-1")
                        {
                            

                            lstDayMonth = DateTime.DaysInMonth(int.Parse(ddlMonth.SelectedItem.Value.Substring(4, 4)), mon.Month);
                        }
                        string strFirstDt = string.Empty;
                        string strLastDt = string.Empty;

                        strFirstDt = "01" + "-" + mon.Month +"-"+ mon.Year;
                        strLastDt = lstDayMonth + "-" + mon.Month + "-" + mon.Year;

                        if (roleId > 10 && roleId < 20)
                        {
                            strQuery = "select a.stages,IFNULL(sum(b.cnt),0) as cnt from appstatusmaster a left join (SELECT a.app_status, count(1) as cnt FROM applicantdetails a where zone='" + Session["EmpZone"].ToString() + "'  group by a.app_Status) b on a.app_status=b.app_status group by a.stages order by AppStatusID";
                        }
                        else
                        {
                            strQuery = "SELECT a.Stages,count(1) as Total FROM appstatusmaster a, applicantdetails b, billdesk_txn c WHERE a.AppStatusID=b.WF_STATUS_CD_C and b.APPLICATION_NO=c.ApplicationNo and DATE_FORMAT(STR_TO_DATE(c.txndate,'%d-%m-%Y %H:%i:%s'), '%Y-%m-%d') between DATE_FORMAT(STR_TO_DATE('" + strFirstDt + "','%d-%m-%Y %H:%i:%s'), '%Y-%m-%d') and DATE_FORMAT(STR_TO_DATE('" + strLastDt + "','%d-%m-%Y %H:%i:%s'), '%Y-%m-%d') and c.AUTHstatus='0300' group by Stages";
                        }
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
                            Chart1.Legends[0].Enabled = true;
                            Chart1.Series[0].XValueMember = "Stages";
                            Chart1.Series[0].YValueMembers = "Total";
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
                                        case "PROCESSING FEES PAYMENT CONFIRMATION PENDING": point.Color = Color.Coral; break;
                                        case "VERIFICATION PENDING AT STU": point.Color = Color.SaddleBrown; break;
                                        case "TECHNICAL FESIBILITY REPORT PENDING": point.Color = Color.SpringGreen; break;
                                        case "UNDER LOAD FLOW STUDY": point.Color = Color.Aqua; break;
                                        case "GC PROPOSAL UNDER STU / APPROVAL": point.Color = Color.BurlyWood; break;
                                        case "PROPOSAL APPROVED.": point.Color = Color.DarkOrchid; break;
                                        case "COMMITTMENT FEES PENDING": point.Color = Color.BlueViolet; break;
                                        case "COMMITTMENT FEES PAYMENT CONFIRMATION PENDING": point.Color = Color.Bisque; break;
                                        case "GRID CONNECTIVITY LETTER ISSUED": point.Color = Color.Chocolate; break;
                                        case "APPLICATION CANCELLED.": point.Color = Color.Chocolate; break;

                                    }
                                    point.Label = string.Format("{0}", point.YValues[0]);

                                }
                            }
                            Chart1.Legends[0].Enabled = true;
                            Chart1.Series[0].Label = "#PERCENT";
                            Chart1.Series[0].LegendText = "#AXISLABEL";
                            Legend leg = new Legend();
                            Chart1.Legends.Add(leg);
                            
                            ////Enabled 3D  
                            //Chart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                            //LegendItem item1 = new LegendItem();
                            //item1.ImageStyle = LegendImageStyle.Rectangle;
                            //item1.Color = Color.Red;
                            //item1.BorderColor = Color.Red;
                            //item1.Cells.Add(LegendCellType.SeriesSymbol, "", ContentAlignment.BottomLeft);
                            //item1.Cells.Add(LegendCellType.Text, "Low Bed Number", ContentAlignment.MiddleLeft);
                            //Chart1.Legends[0].CustomItems.Add(item1);


                            #endregion


                        }
                        else
                        {
                            Response.Write("<script language='javascript'>alert('Error while loading data.');</script>");
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
        private void BindPiechart()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            int roleId = int.Parse(Session["EmpRole"].ToString());
            //int roleId = 2;
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd ;
                        //string strQuery=string.Empty;
                        int lstDayMonth = 0;
                        DateTime mon ;
                        mon = DateTime.Parse(ddlMonth.SelectedItem.Value.ToString());
                        if (ddlMonth.SelectedItem.Value != "-1")
                        {
                           

                            lstDayMonth = DateTime.DaysInMonth(int.Parse(ddlMonth.SelectedItem.Value.Substring(4, 4)), mon.Month);
                        }
                        //else
                        //{
                        //    mon = DateTime.Parse(DateTime.Today.ToString("mmm-yyyyy"));

                        //    lstDayMonth = DateTime.DaysInMonth(int.Parse(ddlMonth.SelectedItem.Value.Substring(4, 4)), mon.Month);
                        //}
                        string strFirstDt = string.Empty;
                        string strLastDt = string.Empty;

                        strFirstDt = "01" + "-" + mon.Month +"-"+ mon.Year;
                        strLastDt = lstDayMonth + "-" + mon.Month + "-" + mon.Year;

                        //strQuery = "select a.stages,IFNULL(sum(b.cnt),0) as cnt from appstatusmaster a left join (SELECT a.app_status, count(1) as cnt FROM applicantdetails a where a.app_status!='OLD GC' group by a.app_Status) b on a.app_status=b.app_status group by a.stages order by AppStatusID";
                        if (roleId > 10 && roleId < 20)
                        {
                            strQuery = "select a.stages,IFNULL(sum(b.cnt),0) as cnt from appstatusmaster a left join (SELECT a.app_status, count(1) as cnt FROM applicantdetails a where zone='" + Session["EmpZone"].ToString() + "'  group by a.app_Status) b on a.app_status=b.app_status group by a.stages order by AppStatusID";
                        }
                        else
                        {
                            strQuery = "SELECT a.Stages,count(1) as Total FROM appstatusmaster a, applicantdetails b, billdesk_txn c WHERE a.AppStatusID=b.WF_STATUS_CD_C and b.APPLICATION_NO=c.ApplicationNo and DATE_FORMAT(STR_TO_DATE(c.txndate,'%d-%m-%Y %H:%i:%s'), '%Y-%m-%d') between DATE_FORMAT(STR_TO_DATE('" + strFirstDt + "','%d-%m-%Y %H:%i:%s'), '%Y-%m-%d') and DATE_FORMAT(STR_TO_DATE('" + strLastDt + "','%d-%m-%Y %H:%i:%s'), '%Y-%m-%d') and c.AUTHstatus='0300' group by Stages";
                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {

                            DataTable ChartData = dsResult.Tables[0];
                            //Chart2.DataSource = ChartData;
                            //Chart2.DataBind();
                            #region Chart2
                            ////storing total rows count to loop on each Record  
                            string[] XPointMember = new string[ChartData.Rows.Count];
                            int[] YPointMember = new int[ChartData.Rows.Count];

                            for (int count = 0; count < ChartData.Rows.Count; count++)
                            {
                                //storing Values for X axis  
                                XPointMember[count] = ChartData.Rows[count]["stages"].ToString();
                                //storing values for Y Axis  
                                YPointMember[count] = Convert.ToInt32(ChartData.Rows[count]["Total"]);

                            }
                            //binding chart control  
                            Chart2.Series[0].Points.DataBindXY(XPointMember, YPointMember);

                            ////Setting width of line  
                            Chart2.Series[0].BorderWidth = 10;
                            ////setting Chart type   
                            //Chart1.Series[0].ChartType = SeriesChartType.Bar;


                            foreach (Series charts in Chart2.Series)
                            {
                                foreach (DataPoint point in charts.Points)
                                {
                                    switch (point.AxisLabel)
                                    {
                                        case "PROCESSING FEES PAYMENT CONFIRMATION PENDING": point.Color = Color.Coral; break;
                                        case "VERIFICATION PENDING AT STU": point.Color = Color.SaddleBrown; break;
                                        case "TECHNICAL FESIBILITY REPORT PENDING": point.Color = Color.SpringGreen; break;
                                        case "UNDER LOAD FLOW STUDY": point.Color = Color.Aqua; break;
                                        case "GC PROPOSAL UNDER STU / APPROVAL": point.Color = Color.BurlyWood; break;
                                        case "PROPOSAL APPROVED.": point.Color = Color.DarkOrchid; break;
                                        case "COMMITTMENT FEES PENDING": point.Color = Color.BlueViolet; break;
                                        case "COMMITTMENT FEES PAYMENT CONFIRMATION PENDING": point.Color = Color.Bisque; break;
                                        case "GRID CONNECTIVITY LETTER ISSUED": point.Color = Color.Chocolate; break;
                                        case "APPLICATION CANCELLED.": point.Color = Color.Chocolate; break;
                                    }
                                    point.Label = string.Format("{0}", point.YValues[0]);

                                }
                            }
                            //Enabled 3D  

                            Chart2.Series[0].ChartType = SeriesChartType.Pie;
                            Chart2.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
                            Chart2.Legends[0].Enabled = true;
                            Chart2.Series[0].Label = "#PERCENT";
                            Chart2.Series[0].LegendText = "#AXISLABEL";
                            Legend leg = new Legend();
                            Chart2.Legends.Add(leg);
                            //Chart2.Series["Series1"]["PieLabelStyle"] = "Outside";
                            Chart2.ChartAreas[0].Area3DStyle.Inclination = 10;
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

        void fillDropdowns()
        {
            //DataSet dsZone = SQLHelper.ExecuteDataset(mySqlConnection, CommandType.Text, "SELECT distinct zone FROM zone_district");
            ////DataSet dsStages = SQLHelper.ExecuteDataset(mySqlConnection, CommandType.Text, "SELECT distinct stages FROM appstatusmaster where stages not in('OLD GC')");
            //ddlMonth.DataSource = dsZone.Tables[0];
            //ddlMonth.DataValueField = "zone";
            //ddlMonth.DataTextField = "zone";
            //ddlMonth.DataBind();
            ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            int j = 1;
            for (int i = 6; i > 0; i--)
            {
                DateTime dt;
                //if (j == 0)
                //{
                //    dt = DateTime.Today;
                //    ddlMonth.Items.Insert(j + 1, new System.Web.UI.WebControls.ListItem(dt.ToString("MMM-yyyy"), dt.ToString("MMM-yyyy")));
                //}
                //else
                //{
                    dt = DateTime.Today.AddMonths(-(j-1));
                    ddlMonth.Items.Insert(j , new System.Web.UI.WebControls.ListItem(dt.ToString("MMM-yyyy"), dt.ToString("MMM-yyyy")));
                //}
                //var month = new DateTime(today.Year, today.Month, 1);
                //var first = month.AddMonths(-1);
                //var last = month.AddDays(-1);
                j++;
            }
                ddlMonth.ClearSelection();
            ddlMonth.SelectedIndex = 1;

            //ddlStages.DataSource = dsStages.Tables[0];
            //ddlStages.DataValueField = "stages";
            //ddlStages.DataTextField = "stages";
            //ddlStages.DataBind();
            //ddlStages.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            //ddlStages.ClearSelection();
            //ddlStages.SelectedIndex = 0;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Bindchart();
            BindPiechart();
        }
    }
}