using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Ionic.Zip;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using log4net;
using System.Reflection;
using GGC.Common;
using iTextSharp.tool.xml.util;

namespace GGC.UI.Emp
{
    public partial class FinalGC : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FinalGC));
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
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //string strGSTIN = string.Empty;
            //if (Session["GSTIN"] != null)
            //    strGSTIN = Session["GSTIN"].ToString();

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State) 
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        //strQuery = "select a.*,b.*  from APPLICANTDETAILS a,finalgcapproval b  where a.APPLICATION_NO=b.APPLICATION_NO and b.roleid='" + Session["EmpRole"].ToString() + "'  and (isAppr_Rej_Ret is null or isAppr_Rej_Ret='N')";
                        strQuery = "select a.*,b.*  from APPLICANTDETAILS a,finalgcapproval b  where a.APPLICATION_NO=b.APPLICATION_NO and b.roleid='" + Session["EmpRole"].ToString() + "' order by createdt desc";
                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        log.Error(strQuery);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        Session["AppDet"] = dsResult;
                        log.Error("dsResult.Tables[0].Rows.Count " + dsResult.Tables[0].Rows.Count.ToString());
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
                string ErrorMessage = "Sql ExceptionMethod Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
                // Use the mySqlException object to handle specific MySql errors
                //// lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "FilData Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
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
        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownAllDoc")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                string applicationId = row.Cells[0].Text;

                using (ZipFile zip = new ZipFile())
                {
                    zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                    zip.AddDirectoryByName(applicationId);
                    string folderPath = Server.MapPath("~/Files/FinalGC/" + applicationId + "/");
                    //string filePath = (row.FindControl("lblFilePath") as Label).Text;
                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Files/FinalGC/" + applicationId + "/"));
                    //List<ListItem> files = new List<ListItem>();
                    //foreach (string filePath in filePaths)
                    //{
                    //    files.Add(new ListItem(Path.GetFileName(filePath), filePath));
                    //}

                    foreach (string filePath in filePaths)
                    {

                        //filePath = (row.FindControl("lblFilePath") as Label).Text;
                        zip.AddFile(filePath, applicationId);

                    }
                    //zip.AddDirectoryByName(folderPath);

                    Response.Clear();
                    Response.BufferOutput = false;
                    string zipName = String.Format("Zip_{0}.zip", DateTime.Now.ToString("yyyy-MMM-dd-HHmmss"));
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "attachment; filename=" + zipName);
                    zip.Save(Response.OutputStream);
                    Response.End();
                }

            }
            if (e.CommandName == "ViewDocs")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;

                Session["APPID"] = row.Cells[0].Text;
                Session["MEDAID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FinalGCDoc.aspx?appid=" + applicationId, false);
            }
            if (e.CommandName == "UploadDraft")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;

                Session["APPID"] = row.Cells[0].Text;
                Session["MEDAID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FinalGCDraftUpload.aspx?appid=" + applicationId, false);
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

                Session["APPID"] = row.Cells[0].Text;
                Session["MEDAID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/FinalGCUpload.aspx?appid=" + applicationId, false);
            }
        }

        protected void GVApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                string strisAppr_Rej_Ret = rowView["isAppr_Rej_Ret"].ToString();
                bool? isApproved = string.IsNullOrEmpty(strisAppr_Rej_Ret) ? (bool?)null : strisAppr_Rej_Ret == "Y";
                int status = rowView["WF_STATUS_CD_C"].ToInt();
                Button btnUploadDraft = e.Row.FindControl("btnUploadDraft") as Button;
                Button btnUploadLetter = e.Row.FindControl("btnUploadLetter") as Button;

                if (status == 27 && isApproved == true)
                {
                    btnUploadDraft.BackColor = System.Drawing.Color.Red;
                    btnUploadLetter.BackColor = System.Drawing.Color.Blue;

                    btnUploadDraft.Enabled = false;
                    btnUploadLetter.Enabled = true;

                }

            }
        }
    }
}