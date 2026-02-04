using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GGC.UI.Emp
{
    public partial class ViewFinalGCDoc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //{
            //    string filePath = Request.QueryString["fileName"];
            //    Response.ContentType = "Application/pdf";
            //    Response.WriteFile(filePath);
            //    Response.End();
            //}
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    string docName = Request.QueryString["docname"].ToString();
                    Session["APPID"] = ID;



                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"500px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/FinalGC/" + ID + "/" + docName));
                    //imgMedaLetter.ImageUrl = "~/Files/MEDA/" + letterName;
                    //lblAppNo.Text = ID;

                    lblAppNo.Text += Session["APPID"].ToString();

                    if (Session["PROJID"] != null)
                    {
                        lblAppNo.Text += " And Project ID : " + Session["PROJID"].ToString();
                    }
                }
            }
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/UI/Emp/FinalGCDoc.aspx?appid=" + Session["APPID"].ToString(), false);
        }
    }
}