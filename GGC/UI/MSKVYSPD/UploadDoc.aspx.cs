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
using System.IO;
using GGC.Scheduler;

namespace GGC.UI.MSKVYSPD
{
    public partial class UploadDoc : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadDoc));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillData();
            }
        }
        void fillData()
        {
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_doc_list_spd where doc_type='SPD' and srno in (1,2)");
                GVApplications.DataSource = dsResult.Tables[0];
                GVApplications.DataBind();
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);

            }
        }
        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UploadDocs")
            {

                string strAppID = Session["APPID"].ToString();
                //string strAppID = "M20700250000061";
                string newFileName = "";
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string strQuery = string.Empty;
                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                FileUpload FUUpload = (FileUpload)row.Cells[2].FindControl("FUUpload");
                Label lblUploadStatus = (Label)row.Cells[2].FindControl("lblUploadStatus");
                string folderPath = Server.MapPath("~/Files/MSKVY/" + strAppID + "/");

                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUUpload.HasFile)
                {
                    try
                    {
                        int srno = int.Parse(row.Cells[0].Text);
                        FUUpload.SaveAs(folderPath + Path.GetFileName(FUUpload.FileName));
                        newFileName = srno + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUUpload.FileName;
                        System.IO.File.Move(folderPath + FUUpload.FileName, folderPath + newFileName);
                        DataSet dsCheck = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_upload_doc_spd where Application_No='" + strAppID + "' and FileType='" + (srno + 3) + "'");
                        if (dsCheck.Tables[0].Rows.Count > 0)
                        {
                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "delete from mskvy_upload_doc_spd where Application_No='" + strAppID + "' and FileType='" + (srno + 3) + "'");

                            strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + strAppID + "', '" + (srno + 3) + "', '" + newFileName + "',  '" + strAppID + "')";
                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                            //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, "where Application_No='" + strAppID + "' and FileType='" + (srno + 3) + "'");

                            //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_upload_doc_spd where Application_No='" + strAppID + "' and FileType='" + (srno + 3) + "'");
                            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"select * from mskvy_applicantdetails_spd where Application_No='{strAppID}'");
                            //NeedToAsk

                            if (dsResult.Tables[0].Rows[0]["isAppApproved"].ToString() == "N")
                            {
                                strQuery = "update mskvy_applicantdetails_spd set isAppApproved='Y',app_status='Application Updated by Developer.' where Application_no='" + Session["APPID"].ToString() + "'";
                                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                                SendEmail sm = new SendEmail();
                                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from mskvy_applicantdetails where application_no =" + Session["AppId"].ToString());

                                string strTo = ConfigurationManager.AppSettings["reminderCCEmail"].ToString();
                                string strCC = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();


                                string strBody = "Dear STU Section<br/>";
                                strBody += "MSKVY application from M/s." + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " has updated the details on MSKVY Grid connectivity application. <br/>";


                                strBody += "<br/>";
                                strBody += "<br/>";
                                strBody += "<br/>";
                                strBody += "Thanks & Regards, " + "<br/>";
                                strBody += "Chief Engineer / STU Department" + "<br/>";
                                strBody += "MSETCL  " + "<br/>";
                                strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";
                                sm.Send(strTo, strCC, "MSKVY application Updated.", strBody);
                                //RemoveComment

                                //Response.Redirect("~/UI/MSKVYSPD/AppHome.aspx", false);
                            }
                            else
                            {
                                //Response.Redirect("~/UI/MSKVY/PayRegConfirm.aspx?appID=" + Session["APPID"].ToString(), false);
                            }
                        }
                        else
                        {
                            strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + strAppID + "', '" + (srno + 3) + "', '" + newFileName + "',  '" + strAppID + "')";
                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                            //RemoveComment
                        }

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        // Use the exception object to handle all other non-MySql specific errors
                        //   lblResult.Text = "Some problem during registration.Please try again.";
                    }
                }


                Button btnUpload = (Button)row.Cells[2].FindControl("btnUpload");
                btnUpload.Enabled = false;
                btnUpload.Text = "Uploaded";
                btnUpload.BackColor = System.Drawing.Color.Green;
                lblUploadStatus.Text = "File Uploaded";
                FUUpload.Enabled = false;
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strAppID = Session["APPID"].ToString();
            // string strAppID = "M20700250000061";

            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"select * from mskvy_applicantdetails_spd where Application_No='{strAppID}'");
                string isAppApproved = dsResult.Tables[0].Rows[0]["isAppApproved"].ToString();

                if (string.IsNullOrEmpty(isAppApproved) || isAppApproved == "N")
                {
                    string strQuery = "update mskvy_applicantdetails_spd set WF_STATUS_CD_C=3, app_status='DOCUMENT UPLOADED.', APP_STATUS_DT=now() WHERE APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    Response.Redirect("~/UI/MSKVYSPD/PayRegConfirm.aspx?appID=" + strAppID, false);
                }
                else
                {
                    Response.Redirect("~/UI/MSKVYSPD/APPHome.aspx?appID=" + strAppID, false);
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                //   lblResult.Text = "Some problem during registration.Please try again.";
            }
        }
    }
}