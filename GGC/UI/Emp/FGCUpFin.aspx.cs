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
using GGC.Common;
using System.Text;
using System.Xml;
using GGC.Scheduler;

namespace GGC.UI.Emp
{
    public partial class FGCUpFin : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FGCUpFin));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
                strAppID = Request.QueryString["application"];
                //GeneratXmlForFile();
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);

            }
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/MSKVY/FGC/" + strAppID + "/");
            //string strAppID = Session["APPID"].ToString();
            string newFileName = "";
           
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUDoc.HasFile)
                {
                    try
                    {

                        FUDoc.SaveAs(folderPath + Path.GetFileName(FUDoc.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUDoc.FileName;
                        System.IO.File.Move(folderPath + FUDoc.FileName, folderPath + newFileName);

                        string strQuery = string.Empty;
                        
                        strQuery = "Update finalgcapproval set WF_STATUS_CD=19, APP_STATUS_DT=CURDATE() , app_status='Final GC uploaded.' where APPLICATION_NO='" + strAppID + "'";
                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                        
                        strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + strAppID + "', 16, '" + newFileName + "',  '" + strAppID + "')";
                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                        objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "19");
           
                        lblResult.Text = "Final GC letter uploaded successfully!!";

                        lblResult.ForeColor = System.Drawing.Color.Green;

                        
                        #region Application Status tracking date
                        strQuery = "insert into APPLICANT_STATUS_TRACKING (APPLICATION_NO,STATUS,STATUS_DT,Created_By)" +
                                " values('" + strAppID + "','FGC letter uploaded.',now(),'" + Session["SAPID"].ToString() + "')";

                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                        #endregion
                        DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from finalgcapproval where application_no='" + strAppID + "'");
                        DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51)");
                        #region Send SMS
                        string strMobNos = string.Empty;
                        strMobNos = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + ",";
                        try
                        {
                            //log.Error("1 ");
                            if (dsEmailMob.Tables[0].Rows.Count > 0)
                            {



                                if (dsEmailMob.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsEmailMob.Tables[0].Rows.Count; i++)
                                    {
                                        if (dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                                            strMobNos += dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                    }

                                }
                            }
                            string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Load%20Flow%20studies%20are%20completed%20for%20your%20application%20no." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".%20The%20proposal%20for%20Grid%20Connectivity%20is%20under%20approval.%20Regards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                            //log.Error("strURL " + strURL);

                            WebRequest request = HttpWebRequest.Create(strURL);
                            //log.Error("2 ");
                            // Get the response back  
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            Stream s = (Stream)response.GetResponseStream();
                            StreamReader readStream = new StreamReader(s);
                            string dataString = readStream.ReadToEnd();
                            log.Error("8 " + dataString);

                            response.Close();
                            s.Close();
                            readStream.Close();
                        }
                        catch (Exception ex)
                        {
                            string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                            log.Error(ErrorMessage);
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                        log.Error(ErrorMessage);
                    }



                }
            
                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            
        }
        }
    
}