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
using GGC.Common;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GGC.WebService;
using GGC.Scheduler;

namespace GGC.UI.MSKVYSPD
{
    public partial class AppDetail : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppDetail));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtGCLtrDt.Attributes.Add("readonly", "readonly");
            txtGCValDt.Attributes.Add("readonly", "readonly");
            //CompareScheduledCommissioningValidator.ValueToCompare = DateTime.Now.ToShortDateString();

            if (!Page.IsPostBack)
            {


                fillDistrict();

                if (Request.QueryString["appid"] != null)
                {
                    string ID = Request.QueryString["appid"].ToString();
                    //fillDistrict();

                    //string gstin = Request.QueryString["GSTIN"].ToString();
                    Session["APPID"] = ID;
                    Session["isValiMEDAID"] = "true";
                    fillData(ID);
                }
            }
        }

        protected void fillDistrict()
        {
            string strQuery = "select distinct district district from zone_district order by 1";
            DataSet dsResult = new DataSet();
            dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            ddlDistrict.DataSource = dsResult.Tables[0];
            ddlDistrict.DataValueField = "district";
            ddlDistrict.DataTextField = "district";
            ddlDistrict.DataBind();
            ddlDistrict.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
            ddlDistrict.ClearSelection();
            ddlDistrict.SelectedIndex = 0;
        }
        protected void fillData(string ID)
        {
            string strQuery = "select * from mskvy_applicantdetails_spd where APPLICATION_NO='" + Session["APPID"] + "'";
            DataSet dsResult = new DataSet();
            try
            {
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                txtAppNo.Text = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString();
                txtProjCode.Text = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                txtNameOfPromoter.Text = dsResult.Tables[0].Rows[0]["NAME_OF_SPD"].ToString();
                txtGSTIN.Text = dsResult.Tables[0].Rows[0]["GSTIN_NO"].ToString();
                txtPANNO.Text = dsResult.Tables[0].Rows[0]["PAN_NUMBER"].ToString();
                txtCorrAdd.Text = dsResult.Tables[0].Rows[0]["ADDRESS_FOR_CORRESPONDENCE"].ToString();
                txtContPer1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString();
                txtDesign1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString();
                txtPhone1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_PHONE_1"].ToString();
                txtMob1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                txtEmail1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                txtNameofSPV.Text = dsResult.Tables[0].Rows[0]["SPV_Name"].ToString();
                //Session["user_name"].ToString() = dsResult.Tables[0].Rows[0]["USER_NAME"].ToString();
                txtClusterName.Text = dsResult.Tables[0].Rows[0]["ClusterName"].ToString();
                //"SOLAR" = dsResult.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                txtProjectCapacity.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                txtProjectLocation.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();
                //ddlDistrict.SelectedItem.Text = dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString();
                //ddlDistrict.Items.FindByText(dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString()).Selected = true;
                txtGCLtrNo.Text = dsResult.Tables[0].Rows[0]["GCLetter_No"].ToString();

                //DateTime dt1 = DateTime.ParseExact(dsResult.Tables[0].Rows[0]["GCLetter_Date"].ToString(),
                //            "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //txtGCLtrDt.Text = dt1.ToString();
                ////txtGCValDt.Text = dsResult.Tables[0].Rows[0]["GCLetter_ValidityDate"].ToString();

                //DateTime dt2 = DateTime.ParseExact(dsResult.Tables[0].Rows[0]["GCLetter_ValidityDate"].ToString(),
                //            "dd-MM-yyyy", CultureInfo.InvariantCulture);
                //txtGCLtrDt.Text = dt2.ToString();


                //string strSCHEDULED_COMM_DATE, schDD, schMM, schYYYY;
                //strSCHEDULED_COMM_DATE = dsResult.Tables[0].Rows[0]["GCLetter_Date"].ToString();

                //if (strSCHEDULED_COMM_DATE != "")
                //{
                //    schDD = strSCHEDULED_COMM_DATE.Substring(8, 2);
                //    schMM = strSCHEDULED_COMM_DATE.Substring(5, 2);
                //    schYYYY = strSCHEDULED_COMM_DATE.Substring(0, 4);
                //    txtGCLtrDt.Text = schDD + "-" + schMM + "-" + schYYYY;
                //}

                //string strGC_DATE, GCDD, GChMM, GCYYYY;
                //strGC_DATE = dsResult.Tables[0].Rows[0]["GCLetter_ValidityDate"].ToString();

                //if (strGC_DATE != "")
                //{
                //    GCDD = strGC_DATE.Substring(8, 2);
                //    GChMM = strGC_DATE.Substring(5, 2);
                //    GCYYYY = strGC_DATE.Substring(0, 4);
                //    txtGCValDt.Text = GCDD + "-" + GChMM + "-" + GCYYYY;
                //}

                txtInterconnection.Text = dsResult.Tables[0].Rows[0]["INTERCONNECTION_AT"].ToString();
                txtDCLTenderNo.Text = dsResult.Tables[0].Rows[0]["MSEDCL_Tender_No"].ToString();
                txtPPADet.Text = dsResult.Tables[0].Rows[0]["PPA_DETAILS"].ToString();
                txtLandReq.Text = dsResult.Tables[0].Rows[0]["TOTAL_REQUIRED_LAND"].ToString();
                txtLandPoss.Text = dsResult.Tables[0].Rows[0]["LAND_IN_POSSESSION"].ToString();
                txtCorrAdd.Text = dsResult.Tables[0].Rows[0]["ADDRESS_FOR_CORRESPONDENCE"].ToString();
                RbIsForestLand.SelectedItem.Text = dsResult.Tables[0].Rows[0]["IS_FOREST_LAND"].ToString();
                txtStatusofForest.Text = dsResult.Tables[0].Rows[0]["STATUS_FOREST_LAND"].ToString();
                ddlDistrict.ClearSelection();
                ddlDistrict.SelectedIndex = ddlDistrict.Items.IndexOf(ddlDistrict.Items.FindByText(dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString()));

                txtLinesthrForest.Text = dsResult.Tables[0].Rows[0]["BIRD_SANCTURY_ETC"].ToString();
                if (dsResult.Tables[0].Rows[0]["is_Change_loc"].ToString() == "1")
                    RbIsChangeLoc.Items.FindByValue("1").Selected = true;
                else
                    RbIsChangeLoc.Items.FindByValue("0").Selected = true;

                txtLatitude.Text = dsResult.Tables[0].Rows[0]["Lat"].ToString();
                txtLongitude.Text = dsResult.Tables[0].Rows[0]["Longt"].ToString();
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            lblResult.Text = "";
            try
            {
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT a.*,b.* FROM mskvy_applicantdetails a, mskvy_spv_det b WHERE a.APPLICATION_NO='" + txtProjCode.Text + "' and a.PAN_TAN_NO=b.generatorPan");
                DataSet dsSSResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM MSKVY_SS_Detail a WHERE APPLICATION_NO='" + txtProjCode.Text + "'");
                DataSet dsGSTIN = SQLHelper.ExecuteDataset(conString, CommandType.Text, "SELECT * FROM applicant_reg_det WHERE user_name='" + dsResult.Tables[0].Rows[0]["user_name"].ToString() + "'");


                txtGSTIN.Text = dsResult.Tables[0].Rows[0]["GSTIN_NO"].ToString();


                txtProjectCapacity.Text = dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString();
                txtNameofSPV.Text = dsResult.Tables[0].Rows[0]["generatorName"].ToString();
                txtAppNo.Text = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                txtPANNO.Text = dsResult.Tables[0].Rows[0]["PAN_TAN_NO"].ToString();
                txtGSTIN.Text = dsGSTIN.Tables[0].Rows[0]["GSTIN_NO"].ToString();
                txtClusterName.Text = dsResult.Tables[0].Rows[0]["ClusterName"].ToString();
                txtProjectLocation.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + " Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString();
                ddlDistrict.Text = dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString();
                txtContPer1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString();
                txtDesign1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_DESIG_1"].ToString();
                txtPhone1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_PHONE_1"].ToString();
                txtMob1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                txtEmail1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                lblSubProjCode.Text = dsResult.Tables[0].Rows[0]["sub_project_code"].ToString();
                txtNameOfPromoter.Text = dsResult.Tables[0].Rows[0]["SPD_Name"].ToString();
                txtInterconnection.Text = dsSSResult.Tables[0].Rows[0]["ssName"].ToString();
                //txtCorrAdd.Text = dsResult.Tables[0].Rows[0]["Add_correspondence"].ToString();
                //if (dsResult.Tables[0].Rows[0]["is_Change_loc"].ToString() == "1")
                //        RbIsChangeLoc.Items.FindByValue("1").Selected = true;
                //        else
                //        RbIsChangeLoc.Items.FindByValue("0").Selected = true;

                txtLatitude.Text = dsResult.Tables[0].Rows[0]["Latitude"].ToString();
                txtLongitude.Text = dsResult.Tables[0].Rows[0]["Longitude"].ToString();
                txtProjectCapacity.Enabled = false;
                txtProjectLocation.Enabled = false;
                txtNameofSPV.Enabled = false;
                txtClusterName.Enabled = false;
                txtProjectLocation.Enabled = false;
                ddlDistrict.Enabled = false;
                txtContPer1.Enabled = false;
                txtDesign1.Enabled = false;
                txtPhone1.Enabled = false;
                txtMob1.Enabled = false;
                txtEmail1.Enabled = false;
                txtInterconnection.Enabled = false;
                txtInterconnection.Enabled = false;


                //txtTaluka.Enabled = false;
                //ddlProjectType.SelectedItem.Text = objProjDet.projectTypeName;


            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }

            //    fetchRegDet();



        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string APPLICATION_NO, MEDAProjectID, NAME_OF_SPD, GSTIN_NO, PAN_NUMBER, ADDRESS_FOR_CORRESPONDENCE, CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_EMAIL_1, CONT_PER_NAME_2, CONT_PER_DESIG_2, CONT_PER_PHONE_2, CONT_PER_MOBILE_2, CONT_PER_EMAIL_2, SPV_Name, USER_NAME, ClusterName, CustomerID, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_TALUKA, PROJECT_DISTRICT, GCLetter_No, GCLetter_Date, GCLetter_ValidityDate, INTERCONNECTION_AT, MSEDCL_Tender_No, PPA_DETAILS, TOTAL_REQUIRED_LAND, LAND_IN_POSSESSION, TOTAL_FOREST_LAND, IS_FOREST_LAND, STATUS_FOREST_LAND, BIRD_SANCTURY_ETC, WF_STATUS_CD_C, app_status, APP_STATUS_DT, ZONE, ROLE_CD, CREATED_DT, CREATED_BY, UPDATED_DT, UPDATED_BY;

            var query = "SELECT a.*,b.* FROM mskvy_applicantdetails a, mskvy_spv_det b WHERE a.APPLICATION_NO='" + txtProjCode.Text + "' and a.PAN_TAN_NO=b.generatorPan";
            var mskvyApplicantDetails = SQLHelper.ExecuteDataset(conString, CommandType.Text, query).Tables[0].Rows[0];

            PROJECT_TALUKA = mskvyApplicantDetails["PROJECT_TALUKA"].ToString();
            //  APPLICATION_NO = txtAppNo.Text;
            APPLICATION_NO = txtProjCode.Text;
            Session["APPID"] = APPLICATION_NO;
            MEDAProjectID = txtAppNo.Text;
            NAME_OF_SPD = txtNameOfPromoter.Text;
            GSTIN_NO = txtGSTIN.Text;
            PAN_NUMBER = txtPANNO.Text;
            ADDRESS_FOR_CORRESPONDENCE = txtCorrAdd.Text;
            CONT_PER_NAME_1 = txtContPer1.Text;
            CONT_PER_DESIG_1 = txtDesign1.Text;
            CONT_PER_PHONE_1 = txtPhone1.Text;
            CONT_PER_MOBILE_1 = txtMob1.Text;
            CONT_PER_EMAIL_1 = txtEmail1.Text;
            SPV_Name = txtNameofSPV.Text;
            USER_NAME = Session["user_name"].ToString();
            ClusterName = txtClusterName.Text;
            PROJECT_TYPE = "SOLAR";
            PROJECT_CAPACITY_MW = txtProjectCapacity.Text;
            PROJECT_LOC = txtProjectLocation.Text;
            PROJECT_DISTRICT = ddlDistrict.SelectedItem.Text;
            GCLetter_No = txtGCLtrNo.Text;
            GCLetter_Date = txtGCLtrDt.Text;
            GCLetter_ValidityDate = txtGCValDt.Text;
            INTERCONNECTION_AT = txtInterconnection.Text;
            MSEDCL_Tender_No = txtDCLTenderNo.Text;
            PPA_DETAILS = txtPPADet.Text;
            TOTAL_REQUIRED_LAND = txtLandReq.Text;
            LAND_IN_POSSESSION = txtLandPoss.Text;
            //TOTAL_FOREST_LAND=  txtForestLand.Text ;
            IS_FOREST_LAND = RbIsForestLand.SelectedItem.Text;
            STATUS_FOREST_LAND = txtStatusofForest.Text;
            BIRD_SANCTURY_ETC = txtLinesthrForest.Text;

            string strSubPrjCode = lblSubProjCode.Text;
            string isChangeLoc = RbIsChangeLoc.SelectedItem.Value;
            string lat, longt;
            lat = txtLatitude.Text;
            longt = txtLongitude.Text;
            string schDD, schMM, schYYYY, strSCHEDULED_COMM_DATE, strValidity_DATE = string.Empty;
            if (txtGCLtrDt.Text != "")
            {
                schDD = txtGCLtrDt.Text.Substring(0, 2);
                schMM = txtGCLtrDt.Text.Substring(3, 2);
                schYYYY = txtGCLtrDt.Text.Substring(6, 4);
                strSCHEDULED_COMM_DATE = schYYYY + "-" + schMM + "-" + schDD;
                log.Error("strSCHEDULED_COMM_DATE " + strSCHEDULED_COMM_DATE);
            }
            else
            {
                strSCHEDULED_COMM_DATE = "";
            }

            if (txtGCValDt.Text != "")
            {
                schDD = txtGCValDt.Text.Substring(0, 2);
                schMM = txtGCValDt.Text.Substring(3, 2);
                schYYYY = txtGCValDt.Text.Substring(6, 4);
                strValidity_DATE = schYYYY + "-" + schMM + "-" + schDD;
                log.Error("strValidity_DATE " + strValidity_DATE);

            }
            else
            {
                strValidity_DATE = "";
            }

            string strQuery = "select count(1) from mskvy_applicantdetails_spd where APPLICATION_NO='" + Session["APPID"] + "'";
            DataSet dsResult = new DataSet();
            dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);



            Session["PROMOTOR_NAME"] = txtNameofSPV.Text;
            if (dsResult.Tables[0].Rows[0][0].ToString() == "0")
            {
                strQuery = "select distinct zone from zone_district where district =(select PROJECT_DISTRICT from mskvy_applicantdetails where application_no='" + APPLICATION_NO + "')";
                dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                string strZone = dsResult.Tables[0].Rows[0]["ZONE"].ToString();

                if (txtGCLtrDt.Text != "")
                {
                    if (txtGCValDt.Text != "")
                    {
                        strQuery = "INSERT INTO mskvy_applicantdetails_spd(APPLICATION_NO, MEDAProjectID, NAME_OF_SPD, GSTIN_NO, PAN_NUMBER, ADDRESS_FOR_CORRESPONDENCE, CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_EMAIL_1, SPV_Name, USER_NAME, ClusterName, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_DISTRICT, GCLetter_No, GCLetter_Date, GCLetter_ValidityDate, INTERCONNECTION_AT, MSEDCL_Tender_No, PPA_DETAILS, TOTAL_REQUIRED_LAND, LAND_IN_POSSESSION, IS_FOREST_LAND, STATUS_FOREST_LAND, BIRD_SANCTURY_ETC, WF_STATUS_CD_C, app_status, APP_STATUS_DT, ZONE, CREATED_DT, CREATED_BY,is_Change_loc,lat,longt,sub_project_code,PROJECT_TALUKA) " +
                            " VALUES ('" + APPLICATION_NO + "','" + MEDAProjectID + "','" + NAME_OF_SPD + "','" + GSTIN_NO + "','" + PAN_NUMBER + "','" + ADDRESS_FOR_CORRESPONDENCE + "','" + CONT_PER_NAME_1 + "','" + CONT_PER_DESIG_1 + "','" + CONT_PER_PHONE_1 + "','" + CONT_PER_MOBILE_1 + "','" + CONT_PER_EMAIL_1 + "','" + SPV_Name + "','" + USER_NAME + "','" + ClusterName + "','" + PROJECT_TYPE + "','" + PROJECT_CAPACITY_MW + "','" + PROJECT_LOC + "','" + PROJECT_DISTRICT + "','" + GCLetter_No + "','" + strSCHEDULED_COMM_DATE + "','" + strValidity_DATE + "','" + INTERCONNECTION_AT + "','" + MSEDCL_Tender_No + "','" + PPA_DETAILS + "','" + TOTAL_REQUIRED_LAND + "','" + LAND_IN_POSSESSION + "','" + IS_FOREST_LAND + "','" + STATUS_FOREST_LAND + "','" + BIRD_SANCTURY_ETC + "',2,'APPLICATION RECEIVED',now(),'" + strZone + "',now(),'" + USER_NAME + "','" + isChangeLoc + "','" + lat + "','" + longt + "','" + strSubPrjCode + "','" + PROJECT_TALUKA + "')";
                    }
                    else
                    {
                        strQuery = "INSERT INTO mskvy_applicantdetails_spd( APPLICATION_NO, MEDAProjectID, NAME_OF_SPD, GSTIN_NO, PAN_NUMBER, ADDRESS_FOR_CORRESPONDENCE, CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_EMAIL_1, SPV_Name, USER_NAME, ClusterName, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_DISTRICT, GCLetter_No, GCLetter_Date,  INTERCONNECTION_AT, MSEDCL_Tender_No, PPA_DETAILS, TOTAL_REQUIRED_LAND, LAND_IN_POSSESSION, IS_FOREST_LAND, STATUS_FOREST_LAND, BIRD_SANCTURY_ETC, WF_STATUS_CD_C, app_status, APP_STATUS_DT, ZONE, CREATED_DT, CREATED_BY,is_Change_loc,lat,longt,sub_project_code,PROJECT_TALUKA) " +
                    " VALUES ('" + APPLICATION_NO + "','" + MEDAProjectID + "','" + NAME_OF_SPD + "','" + GSTIN_NO + "','" + PAN_NUMBER + "','" + ADDRESS_FOR_CORRESPONDENCE + "','" + CONT_PER_NAME_1 + "','" + CONT_PER_DESIG_1 + "','" + CONT_PER_PHONE_1 + "','" + CONT_PER_MOBILE_1 + "','" + CONT_PER_EMAIL_1 + "','" + SPV_Name + "','" + USER_NAME + "','" + ClusterName + "','" + PROJECT_TYPE + "','" + PROJECT_CAPACITY_MW + "','" + PROJECT_LOC + "','" + PROJECT_DISTRICT + "','" + GCLetter_No + "','" + strSCHEDULED_COMM_DATE + "','" + INTERCONNECTION_AT + "','" + MSEDCL_Tender_No + "','" + PPA_DETAILS + "','" + TOTAL_REQUIRED_LAND + "','" + LAND_IN_POSSESSION + "','" + IS_FOREST_LAND + "','" + STATUS_FOREST_LAND + "','" + BIRD_SANCTURY_ETC + "',2,'APPLICATION RECEIVED',now(),'" + strZone + "',now(),'" + USER_NAME + "','" + isChangeLoc + "','" + lat + "','" + longt + "','" + strSubPrjCode + "','" + PROJECT_TALUKA + "')";

                    }
                }
                else
                {

                    if (txtGCValDt.Text != "")
                    {
                        strQuery = "INSERT INTO mskvy_applicantdetails_spd( APPLICATION_NO, MEDAProjectID, NAME_OF_SPD, GSTIN_NO, PAN_NUMBER, ADDRESS_FOR_CORRESPONDENCE, CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_EMAIL_1, SPV_Name, USER_NAME, ClusterName, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_DISTRICT, GCLetter_No, GCLetter_ValidityDate, INTERCONNECTION_AT, MSEDCL_Tender_No, PPA_DETAILS, TOTAL_REQUIRED_LAND, LAND_IN_POSSESSION, IS_FOREST_LAND, STATUS_FOREST_LAND, BIRD_SANCTURY_ETC, WF_STATUS_CD_C, app_status, APP_STATUS_DT, ZONE, CREATED_DT, CREATED_BY,is_Change_loc,lat,longt,sub_project_code,PROJECT_TALUKA) " +
                            " VALUES ('" + APPLICATION_NO + "','" + MEDAProjectID + "','" + NAME_OF_SPD + "','" + GSTIN_NO + "','" + PAN_NUMBER + "','" + ADDRESS_FOR_CORRESPONDENCE + "','" + CONT_PER_NAME_1 + "','" + CONT_PER_DESIG_1 + "','" + CONT_PER_PHONE_1 + "','" + CONT_PER_MOBILE_1 + "','" + CONT_PER_EMAIL_1 + "','" + SPV_Name + "','" + USER_NAME + "','" + ClusterName + "','" + PROJECT_TYPE + "','" + PROJECT_CAPACITY_MW + "','" + PROJECT_LOC + "','" + PROJECT_DISTRICT + "','" + GCLetter_No + "','" + strValidity_DATE + "','" + INTERCONNECTION_AT + "','" + MSEDCL_Tender_No + "','" + PPA_DETAILS + "','" + TOTAL_REQUIRED_LAND + "','" + LAND_IN_POSSESSION + "','" + IS_FOREST_LAND + "','" + STATUS_FOREST_LAND + "','" + BIRD_SANCTURY_ETC + "',2,'APPLICATION RECEIVED',now(),'" + strZone + "',now(),'" + USER_NAME + "','" + isChangeLoc + "','" + lat + "','" + longt + "','" + strSubPrjCode + "','" + PROJECT_TALUKA + "')";
                    }
                    else
                    {
                        strQuery = "INSERT INTO mskvy_applicantdetails_spd( APPLICATION_NO, MEDAProjectID, NAME_OF_SPD, GSTIN_NO, PAN_NUMBER, ADDRESS_FOR_CORRESPONDENCE, CONT_PER_NAME_1, CONT_PER_DESIG_1, CONT_PER_PHONE_1, CONT_PER_MOBILE_1, CONT_PER_EMAIL_1, SPV_Name, USER_NAME, ClusterName, PROJECT_TYPE, PROJECT_CAPACITY_MW, PROJECT_LOC, PROJECT_DISTRICT, GCLetter_No, INTERCONNECTION_AT, MSEDCL_Tender_No, PPA_DETAILS, TOTAL_REQUIRED_LAND, LAND_IN_POSSESSION, IS_FOREST_LAND, STATUS_FOREST_LAND, BIRD_SANCTURY_ETC, WF_STATUS_CD_C, app_status, APP_STATUS_DT, ZONE, CREATED_DT, CREATED_BY,is_Change_loc,lat,longt,sub_project_code,PROJECT_TALUKA) " +
                                " VALUES ('" + APPLICATION_NO + "','" + MEDAProjectID + "','" + NAME_OF_SPD + "','" + GSTIN_NO + "','" + PAN_NUMBER + "','" + ADDRESS_FOR_CORRESPONDENCE + "','" + CONT_PER_NAME_1 + "','" + CONT_PER_DESIG_1 + "','" + CONT_PER_PHONE_1 + "','" + CONT_PER_MOBILE_1 + "','" + CONT_PER_EMAIL_1 + "','" + SPV_Name + "','" + USER_NAME + "','" + ClusterName + "','" + PROJECT_TYPE + "','" + PROJECT_CAPACITY_MW + "','" + PROJECT_LOC + "','" + PROJECT_DISTRICT + "','" + GCLetter_No + "','" + INTERCONNECTION_AT + "','" + MSEDCL_Tender_No + "','" + PPA_DETAILS + "','" + TOTAL_REQUIRED_LAND + "','" + LAND_IN_POSSESSION + "','" + IS_FOREST_LAND + "','" + STATUS_FOREST_LAND + "','" + BIRD_SANCTURY_ETC + "',2,'APPLICATION RECEIVED',now(),'" + strZone + "',now(),'" + USER_NAME + "','" + isChangeLoc + "','" + lat + "','" + longt + "','" + strSubPrjCode + "','" + PROJECT_TALUKA + "')";
                    }
                }
                log.Error(strQuery);
                try
                {

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Application saved sucessfully!!');window.location ='UploadDoc.aspx';", true);
                    Response.Redirect("~/UI/MSKVYSPD/UploadDoc.aspx", false);
                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    lblResult.Text = "Some problem during registration.Please try again.";
                }
            }
            else
            {

                if (txtGCLtrDt.Text != "")
                {
                    if (txtGCValDt.Text != "")
                    {
                        strQuery = "update mskvy_applicantdetails_spd set MEDAProjectID='" + MEDAProjectID + "',NAME_OF_SPD='" + NAME_OF_SPD + "',GSTIN_NO='" + GSTIN_NO + "',PAN_NUMBER='" + PAN_NUMBER + "',ADDRESS_FOR_CORRESPONDENCE='" + ADDRESS_FOR_CORRESPONDENCE + "',CONT_PER_NAME_1='" + CONT_PER_NAME_1 + "',CONT_PER_DESIG_1='" + CONT_PER_DESIG_1 + "',CONT_PER_PHONE_1='" + CONT_PER_PHONE_1 + "',CONT_PER_MOBILE_1='" + CONT_PER_MOBILE_1 + "',CONT_PER_EMAIL_1='" + CONT_PER_EMAIL_1 + "',SPV_Name='" + SPV_Name + "',USER_NAME='" + USER_NAME + "',ClusterName='" + ClusterName + "',PROJECT_CAPACITY_MW='" + PROJECT_CAPACITY_MW + "',PROJECT_LOC='" + PROJECT_LOC + "',PROJECT_DISTRICT='" + PROJECT_DISTRICT + "',GCLetter_No='" + GCLetter_No + "',GCLetter_Date='" + strSCHEDULED_COMM_DATE + "',GCLetter_ValidityDate='" + strValidity_DATE + "',INTERCONNECTION_AT='" + INTERCONNECTION_AT + "',MSEDCL_Tender_No='" + MSEDCL_Tender_No + "',PPA_DETAILS='" + PPA_DETAILS + "',TOTAL_REQUIRED_LAND='" + TOTAL_REQUIRED_LAND + "',LAND_IN_POSSESSION='" + LAND_IN_POSSESSION + "',is_Change_loc='" + isChangeLoc + "',lat='" + lat + "',longt='" + longt + "',PROJECT_TALUKA='" + PROJECT_TALUKA + "' where APPLICATION_NO='" + APPLICATION_NO + "'";
                    }
                    else
                    {
                        strQuery = "update mskvy_applicantdetails_spd set MEDAProjectID='" + MEDAProjectID + "',NAME_OF_SPD='" + NAME_OF_SPD + "',GSTIN_NO='" + GSTIN_NO + "',PAN_NUMBER='" + PAN_NUMBER + "',ADDRESS_FOR_CORRESPONDENCE='" + ADDRESS_FOR_CORRESPONDENCE + "',CONT_PER_NAME_1='" + CONT_PER_NAME_1 + "',CONT_PER_DESIG_1='" + CONT_PER_DESIG_1 + "',CONT_PER_PHONE_1='" + CONT_PER_PHONE_1 + "',CONT_PER_MOBILE_1='" + CONT_PER_MOBILE_1 + "',CONT_PER_EMAIL_1='" + CONT_PER_EMAIL_1 + "',SPV_Name='" + SPV_Name + "',USER_NAME='" + USER_NAME + "',ClusterName='" + ClusterName + "',PROJECT_CAPACITY_MW='" + PROJECT_CAPACITY_MW + "',PROJECT_LOC='" + PROJECT_LOC + "',PROJECT_DISTRICT='" + PROJECT_DISTRICT + "',GCLetter_No='" + GCLetter_No + "',GCLetter_Date='" + strSCHEDULED_COMM_DATE + "',INTERCONNECTION_AT='" + INTERCONNECTION_AT + "',MSEDCL_Tender_No='" + MSEDCL_Tender_No + "',PPA_DETAILS='" + PPA_DETAILS + "',TOTAL_REQUIRED_LAND='" + TOTAL_REQUIRED_LAND + "',LAND_IN_POSSESSION='" + LAND_IN_POSSESSION + "',is_Change_loc='" + isChangeLoc + "',lat='" + lat + "',longt='" + longt + "',PROJECT_TALUKA='" + PROJECT_TALUKA + "' where APPLICATION_NO='" + APPLICATION_NO + "'";
                    }
                }
                else
                {
                    if (txtGCValDt.Text != "")
                    {
                        strQuery = "update mskvy_applicantdetails_spd set MEDAProjectID='" + MEDAProjectID + "',NAME_OF_SPD='" + NAME_OF_SPD + "',GSTIN_NO='" + GSTIN_NO + "',PAN_NUMBER='" + PAN_NUMBER + "',ADDRESS_FOR_CORRESPONDENCE='" + ADDRESS_FOR_CORRESPONDENCE + "',CONT_PER_NAME_1='" + CONT_PER_NAME_1 + "',CONT_PER_DESIG_1='" + CONT_PER_DESIG_1 + "',CONT_PER_PHONE_1='" + CONT_PER_PHONE_1 + "',CONT_PER_MOBILE_1='" + CONT_PER_MOBILE_1 + "',CONT_PER_EMAIL_1='" + CONT_PER_EMAIL_1 + "',SPV_Name='" + SPV_Name + "',USER_NAME='" + USER_NAME + "',ClusterName='" + ClusterName + "',PROJECT_CAPACITY_MW='" + PROJECT_CAPACITY_MW + "',PROJECT_LOC='" + PROJECT_LOC + "',PROJECT_DISTRICT='" + PROJECT_DISTRICT + "',GCLetter_No='" + GCLetter_No + "',GCLetter_ValidityDate='" + strValidity_DATE + "',INTERCONNECTION_AT='" + INTERCONNECTION_AT + "',MSEDCL_Tender_No='" + MSEDCL_Tender_No + "',PPA_DETAILS='" + PPA_DETAILS + "',TOTAL_REQUIRED_LAND='" + TOTAL_REQUIRED_LAND + "',LAND_IN_POSSESSION='" + LAND_IN_POSSESSION + "',is_Change_loc='" + isChangeLoc + "',lat='" + lat + "',longt='" + longt + "',PROJECT_TALUKA='" + PROJECT_TALUKA + "' where APPLICATION_NO='" + APPLICATION_NO + "'";
                    }
                    else
                    {
                        strQuery = "update mskvy_applicantdetails_spd set MEDAProjectID='" + MEDAProjectID + "',NAME_OF_SPD='" + NAME_OF_SPD + "',GSTIN_NO='" + GSTIN_NO + "',PAN_NUMBER='" + PAN_NUMBER + "',ADDRESS_FOR_CORRESPONDENCE='" + ADDRESS_FOR_CORRESPONDENCE + "',CONT_PER_NAME_1='" + CONT_PER_NAME_1 + "',CONT_PER_DESIG_1='" + CONT_PER_DESIG_1 + "',CONT_PER_PHONE_1='" + CONT_PER_PHONE_1 + "',CONT_PER_MOBILE_1='" + CONT_PER_MOBILE_1 + "',CONT_PER_EMAIL_1='" + CONT_PER_EMAIL_1 + "',SPV_Name='" + SPV_Name + "',USER_NAME='" + USER_NAME + "',ClusterName='" + ClusterName + "',PROJECT_CAPACITY_MW='" + PROJECT_CAPACITY_MW + "',PROJECT_LOC='" + PROJECT_LOC + "',PROJECT_DISTRICT='" + PROJECT_DISTRICT + "',GCLetter_No='" + GCLetter_No + "',INTERCONNECTION_AT='" + INTERCONNECTION_AT + "',MSEDCL_Tender_No='" + MSEDCL_Tender_No + "',PPA_DETAILS='" + PPA_DETAILS + "',TOTAL_REQUIRED_LAND='" + TOTAL_REQUIRED_LAND + "',LAND_IN_POSSESSION='" + LAND_IN_POSSESSION + "',is_Change_loc='" + isChangeLoc + "',lat='" + lat + "',longt='" + longt + "',PROJECT_TALUKA='" + PROJECT_TALUKA + "' where APPLICATION_NO='" + APPLICATION_NO + "'";

                    }
                }
                try
                {
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                    saveApp(APPLICATION_NO);
                    //RemoveComment


                    var generateApplicantion = new GenerateApplicantionSPDDoc(Server, MethodBase.GetCurrentMethod(), log, Session["APPID"].ToString(), Session["user_name"].ToString());
                    generateApplicantion.GenerateDoc2();


                    var emailIds = new List<string>();
                    DataSet empDataSet = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"SELECT e.SRNO, e.SAPID, e.EMP_NAME, e.EmpEmailID, e.EmpMobile, e.ROLE_ID FROM empmaster e WHERE e.ROLE_ID IN ({RoleConst.STU},{RoleConst.EE})");
                    emailIds.AddRange(empDataSet.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpEmailID"].ToString())).Select(a => a["EmpEmailID"].ToString().Trim()).Distinct());


                    #region Send Email

                    SendEmail sm = new SendEmail();
                    dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, $"SELECT a.SPV_Name,a.CONT_PER_MOBILE_1,a.CONT_PER_EMAIL_1,a.APPLICATION_NO FROM mskvy_applicantdetails_spd a WHERE a.APPLICATION_NO = '{Session["AppId"]}';");

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

                    if (emailIds.Any()) strTo += emailIds.JoinStrings(";");


                    sm.Send(strTo, strCC, $"MSKVY Application No. {dsResult.Tables[0].Rows[0]["APPLICATION_NO"]} details Updated", strBody);

                    #endregion Send Email

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Application saved sucessfully!!');window.location ='UploadDoc.aspx';", true);
                    Response.Redirect("~/UI/MSKVYSPD/UploadDoc.aspx", false);
                }
                catch (Exception exception)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                    log.Error(ErrorMessage);
                    // Use the exception object to handle all other non-MySql specific errors
                    lblResult.Text = "Some problem during registration.Please try again.";
                }
            }
        }
        public string saveApp(string applicationId)
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
                        strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + applicationId + "'";
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
                        objGetStatusDTO.subProjID = dsResult.Tables[0].Rows[0]["sub_project_code"].ToString();

                        objGetStatusDTO.userName = Session["user_name"].ToString();
                        objGetStatusDTO.statusId = "1";
                        APICall apiCall = new APICall();
                        string str = apiCall.hmacSHA256Checksum("79c016936b0965b0", "?usrName=" + Session["user_name"].ToString() + "&projId=" + txtProjCode.Text + "&entity=MSETCL");

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
        protected void RbIsChangeLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RbIsChangeLoc.SelectedItem.Value == "1")
            {
                txtProjectLocation.Enabled = true;


            }
            else
            {
                txtProjectLocation.Enabled = false;
            }
        }
    }
}