using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using GGC.Scheduler;
using log4net;
using Microsoft.Reporting.WebForms;
using MySql.Data.MySqlClient;

namespace GGC.Common
{
    public class GenerateApplicationMSKVYDoc
    {

        readonly string _connectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        readonly HttpServerUtility _server;
        readonly MethodBase _method;
        readonly ILog _log;
        readonly string _applicationId;
        readonly string _strUserName;
        const string dirPath = "~/Files/MSKVY/";

        public GenerateApplicationMSKVYDoc(HttpServerUtility server, MethodBase method, ILog log, string applicationId, string strUserName)
        {
            _server = server;
            _method = method;
            _log = log;
            _applicationId = applicationId;
            _strUserName = strUserName;

            if (!Directory.Exists(server.MapPath(dirPath + applicationId + "/")))
            {
                Directory.CreateDirectory(server.MapPath(dirPath + applicationId + "/"));
            }
        }

        public FileResult GenerateDoc()
        {
            var result = new FileResult();
            using (var connection = new MySqlConnection(_connectionString))
            {
                var dsResult = new DataSet();
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;

                    command.CommandText = "select * ,date_format(APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT,date_format(SCHEDULED_COMM_DATE,'%Y-%m-%d') SCHEDULED_COMM_DATE_F  from mskvy_applicantdetails where APPLICATION_NO='" + _applicationId + "'";
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

                    command.CommandText = "select * from billdesk_txn where ApplicationNo='" + _applicationId + "' and AuthStatus='0300' and typeofpay='MSKVYRegistration'";
                    using (var da = new MySqlDataAdapter(command))
                    {
                        DataSet dsReceipt = new DataSet();
                        da.Fill(dsReceipt);
                        dsReceipt.Tables[0].TableName = "TableReceipt";
                        dsResult.Tables.Add(dsReceipt.Tables[0].Copy());
                    }

                    string FileName = "ApplicationForm_" + _strUserName + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                    string serverFilePath = _server.MapPath(dirPath + _applicationId + "/") + FileName;

                    try
                    {
                        LocalReport report = new LocalReport();
                        report.ReportPath = _server.MapPath("~/PDFReport/") + "Application_MSKVY.rdlc";

                        ReportDataSource rds = new ReportDataSource();
                        rds.Name = "dsApplication_AppDet";
                        rds.Value = dsResult.Tables[0];
                        report.DataSources.Add(rds);

                        ReportDataSource rds2 = new ReportDataSource();
                        rds2.Name = "dsApplication_AppRegDet";
                        rds2.Value = dsResult.Tables["AppRegDet"];
                        report.DataSources.Add(rds2);

                        ReportDataSource rds3 = new ReportDataSource();
                        rds3.Name = "DataSetReceipt";
                        rds3.Value = dsResult.Tables["TableReceipt"];
                        report.DataSources.Add(rds3);

                        ReportViewer reportViewer = new ReportViewer();
                        reportViewer.LocalReport.ReportPath = _server.MapPath("~/PDFReport/") + "Application_MSKVY.rdlc";
                        reportViewer.LocalReport.DataSources.Clear();

                        reportViewer.LocalReport.DataSources.Add(rds);
                        reportViewer.LocalReport.DataSources.Add(rds2);
                        reportViewer.LocalReport.DataSources.Add(rds3);

                        reportViewer.LocalReport.Refresh();
                        string mimeType, encoding, extension, deviceInfo;
                        string[] streamids;
                        Warning[] warnings;
                        string format = "PDF";

                        deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                        result.Data = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                        FileStream fs1 = new FileStream(serverFilePath, FileMode.Create);
                        fs1.Write(result.Data, 0, result.Data.Length);
                        fs1.Close();
                        result.Success = true;

                        command.CommandText = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + _applicationId + "', 1, '" + FileName + "',  '" + _applicationId + "')";
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        string ErrorMessage = "Method Name: " + _method.Name + " | Description: " + ex.Message + " " + ex.InnerException;
                        _log.Error(ErrorMessage);
                        result.Success = false;
                    }
                }
            }
            return result;
        }
    }
}