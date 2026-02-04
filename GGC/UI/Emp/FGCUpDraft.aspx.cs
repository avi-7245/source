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
    public partial class FGCUpDraft : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(FGCUpDraft));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {


            try
            {
                lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
                strAppID = Request.QueryString["application"];
                Session["ApplicationID"] = Request.QueryString["application"];
                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from finalgcapproval where application_no='" + strAppID + "'");
                string strIsDev = dsResult.Tables[0].Rows[0]["isDeviation"].ToString();

                if (strIsDev == "1")
                {
                    rbIsDeviation.Items.FindByValue("1").Selected = true;
                }
                else
                {
                    rbIsDeviation.Items[1].Selected = true;
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);

            }
        }
        //void generatePDF(string strAppId,string isDev)
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

                    strQuery = "INSERT INTO fgc_app_doc ( Application_No, Doc_Type, File_Name, Create_By) VALUES ( '" + strAppID + "', 15, '" + newFileName + "',  '" + strAppID + "')";

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);


                    strQuery = "update finalgcapproval set WF_STATUS_CD=13, app_status='Proposal sent for approval.' where Application_No='" + strAppID + "'";

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);


                    objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "13");

                    strQuery = "insert into proposalapproval_FGC(APPLICATION_NO , roleid , createDT ,createBy) values ('" + strAppID + "','51', now() , '" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                    strQuery = "insert into proposalapprovaltxn_spd(APPLICATION_NO , isAppr_Rej_Ret, Aprove_Reject_Return_by,roleid,createDT ,createBy) values ('" + strAppID + "','Y','" + Session["SAPID"].ToString() + "', '51', now() , '" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                    //generatePDF();
                    #region Application Status tracking date
                    strQuery = "insert into APPLICANT_STATUS_TRACKING (APPLICATION_NO,STATUS,STATUS_DT,Created_By)" +
                            " values('" + strAppID + "','FGC draft document uploaded.',now(),'" + Session["SAPID"].ToString() + "')";

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    #endregion

                    lblResult.Text = "Draft letter of FGC uploaded successfully!!";

                    lblResult.ForeColor = System.Drawing.Color.Green;

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

                        log.Error("Info : Method Name: " + MethodBase.GetCurrentMethod().Name + $" | Description: {strURL}");

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

                        string ErrorMessage = "Info : Method Name: " + MethodBase.GetCurrentMethod().Name + $" | Description: {strURL} : " + dataString;
                        log.Error(ErrorMessage);

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