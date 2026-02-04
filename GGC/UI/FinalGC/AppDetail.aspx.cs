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
using System.Web.Hosting;

namespace GGC.UI.FinalGC
{
    public partial class AppDetail : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(AppDetail));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string strMEDAPrjID=txtProjCode.Text;

            string strQuery = string.Empty;

            
           
            try
            {
                strQuery = "select * from mskvy_applicantdetails_SPD where APPLICATION_NO='" + strMEDAPrjID + "'";
                DataSet dsResult = new DataSet(); 
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                txtNameOfPromoter.Text = dsResult.Tables[0].Rows[0]["NAME_OF_SPD"].ToString();
                txtAppNo.Text = dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                txtProjectCapacity.Text = dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString();
                txtClusterName.Text = dsResult.Tables[0].Rows[0]["ClusterName"].ToString();
                txtContPer1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_NAME_1"].ToString();
                txtMob1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString();
                txtEmail1.Text = dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString();
                txtNameofSPV.Text = dsResult.Tables[0].Rows[0]["SPV_Name"].ToString();
                txtTaluka.Text = dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString();
                txtCorrAdd.Text = dsResult.Tables[0].Rows[0]["ADDRESS_FOR_CORRESPONDENCE"].ToString();
                txtProjectLocation.Text = dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString();
                txtDistrict.Text = dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString();
                txtNameOfEHV.Text = dsResult.Tables[0].Rows[0]["INTERCONNECTION_AT"].ToString();
                Session["USER_NAME"] = dsResult.Tables[0].Rows[0]["USER_NAME"].ToString();

                DataSet dsResultFGC = new DataSet();
                strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + strMEDAPrjID + "'";
                dsResultFGC = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                if (dsResultFGC.Tables[0].Rows.Count > 0)
                {

                    txtLattitude.Text = dsResultFGC.Tables[0].Rows[0]["Latitude"].ToString();
                    txtLongitude.Text = dsResultFGC.Tables[0].Rows[0]["Longitude"].ToString();
                    txtNameofTransm.Text = dsResultFGC.Tables[0].Rows[0]["NAME_TRANS_LICENSEE"].ToString();
                    txtVoltageInter.Text = dsResultFGC.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString();
                    txtVoltageInter.Text = dsResultFGC.Tables[0].Rows[0]["VOLT_LEVEL_SUBSTATION"].ToString();
                    //txtDetailsInter.Text = dsResultFGC.Tables[0].Rows[0]["POINT_OF_INJECTION"].ToString();
                    txtGenVoltStep.Text = dsResultFGC.Tables[0].Rows[0]["GENERATION_VOLTAGE"].ToString();
                    //txtDetailsOfFeeder.Text = dsResultFGC.Tables[0].Rows[0]["DET_FEEDER_PROT"].ToString();

                }
                

            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                lblResult.Text = "Some problem during registration.Please try again.";
            }
        
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           // string strMEDAPrjID = txtProjCode.Text;
            //string strAppID = txtAppNo.Text;
            string strMEDAPrjID = txtAppNo.Text;
            string strAppID = txtProjCode.Text;
            string strLat = txtLattitude.Text;
            string strLong = txtLongitude.Text;
            string strNAME_OF_TRAN_LIC = txtNameofTransm.Text;
            string strNAME_OF_EHV_SS = txtNameOfEHV.Text;
            string strVOLT_LVL_INTER = txtVoltageInter.Text;
            string strDET_OF_INTER_CON = txtDetailsInter.Text;
            string strGEN_VOLT_STEPUP_VOLT = txtGenVoltStep.Text;
            string strDET_FEEDER_PROT = txtDetailsOfFeeder.Text;
            string strQuery = "select * from finalgcapproval where APPLICATION_NO='" + strAppID + "'";

            DataSet dsResultFGC = new DataSet();
            dsResultFGC = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            if (dsResultFGC.Tables[0].Rows.Count > 0)
            {
                strQuery = "UPDATE finalgcapproval SET user_name='" + Session["USER_NAME"].ToString() + "', LAT='" + strLat + "',Longt='" + strLong + "',NAME_OF_TRAN_LIC='" + strNAME_OF_TRAN_LIC + "',VOLT_LVL_INTER='" + strVOLT_LVL_INTER + "',DET_OF_INTER_CON='" + strDET_OF_INTER_CON + "',GEN_VOLT_STEPUP_VOLT='" + strGEN_VOLT_STEPUP_VOLT + "',DET_FEEDER_PROT='" + strDET_FEEDER_PROT + "' WHERE APPLICATION_NO='" + strAppID + "'";
            }
            else
            {
                strQuery = "INSERT INTO finalgcapproval(APPLICATION_NO, MEDAProjectID, LAT, Longt, NAME_OF_TRAN_LIC, NAME_OF_EHV_SS, VOLT_LVL_INTER, DET_OF_INTER_CON, GEN_VOLT_STEPUP_VOLT, DET_FEEDER_PROT, WF_STATUS_CD,APP_STATUS,APP_STATUS_DT,createBy, createDT,user_name) VALUES" +
                                  "('" + strAppID + "','" + strMEDAPrjID + "','" + strLat + "','" + strLong + "','" + strNAME_OF_TRAN_LIC + "','" + strNAME_OF_EHV_SS + "','" + strVOLT_LVL_INTER + "','" + strDET_OF_INTER_CON + "','" + strGEN_VOLT_STEPUP_VOLT + "','" + strDET_FEEDER_PROT + "',1,'APPLICATION DATA RECEIVED',now(),'" + strMEDAPrjID + "',now(),'" + Session["USER_NAME"].ToString() + "')";
            }
            try
            {
                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Application saved.');window.location ='AppComm.aspx';", true);
                Response.Redirect("~/UI/FinalGC/AppComm.aspx?projectID=" + strMEDAPrjID + "&appId=" + strAppID, false);
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
}