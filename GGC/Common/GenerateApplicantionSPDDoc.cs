using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web;
using log4net;
using Microsoft.Reporting.WebForms;
using MySql.Data.MySqlClient;

namespace GGC.Common
{
    public class GenerateApplicantionSPDDoc
    {
        readonly string _connectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        readonly HttpServerUtility _server;
        readonly MethodBase _method;
        readonly ILog _log;
        readonly string _applicationId;
        readonly string _strUserName;
        const string dirPath = "~/Files/MSKVY/";

        public GenerateApplicantionSPDDoc(HttpServerUtility server, MethodBase method, ILog log, string applicationId, string strUserName)
        {
            _server = server;
            _method = method;
            _log = log;
            _applicationId = applicationId;
            _strUserName = strUserName;
        }

        public FileResult GenerateDoc(bool fileNameSaveInDb = true)
        {
            var result = new FileResult();
            using (var connection = new MySqlConnection(_connectionString))
            {
                var dsResult = new DataSet();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    //command.CommandText = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from mskvy_applicantdetails_spd where APPLICATION_NO='" + _applicationId + "'";
                    command.CommandText = "SELECT maspd.MEDAProjectID" +
                    ", maspd.APPLICATION_NO" +
                    ", maspd.GSTIN_NO" +
                    ", maspd.CONT_PER_NAME_1" +
                    ", maspd.CONT_PER_DESIG_1" +
                    ", maspd.CONT_PER_PHONE_1" +
                    ", maspd.CONT_PER_MOBILE_1" +
                    ", maspd.CONT_PER_EMAIL_1" +
                    ", maspd.PROJECT_TYPE" +
                    ", maspd.PROJECT_CAPACITY_MW" +
                    ", maspd.PROJECT_LOC" +
                    ", maspd.PROJECT_TALUKA" +
                    ", maspd.PROJECT_DISTRICT" +
                    ", maspd.TOTAL_REQUIRED_LAND" +
                    ", maspd.LAND_IN_POSSESSION" +
                    ", maspd.TOTAL_FOREST_LAND" +
                    ", maspd.STATUS_FOREST_LAND" +
                    ", maspd.BIRD_SANCTURY_ETC" +
                    ", ma.PROMOTOR_NAME" +
                    ", ma.NATURE_OF_APP" +
                    ", ma.SCHEDULED_COMM_DATE" +
                    ", ma.Latitude" +
                    ", ma.Longitude" +
                    ", ma.IsAddCapacity" +
                    ", ma.GCApprLetterNo_Date" +
                    ", ma.NAME_TRANS_LICENSEE" +
                    ", ma.INJECTION_VOLTAGE" +
                    ", ma.POINT_OF_INJECTION" +
                    ", ma.DISTANCE_FROM_PLANT" +
                    ", ma.VOLT_LEVEL_SUBSTATION" +
                    ", DATE_FORMAT(maspd.APP_STATUS_DT,'%d-%m-%Y') APP_STATUS_DT" +
                    " FROM mskvy_applicantdetails_spd maspd " +
                    " LEFT JOIN mskvy_applicantdetails ma ON ma.APPLICATION_NO = maspd.APPLICATION_NO AND ma.MEDAProjectID = maspd.MEDAProjectID" +
                    $" WHERE ma.APPLICATION_NO='{_applicationId}'";

                    using (var da = new MySqlDataAdapter(command))
                    {   
                        da.Fill(dsResult);
                    }

                    command.CommandText = "select * from applicant_reg_det where USER_NAME='" + _strUserName + "'";
                    using (var da = new MySqlDataAdapter(command))
                    {
                        DataSet dsRegDet = new DataSet();
                        da.Fill(dsRegDet);
                        dsRegDet.Tables[0].TableName = "AppRegDet";
                        dsResult.Tables.Add(dsRegDet.Tables[0].Copy());
                    }

                    command.CommandText = "select * from billdesk_txn where ApplicationNo='" + _applicationId + "' and AuthStatus='0300' and typeofpay='MSKVYSPDRegistration'";
                    using (var da = new MySqlDataAdapter(command))
                    {
                        DataSet dsReceipt = new DataSet();
                        da.Fill(dsReceipt);
                        dsReceipt.Tables[0].TableName = "TableReceipt";
                        dsResult.Tables.Add(dsReceipt.Tables[0].Copy());
                    }

                    command.CommandText = "SELECT * FROM new_terms_condition WHERE id = (SELECT MAX(id) FROM new_terms_condition) LIMIT 1";
                    using (var da = new MySqlDataAdapter(command))
                    {
                        DataSet dsReceipt = new DataSet();
                        da.Fill(dsReceipt);
                        dsReceipt.Tables[0].TableName = "TermsConditions";
                        dsResult.Tables.Add(dsReceipt.Tables[0].Copy());
                    }

                    string FileName = "ApplicationForm_" + _strUserName + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                    //string serverFilePath = _server.MapPath("~/Reports/") + FileName;
                    string serverFilePath = _server.MapPath(dirPath + _applicationId + "/") + FileName;
                    try
                    {

                        LocalReport report = new LocalReport();
                        report.ReportPath = _server.MapPath("~/PDFReport/") + "Application.rdlc";

                        ReportDataSource rds = new ReportDataSource();
                        rds.Name = "dsApplication_AppDet";//This refers to the dataset name in the RDLC file
                        rds.Value = dsResult.Tables[0];
                        report.DataSources.Add(rds);

                        ReportDataSource rds2 = new ReportDataSource();
                        rds2.Name = "dsApplication_AppRegDet";//This refers to the dataset name in the RDLC file
                        rds2.Value = dsResult.Tables["AppRegDet"];
                        report.DataSources.Add(rds2);

                        ReportDataSource rds3 = new ReportDataSource();
                        rds3.Name = "TableReceipt";//This refers to the dataset name in the RDLC file
                        rds3.Value = dsResult.Tables["TableReceipt"];
                        report.DataSources.Add(rds3);

                        ReportDataSource rds4 = new ReportDataSource();
                        rds4.Name = "TermsConditions";//This refers to the dataset name in the RDLC file
                        rds4.Value = dsResult.Tables["TermsConditions"];
                        report.DataSources.Add(rds4);


                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.LocalReport.ReportPath = _server.MapPath("~/PDFReport/") + "Application.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();

                        reportViewer.LocalReport.DataSources.Add(rds);
                        reportViewer.LocalReport.DataSources.Add(rds2);
                        reportViewer.LocalReport.DataSources.Add(rds3);
                        reportViewer.LocalReport.DataSources.Add(rds4);

                        reportViewer.LocalReport.Refresh();

                        string mimeType, encoding, extension, deviceInfo;
                        string[] streamids;
                        Warning[] warnings;
                        string format = "PDF";

                        deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                        result.Success = true;
                        result.Data = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                        FileStream fs = new FileStream(serverFilePath, FileMode.Create);
                        fs.Write(result.Data, 0, result.Data.Length);
                        fs.Close();
                        result.FileName = FileName;
                        if (fileNameSaveInDb)
                        {
                            command.CommandText = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + _applicationId + "', 6, '" + FileName + "',  '" + _applicationId + "')";
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.Message;
                        string ErrorMessage = "Method Name: " + _method.Name + " | Description: " + ex.Message + " " + ex.InnerException;
                        _log.Error(ErrorMessage);
                        result.Success = false;
                    }
                }
                return result;
            }
        }


