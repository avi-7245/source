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
using GGC.WebService;
namespace GGC.UI.Cons
{
    public partial class FinalGridCon : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FinalGridCon));
        protected string strUserID = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            strUserID = Request.QueryString["usrName"];
          // strUserID = "023530";

            if (!Page.IsPostBack)
            {
                if (strUserID != "")
                {
                    checkUser();
                }
            }
        }
        protected void checkUser()
        {




            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            bool isUser = false;

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd = new MySqlCommand("select *  from APPLICANT_LOGIN_MASTER where USER_NAME='" + strUserID + "'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            cmd = new MySqlCommand("select *  from APPLICANTDETAILS where USER_NAME='" + strUserID + "'", mySqlConnection);
                            DataSet dsResult2 = new DataSet();
                            MySqlDataAdapter da2 = new MySqlDataAdapter(cmd);
                            da2.Fill(dsResult2);
                            Session["isValid"] = "true";
                            //Session["GSTIN"] = dsResult.Tables[0].Rows[0]["GSTIN_NO"].ToString();
                            Session["user_name"] = strUserID;
                            Session["generatorId"] = dsResult.Tables[0].Rows[0]["generatorId"].ToString();


                        }
                        else
                        {
                            //Response.Write("<script language='javascript'>alert('User doesnot exist.');</script>");

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User doesnot exist.');window.location ='Home.aspx';", true);
                            //APICall apiCall = new APICall();
                            //string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + strUserID + "&entity=MSETCL");

                            //try
                            //{
                            //    //string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getUsrDtls?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;
                            //    //string userAuthenticationURI = "https://regridmeda.mahadiscom.in/swPortal/getUsrDtls?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;
                            //    string userAuthenticationURI = ConfigurationManager.AppSettings["MEDAGETUSRDETURL"].ToString() + "?usrName=" + strUserID + "&entity=MSETCL&secKey=" + str;

                            //    WebRequest req = WebRequest.Create(@userAuthenticationURI);


                            //    req.Method = "GET";
                            //    //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                            //    //req.Credentials = new NetworkCredential("username", "password");
                            //    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                            //    StreamReader reader = new StreamReader(resp.GetResponseStream());

                            //    string responseText = reader.ReadToEnd();

                            //    //if your response is in json format just uncomment below line  

                            //    //Response.AddHeader("Content-type", "text/json");  

                            //    //Response.Write(responseText);

                            //    UserReg uReg = JsonConvert.DeserializeObject<UserReg>(responseText);
                            //    DateTime dt = DateTime.ParseExact(uReg.createdDt, "dd-MM-yyyy",
                            //      CultureInfo.InvariantCulture);


                            //    strQuery = "insert into applicant_reg_det(GSTIN_NO,ORGANIZATION_NAME,ContactPerson,address1,address2,ORG_MOB,ORG_EMAIL,CREATED_DT,USER_NAME,PAN_TAN_NO,pincode,createdBy,generatorId)" +
                            //        " values('" + uReg.generatorGst + "','" + uReg.generatorName + "','" + uReg.contactPersonName + "','" + uReg.companyAddressLandMark + "','" + uReg.generatorCity + "','" + uReg.generatorMobileNo + "','" + uReg.generatorEmailId + "','" + dt.ToString("yyyy-MM-dd") + "','" + uReg.generatorUserName + "','" + uReg.generatorPan + "','" + uReg.generatorPinCode + "','" + uReg.createdBy + "','" + uReg.generatorId + "')";
                            //    cmd = new MySqlCommand(strQuery, mySqlConnection);
                            //    cmd.ExecuteNonQuery();

                            //    strQuery = "insert into APPLICANT_LOGIN_MASTER(GSTIN_NO, PASSWORD, CREATED_DT,USER_NAME,generatorId) values('" + uReg.generatorGst + "','" + uReg.generatorGst + "',CURDATE(),'" + uReg.generatorUserName + "','" + uReg.generatorId + "')";
                            //    cmd = new MySqlCommand(strQuery, mySqlConnection);
                            //    log.Error("Insert login" + strQuery);

                            //    cmd.ExecuteNonQuery();

                            //    Session["isValid"] = "true";
                            //    Session["GSTIN"] = uReg.generatorGst.ToString();
                            //    Session["user_name"] = uReg.generatorUserName;
                            //    Session["generatorId"] = uReg.generatorId;
                            //    //Response.Redirect("~/UI/Cons/ConsumerDetail.aspx", false);
                            //    Response.Redirect("~/UI/Cons/apphome.aspx", false);
                            //}
                            //catch (Exception ex)
                            //{
                            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                            //    log.Error(ErrorMessage);
                            //    Response.Write("<script language='javascript'>alert('Kindly try after some time.');</script>");
                            //}

                            //                            Response.Redirect("http://regrid.mahadiscom.in/reGrid/getUsrDtls?usrName='" + strUserID + "'&entity=MSETCL&secKey="+str, false);


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
        void fillGrid()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            //strGSTIN = Session["GSTIN"].ToString();
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select * from finalgcdocmaster where isActive='Y' order by srno", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            GVFiles.DataSource = dsResult.Tables[0];
                            GVFiles.DataBind();
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
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
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
        protected void fillData()
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //Button btnFinalSubmit = (Button)(GVFiles).FooterRow.FindControl("btnFinalSubmit");
            //strGSTIN = Session["GSTIN"].ToString();
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select *,date_format(MEDA_REC_LETTER_DT,'%Y-%m-%d') MEDADateF,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F   from APPLICANTDETAILS where MEDAProjectID='" + txtProjCode.Text + "'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            txtAppNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                            txtNatureApp.Text = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                            txtProjType.Text = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                            txtProjectCapacity.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                            txtProjectLocation.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();
                            txtTaluka.Text = dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString();
                            txtDistrict.Text = dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString();
                            Session["APPLICATION_NO"] = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                            Session["isValid"] = "Yes";
                            //btnFinalSubmit.Enabled = !string.IsNullOrEmpty(txtAppNo.Text);
                        }
                        else
                        {
                            Session["isValid"] = "No";
                            //btnFinalSubmit.Enabled = false;
                        }
                        cmd = new MySqlCommand("select * from finalgridcondocs where APPLICATION_NO='" + txtAppNo.Text + "'", mySqlConnection);
                        dsResult = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            Session["UploadFileDet"] = dsResult;

                        }

                        cmd = new MySqlCommand("select count(1) from finalgcapproval where APPLICATION_NO='" + txtAppNo.Text + "'", mySqlConnection);
                        dsResult = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (int.Parse(dsResult.Tables[0].Rows[0][0].ToString()) > 0)
                        {
                            Session["FinalSubmit"] = "Y";
                            //btnFinalSubmit.Enabled = Session["FinalSubmit"].ToString() == "N"; // Enable if not submitted

                        }
                        else
                        {
                            Session["FinalSubmit"] = "N";
                            //btnFinalSubmit.Enabled = Session["FinalSubmit"].ToString() == "Y"; // Disable  if  submitted

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
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
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
        //protected void fillAppType()
        //{
        //    MySqlConnection mySqlConnection = new MySqlConnection();
        //    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


        //    try
        //    {

        //        mySqlConnection.Open();


        //        switch (mySqlConnection.State)
        //        {

        //            case System.Data.ConnectionState.Open:
        //                string strQuery = string.Empty;
        //                MySqlCommand cmd;
        //                cmd = new MySqlCommand("select distinct app_type ,app_type_no from applicant_project_type where app_type like 'NonConventional Generator'  order by 2", mySqlConnection);
        //                DataSet dsResult = new DataSet();
        //                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //                da.Fill(dsResult);
        //                ddlNatureOfApp.DataSource = dsResult.Tables[0];
        //                ddlNatureOfApp.DataValueField = "app_type_no";
        //                ddlNatureOfApp.DataTextField = "app_type";
        //                ddlNatureOfApp.DataBind();
        //                //ddlNatureOfApp.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
        //                ddlNatureOfApp.ClearSelection();
        //                ddlNatureOfApp.SelectedIndex = 0;
        //                ddlNatureOfApp_SelectedIndexChanged(this, EventArgs.Empty);
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
        //        lblResult.Text = "Some problem during registration.Please try again.";
        //    }

        //    catch (Exception exception)
        //    {
        //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //        log.Error(ErrorMessage);
        //        // Use the exception object to handle all other non-MySql specific errors
        //        lblResult.Text = "Some problem during registration.Please try again.";
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

        //protected void fillDistrict()
        //{
        //    MySqlConnection mySqlConnection = new MySqlConnection();
        //    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();


        //    try
        //    {

        //        mySqlConnection.Open();


        //        switch (mySqlConnection.State)
        //        {

        //            case System.Data.ConnectionState.Open:
        //                string strQuery = string.Empty;
        //                MySqlCommand cmd;
        //                cmd = new MySqlCommand("select distinct district district from zone_district order by 1", mySqlConnection);
        //                DataSet dsResult = new DataSet();
        //                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //                da.Fill(dsResult);
        //                ddlDistrict.DataSource = dsResult.Tables[0];
        //                ddlDistrict.DataValueField = "district";
        //                ddlDistrict.DataTextField = "district";
        //                ddlDistrict.DataBind();
        //                ddlDistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
        //                ddlDistrict.ClearSelection();
        //                ddlDistrict.SelectedIndex = 0;
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
        //        lblResult.Text = "Some problem during registration.Please try again.";
        //    }

        //    catch (Exception exception)
        //    {
        //        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //        log.Error(ErrorMessage);
        //        // Use the exception object to handle all other non-MySql specific errors
        //        lblResult.Text = "Some problem during registration.Please try again.";
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (txtProjCode.Text != "")
            {
                fillData();
                fillGrid();
            }
        }

        protected void GVFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Upload")
            {
                if (Session["isValid"] == "Yes")
                {
                    //Determine the RowIndex of the Row whose Button was clicked.
                    int rowIndex = Convert.ToInt32(e.CommandArgument);

                    //Reference the GridView Row.
                    GridViewRow row = GVFiles.Rows[rowIndex];
                    FileUpload FileUpload1 = (row.FindControl("FileUpload1") as FileUpload);
                    Button btnViewTech = (row.FindControl("btnUpload") as Button);
                    //Fetch value of Name.
                    //string name = (row.FindControl("txtName") as TextBox).Text;

                    //Fetch value of Country
                    log.Error(row.Cells[0].Text);

                    string srNo = row.Cells[0].Text;
                    string strAppID = txtAppNo.Text;
                    string strMEDAID = txtProjCode.Text;
                    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
                    string newFileName = "";

                    //Check whether Directory (Folder) exists.
                    if (!Directory.Exists(folderPath))
                    {
                        //If Directory (Folder) does not exists Create it.
                        Directory.CreateDirectory(folderPath);
                    }
                    if (FileUpload1.HasFile)
                    {
                        MySqlConnection mySqlConnection = new MySqlConnection();
                        //Save the File to the Directory (Folder).
                        try
                        {

                            FileUpload1.SaveAs(folderPath + Path.GetFileName(FileUpload1.FileName));
                            newFileName = strAppID + "_" + srNo + "_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload1.FileName;
                            System.IO.File.Move(folderPath + FileUpload1.FileName, folderPath + newFileName);

                            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                            mySqlConnection.Open();


                            switch (mySqlConnection.State)
                            {

                                case System.Data.ConnectionState.Open:
                                    string strQuery = "select * from FinalGridConDocs where APPLICATION_NO='" + strAppID + "' and FileSrNo=" + srNo;
                                    //log.Error(strQuery);
                                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    DataSet dsResult = new DataSet();
                                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                    da.Fill(dsResult);
                                    string filename = string.Empty;
                                    if (dsResult.Tables[0].Rows.Count > 0)
                                    {
                                        filename = dsResult.Tables[0].Rows[0]["FileName"].ToString();
                                        System.IO.File.Delete(folderPath + filename);

                                        strQuery = "delete from FinalGridConDocs where APPLICATION_NO='" + strAppID + "' and FileSrNo=" + srNo;
                                        log.Error(strQuery);
                                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                                        cmd.ExecuteNonQuery();
                                    }
                                    strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileSrNo,FileName) values('" + strAppID + "','" + strMEDAID + "'," + srNo + ",'" + newFileName + "')";
                                    log.Error(strQuery);
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    //lblResult.Text = "Application form uploaded Successfully!!";

                                    btnViewTech.Enabled = false;
                                    btnViewTech.BackColor = System.Drawing.Color.Red;

                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");

                                    break;

                                case System.Data.ConnectionState.Closed:

                                    // Connection could not be made, throw an error

                                    throw new Exception("The database connection state is Closed");

                                    break;

                                default:
                                    break;

                            }
                        }
                        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                            log.Error(ErrorMessage);
                            lblResult.Text = "Letter Uploaded Failed!!";

                        }
                        catch (Exception exception)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                            log.Error(ErrorMessage);
                            lblResult.Text = "Letter Uploaded Failed!!";
                        }

                        finally
                        {
                            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                            {
                                mySqlConnection.Close();
                            }

                        }





                        //Display the Picture in Image control.
                        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Project Code invalid. Cannot upload documents.');", true);
                }
            }

            if (e.CommandName == "FinalSubmit")
            {
                if (Session["isValid"] == "Yes")
                {
                    string strAppID = txtAppNo.Text;
                    string strMEDAID = txtProjCode.Text;
                    //int rowIndex = Convert.ToInt32(e.CommandArgument);   
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {
                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                MySqlCommand cmd = new MySqlCommand("SELECT count(1) FROM finalgridcondocs a, finalgcdocmaster b where a.FileSrNo=b.Srno and a.APPLICATION_NO='" + strAppID + "'", mySqlConnection);
                                DataSet dsResult = new DataSet();
                                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                                da.Fill(dsResult);

                                int totalDocumentsUploaded = 0;
                                totalDocumentsUploaded = int.Parse(dsResult.Tables[0].Rows[0][0].ToString());

                                if (totalDocumentsUploaded >= 10)
                                {
                                    string strQuery = "insert into finalgcapproval(APPLICATION_NO,MEDAProjectID,roleid,createby) values('" + strAppID + "','" + strMEDAID + "',2," + strAppID + ")";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();

                                    strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=25, app_status='FINAL GRID CONNECTIVITY ALL DOCUMENTS UPLOADED.',APP_STATUS_DT=NOW() where APPLICATION_NO='" + strAppID + "'";
                                    cmd = new MySqlCommand(strQuery, mySqlConnection);
                                    cmd.ExecuteNonQuery();
                                    //lblResult.Text = "Application form uploaded Successfully!!";
                                    Button btnFinalSubmit = (Button)((GridView)sender).FooterRow.FindControl("btnFinalSubmit");

                                    #region Save To MEDA
                                    saveApp(strMEDAID);
                                    //Remove Comment
                                    #endregion Save To MEDA

                                    #region Send Mail
                                    string strQueryGetDetails = "select * from applicantdetails where APPLICATION_NO='" + txtAppNo.Text + "'";
                                    MySqlCommand cmdQueryGetDetails = new MySqlCommand(strQueryGetDetails, mySqlConnection);

                                    DataSet dsResultGetDetails = new DataSet();
                                    MySqlDataAdapter daQueryGetDetails = new MySqlDataAdapter(cmdQueryGetDetails);
                                    daQueryGetDetails.Fill(dsResultGetDetails);

                                    string strQueryGetDetails1 = "select * from empmaster where role_id=51";
                                    MySqlCommand cmdQueryGetDetails1 = new MySqlCommand(strQueryGetDetails1, mySqlConnection);

                                    DataSet dsResultGetDetails1 = new DataSet();
                                    MySqlDataAdapter daQueryGetDetails1 = new MySqlDataAdapter(cmdQueryGetDetails1);
                                    daQueryGetDetails1.Fill(dsResultGetDetails1);
                                    //string strEmail = dsResultGetDetails.Tables[0].Rows[0]["Zone_email"].ToString();
                                    if (dsResultGetDetails.Tables[0].Rows.Count > 0)
                                    {
                                        #region Send Mail
                                        //sendMailOTP(strRegistrationno, strEmailID);
                                        string strBody = string.Empty;

                                        strBody += "Dear Sir/Madam" + ",<br/>";
                                        strBody += "Regarding request of Final Grid Connectivity " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResultGetDetails.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " Project proposed by M/s. " + dsResultGetDetails.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " at Village: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Tal.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + ", submitted all documents for scrutiny. ";

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
                                            Msg.To.Add(new MailAddress(dsResultGetDetails1.Tables[0].Rows[0]["EmpEmailID"].ToString()));
                                            Msg.CC.Add(new MailAddress(dsResultGetDetails.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString()));
                                            Msg.CC.Add(new MailAddress(ConfigurationManager.AppSettings["MMEmailID"].ToString()));
                                            //  Msg.To.Add(new MailAddress(toAddress));

                                            //Msg.Subject = "SLD document rejected in Online Grid connectivity application.";
                                            Msg.Subject = "Regarding request of Final Grid Connectivity for " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW " + dsResultGetDetails.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " Project proposed by M/s. " + dsResultGetDetails.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString() + " at Village: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Tal.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResultGetDetails.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() + "";
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
                                    #endregion


                                    btnFinalSubmit.Enabled = false;
                                    btnFinalSubmit.BackColor = System.Drawing.Color.Red;
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('Submitted Succsefully.')</script>");
                                }
                                else
                                {
                                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('Upload all documents first.')</script>");

                                }
                                break;

                            case System.Data.ConnectionState.Closed:

                                // Connection could not be made, throw an error

                                throw new Exception("The database connection state is Closed");

                                break;

                            default:
                                break;

                        }
                    }
                    catch (MySql.Data.MySqlClient.MySqlException mySqlException)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                        log.Error(ErrorMessage);
                        lblResult.Text = "Submission Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblResult.Text = "Submission Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Project Code invalid. Cannot upload documents.');", true);
                }
            }
        }

        public string saveApp(string projId)
        {
            GetStatusDTO objGetStatusDTO = new GetStatusDTO();
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        strQuery = "select * from applicantdetails where MEDAProjectID='" + projId + "'";
                        MySqlCommand cmd;
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        //txtEmail1.Text = dsResult.Tables[0].Rows[0]["ORG_EMAIL"].ToString();
                        objGetStatusDTO.projID = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                        objGetStatusDTO.status = dsResult.Tables[0].Rows[0]["app_status"].ToString();
                        //objGetStatusDTO.statusDT = dsResult.Tables[0].Rows[0]["APP_STATUS_DT"].ToString();
                        string strStatusDate = dsResult.Tables[0].Rows[0]["APP_STATUS_DT"].ToString();
                        objGetStatusDTO.statusDT = strStatusDate; //DateTime.ParseExact(strStatusDate, "dd-MM-yyyy", null);
                        objGetStatusDTO.APPID = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                        objGetStatusDTO.Contact_Person = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString();
                        objGetStatusDTO.Contact_Person_No = dsResult.Tables[0].Rows[0]["CONT_PER_PHONE_1"].ToString();
                        objGetStatusDTO.Contact_Person_MobNo = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                        objGetStatusDTO.Contact_Person_Email = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                        objGetStatusDTO.TotalLandRequired = dsResult.Tables[0].Rows[0]["TOTAL_REQUIRED_LAND"].ToString();
                        objGetStatusDTO.ProjecType = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                        objGetStatusDTO.ProjCapacityMW = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                        objGetStatusDTO.GenerationVoltage = dsResult.Tables[0].Rows[0]["GENERATION_VOLTAGE"].ToString();
                        objGetStatusDTO.PointOfInjection = dsResult.Tables[0].Rows[0]["POINT_OF_INJECTION"].ToString();
                        objGetStatusDTO.InjectionVoltage = dsResult.Tables[0].Rows[0]["INJECTION_VOLTAGE"].ToString();
                        objGetStatusDTO.GENERATOR_NAME = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                        objGetStatusDTO.TYPE_OF_GENERATION = dsResult.Tables[0].Rows[0]["TYPE_OF_GENERATION"].ToString();
                        objGetStatusDTO.NAME_OF_SUBSTATION = dsResult.Tables[0].Rows[0]["POINT_OF_INJECTION"].ToString();
                        objGetStatusDTO.DISTANCE_FROM_PLANT = dsResult.Tables[0].Rows[0]["DISTANCE_FROM_PLANT"].ToString();
                        objGetStatusDTO.VOLT_LEVEL_SUBSTATION = dsResult.Tables[0].Rows[0]["VOLT_LEVEL_SUBSTATION"].ToString();


                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.statusId = "2";
                        APICall apiCall = new APICall();
                        string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + Session["user_name"].ToString() + "&projId=" + Session["ProjectID"].ToString() + "&entity=MSETCL");

                        objGetStatusDTO.secKey = str;
                        objGetStatusDTO.entity = "MSETCL";



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
                //lblResult.Text = "Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                //lblResult.Text = "Please try again.";
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



            try
            {
                //                                string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projid=" + txtProjCode + "&entity=MSETCL&secKey=" + str;
                //string userAuthenticationURI = "https://regrid.mahadiscom.in/reGrid/saveToMedaGcAppln";
                string userAuthenticationURI = ConfigurationManager.AppSettings["MEDASAVETOMEDAURL"].ToString();
                //WebRequest req = WebRequest.Create(@userAuthenticationURI);
                var req = (HttpWebRequest)WebRequest.Create(@userAuthenticationURI);


                req.Method = "POST";
                req.ContentType = "application/json";
                //req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("20036:ezy@123"));
                //req.Credentials = new NetworkCredential("username", "password");
                using (var streamWriter = new StreamWriter(req.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);

                    streamWriter.Write(JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None));
                }
                HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

                StreamReader reader = new StreamReader(resp.GetResponseStream());

                string responseText = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
            }
            //string str = JsonConverter. .Serialize(objGetStatusDTO, Formatting.Indented);
            return JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);
            //return  new JavaScriptSerializer().Serialize(objGetStatusDTO);
        }
        protected void GVFiles_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            DataSet ds = new DataSet();
            ds = (DataSet)Session["UploadFileDet"];
            if (ds != null)
            {
                try
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        int rowno = e.Row.RowIndex;

                        Button btnUpload = (Button)e.Row.Cells[3].FindControl("btnUpload");
                        Label lblRemark = (Label)e.Row.Cells[4].FindControl("lblRemark");
                        string srNo = e.Row.Cells[0].Text;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i]["FileSrNo"].ToString() == srNo)
                            {
                                if (ds.Tables[0].Rows[i]["isVerified"].ToString() == "N" || ds.Tables[0].Rows[i]["isVerified"].ToString() == "")
                                {
                                    btnUpload.Enabled = true;
                                    //btnUpload.BackColor = System.Drawing.Color.Red;
                                    lblRemark.Text = ds.Tables[0].Rows[i]["Remark"].ToString();
                                }
                                if (ds.Tables[0].Rows[i]["isVerified"].ToString() == "Y")
                                {
                                    btnUpload.Enabled = false;
                                    btnUpload.BackColor = System.Drawing.Color.Red;

                                }
                                break;
                            }
                        }
                    }

                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        int rowno = e.Row.RowIndex;

                        Button btnFinalSubmit = (Button)e.Row.Cells[1].FindControl("btnFinalSubmit");

                        if (Session["FinalSubmit"] == "Y")
                        {
                            btnFinalSubmit.Enabled = false;
                            btnFinalSubmit.BackColor = System.Drawing.Color.Red;

                        }
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

        protected void imgAppForm_Click(object sender, ImageClickEventArgs e)
        {
            //string filePath = "C:\\Users\\Admi\\FinalGCFiles\\abc.txt";
            string filePath = Server.MapPath("~/Files/FinalGCFiles/FGC_Application_Form.docx");
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                // Clear Rsponse reference  
                Response.Clear();
                // Add header by specifying file name  
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                // Add header for content length  
                Response.AddHeader("Content-Length", file.Length.ToString());
                // Specify content type  
                Response.ContentType = "application/octet-stream";


                Response.Flush();
                // Transimiting file  
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

        protected void imgConnAgg_Click(object sender, ImageClickEventArgs e)
        {
            string filePath = Server.MapPath("~/Files/FinalGCFiles/CONNECTION_AGREEMENT.docx");
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                // Clear Rsponse reference  
                Response.Clear();
                // Add header by specifying file name  
                Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                // Add header for content length  
                Response.AddHeader("Content-Length", file.Length.ToString());
                // Specify content type  
                Response.ContentType = "application/octet-stream";

                // Clearing flush  
                Response.Flush();
                // Transimiting file  
                Response.TransmitFile(file.FullName);
                Response.End();
            }
        }

        protected void GVFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #region Seperate code
        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/"+strAppID+"/");
        //    string newFileName = "";

        //        //Check whether Directory (Folder) exists.
        //        if (!Directory.Exists(folderPath))
        //        {
        //            //If Directory (Folder) does not exists Create it.
        //            Directory.CreateDirectory(folderPath);
        //        }
        //        if (FileUpload1.HasFile)
        //        {
        //            MySqlConnection mySqlConnection = new MySqlConnection();
        //            //Save the File to the Directory (Folder).
        //            try
        //            {

        //                FileUpload1.SaveAs(folderPath + Path.GetFileName(FileUpload1.FileName));
        //                newFileName = strAppID + "_" + "1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload1.FileName;
        //                System.IO.File.Move(folderPath + FileUpload1.FileName, folderPath + newFileName);

        //                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //                mySqlConnection.Open();


        //                switch (mySqlConnection.State)
        //                {

        //                    case System.Data.ConnectionState.Open:
        //                        string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('"+strAppID+"','"+strMEDAID+"','" + newFileName + "')";
        //                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                        cmd.ExecuteNonQuery();
        //                        //lblResult.Text = "Application form uploaded Successfully!!";
        //                        ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                        break;

        //                    case System.Data.ConnectionState.Closed:

        //                        // Connection could not be made, throw an error

        //                        throw new Exception("The database connection state is Closed");

        //                        break;

        //                    default:
        //                        break;

        //                }
        //            }
        //            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //            {
        //                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //                log.Error(ErrorMessage);
        //                lblResult.Text = "Letter Uploaded Failed!!";

        //            }
        //            catch (Exception exception)
        //            {
        //                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //                log.Error(ErrorMessage);
        //                lblResult.Text = "Letter Uploaded Failed!!";
        //            }

        //            finally
        //            {
        //                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //                {
        //                    mySqlConnection.Close();
        //                }

        //            }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button2_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload2.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload2.SaveAs(folderPath + Path.GetFileName(FileUpload2.FileName));
        //            newFileName = strAppID + "_" + "2_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload2.FileName;
        //            System.IO.File.Move(folderPath + FileUpload2.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button3_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload3.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload3.SaveAs(folderPath + Path.GetFileName(FileUpload3.FileName));
        //            newFileName = strAppID + "_" + "3_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload3.FileName;
        //            System.IO.File.Move(folderPath + FileUpload3.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button4_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload4.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload4.SaveAs(folderPath + Path.GetFileName(FileUpload4.FileName));
        //            newFileName = strAppID + "_" + "4_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload4.FileName;
        //            System.IO.File.Move(folderPath + FileUpload4.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button5_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload5.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload5.SaveAs(folderPath + Path.GetFileName(FileUpload5.FileName));
        //            newFileName = strAppID + "_" + "5_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload5.FileName;
        //            System.IO.File.Move(folderPath + FileUpload5.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button6_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload6.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload6.SaveAs(folderPath + Path.GetFileName(FileUpload6.FileName));
        //            newFileName = strAppID + "_" + "6_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload6.FileName;
        //            System.IO.File.Move(folderPath + FileUpload6.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button7_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload7.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload7.SaveAs(folderPath + Path.GetFileName(FileUpload7.FileName));
        //            newFileName = strAppID + "_" + "7_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload7.FileName;
        //            System.IO.File.Move(folderPath + FileUpload7.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button8_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload8.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload8.SaveAs(folderPath + Path.GetFileName(FileUpload8.FileName));
        //            newFileName = strAppID + "_" + "8_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload8.FileName;
        //            System.IO.File.Move(folderPath + FileUpload8.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button9_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload9.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload9.SaveAs(folderPath + Path.GetFileName(FileUpload9.FileName));
        //            newFileName = strAppID + "_" + "9_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload9.FileName;
        //            System.IO.File.Move(folderPath + FileUpload9.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        //protected void Button10_Click(object sender, EventArgs e)
        //{
        //    string strAppID = txtAppNo.Text;
        //    string strMEDAID = txtProjCode.Text;
        //    string folderPath = Server.MapPath("~/Files/FinalGC/" + strAppID + "/");
        //    string newFileName = "";

        //    //Check whether Directory (Folder) exists.
        //    if (!Directory.Exists(folderPath))
        //    {
        //        //If Directory (Folder) does not exists Create it.
        //        Directory.CreateDirectory(folderPath);
        //    }
        //    if (FileUpload10.HasFile)
        //    {
        //        MySqlConnection mySqlConnection = new MySqlConnection();
        //        //Save the File to the Directory (Folder).
        //        try
        //        {

        //            FileUpload10.SaveAs(folderPath + Path.GetFileName(FileUpload10.FileName));
        //            newFileName = strAppID + "_" + "10_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FileUpload10.FileName;
        //            System.IO.File.Move(folderPath + FileUpload10.FileName, folderPath + newFileName);

        //            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //            mySqlConnection.Open();


        //            switch (mySqlConnection.State)
        //            {

        //                case System.Data.ConnectionState.Open:
        //                    string strQuery = "insert into FinalGridConDocs(APPLICATION_NO,MEDAProjectID,FileName) values('" + strAppID + "','" + strMEDAID + "','" + newFileName + "')";
        //                    MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                    cmd.ExecuteNonQuery();
        //                    //lblResult.Text = "Application form uploaded Successfully!!";
        //                    ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", "<script type = 'text/javascript'>alert('File Uploaded.')</script>");
        //                    break;

        //                case System.Data.ConnectionState.Closed:

        //                    // Connection could not be made, throw an error

        //                    throw new Exception("The database connection state is Closed");

        //                    break;

        //                default:
        //                    break;

        //            }
        //        }
        //        catch (MySql.Data.MySqlClient.MySqlException mySqlException)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";

        //        }
        //        catch (Exception exception)
        //        {
        //            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
        //            log.Error(ErrorMessage);
        //            lblResult.Text = "Letter Uploaded Failed!!";
        //        }

        //        finally
        //        {
        //            if (mySqlConnection.State != System.Data.ConnectionState.Closed)
        //            {
        //                mySqlConnection.Close();
        //            }

        //        }





        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}
        #endregion
    }
}