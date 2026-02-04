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

namespace GGC.UI.MSKVYSPD
{
    public partial class ViewDoc : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ViewDoc));
        int filetype = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session["filetype"] = filetype;

                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    lblAppNo.Text = ID;
                    Session["APPID"] = ID;
                    getDocument(ID, filetype);
                }
            }

        }

        protected string getDocument(string applicationId, int doctype)
        {
            string strFileName = string.Empty;
            string strQuery = string.Empty;
            string strFolderName = string.Empty;


            try
            {




                strQuery = "select a.*,b.* from mskvy_upload_doc_spd a,mskvy_doc_list_spd b where b.doc_seq=a.filetype and APPLICATION_NO='" + applicationId + "' and FileType=" + doctype + " order by CreateDT ";

                DataSet dsResult = new DataSet();

                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    if (dsResult.Tables[0].Rows[0][0].ToString() != "")
                    {
                        strFileName = dsResult.Tables[0].Rows[0]["fiename"].ToString();
                        string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                        embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                        embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                        embed += "</object>";
                        ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/MSKVY/" + applicationId + "/" + strFileName));
                    }
                    else
                    {
                        Response.Write("<script language='javascript'>alert('No document found.');</script>");
                    }
                }

            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors

            }


            return strFileName;
        }

        protected void btnViewDocNext_Click(object sender, EventArgs e)
        {
            filetype = int.Parse(Session["filetype"].ToString());
            if (filetype == 7)
            {
                filetype = 1;
            }
            else
            {
                filetype++;
            }
            getDocument(Session["APPID"].ToString(), filetype);
            Session["filetype"] = filetype;
        }

        protected void btnViewDocPrev_Click(object sender, EventArgs e)
        {
            filetype = int.Parse(Session["filetype"].ToString());
            if (filetype == 1)
            {
                filetype = 7;
            }
            else
            {
                filetype--;
            }
            getDocument(Session["APPID"].ToString(), filetype);
            Session["filetype"] = filetype;
        }
    }
}