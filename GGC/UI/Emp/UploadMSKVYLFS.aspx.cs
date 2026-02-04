using System;
using System.IO;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;
using System.Net;
using log4net;
using System.Reflection;
using GGC.Common;
using GGC.Scheduler;
using System.Linq;

namespace GGC.UI.Emp
{
    public partial class UploadMSKVYLFS : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(UploadMSKVYLFS));
        string strAppID;
        SaveToMEDA objSaveToMEDA = new SaveToMEDA();
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
                strAppID = Request.QueryString["application"];
                Session["APPID"] = strAppID;
                //GeneratXmlForFile();
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                //lblOtherMsg.Text = ErrorMessage;
            }
        }

        protected void btnUploadDoc_Click(object sender, EventArgs e)
        {
            strAppID = Session["APPID"].ToString();
            string folderPath = Server.MapPath("~/Files/MSKVY/" + strAppID + "/");

            string newFileName = "";
            if (Session["IsValid"] != "false")
            {
                //Check whether Directory (Folder) exists.
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                if (FUDoc.HasFile)
                {
                    MySqlConnection mySqlConnection = new MySqlConnection();
                    //Save the File to the Directory (Folder).
                    try
                    {

                        FUDoc.SaveAs(folderPath + Path.GetFileName(FUDoc.FileName));
                        newFileName = strAppID + "_" + "_1_" + DateTime.Now.ToString().Replace("/", "").Replace(" ", "").Replace(":", "").ToString() + "_" + FUDoc.FileName;
                        System.IO.File.Move(folderPath + FUDoc.FileName, folderPath + newFileName);

                        mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();



                        mySqlConnection.Open();


                        switch (mySqlConnection.State)
                        {

                            case System.Data.ConnectionState.Open:
                                string strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=11, docLFS='" + newFileName + "' , APP_STATUS_DT=CURDATE() , app_status='Load Flow Study Uploaded.',STU_INJECTION_VOLTAGE='" + txtInject.Text + "',STU_POINT_OF_INJECT='" + txtPonit.Text + "' where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                //RemoveComment

                                strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + strAppID + "', 2, '" + newFileName + "',  '" + strAppID + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                //RemoveComment



                                lblResult.Text = "Load Flow Study Uploaded Successfully!!";
                                lblResult.ForeColor = System.Drawing.Color.Green;

                                #region Application Status tracking date
                                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                        " values('" + strAppID + "','Load Flow Study Uploaded.',now(),'" + Session["SAPID"].ToString() + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                //RemoveComment
                                //GeneratXmlForFile(newFileName);
                                #endregion


                                #region Save To MEDA
                                objSaveToMEDA.saveMSKVYApp(Session["PROJID"].ToString(), "11");
                                //RemoveComment
                                #endregion




                                //Commented As Per Requirement 27.03.2024
                                //#region Send SMS 

                                //DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from MSKVY_applicantdetails  where application_no='" + strAppID + "'");
                                //DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53)");
                                //string strMobNos = string.Empty;
                                //strMobNos = dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() + ",";
                                //try
                                //{
                                //    //log.Error("1 ");
                                //    if (dsEmailMob.Tables[0].Rows.Count > 0)
                                //    {



                                //        if (dsEmailMob.Tables[0].Rows.Count > 0)
                                //        {
                                //            //var mobileNumbers = dsEmailMob.Tables[0].AsEnumerable().Select(a => a["EmpMobile"].ToString()).Distinct();

                                //            //if (mobileNumbers.Any())
                                //            //{
                                //            //    SMS.Send(message: "", string.Join(",", mobileNumbers), MethodBase.GetCurrentMethod(), log);
                                //            //}


                                //            for (int i = 0; i < dsEmailMob.Tables[0].Rows.Count; i++)
                                //            {
                                //                if (dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() != "")
                                //                    strMobNos += dsEmailMob.Tables[0].Rows[i]["EmpMobile"].ToString() + ",";
                                //            }

                                //        }
                                //    }
                                //    //string strURL = ConfigurationManager.AppSettings["SMSURL"].ToString() + "?SenderId=" + ConfigurationManager.AppSettings["SMSSENDER"].ToString() + "&Message=Load%20Flow%20studies%20are%20completed%20for%20your%20application%20no." + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ".%20The%20proposal%20for%20Grid%20Connectivity%20is%20under%20approval.%20Regards%2C%20C.E.%20STU%2C%20MSETCL&MobileNumbers=" + strMobNos.Remove(strMobNos.Length - 1, 1) + "&ApiKey=" + ConfigurationManager.AppSettings["SMSAPIKEY"].ToString() + "&ClientId=" + ConfigurationManager.AppSettings["SMSCLIENTID"].ToString();
                                //    ////log.Error("strURL " + strURL);

                                //    //WebRequest request = HttpWebRequest.Create(strURL);
                                //    ////log.Error("2 ");
                                //    //// Get the response back  
                                //    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                //    //Stream s = (Stream)response.GetResponseStream();
                                //    //StreamReader readStream = new StreamReader(s);
                                //    //string dataString = readStream.ReadToEnd();
                                //    //log.Error("8 " + dataString);

                                //    //response.Close();
                                //    //s.Close();
                                //    //readStream.Close();
                                //    //RemoveComment


                                //    SMS.Send(message: SMSTemplates.DeemedGCApproved(applicationNo: dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(), spvName: dsResult.Tables[0].Rows[0]["SPV_Name"].ToString()), strMobNos.Remove(strMobNos.Length - 1, 1), MethodBase.GetCurrentMethod(), log);
                                //}
                                //catch (Exception ex)
                                //{
                                //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                                //    log.Error(ErrorMessage);
                                //}
                                //#endregion

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
                        lblResult.Text = "Letter Uploaded Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblResult.Text = "Letter Uploaded Failed!!";
                    }

                    finally
                    {
                        if (mySqlConnection.State != System.Data.ConnectionState.Closed)
                        {
                            mySqlConnection.Close();
                        }

                    }
                    generateDeemedGC(strAppID);

                }



                //Display the Picture in Image control.
                //ImgPhoto.ImageUrl = "~/Files/Photos/" + newFileName;
            }
        }
        void generateDeemedGC(string strAppID)
        {
            string folderPath = Server.MapPath("~/Files/MSKVY/" + strAppID + "/");
            string FileName = string.Empty;
            string strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + strAppID + "'";
            DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

            DataSet dsCommDet = new DataSet();
            strQuery = "SELECT * FROM empmaster WHERE role_id=53 and isactive='Y' order by 1";
            dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            dsCommDet.Tables[0].TableName = "empDet";
            dsResult.Tables.Add(dsCommDet.Tables[0].Copy());


            DataSet dsFWC = new DataSet();
            strQuery = $"SELECT z.zone_name FROM zone_circle_district_map z WHERE dist_name = '{dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"]}' LIMIT 1;";
            dsFWC = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
            if (dsFWC.Tables.Count > 0 && dsFWC.Tables[0].Rows.Count > 0)
            {
                DataTable zoneTable = dsFWC.Tables[0].Copy();
                zoneTable.TableName = "ZoneData";
                dsResult.Tables.Add(zoneTable);
            }

            string strSubject = "Approval for Deemed Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + " Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 (Project Code : " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online GC Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ") .";
            string strData = "In view of above, as approved by competent authority, In-principle Deemed Grid Connectivity for your " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                                " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + " KV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

            DataTable RDLCData = new DataTable("RDLCData");

            DataColumn dtaColumn;

            DataRow myDataRow;

            // Create id column

            dtaColumn = new DataColumn();

            dtaColumn.DataType = typeof(String);

            dtaColumn.ColumnName = "subject";



            RDLCData.Columns.Add(dtaColumn);
            dtaColumn = new DataColumn();

            dtaColumn.DataType = typeof(String);

            dtaColumn.ColumnName = "MsgData";

            RDLCData.Columns.Add(dtaColumn);

            myDataRow = RDLCData.NewRow();
            myDataRow["subject"] = strSubject;

            myDataRow["MsgData"] = strData;
            RDLCData.Rows.Add(myDataRow);


            dsResult.Tables.Add(RDLCData.Copy());

            // dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\TestRDLC\testRDLCWeb\Deemed.xsd");
            if (dsResult.Tables[0].Rows.Count > 0)
            {
                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                FileName = "DeemGC" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";
                //string serverFilePath = Server.MapPath("~/Files/") + FileName;
                if (!Directory.Exists(folderPath))
                {
                    //If Directory (Folder) does not exists Create it.
                    Directory.CreateDirectory(folderPath);
                }
                try
                {

                    //LocalReport report = new LocalReport();
                    //report.ReportPath = Server.MapPath("~/PDFReport/") + "RDLCDeem.rdlc";


                    //ReportDataSource rds = new ReportDataSource();
                    //rds.Name = "dsDeemed_Table";//This refers to the dataset name in the RDLC file
                    //rds.Value = dsResult.Tables[0];
                    //report.DataSources.Add(rds);

                    //ReportDataSource rds2 = new ReportDataSource();
                    //rds2.Name = "dsDeemed_EmpData";//This refers to the dataset name in the RDLC file
                    //rds2.Value = dsResult.Tables["empDet"];
                    //report.DataSources.Add(rds2);

                    //ReportDataSource rds3 = new ReportDataSource();
                    //rds3.Name = "dsDeemed_Data";//This refers to the dataset name in the RDLC file
                    //rds3.Value = dsResult.Tables["RDLCData"];
                    //report.DataSources.Add(rds3);



                    //ReportViewer reportViewer = new ReportViewer();
                    //reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "RDLCDeem.rdlc";
                    //reportViewer.LocalReport.DataSources.Clear();

                    //reportViewer.LocalReport.DataSources.Add(rds);
                    //reportViewer.LocalReport.DataSources.Add(rds2);
                    //reportViewer.LocalReport.DataSources.Add(rds3);

                    //// reportViewer.LocalReport.DataSources.Add(rds3);

                    //reportViewer.LocalReport.EnableHyperlinks = true;
                    //reportViewer.LocalReport.Refresh();


                    //string mimeType, encoding, extension, deviceInfo;
                    //string[] streamids;
                    //Warning[] warnings;
                    //string format = "PDF";

                    //deviceInfo = "<DeviceInfo><OutputFormat>PDF</OutputFormat><PageWidth>8.5in</PageWidth><PageHeight>11in</PageHeight> <MarginTop>0.1in</MarginTop><MarginLeft>1.1in</MarginLeft><MarginRight>0.1in</MarginRight><MarginBottom>0.1in</MarginBottom></DeviceInfo>";

                    //byte[] bytes = reportViewer.LocalReport.Render(format, deviceInfo, out mimeType, out encoding, out extension, out streamids, out warnings);
                    //FileStream fs1 = new FileStream(folderPath + FileName, FileMode.Create);
                    //fs1.Write(bytes, 0, bytes.Length);
                    //fs1.Close();

                    var doc = new GenerateDeemGCDoc(Server)
                    {
                        FilePath = folderPath + FileName,
                        Subject = strSubject,
                        MsgData = strData,
                        ApprovalDate = ((DateTime)dsResult.Tables[0].Rows[0]["APP_STATUS_DT"]).ToString(format: "dd/MM/yyyy"),
                        SpvName = dsResult.Tables[0].Rows[0]["SPV_Name"].ToString(),
                        AddCorrespondence = dsResult.Tables[0].Rows[0]["Add_correspondence"].ToString(),
                        ApplicationNo = dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString(),
                        EmpName = dsResult.Tables["empDet"].Rows[0]["EMP_NAME"].ToString(),
                        QuantumPowerInjectedMW = dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString(),
                        Zone = dsResult.Tables[0].Rows[0]["ZONE"].ToString(),
                        ZoneName = dsResult.Tables["ZoneData"].Rows[0]["zone_name"].ToString()
                    };

                    //doc.GeneratePdf();
                    //return;
                    //AddComment

                    if (doc.GeneratePdf().Success)
                    {
                        strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + strAppID + "', 3, '" + FileName + "',  '" + strAppID + "')";
                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                        //RemoveComment

                        strQuery = "INSERT INTO deemed_GC_file(Application_no, file_name, created_by) VALUES ('" + strAppID + "','" + FileName + "','" + Session["SAPID"].ToString() + "')";
                        SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                        //RemoveComment
                    }

                    //strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + strAppID + "', 3, '" + FileName + "',  '" + strAppID + "')";
                    //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                    //strQuery = "INSERT INTO deemed_GC_file(Application_no, file_name, created_by) VALUES ('" + strAppID + "','" + FileName + "','" + Session["SAPID"].ToString() + "')";
                    //SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, $"DELETE FROM proposalapproval WHERE APPLICATION_NO = '{strAppID}'");

                    strQuery = "insert into proposalapproval(APPLICATION_NO , roleid , createDT ,createBy) values ('" + strAppID + "','51', now() , '" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = "insert into proposalapprovaltxn(APPLICATION_NO , isAppr_Rej_Ret, Aprove_Reject_Return_by,roleid,createDT ,createBy) values ('" + strAppID + "','Y','" + Session["SAPID"].ToString() + "', '51', now() , '" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=12, app_status='Deemed grid connectivity CREATED.' ,APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + strAppID + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    #region Application Status tracking date
                    strQuery = "insert into APPLICANT_STATUS_TRACKING(APPLICATION_NO, STATUS, STATUS_DT, Created_By) " +
                            " values('" + strAppID + "','Deemed grid connectivity CREATED.',now(),'" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    #endregion
                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.ContentType = "PDF";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    //Response.BinaryWrite(bytes);
                    //Response.Flush();
                    //Response.End();

                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    log.Error(ErrorMessage);

                }

            }
        }

        protected void btnOther_Click(object sender, EventArgs e)
        {
            string folderPath = Server.MapPath("~/Files/LFSOther/");
            //string strAppID = Session["APPID"].ToString();
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
                                string strQuery = "Update MSKVY_applicantdetails  set WF_STATUS_CD_C=11, docLFSOther='" + newFileName + "' , APP_STATUS_DT=CURDATE() , app_status='LOAD FLOW STUDY UPLOADED.' where APPLICATION_NO='" + strAppID + "'";
                                MySqlCommand cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                objSaveToMEDA.saveApp(Session["PROJID"].ToString(), "11");
                                lblOtherMsg.Text = "Load Flow Study other documents Uploaded Successfully!!";
                                #region Application Status tracking date
                                strQuery = "insert into APPLICANT_STATUS_TRACKING " +
                                        " values('" + strAppID + "','Load Flow Study other documents Uploaded.',now(),'" + Session["SAPID"].ToString() + "')";
                                cmd = new MySqlCommand(strQuery, mySqlConnection);
                                cmd.ExecuteNonQuery();
                                #endregion
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
                        lblOtherMsg.Text = "Letter Uploaded Failed!!";

                    }
                    catch (Exception exception)
                    {
                        string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                        log.Error(ErrorMessage);
                        lblOtherMsg.Text = "Letter Uploaded Failed!!";
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
    }
}