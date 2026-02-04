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

namespace GGC.UI.FinalGC
{
    public partial class DocUpload : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(DocUpload));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string strAppID = string.Empty;
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
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from fgc_doc_list_master where doc_type='FGC_DOC'");
                //DataSet dsUploadedDoc = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from fgc_doc_list_master where doc_type='FGC_DOC'");
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

            string strAppID = Session["strAPPID"].ToString();
            //RemoveComment
            //string strAppID = "M20700250000061";
            //string strAppID = "MSLR20234870004";
            string relativePath = $"~/Files/MSKVY/FGC/{strAppID}/";
            string folderPath = Server.MapPath(relativePath);
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = GVApplications.Rows[rowIndex];
            HiddenField hfFileName = (HiddenField)row.FindControl("HFFileName");

            if (e.CommandName == "UploadDocs")
            {

                Button btnUpload = (Button)row.Cells[2].FindControl("btnUpload");
                Button btnViewDocument = (Button)row.Cells[2].FindControl("btnViewDocument");
                FileUpload FUUpload = (FileUpload)row.Cells[2].FindControl("FUUpload");
                Label lblUploadStatus = (Label)row.Cells[3].FindControl("lblUploadStatus");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                if (FUUpload.HasFile)
                {
                    try
                    {
                        string srno = row.Cells[1].Text;
                        FUUpload.SaveAs(folderPath + Path.GetFileName(FUUpload.FileName));
                        string newFileName = srno + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUUpload.FileName;
                        System.IO.File.Move(folderPath + FUUpload.FileName, folderPath + newFileName);


                        string strQuery;
                        if (Convert.ToBoolean(SQLHelper.ExecuteScalar(conString, CommandType.Text, $"SELECT 1 FROM fgc_app_doc fad WHERE fad.application_no='{strAppID}' AND fad.DOC_TYPE='{srno}' LIMIT 1;")))
                        {
                            strQuery = $"UPDATE fgc_app_doc fad SET fad.File_Name = '{newFileName}', fad.CREATE_BY = '{strAppID}' WHERE fad.application_no='{strAppID}' AND fad.DOC_TYPE = '{srno}'";
                        }
                        else
                        {
                            strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + strAppID + "', '" + srno + "', '" + newFileName + "',  '" + strAppID + "')";
                        }
                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                        hfFileName.Value = newFileName;

                        if (!btnViewDocument.Enabled)
                        {
                            btnViewDocument.Enabled = true;
                            btnViewDocument.CssClass = "button";
                        }
                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        // Use the exception object to handle all other non-MySql specific errors
                        //   lblResult.Text = "Some problem during registration.Please try again.";
                        lblUploadStatus.Text = "Document upload failed";
                    }

                    btnUpload.Text = "Re Upload";
                    btnUpload.BackColor = System.Drawing.Color.Green;
                    lblUploadStatus.Text = "File Uploaded";
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "RequiredFile", "alert('File Required File!')", true);
                }
            }
            else if (e.CommandName == "ViewUploadedDocs")
            {
                if (File.Exists(folderPath + hfFileName.Value))
                {
                    htmlLiteral.Text = $"<object data='{ResolveUrl($"{relativePath}{hfFileName.Value}")}' type='application/pdf'  width='100%' height='100%'></object>";
                    lblSelectedDocName.Text = row.Cells[2].Text;
                    ClientScript.RegisterStartupScript(this.GetType(), "ViewUploadedDocs", $"ViewUploadedDocModal();", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "AlertFileNotExists", "alert('File Not Found!')", true);
                }
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strAppID = Session["strAPPID"].ToString();
            // string strAppID = "M20700250000061";
            //Remove Comment

            try
            {
                string strQuery = "update finalgcapproval set WF_STATUS_CD=4, app_status='DOCUMENTS UPLOADED.', APP_STATUS_DT=now() WHERE APPLICATION_NO='" + strAppID + "'";
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                lblResult.Text = "Final submit done.";

                var needToDisable = GVApplications.Rows.Cast<GridViewRow>().Select(row => new { FileUpload = (FileUpload)row.Cells[4].FindControl("FUUpload"), Button = (Button)row.Cells[4].FindControl("btnUpload") }).ToList();

                needToDisable.ForEach(c =>
                {
                    c.FileUpload.Enabled = false;
                    c.Button.Enabled = false;
                    c.Button.BackColor = System.Drawing.ColorTranslator.FromHtml("#6e8a99");
                    c.Button.Style["cursor"] = "not-allowed";
                });

                btnUpload.Enabled = false;
                btnUpload.BackColor = System.Drawing.ColorTranslator.FromHtml("#6e8a99");
                btnUpload.Style["cursor"] = "not-allowed";


                //Response.Redirect("~/UI/MSKVYSPD/PayRegConfirm.aspx?appID=" + strAppID, false);
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Problem during final submit.Please try again.";
            }

        }
    }
}