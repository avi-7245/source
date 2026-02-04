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
using iTextSharp.text.pdf;
using System.Web.UI.DataVisualization.Charting;
using iTextSharp.text;


namespace GGC.UI.Emp
{
    public partial class ViewFullDocs : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ViewFullDocs));
        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            //int roleId = int.Parse(Session["EmpRole"].ToString());
            //if (roleId > 50 && roleId < 56)
            //{
            //    mgtHome.Visible = true;
            //    empHome.Visible = false;
            //}
            //if (roleId > 1 && roleId < 20)
            //{
            //    empHome.Visible = true;
            //    mgtHome.Visible = false;
            //}
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["application"] != null)
                {
                    string ID = Request.QueryString["application"].ToString();
                    lblAppNo.Text = "Application No : " + ID;
                    Session["APPID"] = ID;
                    getDocument(ID, 1);
                }
            }
        }

        //protected string getDocument(string applicationId, int doctype)
        //{
        //    string strFileName = string.Empty;
        //    string strQuery = string.Empty;
        //    string strFolderName = string.Empty;

        //    MySqlConnection mySqlConnection = new MySqlConnection();
        //    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        //    try
        //    {

        //        mySqlConnection.Open();


        //        switch (mySqlConnection.State)
        //        {

        //            case System.Data.ConnectionState.Open:
        //                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                switch (doctype)
        //                {
        //                    case 1:
        //                        strQuery = "select AppFormDocName as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
        //                        strFolderName = "AppForm";
        //                        break;
        //                    case 2:
        //                        strQuery = "select SLDDocName, otherdocname, docSLDOther3, docSLDOther4, docSLDOther5,docSLDOther6,docSLDOther7,docSLDOther8,docSLDOther9,docSLDOther10 as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
        //                        strFolderName = "SLD";
        //                        break;
        //                    case 3:
        //                        strQuery = "select docTech as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
        //                        strFolderName = "Tech";
        //                        break;
        //                    case 4:
        //                        strQuery = "select docLFS as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
        //                        strFolderName = "LFS";
        //                        break;
        //                    case 5:
        //                        strQuery = "select docLFSOther as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
        //                        strFolderName = "LFSOther";
        //                        break;
        //                    case 6:
        //                        strQuery = "select IssueLetter as docname,WF_STATUS_CD_C from applicantdetails where APPLICATION_NO='" + applicationId + "'";
        //                        strFolderName = "IssueLetter";
        //                        break;
        //                }
        //                cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                DataSet dsResult = new DataSet();
        //                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
        //                da.Fill(dsResult);
        //                if (dsResult.Tables[0].Rows.Count > 0)
        //                {
        //                    if (dsResult.Tables[0].Rows[0][0].ToString() != "")
        //                    {
        //                        if (dsResult.Tables[0].Rows[0]["WF_STATUS_CD_C"].ToString() == "19")
        //                        {
        //                            btnGCLetter.Visible = true;
        //                        }
        //                        strFileName = dsResult.Tables[0].Rows[0]["docname"].ToString();
        //                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"1000px\" height=\"500px\">";
        //                        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
        //                        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
        //                        embed += "</object>";
        //                        //ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/" + strFolderName + "/" + strFileName));
        //                        embed += string.Format(embed, ResolveUrl("~/Files/" + strFolderName + "/" + strFileName));
        //                    }
        //                    else
        //                    {
        //                        Response.Write("<script language='javascript'>alert('No document found.');</script>");
        //                    }
        //                }

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
        //    return strFileName;
        //}
        protected string getDocument(string applicationId, int doctype)
        {
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strFolderName = string.Empty;
            string allDocuments = string.Empty;

            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            try
            {
                mySqlConnection.Open();

                if (mySqlConnection.State == System.Data.ConnectionState.Open)
                {
                    // Set the query and folder name based on the document type
                    switch (doctype)
                    {
                        case 1:
                            strQuery = "SELECT AppFormDocName AS docname, WF_STATUS_CD_C FROM applicantdetails WHERE APPLICATION_NO=@applicationId";
                            strFolderName = "AppForm";
                            break;
                        case 2:
                            strQuery = "SELECT SLDDocName, otherdocname, docSLDOther3, docSLDOther4, docSLDOther5, docSLDOther6, docSLDOther7, docSLDOther8, docSLDOther9, docSLDOther10, WF_STATUS_CD_C FROM applicantdetails WHERE APPLICATION_NO=@applicationId";
                            strFolderName = "Other";
                            break;
                        case 3:
                            strQuery = "SELECT docTech AS docname, WF_STATUS_CD_C FROM applicantdetails WHERE APPLICATION_NO=@applicationId";
                            strFolderName = "Tech";
                            break;
                        case 4:
                            strQuery = "SELECT docLFS AS docname, WF_STATUS_CD_C FROM applicantdetails WHERE APPLICATION_NO=@applicationId";
                            strFolderName = "LFS";
                            break;
                        case 5:
                            strQuery = "SELECT docLFSOther AS docname, WF_STATUS_CD_C FROM applicantdetails WHERE APPLICATION_NO=@applicationId";
                            strFolderName = "LFSOther";
                            break;
                        case 6:
                            strQuery = "SELECT IssueLetter AS docname, WF_STATUS_CD_C FROM applicantdetails WHERE APPLICATION_NO=@applicationId";
                            strFolderName = "IssueLetter";
                            break;
                        default:
                            throw new ArgumentException("Invalid document type.");
                    }

                    // Create the command and add the parameter
                    using (MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection))
                    {
                        cmd.Parameters.AddWithValue("@applicationId", applicationId);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dsResult.Tables[0].Rows)
                            {
                                // Iterate over all document columns for this row
                                foreach (DataColumn col in dsResult.Tables[0].Columns)
                                {
                                    // Skip the WF_STATUS_CD_C column
                                    if (col.ColumnName == "WF_STATUS_CD_C") continue;

                                    string documentName = row[col].ToString();
                                    if (!string.IsNullOrEmpty(documentName))
                                    {
                                        if (row["WF_STATUS_CD_C"].ToString() == "19")
                                        {
                                            btnGCLetter.Visible = true;
                                        }

                                        string documentPath = ResolveUrl("~/Files/" + strFolderName + "/" + documentName);
                                        string fullPath = Server.MapPath(documentPath);

                                        if (!File.Exists(fullPath))
                                        {
                                            Response.Write("<script language='javascript'>alert('Document not found: " + documentName + "');</script>");
                                            continue;
                                        }

                                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"1000px\" height=\"500px\">";
                                        embed += "If you are unable to view the file, you can download it from <a href=\"{0}\">here</a>.";
                                        embed += " Or download <a target=\"_blank\" href=\"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                                        embed += "</object>";

                                        allDocuments += string.Format(embed, documentPath) + "<br />";
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(allDocuments))
                            {
                                Response.Write("<script language='javascript'>alert('No document found.');</script>");
                            }
                            else
                            {
                                ltEmbed.Text = allDocuments; // Display all documents
                            }
                        }
                        else
                        {
                            Response.Write("<script language='javascript'>alert('No documents found for the given application ID.');</script>");
                        }
                    }
                }
                else
                {
                    throw new Exception("The database connection state is Closed");
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException mySqlException)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                log.Error(ErrorMessage);
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
            }
            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }
            }

            return strFileName;
        }






        protected void btnApplication_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 1);
        }
        protected void btnSLD_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 2);
        }

        protected void btnTech_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 3);
        }

        protected void btnLFS_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 4);
        }

        protected void btnLFSOthers_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 5);
        }

        protected void btnGCLetter_Click(object sender, EventArgs e)
        {
            getDocument(Session["APPID"].ToString(), 6);
        }
    }
}