        public FileResult GenerateDoc2(bool fileNameSaveInDb = true)
        {
            var result = new FileResult();
            using (var connection = new MySqlConnection(_connectionString))
            {
                var dsResult = new DataSet();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    //command.CommandText = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from mskvy_applicantdetails_spd where APPLICATION_NO='" + _applicationId + "'";
                    command.CommandText = "SELECT mas.MEDAProjectID" +
                    ",mas.APPLICATION_NO" +
                    ",mas.NAME_OF_SPD" +
                    ",mas.GSTIN_NO" +
                    ",mas.PAN_NUMBER" +
                    ",mas.CONT_PER_NAME_1" +
                    ",mas.CONT_PER_DESIG_1" +
                    ",mas.CONT_PER_PHONE_1" +
                    ",mas.CONT_PER_MOBILE_1" +
                    ",mas.CONT_PER_EMAIL_1" +
                    ",mas.ADDRESS_FOR_CORRESPONDENCE" +
                    ",mas.is_Change_loc" +
                    ",mas.PROJECT_CAPACITY_MW" +
                    ",mas.PROJECT_LOC" +
                    ",mas.PROJECT_TALUKA" +
                    ",mas.PROJECT_DISTRICT" +
                    ",mas.lat" +
                    ",mas.longt" +
                    ",mas.INTERCONNECTION_AT" +
                    ",mas.GCLetter_Date" +
                    ",mas.MSEDCL_Tender_No" +
                    ",mas.PPA_DETAILS" +
                    ",mas.TOTAL_REQUIRED_LAND" +
                    ",mas.LAND_IN_POSSESSION" +
                    ",mas.IS_FOREST_LAND" +
                    ",mas.STATUS_FOREST_LAND" +
                    ",mas.BIRD_SANCTURY_ETC" +
                    ",mas.SPV_Name" +
                    ",bt.TxnAmount" +
                    ",bt.TxnDate" +
                    ",bt.TxnNo" +
                    " FROM mskvy_applicantdetails_spd mas, billdesk_txn bt" +
                    " WHERE mas.APPLICATION_NO = bt.ApplicationNo" +
                    $" AND mas.APPLICATION_NO = '{_applicationId}'" +
                    " AND bt.AuthStatus = '0300'" +
                    " AND bt.typeofpay = 'MSKVYSPDRegistration';";

                    using (var da = new MySqlDataAdapter(command))
                    {   
                        da.Fill(dsResult);
                    }

                    string FileName = "Form_" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";

                    string serverFilePath = _server.MapPath(dirPath + _applicationId + "/") + FileName;
                    try
                    {

                        LocalReport report = new LocalReport();
                        report.ReportPath = _server.MapPath("~/PDFReport/") + "Application_SPD.rdlc";

                        ReportDataSource rds = new ReportDataSource();
                        rds.Name = "ds_Table";//This refers to the dataset name in the RDLC file
                        rds.Value = dsResult.Tables[0];
                        report.DataSources.Add(rds);

                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.LocalReport.ReportPath = _server.MapPath("~/PDFReport/") + "Application_SPD.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();

                        reportViewer.LocalReport.DataSources.Add(rds);
                        reportViewer.LocalReport.Refresh();


                        string mimeType, encoding, extension, deviceInfo;
                        string[] streamids;
                        Warning[] warnings;
                        string format = "PDF";

                        deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                        result.Success = true;
                        result.Data = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                        FileStream fs = new FileStream(serverFilePath, FileMode.Create);
                        fs.Write(result.Data, 0, result.Data.Length);
                        fs.Close();

                        result.FileName = FileName;

                        if (fileNameSaveInDb)
                        {
                            command.CommandText = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + _applicationId + "', 6, '" + FileName + "',  '" + _applicationId + "')";
                            command.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        result.ErrorMessage = ex.Message;
                        string ErrorMessage = "Method Name: " + _method.Name + " | Description: " + ex.Message + " " + ex.InnerException;
                        _log.Error(ErrorMessage);
                        result.Success = false;
                    }
                }
                return result;
            }
        }

    }
}