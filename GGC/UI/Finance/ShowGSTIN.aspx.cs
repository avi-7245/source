using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GGC.UI.Finance
{
    public partial class ShowGSTIN : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                if (Request.QueryString["GSTIN"] != null)
                {
                    string ID = Request.QueryString["GSTIN"].ToString();
                    string letterName = Request.QueryString["docName"].ToString();
                    string strPan = Request.QueryString["PAN"].ToString();
                    Session["APPID"] = ID;
                    string embed = "<object data=\"{0}\" type=\"application/pdf\" width=\"800px\" height=\"400px\">";
                    embed += "If you are unable to view file, you can download from <a href = \"{0}\">here</a>";
                    embed += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
                    embed += "</object>";
                    ltEmbed.Text = string.Format(embed, ResolveUrl("~/Files/GSTIN/" + letterName));
                    //imgMedaLetter.ImageUrl = "~/Files/MEDA/" + letterName;
                    lblAppNo.Text += strPan;
                }
            }
        }
    }
}