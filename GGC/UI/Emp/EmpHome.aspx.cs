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
    public partial class EmpHome : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(EmpHome));
        
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text= Session["EmpName"]+"("+Session["EmpDesignation"]+")";
            int role = int.Parse(Session["EmpRole"].ToString());
            if (role > 10 && role <= 20)
            {
                pending.Visible = false;
                lfs.Visible = false;
                //lfsother.Visible = false;
                viewDoc.Visible = true;
                Usrmgt.Visible = false;
                MSKVYHome.Visible = false;
                gcIssued.Visible = false;
                gcCancelled.Visible = false;
                MSSPD.Visible = true;
            }
            if (role > 50 && role <= 60)
            {
                pending.Visible = false;
                lfs.Visible = false;
                //lfsother.Visible = false;
                viewDoc.Visible = false;
                Usrmgt.Visible = false;
                MSKVYHome.Visible = false;
                MSSPD.Visible = false;
                
            }

            if (!Page.IsPostBack)
            {
                
                fillGrid();
         
            }
        }
        void signout()
        {
            Session.Clear();
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
                        if (rollid<=10)
                        {
                            strQuery = "select * ,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days ,b.ORGANIZATION_NAME  from APPLICANTDETAILS a,applicant_reg_det b where a.USER_NAME=b.USER_NAME and IsPaymentDone='Y' and WF_STATUS_CD_C between 6 and 18 order by a.CREATED_DT desc";
                        }
                        else
                        {
                            //strQuery = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F,datediff(curdate(),paymentdate) as days  from APPLICANTDETAILS where ZONE='" + Session["EmpZone"].ToString() + "' AND IsPaymentDone='Y'";
                            
                            strQuery = "select * ,b.ORGANIZATION_NAME  from APPLICANTDETAILS a,applicant_reg_det b where a.USER_NAME=b.USER_NAME and IsPaymentDone='Y' and ZONE='" + Session["EmpZone"].ToString() + "' and WF_STATUS_CD_C=8  order by a.CREATED_DT desc";
                            


                        }
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        //log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
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
                        Button btnViewAppZone = (Button)e.Row.Cells[11].FindControl("btnViewAppZone");
                        Button btnViewApp = (Button)e.Row.Cells[11].FindControl("btnViewApp");
                        Button btnView = (Button)e.Row.Cells[12].FindControl("btnView");

                        Button btnUpload = (Button)e.Row.Cells[13].FindControl("btnUpload");
                        Button btnViewTech = (Button)e.Row.Cells[14].FindControl("btnViewTech");

                        // int wfStatus = int.Parse(e.Row.Cells[14].Text);

                        string app = e.Row.Cells[0].Text;
                        int wfStatus = int.Parse(ds.Tables[0].Rows[rowno]["WF_STATUS_CD_C"].ToString());
                        string IsIssueLetter = ds.Tables[0].Rows[rowno]["IssueLetter"].ToString();
                        string appno = ds.Tables[0].Rows[rowno]["APPLICATION_NO"].ToString();
                        
                        log.Error("AppNo "+appno+" status "+ wfStatus.ToString());

                        string hex = "#008CBA";
                        System.Drawing.Color _color = System.Drawing.ColorTranslator.FromHtml(hex);

                        btnViewAppZone.BackColor = System.Drawing.Color.Red;

                        btnViewApp.BackColor = System.Drawing.Color.Red;

                        btnView.BackColor = System.Drawing.Color.Red;

                        btnUpload.BackColor = System.Drawing.Color.Red;

                        btnViewTech.BackColor = System.Drawing.Color.Red;

                        if (wfStatus == 6)
                        {

                            btnViewAppZone.Enabled = true;
                            btnView.BackColor = _color;

                            btnViewApp.Enabled = true;
                            btnViewApp.BackColor = _color;

                            btnView.Enabled = false;
                            btnView.BackColor = System.Drawing.Color.Red;
                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;
                            btnViewTech.Enabled = false;
                            btnViewTech.BackColor = System.Drawing.Color.Red;
                        }
                        if (wfStatus == 7)
                        {
                            btnViewAppZone.Enabled = true;
                            btnViewAppZone.BackColor = _color;
                            btnViewApp.Enabled = false;

                            btnViewApp.BackColor = System.Drawing.Color.Red;
                            btnView.Enabled = true;
                            btnView.BackColor = _color;
                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;
                            btnViewTech.Enabled = false;
                            btnViewTech.BackColor = System.Drawing.Color.Red;

                        }

                        if (wfStatus == 8)
                        {
                            btnViewAppZone.Enabled = true;
                            btnViewAppZone.BackColor = _color;
                            btnViewApp.Enabled = false;
                            btnViewApp.BackColor = System.Drawing.Color.Red;

                            btnView.Enabled = false;
                            btnView.BackColor = System.Drawing.Color.Red;

                            btnUpload.Enabled = true;
                            btnUpload.BackColor = _color;
                            btnViewTech.Enabled = false;
                            btnViewTech.BackColor = System.Drawing.Color.Red;


                        }

                        if (wfStatus == 9)
                        {
                            btnViewAppZone.Enabled = true;
                            btnViewAppZone.BackColor = _color;
                            btnViewApp.Enabled = false;
                            btnViewApp.BackColor = System.Drawing.Color.Red;

                            btnView.Enabled = false;
                            btnView.BackColor = System.Drawing.Color.Red;


                            btnUpload.Enabled = false;
                            btnUpload.BackColor = System.Drawing.Color.Red;

                            btnViewTech.Enabled = true;
                            btnViewTech.BackColor = _color;

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
        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewApp")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                Session["PROJID"] = row.Cells[1].Text.Replace("&nbsp;", "");
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                string letterName = string.Empty;
                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd = new MySqlCommand("select *  from applicantdetails where APPLICATION_NO='" + applicationId + "'", mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {

                                letterName = dsResult.Tables[0].Rows[0]["AppFormDocName"].ToString();

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

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors

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
                //if (letterName != "")
                //{
                    Response.Redirect("~/UI/Emp/viewapp.aspx?appid=" + applicationId + "&DocName=" + letterName, false);
                //}
                //else
                //{
                //    Response.Write("<script language='javascript'>alert('Application form not yet uploaded.');</script>");
                //}

            }
            if (e.CommandName == "ViewAppZone")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                Session["PROJID"] = row.Cells[1].Text;
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                string letterName = string.Empty;
                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd = new MySqlCommand("select *  from applicantdetails where APPLICATION_NO='" + applicationId + "'", mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {

                                letterName = dsResult.Tables[0].Rows[0]["AppFormDocName"].ToString();

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

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors

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
                //if (letterName != "")
                //{
                    Response.Redirect("~/UI/Emp/viewapp.aspx?appid=" + applicationId + "&DocName=" + letterName, false);
                //}
                //else
                //{
                //    Response.Write("<script language='javascript'>alert('Application form not yet uploaded.');</script>");
                //}

            }
            if (e.CommandName == "View")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                Session["PROJID"] = row.Cells[1].Text;
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                string strisAppApproved = string.Empty;
                string letterName =string.Empty;
                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd = new MySqlCommand("select *  from applicantdetails where APPLICATION_NO='" + applicationId + "'", mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            if (dsResult.Tables[0].Rows.Count >0)
                            {


                                strisAppApproved = dsResult.Tables[0].Rows[0]["isAppApproved"].ToString();
                                letterName = dsResult.Tables[0].Rows[0]["SLDDocName"].ToString();
                 
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
                 
                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                 
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
                //if (letterName != "")
                //{
                //    if (strisAppApproved == "Y")
                //    {
                        Response.Redirect("~/UI/Emp/ShowLetter.aspx?appid=" + applicationId + "&DocName=" + letterName, false);
                //    }
                //    else
                //    {
                //        Response.Write("<script language='javascript'>alert('Application is not yet verified.');</script>");
                //    }
                //}
                //else
                //{
                //    Response.Write("<script language='javascript'>alert('SLD Not uploaded.');</script>");
                //}


            }
            if (e.CommandName == "Upload")
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
                //if (Session["EmpRole"].ToString() == "11")
                //{
                    Response.Redirect("~/UI/Emp/EmpDocUpload.aspx", false);
                //}
                //else
                //{
                //}

            }
            if (e.CommandName == "CancelApp")
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
                //if (Session["EmpRole"].ToString() == "11")
                //{
                Response.Redirect("~/UI/Emp/AppCancel.aspx?application=" + applicationId, false);
                //}
                //else
                //{
                //}

            }
            //if (e.CommandName == "ApproveMEDA")
            //{
            //    //Determine the RowIndex of the Row whose Button was clicked.
            //    int rowIndex = Convert.ToInt32(e.CommandArgument);

            //    //Reference the GridView Row.
            //    GridViewRow row = GVApplications.Rows[rowIndex];

            //    //Fetch value of Name.
            //    //string name = (row.FindControl("txtName") as TextBox).Text;

            //    //Fetch value of Country
            //    string applicationId = row.Cells[0].Text;
            //    Session["APPID"] = row.Cells[0].Text;
            //    MySqlConnection mySqlConnection = new MySqlConnection();
            //    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            //    string letterName = string.Empty;
            //    try
            //    {

            //        mySqlConnection.Open();
            //        

            //        switch (mySqlConnection.State)
            //        {

            //            case System.Data.ConnectionState.Open:
            //                string strQuery = string.Empty;
            //                strQuery = "Update APPLICANTDETAILS set isMEDAApp='Y' , APP_STATUS_DT=CURDATE() , app_status='MEDA letter approved.' where APPLICATION_NO='" + applicationId + "'";
            //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
            //                    cmd.ExecuteNonQuery();
            //                    fillGrid();
            //                      #region Application Status tracking date
            //                            strQuery = "insert into APPLICANT_STATUS_TRACKING " +
            //                                    " values('" + applicationId + "','MEDA letter approved.',CURDATE())";
            //                            cmd = new MySqlCommand(strQuery, mySqlConnection);
            //                            cmd.ExecuteNonQuery();
            //                            Response.Write("<script language='javascript'>alert('MEDA Letter Approved.');</script>");

            //                        #endregion
            //                               strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
            //                        MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
            //                        DataSet dsAppDet = new DataSet();
            //                        MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
            //                        daAppDet.Fill(dsAppDet);

            //                        string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
            //                        if (dsAppDet.Tables[0].Rows.Count > 0)
            //                        {
            //                            #region Send Mail
            //                            //sendMailOTP(strRegistrationno, strEmailID);
            //                            string strBody = string.Empty;

            //                            strBody += "Respected Sir/Madam" + ",<br/>";
            //                            strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
            //                            //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
            //                            //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
            //                            //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
            //                            //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
            //                            //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
            //                            strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString() ;
            //                            strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
            //                            //strBody += "Please use following information for login for further process. <br/>";
            //                            strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".The copy of application mentioning details of project and applicant is attached herewith.<br/>";
            //                            strBody += "You are requested to carry out the joint survey along with representative of M/s. " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " and inform to this office the various possible evacuation arrangements from the existing MSETCL network.<br/><br/>";
            //                            strBody += "The Technical Feasibility Report should have information such as:<br/>";
            //                            strBody += "1.	Nearest MSETCL’s Substation.<br/>";
            //                            strBody += "2.	Present load and capacity of transformers.<br/>";
            //                            strBody += "3.	Length of line from solar plant site to the MSETCL’s Substation.<br/>";
            //                            strBody += "4.	Availability of space at existing MSETCL’s Substation for line bays and transformer bays and possibility of procurement of adjacent land for Bus extension.<br/>";
            //                            strBody += "5.	Any augmentation in transformation capacity required.<br/>";
            //                            strBody += "6.	Any EHV Line / Railway / Highway / Forest crossing etc.<br/>";
            //                            strBody += "7.	Route proposed for Grid Connectivity.<br/>";
            //                            strBody += "8.	Any ROW problems.<br/>";
            //                            strBody += "<b>While issuing technical feasibility report, please ensure that, the existing & proposed RE power projects in the region are considered, along with the space availability for the proposed network already planned in STU plan. </b><br/>";
            //                            strBody += "<b>The Technical Feasibility Report should be submitted in the enclosed format for Technical Feasibility Report. </b><br/>";
            //                            strBody += "<b>The Technical Feasibility Report as per the enclosed format furnishing above- mentioned details should be submitted to this office within 15 days from the date of this letter. 	</b><br/>";
            //                            strBody += "Thanks & Regards, " + "<br/>";
            //                            strBody += " Chief Engineer" + "<br/>";
            //                            strBody += "(State Transmission Utility)" + "<br/>";
            //                            strBody += "MSETCL  " + "<br/>";
            //                            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

            //                            //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
            //                            try
            //                            {


            //                                #region using MailMessage
            //                                MailMessage Msg = new MailMessage();
            //                                MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
            //                                Msg.From = fromMail;
            //                                Msg.IsBodyHtml = true;
            //                                //log.Error("from:" + fromAddress);
            //                                //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
            //                                Msg.To.Add(new MailAddress(strEmail));
            //                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
            //                                Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
            //                                //  Msg.To.Add(new MailAddress(toAddress));

            //                                Msg.Subject = "Online Grid connectivity application.";
            //                                Msg.Body = strBody;
            //                                //SmtpClient a = new SmtpClient("mail.mahatransco.in");
            //                                SmtpClient a = new SmtpClient("23.103.140.170");
            //                                a.Send(Msg);

            //                                Msg = null;
            //                                fromMail = null;
            //                                a = null;
            //                                #endregion
            //                            }
            //                            catch (Exception ex)
            //                            {
            //                                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
            //                                log.Error(ErrorMessage);
            //                                // throw ex;
            //                            }
            //                            #endregion
            //                        }
        
            //                break;

            //            case System.Data.ConnectionState.Closed:

            //                // Connection could not be made, throw an error

            //                throw new Exception("The database connection state is Closed");

            //                break;

            //            default:

            //                // Connection is actively doing something else

            //                break;

            //        }


            //        // Place Your Code Here to Process Data //

            //    }

            //    catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            //    {
            //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
            //        log.Error(ErrorMessage);
            //        // Use the mySqlException object to handle specific MySql errors

            //    }

            //    catch (Exception exception)
            //    {
            //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
            //        log.Error(ErrorMessage);
            //        // Use the exception object to handle all other non-MySql specific errors

            //    }

            //    finally
            //    {

            //        // Make sure to only close connections that are not in a closed state

            //        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
            //        {

            //            // Close the connection as a good Garbage Collecting practice

            //            mySqlConnection.Close();

            //        }

            //    }
                


            //}
            if (e.CommandName == "ViewTech")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                string docName = string.Empty;
                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd = new MySqlCommand("select *  from applicantdetails where APPLICATION_NO='" + applicationId + "'", mySqlConnection);
                            DataSet dsResult = new DataSet();
                            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            if (dsResult.Tables[0].Rows.Count > 0)
                            {

                                docName = dsResult.Tables[0].Rows[0]["docTech"].ToString();
                                Session["PROJID"] = row.Cells[1].Text;
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

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors

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
                Response.Redirect("~/UI/Emp/ShowTech.aspx?appid=" + applicationId + "&docName=" + docName, false);


            }
            if (e.CommandName == "PutUp")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                string letterName = string.Empty;
                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            strQuery = "Update APPLICANTDETAILS set APP_STATUS_DT=CURDATE() , app_status='Put up to Commitee.' where APPLICATION_NO='" + applicationId + "'";
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            fillGrid();
                            #region Application Status tracking date
                            strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                    " values('" + applicationId + "','Put up to Commitee.',CURDATE(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            Response.Write("<script language='javascript'>alert('Put up to Commitee.');</script>");

                            #endregion
                            strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                            MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsAppDet = new DataSet();
                            MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                            daAppDet.Fill(dsAppDet);

                            string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
                            if (dsAppDet.Tables[0].Rows.Count > 0)
                            {
                                #region Send Mail
                                //sendMailOTP(strRegistrationno, strEmailID);
                                string strBody = string.Empty;

                                strBody += "Respected Sir/Madam" + ",<br/>";
                                strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                                //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                                //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                                //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                                //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                                //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                                strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                                strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                                //strBody += "Please use following information for login for further process. <br/>";
                                strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>";
                                strBody += "Your application is Put up to Commitee. <br/><br/>";
                                
                                strBody += "Thanks & Regards, " + "<br/>";
                                strBody += " Chief Engineer" + "<br/>";
                                strBody += "(State Transmission Utility)" + "<br/>";
                                strBody += "MSETCL  " + "<br/>";
                                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                                //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
                                try
                                {


                                    #region using MailMessage
                                    MailMessage Msg = new MailMessage();
                                    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                                    Msg.From = fromMail;
                                    Msg.IsBodyHtml = true;
                                    //log.Error("from:" + fromAddress);
                                    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                                    Msg.To.Add(new MailAddress(strEmail));
                                    Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                    Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                    //  Msg.To.Add(new MailAddress(toAddress));

                                    Msg.Subject = "Online Grid connectivity application.";
                                    Msg.Body = strBody;
                                    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                    SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                                    a.EnableSsl = true;
                                    NetworkCredential n = new NetworkCredential();
                                    n.UserName = "donotreply@mahatransco.in";
                                    n.Password = "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~";
                                    a.UseDefaultCredentials = false;
                                    a.Credentials = n;
                                    a.Port = 587;
                                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                                    a.Send(Msg);

                                    Msg = null;
                                    fromMail = null;
                                    a = null;
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                    log.Error(ErrorMessage);
                                    // throw ex;
                                }
                                #endregion
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

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors

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
            if (e.CommandName == "PutUp")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                MySqlConnection mySqlConnection = new MySqlConnection();
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                string letterName = string.Empty;
                try
                {

                    mySqlConnection.Open();
                    

                    switch (mySqlConnection.State)
                    {

                        case System.Data.ConnectionState.Open:
                            string strQuery = string.Empty;
                            strQuery = "Update APPLICANTDETAILS set APP_STATUS_DT=CURDATE() , app_status='Put up to Commitee.' where APPLICATION_NO='" + applicationId + "'";
                            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            fillGrid();
                            #region Application Status tracking date
                            strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                    " values('" + applicationId + "','Put up to Commitee.',CURDATE(),'" + Session["SAPID"].ToString() + "')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            Response.Write("<script language='javascript'>alert('Put up to Commitee.');</script>");

                            #endregion
                            strQuery = "select distinct a.*,b.*,c.email Zone_email from APPLICANTDETAILS a,APPLICANT_REG_DET b ,zone_district c where a.GSTIN_NO=b.GSTIN_NO and lower(a.project_district)=lower(c.district) and a.application_no='" + applicationId + "'";
                            MySqlCommand cmdAppDet = new MySqlCommand(strQuery, mySqlConnection);
                            DataSet dsAppDet = new DataSet();
                            MySqlDataAdapter daAppDet = new MySqlDataAdapter(cmdAppDet);
                            daAppDet.Fill(dsAppDet);

                            string strEmail = dsAppDet.Tables[0].Rows[0]["Zone_email"].ToString();
                            if (dsAppDet.Tables[0].Rows.Count > 0)
                            {
                                #region Send Mail
                                //sendMailOTP(strRegistrationno, strEmailID);
                                string strBody = string.Empty;

                                strBody += "Respected Sir/Madam" + ",<br/>";
                                strBody += "With reference to above subject, M/s. " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + " has proposed to setup " + dsAppDet.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW<br/>";
                                //strBody += "Application ID for further reference is : " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + "<br/>";
                                //strBody += "Organization Name : " + dsAppDet.Tables[0].Rows[0]["ORGANIZATION_NAME"].ToString() + "<br/>";
                                //strBody += "Contact Person Name : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString() + "<br/>";
                                //strBody += "Contact Person Designation : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString() + "<br/>";
                                //strBody += "Contact Person Mobile : " + dsAppDet.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + "<br/>";
                                strBody += dsAppDet.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                                strBody += " Power Project at Village: " + dsAppDet.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "&nbsp; &nbsp; Tal: " + dsAppDet.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + "&nbsp; &nbsp;District: " + dsAppDet.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "<br/>";
                                //strBody += "Please use following information for login for further process. <br/>";
                                strBody += "vide Application No. " + dsAppDet.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".<br/>";
                                strBody += "Your application is Put up to Commitee. <br/><br/>";

                                strBody += "Thanks & Regards, " + "<br/>";
                                strBody += " Chief Engineer" + "<br/>";
                                strBody += "(State Transmission Utility)" + "<br/>";
                                strBody += "MSETCL  " + "<br/>";
                                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                                //objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
                                try
                                {


                                    #region using MailMessage
                                    MailMessage Msg = new MailMessage();
                                    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                                    Msg.From = fromMail;
                                    Msg.IsBodyHtml = true;
                                    //log.Error("from:" + fromAddress);
                                    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                                    Msg.To.Add(new MailAddress(strEmail));
                                    Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["ORG_EMAIL"].ToString()));
                                    Msg.CC.Add(new MailAddress(dsAppDet.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                    //  Msg.To.Add(new MailAddress(toAddress));

                                    Msg.Subject = "Your Online Grid connectivity application is put up to commitee.";
                                    Msg.Body = strBody;
                                    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                                    SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                                    a.EnableSsl = true;
                                    NetworkCredential n = new NetworkCredential();
                                    n.UserName = "donotreply@mahatransco.in";
                                    n.Password = "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~";
                                    a.UseDefaultCredentials = false;
                                    a.Credentials = n;
                                    a.Port = 587;
                                    System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                                    a.Send(Msg);

                                    Msg = null;
                                    fromMail = null;
                                    a = null;
                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                    log.Error(ErrorMessage);
                                    // throw ex;
                                }
                                #endregion
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

                }

                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors

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
            if (e.CommandName == "Approvedbycommitee")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;

                Response.Redirect("~/UI/Emp/ApprComm.aspx", false);

                //MySqlConnection mySqlConnection = new MySqlConnection();
                //mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

                //string letterName = string.Empty;
                //try
                //{

                //    mySqlConnection.Open();
                //    

                //    switch (mySqlConnection.State)
                //    {

                //        case System.Data.ConnectionState.Open:
                //            string strQuery = string.Empty;
                //            strQuery = "Update APPLICANTDETAILS set APP_STATUS_DT=CURDATE() , app_status='Approved by commitee.' where APPLICATION_NO='" + applicationId + "'";
                //            MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                //            cmd.ExecuteNonQuery();
                //            fillGrid();
                            
                //            break;

                //        case System.Data.ConnectionState.Closed:

                //            // Connection could not be made, throw an error

                //            throw new Exception("The database connection state is Closed");

                //            break;

                //        default:

                //            // Connection is actively doing something else

                //            break;

                //    }


                //    // Place Your Code Here to Process Data //

                //}

                //catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                //{
                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                //    log.Error(ErrorMessage);
                //    // Use the mySqlException object to handle specific MySql errors

                //}

                //catch (Exception exception)
                //{
                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                //    log.Error(ErrorMessage);
                //    // Use the exception object to handle all other non-MySql specific errors

                //}

                //finally
                //{

                //    // Make sure to only close connections that are not in a closed state

                //    if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                //    {

                //        // Close the connection as a good Garbage Collecting practice

                //        mySqlConnection.Close();

                //    }

                //}



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
                .Where(fld => fld.HeaderText == "Document verification")
                .SingleOrDefault()).Visible = true;

                
                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Application verification")
                .SingleOrDefault()).Visible = true;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "View Application")
                .SingleOrDefault()).Visible = false;

                

               // ((DataControlField)GVApplications.Columns
               //.Cast<DataControlField>()
               //.Where(fld => fld.HeaderText == "Approve MEDA Report")
               //.SingleOrDefault()).Visible = true;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Technical Feasibility")
                .SingleOrDefault()).Visible = false;

                ((DataControlField)GVApplications.Columns
               .Cast<DataControlField>()
               .Where(fld => fld.HeaderText == "View Feasibility")
               .SingleOrDefault()).Visible = true;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Cancel Application")
                .SingleOrDefault()).Visible = true;

            }
            else
            {
                {
                    ((DataControlField)GVApplications.Columns
                    .Cast<DataControlField>()
                    .Where(fld => fld.HeaderText == "Document verification")
                    .SingleOrDefault()).Visible = false;

                    ((DataControlField)GVApplications.Columns
                    .Cast<DataControlField>()
                    .Where(fld => fld.HeaderText == "Application verification")
                    .SingleOrDefault()).Visible = false;

                    ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "View Application")
                .SingleOrDefault()).Visible = true;

                   // ((DataControlField)GVApplications.Columns
                   //.Cast<DataControlField>()
                   // .Where(fld => fld.HeaderText == "Approved by commitee")
                   //.SingleOrDefault()).Visible = false;

                    ((DataControlField)GVApplications.Columns
                    .Cast<DataControlField>()
                    .Where(fld => fld.HeaderText == "Technical Feasibility")
                    .SingleOrDefault()).Visible = true;

                    ((DataControlField)GVApplications.Columns
                   .Cast<DataControlField>()
                   .Where(fld => fld.HeaderText == "View Feasibility")
                   .SingleOrDefault()).Visible = false;

                    ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Cancel Application")
                .SingleOrDefault()).Visible = false;
                }

            }
        }

        protected void GVApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            log.Error("Before PageIndex" + e.NewPageIndex);
            GVApplications.PageIndex = e.NewPageIndex;
            log.Error("After PageIndex");
            fillGrid();
            log.Error("Before Grid");
        }

        protected void lnkSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
        }
    }
}