using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using log4net;
using System.Reflection;
using System.Globalization;
using Newtonsoft.Json;
using GGC.Common;
using GGC.WebService;
using GGC.Scheduler;

namespace GGC.UI.Cons
{
    public partial class UploadReDoc : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadReDoc));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                

                if (Request.QueryString["appid"] != null)
                {

                    Session["APPID"] = Request.QueryString["appid"].ToString();

                    DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from applicantdetails where application_no='" + Session["APPID"].ToString() + "'");
                    DateTime dtStatus = DateTime.Parse(dsResult.Tables[0].Rows[0]["APP_STATUS_DT"].ToString());
                    string strUserID = dsResult.Tables[0].Rows[0]["User_Name"].ToString();
                    double days = (DateTime.Now - dtStatus).TotalDays;
                    if (days >= 10)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Link expired!!.');window.location ='Home.aspx?usrName="+strUserID+"';", true);
                    }
                }
            }
        }

        protected void ValidateMEDAFileSize(object sender, ServerValidateEventArgs e)
        {
            //System.Drawing.Image img = System.Drawing.Image.FromStream(FUMEDADoc.PostedFile.InputStream);
            //int height = img.Height;
            //int width = img.Width;
            decimal sizeFUDoc1 = 0;
            sizeFUDoc1 = Math.Round(((decimal)FUSLD.PostedFile.ContentLength / (decimal)1024), 2);

            if (Session["IsValid"] != "false")
            {
                e.IsValid = true;
                Session["IsValid"] = "true";
            }


        }
        protected void btnUploadSLD_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/SLD/");
            string strAppID = Session["APPID"].ToString();
            string newFileName = "";
            if (Session["IsValid"] != "false")
            {
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUSLD.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        FUSLD.SaveAs(folderPath + Path.GetFileName(FUSLD.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUSLD.FileName;
                        System.IO.File.Move(folderPath + FUSLD.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=7, SLDDocName='" + newFileName + "' , app_status='DOCUMENT RE-UPLOADED' where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblSLD.Text = "SLD Uploaded Successfully!!";
                                lblSLD.ForeColor = System.Drawing.Color.Green;
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
                        lblSLD.Text = "SLD Uploaded Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblSLD.Text = "SLD Uploaded Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }

                //saveApp(Session["ProjectID"].ToString());

                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }

        protected void btnOther_Click(object sender, EventArgs e)
        {

        }

        protected void Button3_Click(object sender, EventArgs e)
        {

        }

        protected void Button4_Click(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }
    }
}