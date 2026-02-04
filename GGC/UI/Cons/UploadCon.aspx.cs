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
using System.Reflection.Emit;
using System.Data.SqlClient;
using MySqlX.XDevAPI.Relational;
using AjaxControlToolkit.Config;
using Mysqlx.Resultset;

namespace GGC.UI.Cons
{
    public partial class UploadCon : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadCon));
        int visiblecount = 0;
        int count = 1;
        List<string> labelsToCheck = new List<string>();
        MySqlConnection mySqlConnection = new MySqlConnection();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //getDocDetails();
                //row1.Visible = true;
                //row2.Visible = true;
                //row3.Visible = true;
                //row4.Visible = true;
                //row5.Visible = true;
                //row6.Visible = true;
                //row7.Visible = true;
                //row8.Visible = true;
                //row9.Visible = true;
                ////row10.Visible = true;
                labelsToCheck.Add(lblSLD.ID.ToString());
                
                try
                {
                    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                    mySqlConnection.Open();
                    string query = "SELECT * FROM new_terms_condition WHERE id = (SELECT MAX(id) FROM new_terms_condition) LIMIT 1;";
                    MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                    DataSet dsResult = new DataSet();
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dsResult);

                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["notaff"].ToString()) == 1)
                        {
                            row1.Visible = true;
                            count++;
                            labelsToCheck.Add(lblOther.ID.ToString());
                        }
                        else
                        {
                            row1.Visible = false;
                        }
                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["boardresul"].ToString()) == 1)
                        {
                            row2.Visible = true;
                            labelsToCheck.Add(lblOther3.ID.ToString());
                            count++;
                        }
                        else
                        {
                            row2.Visible = false;
                        }

                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["momnaoa"].ToString()) == 1)
                        {
                            row3.Visible = true;
                            labelsToCheck.Add(lblOther4.ID.ToString());
                            count++;
                        }
                        else
                        {
                            row3.Visible = false;
                        }
                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["govtAuthorization"].ToString()) == 1)
                        {
                            row4.Visible = true;
                            labelsToCheck.Add(Label4.ID.ToString());
                            count++;
                        }
                        else
                        {
                            row4.Visible = false;
                        }
                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["proofOfOwnership"].ToString()) == 1)
                        {
                            row5.Visible = true;
                            labelsToCheck.Add(Label6.ID.ToString());
                            count++;
                        }
                        else
                        {
                            row5.Visible = false;
                        }

                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["bankGuarantee"].ToString()) == 1)
                        {
                            row6.Visible = true;
                            labelsToCheck.Add(Label8.ID.ToString());
                            count++;
                        }
                        else
                        {
                            row6.Visible = false;
                        }

                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["loAorPPA"].ToString()) == 1)
                        {
                            row7.Visible = true;
                            labelsToCheck.Add(Label10.ID.ToString());
                            count++;

                        }
                        else
                        {
                            row7.Visible = false;
                        }

                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["reProofoOwn"].ToString()) == 1)
                        {
                            row8.Visible = true;
                            labelsToCheck.Add(Label12.ID.ToString());
                            count++;

                        }
                        else
                        {
                            row8.Visible = false;
                        }

                        if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["reBankGuarantee"].ToString()) == 1)
                        {
                            row9.Visible = true;
                            labelsToCheck.Add(Label14.ID.ToString());
                            count++;
                        }
                        else
                        {
                            row9.Visible = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                }
                finally
                {
                    mySqlConnection.Close();
                }
            }
            try
            {
                labelsToCheck.Add(lblSLD.ID.ToString());
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection.Open();
                string query = "SELECT * FROM new_terms_condition WHERE id = (SELECT MAX(id) FROM new_terms_condition) LIMIT 1;";
                MySqlCommand cmd = new MySqlCommand(query, mySqlConnection);
                DataSet dsResult = new DataSet();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dsResult);

                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["notaff"].ToString()) == 1)
                    {
                        row1.Visible = true;
                        count++;
                        labelsToCheck.Add(lblOther.ID.ToString());
                    }
                    else
                    {
                        row1.Visible = false;
                    }
                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["boardresul"].ToString()) == 1)
                    {
                        row2.Visible = true;
                        labelsToCheck.Add(lblOther3.ID.ToString());
                        count++;
                    }
                    else
                    {
                        row2.Visible = false;
                    }

                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["momnaoa"].ToString()) == 1)
                    {
                        row3.Visible = true;
                        labelsToCheck.Add(lblOther4.ID.ToString());
                        count++;
                    }
                    else
                    {
                        row3.Visible = false;
                    }
                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["govtAuthorization"].ToString()) == 1)
                    {
                        row4.Visible = true;
                        labelsToCheck.Add(Label4.ID.ToString());
                        count++;
                    }
                    else
                    {
                        row4.Visible = false;
                    }
                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["proofOfOwnership"].ToString()) == 1)
                    {
                        row5.Visible = true;
                        labelsToCheck.Add(Label6.ID.ToString());
                        count++;
                    }
                    else
                    {
                        row5.Visible = false;
                    }

                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["bankGuarantee"].ToString()) == 1)
                    {
                        row6.Visible = true;
                        labelsToCheck.Add(Label8.ID.ToString());
                        count++;
                    }
                    else
                    {
                        row6.Visible = false;
                    }

                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["loAorPPA"].ToString()) == 1)
                    {
                        row7.Visible = true;
                        labelsToCheck.Add(Label10.ID.ToString());
                        count++;

                    }
                    else
                    {
                        row7.Visible = false;
                    }

                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["reProofoOwn"].ToString()) == 1)
                    {
                        row8.Visible = true;
                        labelsToCheck.Add(Label12.ID.ToString());
                        count++;

                    }
                    else
                    {
                        row8.Visible = false;
                    }

                    if (Convert.ToInt16(dsResult.Tables[0].Rows[0]["reBankGuarantee"].ToString()) == 1)
                    {
                        row9.Visible = true;
                        labelsToCheck.Add(Label14.ID.ToString());
                        count++;
                    }
                    else
                    {
                        row9.Visible = false;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                mySqlConnection.Close();
            }
        }
        protected void getDocDetails()
        {
            string strAppID =  Session["APPID"].ToString();
            MySqlConnection mySqlConnection = new MySqlConnection();
            //Save the File to the Directory (Folder).
            try
            {
                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                mySqlConnection.Open();
                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = "select count(MEDALettername) MEDALettername, count(SLDDocName) SLDDocName from APPLICANTDETAILS where APPLICATION_NO='" + strAppID + "'";
                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);
                        if (dsResult.Tables[0].Rows[0]["MEDALettername"].ToString() != "0")
                        {
                         //   lblResult.Text = "MEDALetter Document already uploaded";
                        }
                        if (dsResult.Tables[0].Rows[0]["SLDDocName"].ToString() != "0")
                        {
                            lblSLD.Text = "SLD Document already uploaded";
                            lblSLD.ForeColor = System.Drawing.Color.Green;
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
                //lblResult.Text = "Letter Uploaded Failed!!";

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //lblResult.Text = "Letter Uploaded Failed!!";
            }

            finally
            {
                if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                {
                    mySqlConnection.Close();
                }

            }

        }
        
        //protected void UploadMEDALetter(object sender, EventArgs e)
        //{
        //    string folderPath = Server.MapPath("~/Files/MEDA/");
        //    string strAppID = Session["APPID"].ToString();
        //    string newFileName = "";
        //    if (Session["IsValid"] != "false")
        //    {
        //        //Check whether Directory (Folder) exists.
        //        if (!Directory.Exists(folderPath))
        //        {
        //            //If Directory (Folder) does not exists Create it.
        //            Directory.CreateDirectory(folderPath);
        //        }
        //        if (FUMEDADoc.HasFile)
        //        {
        //            MySqlConnection mySqlConnection = new MySqlConnection();
        //            //Save the File to the Directory (Folder).
        //            try
        //            {

        //                FUMEDADoc.SaveAs(folderPath + Path.GetFileName(FUMEDADoc.FileName));
        //                newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUMEDADoc.FileName;
        //                System.IO.File.Move(folderPath + FUMEDADoc.FileName, folderPath + newFileName);

        //                mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



        //                mySqlConnection.Open();


        //                switch (mySqlConnection.State)
        //                {

        //                    case System.Data.ConnectionState.Open:
        //                        string strQuery = "Update APPLICANTDETAILS set MEDALettername='" + newFileName + "' , app_status='DOCUMENT UPLOADED' where APPLICATION_NO='" + strAppID + "'";
        //                        MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
        //                        cmd.ExecuteNonQuery();
        //                        lblResult.Text = "Letter Uploaded Successfully!!";
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

        //        }



        //        //Display the Picture in Image control.
        //        //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
        //    }
        //}

        protected void UploadSLD(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");

            string strAppID =  Session["APPID"].ToString();
           
            

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
                                //string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, SLDDocName='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  SLDDocName='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";

                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblSLD.Text = "Documents uploaded successfully!!";
                                lblSLD.ForeColor = System.Drawing.Color.Green;
                                visiblecount++;
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

                

                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
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
                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.statusId = "3";
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

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        if (Session["apptype"].ToString() == "Consumer")
                        {
                            if (objGetStatusDTO.WF_STATUS_CD_C > 2)
                                strQuery = "update mskvy_applicantdetails set NAME_TRANS_LICENSEE='" + objGetStatusDTO.NameTransLicensee + "'," + "GENERATION_VOLTAGE='" + objGetStatusDTO.GenerationVoltage + "',INJECTION_VOLTAGE='" + objGetStatusDTO.InjectionVoltage + "',POINT_OF_INJECTION='" + objGetStatusDTO.POINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + objGetStatusDTO.DISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + objGetStatusDTO.VOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + objGetStatusDTO.SaleOfPower + "' ,INTER_STATE='" + objGetStatusDTO.InterState + "' ,TOTAL_REQUIRED_LAND='" + objGetStatusDTO.TotalLandRequired + "' ,LAND_IN_POSSESSION='" + objGetStatusDTO.LandInPossession + "' ,TOTAL_PRIVATE_LAND='" + objGetStatusDTO.TotalPrivateLand + "' ,TOTAL_FOREST_LAND='" + objGetStatusDTO.TotalForestLand + "'  ,STATUS_FOREST_LAND='" + objGetStatusDTO.StatusForestLand + "',BIRD_SANCTURY_ETC='" + objGetStatusDTO.BirdSanctuaryEtc + "'  ,PPA_POWER_TOBE_INJECTED ='" + objGetStatusDTO.PPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + objGetStatusDTO.AGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + objGetStatusDTO.StatusFuelLinkage + "'  ,STATUS_OF_WATER_SUPPLY='" + objGetStatusDTO.StatusOfWaterSupply + "' where USER_NAME='" + objGetStatusDTO.userName + "' and MEDAProjectID='" + objGetStatusDTO.MEDAProjectID + "'";
                            else
                            {
                                //if (Session["isAppliedMEDAID"].ToString() == "true")
                                //{
                                //    strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=25,NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "',app_status='APPLICATION RECEIVED' where USER_NAME='" + strUserName + "'";
                                //}
                                //else
                                //{
                                strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=2,NAME_TRANS_LICENSEE='" + objGetStatusDTO.NameTransLicensee + "'," + "GENERATION_VOLTAGE='" + objGetStatusDTO.GenerationVoltage + "',INJECTION_VOLTAGE='" + objGetStatusDTO.InjectionVoltage + "',POINT_OF_INJECTION='" + objGetStatusDTO.POINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + objGetStatusDTO.DISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + objGetStatusDTO.VOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + objGetStatusDTO.SaleOfPower + "' ,INTER_STATE='" + objGetStatusDTO.InterState + "' ,TOTAL_REQUIRED_LAND='" + objGetStatusDTO.TotalLandRequired + "' ,LAND_IN_POSSESSION='" + objGetStatusDTO.LandInPossession + "' ,TOTAL_PRIVATE_LAND='" + objGetStatusDTO.TotalPrivateLand + "' ,TOTAL_FOREST_LAND='" + objGetStatusDTO.TotalForestLand + "'  ,STATUS_FOREST_LAND='" + objGetStatusDTO.StatusForestLand + "',BIRD_SANCTURY_ETC='" + objGetStatusDTO.BirdSanctuaryEtc + "'  ,PPA_POWER_TOBE_INJECTED ='" + objGetStatusDTO.PPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + objGetStatusDTO.AGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + objGetStatusDTO.StatusFuelLinkage + "'  ,STATUS_OF_WATER_SUPPLY='" + objGetStatusDTO.StatusOfWaterSupply + "' where USER_NAME='" + objGetStatusDTO.userName + "' and MEDAProjectID='" + objGetStatusDTO.MEDAProjectID + "'";

                                //}
                            }
                        }
                        else
                        {
                            if (objGetStatusDTO.WF_STATUS_CD_C > 2)
                                strQuery = "update mskvy_applicantdetails set NAME_TRANS_LICENSEE='" + objGetStatusDTO.NameTransLicensee + "'," + "GENERATION_VOLTAGE='" + objGetStatusDTO.GenerationVoltage + "',INJECTION_VOLTAGE='" + objGetStatusDTO.InjectionVoltage + "',POINT_OF_INJECTION='" + objGetStatusDTO.POINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + objGetStatusDTO.DISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + objGetStatusDTO.VOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + objGetStatusDTO.SaleOfPower + "' ,INTER_STATE='" + objGetStatusDTO.InterState + "' ,TOTAL_REQUIRED_LAND='" + objGetStatusDTO.TotalLandRequired + "' ,LAND_IN_POSSESSION='" + objGetStatusDTO.LandInPossession + "' ,TOTAL_PRIVATE_LAND='" + objGetStatusDTO.TotalPrivateLand + "' ,TOTAL_FOREST_LAND='" + objGetStatusDTO.TotalForestLand + "'  ,STATUS_FOREST_LAND='" + objGetStatusDTO.StatusForestLand + "',BIRD_SANCTURY_ETC='" + objGetStatusDTO.BirdSanctuaryEtc + "'  ,PPA_POWER_TOBE_INJECTED ='" + objGetStatusDTO.PPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + objGetStatusDTO.AGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + objGetStatusDTO.StatusFuelLinkage + "'  ,STATUS_OF_WATER_SUPPLY='" + objGetStatusDTO.StatusOfWaterSupply + "' where USER_NAME='" + objGetStatusDTO.userName + "' and MEDAProjectID='" + objGetStatusDTO.MEDAProjectID + "'";
                            else
                            {
                                //if (Session["isAppliedMEDAID"].ToString() == "true")
                                //{
                                //    strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=25,NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "',app_status='APPLICATION RECEIVED' where USER_NAME='" + strUserName + "'";
                                //}
                                //else
                                //{
                                strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=2,NAME_TRANS_LICENSEE='" + objGetStatusDTO.NameTransLicensee + "'," + "GENERATION_VOLTAGE='" + objGetStatusDTO.GenerationVoltage + "',INJECTION_VOLTAGE='" + objGetStatusDTO.InjectionVoltage + "',POINT_OF_INJECTION='" + objGetStatusDTO.POINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + objGetStatusDTO.DISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + objGetStatusDTO.VOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + objGetStatusDTO.SaleOfPower + "' ,INTER_STATE='" + objGetStatusDTO.InterState + "' ,TOTAL_REQUIRED_LAND='" + objGetStatusDTO.TotalLandRequired + "' ,LAND_IN_POSSESSION='" + objGetStatusDTO.LandInPossession + "' ,TOTAL_PRIVATE_LAND='" + objGetStatusDTO.TotalPrivateLand + "' ,TOTAL_FOREST_LAND='" + objGetStatusDTO.TotalForestLand + "'  ,STATUS_FOREST_LAND='" + objGetStatusDTO.StatusForestLand + "',BIRD_SANCTURY_ETC='" + objGetStatusDTO.BirdSanctuaryEtc + "'  ,PPA_POWER_TOBE_INJECTED ='" + objGetStatusDTO.PPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + objGetStatusDTO.AGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + objGetStatusDTO.StatusFuelLinkage + "'  ,STATUS_OF_WATER_SUPPLY='" + objGetStatusDTO.StatusOfWaterSupply + "' where USER_NAME='" + objGetStatusDTO.userName + "' and MEDAProjectID='" + objGetStatusDTO.MEDAProjectID + "'";

                                //}
                            }
                        }
                        //log.Error("Update " + strQuery);

                        cmd = new MySqlCommand(strQuery, mySqlConnection);
                        cmd.ExecuteNonQuery();


                        #region Send Mail
                        ////sendMailOTP(strRegistrationno, strEmailID);
                        //string strBody = string.Empty;

                        //strBody += "Dear " + strFname + ",<br/>";
                        //strBody += strFname + " " + strMname + " " + strLname + " has registered to MSETCL online recruitment system for the post of " + strPostName + "<br/>";
                        //strBody += "against Advertisement No. " + strAdvDesc + "<br/>";
                        ////strBody += "Full Name " + strFname + " " + strMname + " " + strLname + "\n";
                        ////strBody += "Advertisement No : " + strAdvDesc + "\n";
                        //strBody += "Please use following information for login for further process. <br/>";
                        //strBody += "MSETCL Registration No : " + strRegistrationno + "<br/>";
                        //strBody += "Password : " + strPass + "<br/>";
                        //strBody += "<br/>";
                        //strBody += "Thanks & Regards, " + "<br/>";
                        //strBody += "MSETCL  " + "<br/>";
                        //strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

                        ////objSendMail.Send("ogp_support@mahadiscom.in", txtemailid1.Text.ToString(), WebConfigurationManager.AppSettings["MSEDCLEMAILCC1"].ToString(), WebConfigurationManager.AppSettings["WALLETRECHARGEAPPROVEDSUBJECT"].ToString(), strBody);
                        //try
                        //{


                        //    #region using MailMessage
                        //    MailMessage Msg = new MailMessage();
                        //    MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                        //    Msg.From = fromMail;
                        //    Msg.IsBodyHtml = true;
                        //    //log.Error("from:" + fromAddress);
                        //    //Msg.To.Add(new MailAddress("progit4000@mahatransco.in"));
                        //    Msg.To.Add(new MailAddress(strEmailID));


                        //    //  Msg.To.Add(new MailAddress(toAddress));

                        //    Msg.Subject = "Online Exam Registration MSETCL";
                        //    Msg.Body = strBody;
                        //    //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                        //    SmtpClient a = new SmtpClient("23.103.140.170");
                        //    a.Send(Msg);

                        //    Msg = null;
                        //    fromMail = null;
                        //    a = null;
                        //    #endregion
                        //}
                        //catch (Exception ex)
                        //{
                        //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                        //    log.Error(ErrorMessage);
                        //    // throw ex;
                        //}
                        #endregion

                        //saveApp(Session["ProjectID"].ToString());
                        //string scriptText = "alert('Registration successfully done.Kindly make payment of registration. '); window.location='" + "ConsumerDetail.aspx';";

                        ////      ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
                        ////    lblResult.Text = "Registration No is : " + strRegistrationno;
                        ////ClientScript.RegisterStartupScript(this.GetType(), "alert", scriptText , true);
                        //Response.Write("<script language='javascript'>window.alert('" + scriptText + "');window.location='PayRegConfirm.aspx';</script>");
                        ////if (Session["apptype"].ToString().Contains("Consumer") || Session["apptype"].ToString()=="Conventional Generator")


                        //Response.Redirect("~/UI/MSKVY/PayRegConfirm.aspx?appID=" + Session["APPID"].ToString(), false);

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
                lblMessage.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblMessage.Text = "Some problem during registration.Please try again.";
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
            //string str = JsonConverter. .Serialize(objGetStatusDTO, Formatting.Indented);
            return JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);
            //return  new JavaScriptSerializer().Serialize(objGetStatusDTO);
        }
        protected void UploadOther(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (FUOther.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        FUOther.SaveAs(folderPath + Path.GetFileName(FUOther.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUOther.FileName;
                        System.IO.File.Move(folderPath + FUOther.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                //string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3,OtherDocname='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  OtherDocname='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblOther.Text = "Documents uploaded successfully!!";
                                visiblecount++;
                                lblOther.ForeColor = System.Drawing.Color.Green;
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
                        lblOther.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblOther.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }

        protected void UploadOther3(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuBoardResolution.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuBoardResolution.SaveAs(folderPath + Path.GetFileName(fuBoardResolution.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuBoardResolution.FileName;
                        System.IO.File.Move(folderPath + fuBoardResolution.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                //string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther3 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther3='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblOther3.Text = "Documents uploaded successfully!!";
                                lblOther3.ForeColor = System.Drawing.Color.Green;
                                visiblecount++;
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
                        lblOther3.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblOther3.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }

        protected void UploadOther4(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuMemorandumAndArticlesofAssociation.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuMemorandumAndArticlesofAssociation.SaveAs(folderPath + Path.GetFileName(fuMemorandumAndArticlesofAssociation.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuMemorandumAndArticlesofAssociation.FileName;
                        System.IO.File.Move(folderPath + fuMemorandumAndArticlesofAssociation.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                // string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther4 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther4='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                lblOther4.Text = "Documents uploaded successfully!!";
                                lblOther4.ForeColor = System.Drawing.Color.Green;
                                visiblecount++;

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
                        lblSLD.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblSLD.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }

        protected void UploadOther5(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuGovernmentAuthorization.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuGovernmentAuthorization.SaveAs(folderPath + Path.GetFileName(fuGovernmentAuthorization.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuGovernmentAuthorization.FileName;
                        System.IO.File.Move(folderPath + fuGovernmentAuthorization.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                // string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther5 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther5='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Label4.Text = "Documents uploaded successfully!!";
                                Label4.ForeColor = System.Drawing.Color.Green;
                                visiblecount++;
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
                        Label4.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        Label4.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }


        protected void UploadOther6(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuLeaseRightsorOwnershipProof.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuLeaseRightsorOwnershipProof.SaveAs(folderPath + Path.GetFileName(fuLeaseRightsorOwnershipProof.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuLeaseRightsorOwnershipProof.FileName;
                        System.IO.File.Move(folderPath + fuLeaseRightsorOwnershipProof.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                               // string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther6 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther6='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Label6.Text = "Documents uploaded successfully!!";
                                Label6.ForeColor = System.Drawing.Color.Green;
                                visiblecount++;
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
                        Label6.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        Label6.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }
        protected void UploadOther7(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuBankGuaranteeDocument.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuBankGuaranteeDocument.SaveAs(folderPath + Path.GetFileName(fuBankGuaranteeDocument.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuBankGuaranteeDocument.FileName;
                        System.IO.File.Move(folderPath + fuBankGuaranteeDocument.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                              //  string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther7 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther7='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Label8.Text = "Documents uploaded successfully!!";
                                Label8.ForeColor = System.Drawing.Color.Green;
                                visiblecount++;

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
                        Label8.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        Label8.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }
        protected void UploadOther8(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fulblLetterofAward.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fulblLetterofAward.SaveAs(folderPath + Path.GetFileName(fulblLetterofAward.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fulblLetterofAward.FileName;
                        System.IO.File.Move(folderPath + fulblLetterofAward.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                               // string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther8 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther8='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Label10.Text = "Documents uploaded successfully!!";
                                Label10.ForeColor = System.Drawing.Color.Green;
                                uploadedFiles.Add("Letter of Award", true);
                                visiblecount++;
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
                        Label10.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        Label10.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }
        protected void UploadOther9(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuProofofOwnership.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuProofofOwnership.SaveAs(folderPath + Path.GetFileName(fuProofofOwnership.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuProofofOwnership.FileName;
                        System.IO.File.Move(folderPath + fuProofofOwnership.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                               // string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther9 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther9='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Label12.Text = "Documents uploaded successfully!!";
                                Label12.ForeColor = System.Drawing.Color.Green;
                                uploadedFiles.Add("Proof of Ownership", true);
                                visiblecount++;
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
                        Label12.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        Label12.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }

        protected void UploadOther10(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/OTHER/");
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
                if (fuBankGuarantee.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        fuBankGuarantee.SaveAs(folderPath + Path.GetFileName(fuBankGuarantee.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + fuBankGuarantee.FileName;
                        System.IO.File.Move(folderPath + fuBankGuarantee.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                               // string strQuery = "Update APPLICANTDETAILS set WF_STATUS_CD_C=3, docSLDOther10 ='" + newFileName + "' , app_status='DOCUMENT UPLOADED.UPLOAD APPLICATION FORM PENDING.' where APPLICATION_NO='" + strAppID + "'";
                                string strQuery = "Update APPLICANTDETAILS set  docSLDOther10='" + newFileName + "'  where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                Label14.Text = "Documents uploaded successfully!!";
                                Label14.ForeColor = System.Drawing.Color.Green;
                                uploadedFiles.Add("Bank Guarantee", true);
                                visiblecount++;
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
                        Label14.Text = "Letter Upload Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        Label14.Text = "Letter Upload Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }
        
        Dictionary<string, bool> uploadedFiles = new Dictionary<string, bool>();
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Dictionary of file upload controls and corresponding document names

            // List of label controls to check

            

            int jitr = 0;
            System.Web.UI.WebControls.Label lbl = new System.Web.UI.WebControls.Label();// = FindControl(labelID) as System.Web.UI.WebControls.Label;
            // Loop through each label ID
            foreach (string labelID in labelsToCheck)
            {
                // Find the label control on the page
                lbl = FindControl(labelID) as System.Web.UI.WebControls.Label;

                if (lbl != null)
                {
                    log.Error("Label found: " + lbl.Text + " " + lbl.ID);
                    if (lbl.Text.Equals("Documents uploaded successfully!!"))
                    {
                        jitr++;
                        // Check if the label is visible
                    }

                }
                else
                {
                    lblMessage.Text = "Upload Required  Documents";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    //log.Info($"{labelID}: Label not found.");
                }

                
            }
            if (jitr >= 5)
            {
                if (lbl.Text.Equals("Documents uploaded successfully!!"))
                {
                    log.Error("Condition met for document upload");

                    string strAppID = Session["APPID"].ToString();
                    saveApp(Session["ProjectID"].ToString());

                    string strQuery = "UPDATE APPLICANTDETAILS SET WF_STATUS_CD_C = 3, app_status = 'DOCUMENT UPLOADED. UPLOAD APPLICATION FORM PENDING.' WHERE APPLICATION_NO = @ApplicationNo";

                    using (MySqlConnection mySqlConnection = new MySqlConnection(ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString()))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection))
                        {
                            cmd.Parameters.AddWithValue("@ApplicationNo", strAppID);

                            try
                            {
                                mySqlConnection.Open();
                                cmd.ExecuteNonQuery();
                                lblMessage.Text = "All documents uploaded successfully.";
                                lblMessage.ForeColor = System.Drawing.Color.Green;
                                log.Error("Database update successful");
                            }
                            catch (Exception ex)
                            {
                                //log.Error("Error updating database: " + ex.Message);
                                lblMessage.Text = "An error occurred while updating the database.";
                                lblMessage.ForeColor = System.Drawing.Color.Red;
                            }
                        }
                    }

                    //Response.Write($"{labelID}: Visible and text matches.<br>");
                }
                else
                {
                    lblMessage.Text = "Upload Required  Documents";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    //  log.Info($"{labelID}: Label is not visible.");
                }
            }
        }



        protected void btnAdd_Click(object sender, EventArgs e)
        {
            //int no = 0;
            //if (trother3.Visible == false && trother4.Visible==false)
            //{
            //    no = 1;
                
            //}
            //if (trother3.Visible == true && trother4.Visible == false)
            //{
            //    no = 2;
            //}
            //if (no == 1)
            //{
            //    trother3.Visible = true;
            //}
            //if (no == 2)
            //{
            //    trother3.Visible = true;
            //    trother4.Visible = true;
            //}
        }
    }
}