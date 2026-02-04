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
using Newtonsoft.Json;
using GGC.Common;
using GGC.WebService;

namespace GGC.UI.MSKVY
{
    public partial class ProjectDetail : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ProjectDetail));
        protected string strPROMOTOR_NAME = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session["apptype"].ToString() == "Consumer")
                {
                    btnSave.Text = "Save";
                }
                {
                    btnSave.Text = "Save & Next";
                }
                if (Session["NatureOfApp"] == "Generator")
                {
                    trFuel.Visible = true;
                }
                else
                {
                    trFuel.Visible = false;

                }
                //string strGSTIN = Session["GSTIN"].ToString();
                string strAPPID = Session["APPID"].ToString();
                fillData(strAPPID);
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
                        strQuery = "select * from mskvy_applicantdetails where MEDAProjectID='" + projId + "'";
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
                        objGetStatusDTO.DISTANCE_FROM_PLANT = dsResult.Tables[0].Rows[0]["DISTANCE_FROM_PLANT"].ToString();
                        objGetStatusDTO.VOLT_LEVEL_SUBSTATION = dsResult.Tables[0].Rows[0]["VOLT_LEVEL_SUBSTATION"].ToString();
                        Session["PROMOTOR_NAME"] = dsResult.Tables[0].Rows[0]["PROMOTOR_NAME"].ToString();
                        Session["AppType"] = dsResult.Tables[0].Rows[0]["NATURE_OF_APP"].ToString();
                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.statusId = "2";
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
            //string str = JsonConverter. .Serialize(objGetStatusDTO, Formatting.Indented);
            return JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);
            //return  new JavaScriptSerializer().Serialize(objGetStatusDTO);
        }
        protected void fillData(string appId)
        {
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

            //strGSTIN = Session["GSTIN"].ToString();
            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        string strQuery = string.Empty;
                        MySqlCommand cmd;
                        cmd = new MySqlCommand("select * from mskvy_applicantdetails where APPLICATION_NO='" + appId + "'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        txtNameOfTransmission.Text = dsResult.Tables[0].Rows[0]["NAME_TRANS_LICENSEE"].ToString();
                        txtGenVolt.Text = dsResult.Tables[0].Rows[0]["GENERATION_VOLTAGE"].ToString();
                        txtInjVolt.Text = dsResult.Tables[0].Rows[0]["INJECTION_VOLTAGE"].ToString();
                        txtPointInj.Text = dsResult.Tables[0].Rows[0]["POINT_OF_INJECTION"].ToString();
                        txtDistancePlant.Text = dsResult.Tables[0].Rows[0]["DISTANCE_FROM_PLANT"].ToString();
                        txtVoltLevelSubstn.Text = dsResult.Tables[0].Rows[0]["VOLT_LEVEL_SUBSTATION"].ToString();
                        ddlNatureOfTransLic.SelectedValue = dsResult.Tables[0].Rows[0]["SALE_OF_POWER"].ToString();
                        ddlInterState.SelectedValue = dsResult.Tables[0].Rows[0]["INTER_STATE"].ToString();
                        txtTotReqLand.Text = dsResult.Tables[0].Rows[0]["TOTAL_REQUIRED_LAND"].ToString();
                        txtLandInPoss.Text = dsResult.Tables[0].Rows[0]["LAND_IN_POSSESSION"].ToString();
                        txtTotPvtLnd.Text = dsResult.Tables[0].Rows[0]["TOTAL_PRIVATE_LAND"].ToString();
                        txtTotForestLnd.Text = dsResult.Tables[0].Rows[0]["TOTAL_FOREST_LAND"].ToString();
                        txtStatusOfFoestLnd.Text = dsResult.Tables[0].Rows[0]["STATUS_FOREST_LAND"].ToString();
                        txtIsSanctury.Text = dsResult.Tables[0].Rows[0]["BIRD_SANCTURY_ETC"].ToString();
                        txtDetailPPA.Text = dsResult.Tables[0].Rows[0]["PPA_POWER_TOBE_INJECTED"].ToString();
                        txtAgreement.Text = dsResult.Tables[0].Rows[0]["AGGREEMENT_WITH_TRADER"].ToString();
                        txtStatusFuelLinkages.Text = dsResult.Tables[0].Rows[0]["STATUS_FUEL_LINKAGE"].ToString();
                        txtStatusWaterSupp.Text = dsResult.Tables[0].Rows[0]["STATUS_OF_WATER_SUPPLY"].ToString();
                        Session["WF_STATUS_CD_C"] = dsResult.Tables[0].Rows[0]["WF_STATUS_CD_C"].ToString();
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
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strNAME_TRANS_LICENSEE, strGENERATION_VOLTAGE, strINJECTION_VOLTAGE, strPOINT_OF_INJECTION, strDISTANCE_FROM_PLANT;
            string strVOLT_LEVEL_SUBSTATION, strSALE_OF_POWER, strINTER_STATE, strTOTAL_REQUIRED_LAND, strLAND_IN_POSSESSION;
            string strTOTAL_PRIVATE_LAND, strTOTAL_FOREST_LAND, strSTATUS_FOREST_LAND, strBIRD_SANCTURY_ETC;
            string strPPA_POWER_TOBE_INJECTED, strAGGREEMENT_WITH_TRADER, strSTATUS_FUEL_LINKAGE, strSTATUS_OF_WATER_SUPPLY;
            string strMEDAProjectID = Session["ProjectID"].ToString();
            strNAME_TRANS_LICENSEE = txtNameOfTransmission.Text;
            strGENERATION_VOLTAGE = txtGenVolt.Text!=""?txtGenVolt.Text:"0.0";
            strINJECTION_VOLTAGE = txtInjVolt.Text!=""?txtInjVolt.Text:"0.0";
            strPOINT_OF_INJECTION = txtPointInj.Text;
            strDISTANCE_FROM_PLANT = txtDistancePlant.Text;
            strVOLT_LEVEL_SUBSTATION = txtVoltLevelSubstn.Text;
            strSALE_OF_POWER = ddlNatureOfTransLic.SelectedItem.Text;
            strINTER_STATE = ddlInterState.SelectedItem.Text;
            strTOTAL_REQUIRED_LAND = txtTotReqLand.Text;
            strLAND_IN_POSSESSION = txtLandInPoss.Text;
            strTOTAL_PRIVATE_LAND = txtTotPvtLnd.Text;
            strTOTAL_FOREST_LAND = txtTotForestLnd.Text;
            strSTATUS_FOREST_LAND = txtStatusOfFoestLnd.Text;
            strBIRD_SANCTURY_ETC = txtIsSanctury.Text;

            strPPA_POWER_TOBE_INJECTED = txtDetailPPA.Text;
            strAGGREEMENT_WITH_TRADER = txtAgreement.Text;
            strSTATUS_FUEL_LINKAGE = txtStatusFuelLinkages.Text;
            strSTATUS_OF_WATER_SUPPLY = txtStatusWaterSupp.Text;
            int WF_STATUS_CD_C = int.Parse(Session["WF_STATUS_CD_C"].ToString());
            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            //string strGSTIN = string.Empty;
            //if (Session["GSTIN"] != null)
            //    strGSTIN = Session["GSTIN"].ToString();
            string strUserName = Session["user_name"].ToString();
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
                            if (WF_STATUS_CD_C > 2)
                                strQuery = "update mskvy_applicantdetails set NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "' where USER_NAME='" + strUserName + "' and MEDAProjectID='" + strMEDAProjectID + "'";
                            else
                            {
                                //if (Session["isAppliedMEDAID"].ToString() == "true")
                                //{
                                //    strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=25,NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "',app_status='APPLICATION RECEIVED' where USER_NAME='" + strUserName + "'";
                                //}
                                //else
                                //{
                                strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=2,NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "',app_status='APPLICATION RECEIVED' where USER_NAME='" + strUserName + "' and MEDAProjectID='" + strMEDAProjectID + "'";

                                //}
                            }
                        }
                        else
                        {
                            if (WF_STATUS_CD_C > 2)
                                strQuery = "update mskvy_applicantdetails set NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "' where USER_NAME='" + strUserName + "' and MEDAProjectID='" + strMEDAProjectID + "'";
                            else
                            {
                                //if (Session["isAppliedMEDAID"].ToString() == "true")
                                //{
                                //    strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=25,NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "',app_status='APPLICATION RECEIVED' where USER_NAME='" + strUserName + "'";
                                //}
                                //else
                                //{
                                strQuery = "update mskvy_applicantdetails set WF_STATUS_CD_C=2,NAME_TRANS_LICENSEE='" + strNAME_TRANS_LICENSEE + "'," + "GENERATION_VOLTAGE='" + strGENERATION_VOLTAGE + "',INJECTION_VOLTAGE='" + strINJECTION_VOLTAGE + "',POINT_OF_INJECTION='" + strPOINT_OF_INJECTION + "' ,DISTANCE_FROM_PLANT='" + strDISTANCE_FROM_PLANT + "' ,VOLT_LEVEL_SUBSTATION='" + strVOLT_LEVEL_SUBSTATION + "' ,SALE_OF_POWER='" + strSALE_OF_POWER + "' ,INTER_STATE='" + strINTER_STATE + "' ,TOTAL_REQUIRED_LAND='" + strTOTAL_REQUIRED_LAND + "' ,LAND_IN_POSSESSION='" + strLAND_IN_POSSESSION + "' ,TOTAL_PRIVATE_LAND='" + strTOTAL_PRIVATE_LAND + "' ,TOTAL_FOREST_LAND='" + strTOTAL_FOREST_LAND + "'  ,STATUS_FOREST_LAND='" + strSTATUS_FOREST_LAND + "',BIRD_SANCTURY_ETC='" + strBIRD_SANCTURY_ETC + "'  ,PPA_POWER_TOBE_INJECTED ='" + strPPA_POWER_TOBE_INJECTED + "'  ,AGGREEMENT_WITH_TRADER ='" + strAGGREEMENT_WITH_TRADER + "'  ,STATUS_FUEL_LINKAGE ='" + strSTATUS_FUEL_LINKAGE + "'  ,STATUS_OF_WATER_SUPPLY='" + strSTATUS_OF_WATER_SUPPLY + "',app_status='APPLICATION RECEIVED' where USER_NAME='" + strUserName + "' and MEDAProjectID='" + strMEDAProjectID + "'";

                                // }
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

                        saveApp(Session["ProjectID"].ToString());
                        string scriptText = "alert('Registration successfully done.Kindly make payment of registration. '); window.location='" + "ConsumerDetail.aspx';";

                        //      ScriptManager.RegisterStartupScript(this, this.GetType(), "alertMessage", scriptText, true);
                        //    lblResult.Text = "Registration No is : " + strRegistrationno;
                        //ClientScript.RegisterStartupScript(this.GetType(), "alert", scriptText , true);
                        Response.Write("<script language='javascript'>window.alert('" + scriptText + "');window.location='PayRegConfirm.aspx';</script>");
                        //if (Session["apptype"].ToString().Contains("Consumer") || Session["apptype"].ToString()=="Conventional Generator")


                        Response.Redirect("~/UI/MSKVY/PayRegConfirm.aspx?appID=" + Session["APPID"].ToString(), false);

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
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
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

    }
}