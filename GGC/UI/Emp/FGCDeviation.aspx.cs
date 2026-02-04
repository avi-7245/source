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
using Microsoft.Reporting.WebForms;


namespace GGC.UI.Emp
{
    public partial class FGCDeviation : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FGCDeviation));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtDeviationTrmark.Attributes.Add("maxlength", "200");

            try
            {
                lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
                Session["ApplicationID"] = Request.QueryString["application"];
                lblAppID.Text = Request.QueryString["application"];
                //GeneratXmlForFile();
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);

            }
        }

        //protected void btnConfirm_Click(object sender, EventArgs e)
        //{
        //    string strQuery1 = string.Empty;
        //    if (rbIsDeviation.SelectedItem.Value == "0")
        //    {
        //        strQuery1 = "Update finalgcapproval set WF_STATUS_CD=10, APP_STATUS_DT=CURDATE() , app_status='Uploading of proposal pending',isDeviation=0 where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";

        //        string strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
        //        DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

        //        DataSet dsCommDet = new DataSet();
        //        strQuery = "SELECT * FROM empmaster WHERE role_id=53 and isactive='Y' order by 1";
        //        dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
        //        dsCommDet.Tables[0].TableName = "empDet";
        //        dsResult.Tables.Add(dsCommDet.Tables[0].Copy());




        //        string strSubject = " Approval for Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
        //                            " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 (Project Code : " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online GC Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ") .";

        //        string strData = "In context to above subject, this office has issued Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
        //                            " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + "kV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

        //        string strData2 = "In view of above, Final Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
        //                            " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + "kV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

        //        DataTable RDLCData = new DataTable("RDLCData");

        //        DataColumn dtaColumn;

        //        DataRow myDataRow;

        //        // Create id column

        //        dtaColumn = new DataColumn();
        //        dtaColumn.DataType = typeof(String);
        //        dtaColumn.ColumnName = "subject";
        //        RDLCData.Columns.Add(dtaColumn);

        //        dtaColumn = new DataColumn();
        //        dtaColumn.DataType = typeof(String);
        //        dtaColumn.ColumnName = "MsgData";
        //        RDLCData.Columns.Add(dtaColumn);

        //        dtaColumn = new DataColumn();
        //        dtaColumn.DataType = typeof(String);
        //        dtaColumn.ColumnName = "MsgData2";
        //        RDLCData.Columns.Add(dtaColumn);

        //        myDataRow = RDLCData.NewRow();
        //        myDataRow["subject"] = strSubject;
        //        myDataRow["MsgData"] = strData;
        //        myDataRow["MsgData2"] = strData2;

        //        RDLCData.Rows.Add(myDataRow);


        //        dsResult.Tables.Add(RDLCData.Copy());

        //        DataSet dsLandDet = new DataSet();
        //        strQuery = "SELECT * FROM mskvy_landdet WHERE APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
        //        dsLandDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
        //        dsLandDet.Tables[0].TableName = "LandDet";
        //        dsResult.Tables.Add(dsLandDet.Tables[0].Copy());

        //        //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\TestRDLC\testRDLCWeb\MSKVYGC.xsd");
        //        if (dsResult.Tables[0].Rows.Count > 0)
        //        {
        //            //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
        //            string FileName = "FinalGC" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
        //            string serverFilePath = Server.MapPath("~/Files/MSKVY/FGC/" + Session["ApplicationID"].ToString() + "/");
        //            try
        //            {

        //                LocalReport report = new LocalReport();
        //                report.ReportPath = Server.MapPath("~/PDFReport/") + "FinalGC1.rdlc";


        //                ReportDataSource rds = new ReportDataSource();
        //                rds.Name = "dsGC_Final";//This refers to the dataset name in the RDLC file
        //                rds.Value = dsResult.Tables[0];
        //                report.DataSources.Add(rds);

        //                ReportDataSource rds2 = new ReportDataSource();
        //                rds2.Name = "dsGC_EmpData";//This refers to the dataset name in the RDLC file
        //                rds2.Value = dsResult.Tables["empDet"];
        //                report.DataSources.Add(rds2);

        //                ReportDataSource rds3 = new ReportDataSource();
        //                rds3.Name = "dsGC_Msg";//This refers to the dataset name in the RDLC file
        //                rds3.Value = dsResult.Tables["RDLCData"];
        //                report.DataSources.Add(rds3);

        //                ReportDataSource rds4 = new ReportDataSource();
        //                rds4.Name = "dsGC_LandDet";//This refers to the dataset name in the RDLC file
        //                rds4.Value = dsResult.Tables["LandDet"];
        //                report.DataSources.Add(rds4);


        //                ReportViewer reportViewer = new ReportViewer();
        //                reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "FinalGC1.rdlc";
        //                reportViewer.LocalReport.DataSources.Clear();

        //                reportViewer.LocalReport.DataSources.Add(rds);
        //                reportViewer.LocalReport.DataSources.Add(rds2);
        //                reportViewer.LocalReport.DataSources.Add(rds3);
        //                reportViewer.LocalReport.DataSources.Add(rds4);

        //                // reportViewer.LocalReport.DataSources.Add(rds3);

        //                reportViewer.LocalReport.EnableHyperlinks = true;
        //                reportViewer.LocalReport.Refresh();



        //                string mimeType, encoding, extension, deviceInfo;
        //                string[] streamids;
        //                Warning[] warnings;
        //                string format = "PDF";

        //                deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

        //                byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        //                FileStream fs1 = new FileStream(serverFilePath + FileName, FileMode.Create);
        //                fs1.Write(bytes, 0, bytes.Length);
        //                fs1.Close();
        //                strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + strAppID + "', '" + 16 + "', '" + FileName + "',  '" + strAppID + "')";
        //                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

        //                Response.Clear();
        //                Response.Buffer = true;
        //                Response.Charset = "";
        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                Response.ContentType = "PDF";
        //                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
        //                Response.BinaryWrite(bytes);
        //                Response.Flush();
        //                Response.End();

        //            }
        //            catch (Exception ex)
        //            {
        //                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
        //                //   log.Error(ErrorMessage);

        //            }
        //        }
        //    }
        //    else
        //    {
        //        strQuery1 = "Update finalgcapproval set WF_STATUS_CD=1, APP_STATUS_DT=CURDATE() , app_status='Deviation in the application',isDeviation=1,Deviation_Remark='" + txtDeviationTrmark.Text + "'  where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
        //        string strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
        //        DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

        //        DataSet dsCommDet = new DataSet();
        //        strQuery = "SELECT * FROM empmaster WHERE role_id=53 and isactive='Y' order by 1";
        //        dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
        //        dsCommDet.Tables[0].TableName = "empDet";
        //        dsResult.Tables.Add(dsCommDet.Tables[0].Copy());




        //        string strSubject = " Approval for Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
        //                            " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 (Project Code : " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online GC Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ") .";

        //        string strData = "In context to above subject, this office has issued Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
        //                            " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + "kV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

        //        string strData2 = "In view of above, Final Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
        //                            " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + "kV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

        //        DataTable RDLCData = new DataTable("RDLCData");

        //        DataColumn dtaColumn;

        //        DataRow myDataRow;

        //        // Create id column

        //        dtaColumn = new DataColumn();
        //        dtaColumn.DataType = typeof(String);
        //        dtaColumn.ColumnName = "subject";
        //        RDLCData.Columns.Add(dtaColumn);

        //        dtaColumn = new DataColumn();
        //        dtaColumn.DataType = typeof(String);
        //        dtaColumn.ColumnName = "MsgData";
        //        RDLCData.Columns.Add(dtaColumn);

        //        dtaColumn = new DataColumn();
        //        dtaColumn.DataType = typeof(String);
        //        dtaColumn.ColumnName = "MsgData2";
        //        RDLCData.Columns.Add(dtaColumn);

        //        myDataRow = RDLCData.NewRow();
        //        myDataRow["subject"] = strSubject;
        //        myDataRow["MsgData"] = strData;
        //        myDataRow["MsgData2"] = strData2;

        //        RDLCData.Rows.Add(myDataRow);


        //        dsResult.Tables.Add(RDLCData.Copy());

        //        DataSet dsLandDet = new DataSet();
        //        strQuery = "SELECT * FROM mskvy_landdet WHERE APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
        //        dsLandDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
        //        dsLandDet.Tables[0].TableName = "LandDet";
        //        dsResult.Tables.Add(dsLandDet.Tables[0].Copy());

        //        //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\TestRDLC\testRDLCWeb\MSKVYGC.xsd");
        //        if (dsResult.Tables[0].Rows.Count > 0)
        //        {
        //            //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
        //            string FileName = "FinalGC" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";

        //            string serverFilePath = Server.MapPath("~/Files/MSKVY/FGC/" + Session["ApplicationID"].ToString() + "/");
        //            try
        //            {

        //                LocalReport report = new LocalReport();
        //                report.ReportPath = Server.MapPath("~/PDFReport/") + "FinalGC2.rdlc";


        //                ReportDataSource rds = new ReportDataSource();
        //                rds.Name = "dsGC_Final";//This refers to the dataset name in the RDLC file
        //                rds.Value = dsResult.Tables[0];
        //                report.DataSources.Add(rds);

        //                ReportDataSource rds2 = new ReportDataSource();
        //                rds2.Name = "dsGC_EmpData";//This refers to the dataset name in the RDLC file
        //                rds2.Value = dsResult.Tables["empDet"];
        //                report.DataSources.Add(rds2);

        //                ReportDataSource rds3 = new ReportDataSource();
        //                rds3.Name = "dsGC_Msg";//This refers to the dataset name in the RDLC file
        //                rds3.Value = dsResult.Tables["RDLCData"];
        //                report.DataSources.Add(rds3);

        //                ReportDataSource rds4 = new ReportDataSource();
        //                rds4.Name = "dsGC_LandDet";//This refers to the dataset name in the RDLC file
        //                rds4.Value = dsResult.Tables["LandDet"];
        //                report.DataSources.Add(rds4);


        //                ReportViewer reportViewer = new ReportViewer();
        //                reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "FinalGC2.rdlc";
        //                reportViewer.LocalReport.DataSources.Clear();

        //                reportViewer.LocalReport.DataSources.Add(rds);
        //                reportViewer.LocalReport.DataSources.Add(rds2);
        //                reportViewer.LocalReport.DataSources.Add(rds3);
        //                reportViewer.LocalReport.DataSources.Add(rds4);

        //                // reportViewer.LocalReport.DataSources.Add(rds3);

        //                reportViewer.LocalReport.EnableHyperlinks = true;
        //                reportViewer.LocalReport.Refresh();


        //                string mimeType, encoding, extension, deviceInfo;
        //                string[] streamids;
        //                Warning[] warnings;
        //                string format = "PDF";

        //                deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

        //                byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
        //                FileStream fs1 = new FileStream(serverFilePath + FileName, FileMode.Create);
        //                fs1.Write(bytes, 0, bytes.Length);
        //                fs1.Close();
        //                strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + Session["ApplicationID"].ToString() + "', '" + 17 + "', '" + FileName + "',  '" + strAppID + "')";
        //                SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

        //                Response.Clear();
        //                Response.Buffer = true;
        //                Response.Charset = "";
        //                Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                Response.ContentType = "PDF";
        //                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
        //                Response.BinaryWrite(bytes);
        //                Response.Flush();
        //                Response.End();

        //            }
        //            catch (Exception ex)
        //            {
        //                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
        //                //   log.Error(ErrorMessage);

        //            }
        //        }
        //    }

        //    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery1);

        //}

        protected void btnConfirm_Click(object sender, EventArgs e)
        {


            string strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

            //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\TestRDLC\testRDLCWeb\MSKVYGC.xsd");
            if (dsResult.Tables[0].Rows.Count > 0)
            {
                try
                {
                    //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                    var confirmationDate = DateTime.Now;

                    //FinalGC04_Mar_2406_45_54.pdf
                    string fileName = "FinalGC" + confirmationDate.ToString("dd_MMM_yy") + confirmationDate.ToString("hh_mm_ss") + ".pdf";

                    string serverFilePath = Server.MapPath("~/Files/MSKVY/FGC/" + Session["ApplicationID"].ToString() + "/");


                    DataSet dsCommDet = new DataSet();
                    strQuery = "SELECT * FROM empmaster WHERE role_id=53 and isactive='Y' order by 1";
                    dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                    dsCommDet.Tables[0].TableName = "empDet";
                    dsResult.Tables.Add(dsCommDet.Tables[0].Copy());

                    DataSet dsLandDet = new DataSet();
                    strQuery = "SELECT * FROM mskvy_landdet WHERE APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
                    dsLandDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                    dsLandDet.Tables[0].TableName = "LandDet";
                    dsResult.Tables.Add(dsLandDet.Tables[0].Copy());


                    string updateFinalGCApprovalQuery = string.Empty;


                    string strSubject = " Approval for Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                        " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 (Project Code : " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online GC Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ").";

                    string strData = "In context to above subject, this office has issued Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + "Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                        " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + "kV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

                    string strData2 = "In view of above, Final Grid Connectivity to " + dsResult.Tables[0].Rows[0]["PROJECT_CAPACITY_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                        " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + "kV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

                    var fgcDoc = new GenerateFinalGridConnectivityDoc(Server, serverFilePath + fileName)
                    {
                        Add_correspondence = dsResult.Tables[0].Rows[0]["Add_correspondence"].ToString(),
                        Date = confirmationDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        EMP_NAME = dsResult.Tables["empDet"].Rows[0]["EMP_NAME"].ToString(),
                        SPV_Name = dsResult.Tables[0].Rows[0]["SPV_Name"].ToString(),
                        Subject = strSubject,
                        MsgData = strData,
                        MsgData2 = strData2
                    };


                    var result = fgcDoc.GeneratePdf();

                    if (result.Success)
                    {
                        if (rbIsDeviation.SelectedItem.Value == "0")
                        {
                            updateFinalGCApprovalQuery = "Update finalgcapproval set WF_STATUS_CD=10, APP_STATUS_DT=CURDATE() , app_status='Uploading of proposal pending',isDeviation=0 where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
                            strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + Session["ApplicationID"] + "', '" + 16 + "', '" + fileName + "',  '" + strAppID + "')";
                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                            //RemoveComment 
                        }
                        else
                        {
                            updateFinalGCApprovalQuery = "Update finalgcapproval set WF_STATUS_CD=1, APP_STATUS_DT=CURDATE() , app_status='Deviation in the application',isDeviation=1,Deviation_Remark='" + txtDeviationTrmark.Text + "'  where APPLICATION_NO='" + Session["ApplicationID"].ToString() + "'";
                            strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + Session["ApplicationID"].ToString() + "', '" + 17 + "', '" + fileName + "',  '" + strAppID + "')";
                            SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                            //RemoveComment 
                        }
                    }

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, updateFinalGCApprovalQuery);
                    //RemoveComment

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ApplicationConfirmationAlert", $"alert('APPLICATION {lblAppID.Text} HAS BEEN CONFIRMED'); window.location = '{ResolveUrl("~/UI/Emp/EmpHome.aspx")}';", true);
                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    log.Error(ErrorMessage);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "AlertApplicationNotReceived", $"alert('APPLICATION {lblAppID.Text} IS NOT RECEIVED')", true);
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            SendEmail sm = new SendEmail();

            string strBody = string.Empty;

            string strQuery = "select * from mskvy_applicantdetails_spd where application_no='" + Session["ApplicationID"] + "'";
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);


            strBody = "M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " , <br/>" +
                "Your Final Grid connectivity application for application no " + Session["APPID"].ToString() + " has been returned due to below reason <br/>" + txtDeviationTrmark.Text;
            //strBody += "<b>" + dsDocName.Tables[0].Rows[0]["DOC_NAME"].ToString() + "</b><br/>";
            strBody += "State Transmission Utility (STU)" + "<br/>";
            strBody += "MSETCL  " + "<br/>";
            strBody += "[This is an System-generated response. Kindly do not reply on this mail.]" + "\n";

            sm.Send(dsResult.Tables[0].Rows[0]["CONT_PER_EMAIL_1"].ToString(), ConfigurationManager.AppSettings["MMCCEmailID"].ToString(), "Final GC document Returned", strBody);
            //RemoveComment

        }
    }
}