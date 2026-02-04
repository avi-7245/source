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
using Microsoft.Reporting.WebForms;
using GGC.Scheduler;
using GGC.Common;


namespace GGC.UI.Emp
{
    public partial class MSSPDDraft : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(MSSPDDraft));
        string conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
        int roleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblLoginname.Text = Session["EmpName"] + "(" + Session["EmpDesignation"] + ")";
            roleId = int.Parse(Session["EmpRole"].ToString());

            if (!Page.IsPostBack)
            {

                fillGrid();

            }

        }

        protected void fillGrid()
        {
            string strGSTIN = string.Empty;
            //int rollid = int.Parse(Session["EmpRole"].ToString());
            //Session["EmpZone"] = "Thane";

            try
            {

                string strQuery = string.Empty;

                if (roleId >= 51 && roleId <= 57)
                {
                    strQuery = "select a.* ,date_format(a.APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from mskvy_applicantdetails_spd a,proposalapproval_spd b where a.WF_STATUS_CD_C=12 and b.roleId=" + roleId + " order by a.CREATED_DT desc";
                }
                else
                {
                    strQuery = "select a.* ,date_format(a.APP_STATUS_DT,'%Y-%m-%d') APP_STATUS_DT from mskvy_applicantdetails_spd a where a.WF_STATUS_CD_C=6 order by a.CREATED_DT desc";

                }

                DataSet dsResult = new DataSet();
                dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                Session["AppDet"] = dsResult;


                if (dsResult.Tables[0].Rows.Count > 0)
                {
                    GVApplications.DataSource = dsResult.Tables[0];
                    GVApplications.DataBind();

                }
                else
                {
                    //lblResult.Text = "Already Register for same GATE registration ID,Your Registration ID is : " + dsResult.Tables[0].Rows[0]["RegistrationId"].ToString();
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
                // Use the exception object to handle all other non-MySql specific errors
                // lblResult.Text = "Some problem during registration.Please try again.";
            }


        }

        protected void GVApplications_RowCreated(object sender, GridViewRowEventArgs e)
        {
            int roll_Id;
            roll_Id = int.Parse(Session["EmpRole"].ToString());


            if (roll_Id <= 10)
            {
                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Verify & Download GC")
                .SingleOrDefault()).Visible = true;


                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Return")
                .SingleOrDefault()).Visible = true;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Upload GC")
                .SingleOrDefault()).Visible = false;



                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Approve")
                .SingleOrDefault()).Visible = false;

            }
            if (roll_Id > 50)
            {
                ((DataControlField)GVApplications.Columns
               .Cast<DataControlField>()
               .Where(fld => fld.HeaderText == "Verify & Download GC")
               .SingleOrDefault()).Visible = false;


                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Return")
                .SingleOrDefault()).Visible = false;

                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Upload GC")
                .SingleOrDefault()).Visible = false;



                ((DataControlField)GVApplications.Columns
                .Cast<DataControlField>()
                .Where(fld => fld.HeaderText == "Approve")
                .SingleOrDefault()).Visible = true;

            }

        }

        protected void GVApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewDoc")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/ViewDocSPD.aspx?application=" + applicationId, false);

            }

            if (e.CommandName == "VerifyDraft")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];


                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;



                //string folderPath = Server.MapPath("~/Files/MSKVY/" + applicationId + "/");
                //string strQuery = "select * from mskvy_applicantdetails where APPLICATION_NO='" + applicationId + "'";
                string strQuery = "SELECT maspd.ADDRESS_FOR_CORRESPONDENCE" +
                    ",maspd.SPV_Name" +
                    ",maspd.CONT_PER_MOBILE_1" +
                    ",ma.APPLICATION_NO" +
                    ",ma.MEDAProjectID" +
                    ",ma.Quantum_power_injected_MW" +
                    ",ma.SPV_Name" +
                    ",ma.STU_INJECTION_VOLTAGE" +
                    ",ma.STU_POINT_OF_INJECT" +
                    ",ma.ZONE,ma.PROJECT_LOC" +
                    ",ma.PROJECT_TALUKA" +
                    ",ma.PROJECT_DISTRICT" +
                    " FROM mskvy_applicantdetails_spd maspd LEFT JOIN mskvy_applicantdetails ma ON ma.APPLICATION_NO = maspd.APPLICATION_NO AND ma.MEDAProjectID = maspd.MEDAProjectID" +
                    $" where ma.APPLICATION_NO='{applicationId}'";

                DataSet dsResult = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);


                //DataSet dsFWC = new DataSet();
                //strQuery = $"SELECT z.zone_name FROM zone_circle_district_map z WHERE dist_name = '{dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"]}' LIMIT 1;";
                //dsFWC = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);

                //DataTable zoneTable = dsFWC.Tables[0].Copy();
                //zoneTable.TableName = "ZoneData";
                //dsResult.Tables.Add(zoneTable);

                //DataSet dsCommDet = new DataSet();
                //strQuery = "SELECT * FROM empmaster WHERE role_id=53 and isactive='Y' order by 1";
                //dsCommDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                //dsCommDet.Tables[0].TableName = "empDet";
                //dsResult.Tables.Add(dsCommDet.Tables[0].Copy());


                //string strSubject = "Approval for Grid Connectivity to " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                //                    " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 (Project Code : " + dsResult.Tables[0].Rows[0]["MEDAProjectID"].ToString() + ", Online GC Application No. " + dsResult.Tables[0].Rows[0]["APPLICATION_NO"].ToString() + ") .";

                //string strData = "In view of above, as approved by competent authority, In-principle Grid Connectivity for your " + dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString() + " MW Solar Power Project proposed by M/s. " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " at Site: " + dsResult.Tables[0].Rows[0]["PROJECT_LOC"].ToString() + ",Tal." + dsResult.Tables[0].Rows[0]["PROJECT_TALUKA"].ToString() + ", Dist.: " + dsResult.Tables[0].Rows[0]["PROJECT_DISTRICT"].ToString() +
                //                    " proposed under Mukhyamantri Saur Krishi Vahini Yojana - 2.0 is hereby granted at " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + " KV level of " + dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString() + " Substation.";

                //string strData2 = "The evacuation arrangement shall be totally at the risk and cost of M/s " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + ". In case of tripping and/or outage on this " + dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString() + " kV Single Circuit (S/C) line and/or bay, there will be loss of generation and which will be M/s " + dsResult.Tables[0].Rows[0]["SPV_Name"].ToString() + " responsibility and MSETCL will not be held responsible for the said loss and you will not claim for the loss of generation by what so ever may be the reason from MSETCL.";

                //DataTable RDLCData = new DataTable("RDLCData");

                //DataColumn dtaColumn;

                //DataRow myDataRow;

                // Create id column

                //dtaColumn = new DataColumn();
                //dtaColumn.DataType = typeof(String);
                //dtaColumn.ColumnName = "subject";
                //RDLCData.Columns.Add(dtaColumn);

                //dtaColumn = new DataColumn();
                //dtaColumn.DataType = typeof(String);
                //dtaColumn.ColumnName = "MsgData";
                //RDLCData.Columns.Add(dtaColumn);

                //dtaColumn = new DataColumn();
                //dtaColumn.DataType = typeof(String);
                //dtaColumn.ColumnName = "MsgData2";
                //RDLCData.Columns.Add(dtaColumn);

                //myDataRow = RDLCData.NewRow();
                //myDataRow["subject"] = strSubject;
                //myDataRow["MsgData"] = strData;
                //myDataRow["MsgData2"] = strData2;

                //RDLCData.Rows.Add(myDataRow);


                //dsResult.Tables.Add(RDLCData.Copy());

                //DataSet dsLandDet = new DataSet();
                //strQuery = "SELECT * FROM mskvy_landdet WHERE APPLICATION_NO='" + applicationId + "'";
                //dsLandDet = SQLHelper.ExecuteDataset(conString, CommandType.Text, strQuery);
                //dsLandDet.Tables[0].TableName = "LandDet";
                //dsResult.Tables.Add(dsLandDet.Tables[0].Copy());

                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\TestRDLC\testRDLCWeb\MSKVYGC.xsd");
                //if (dsResult.Tables[0].Rows.Count > 0)
                //{
                //dsResult.WriteXmlSchema(@"E:\Ashish\Projects\DotNet\OnlineExamRegistration\ExamRegistration\ExamRegistrationUI\XMLSchema\Personal.xsd");
                //if (!Directory.Exists(folderPath))
                //{
                //    //If Directory (Folder) does not exists Create it.
                //    Directory.CreateDirectory(folderPath);
                //}

                try
                {

                    //LocalReport report = new LocalReport();
                    //report.ReportPath = Server.MapPath("~/PDFReport/") + "RDLCGc.rdlc";


                    //ReportDataSource rds = new ReportDataSource();
                    //rds.Name = "dsGC_";//This refers to the dataset name in the RDLC file
                    //rds.Value = dsResult.Tables[0];
                    //report.DataSources.Add(rds);

                    //ReportDataSource rds2 = new ReportDataSource();
                    //rds2.Name = "dsGC_EmpData";//This refers to the dataset name in the RDLC file
                    //rds2.Value = dsResult.Tables["empDet"];
                    //report.DataSources.Add(rds2);

                    //ReportDataSource rds3 = new ReportDataSource();
                    //rds3.Name = "dsGC_Msg";//This refers to the dataset name in the RDLC file
                    //rds3.Value = dsResult.Tables["RDLCData"];
                    //report.DataSources.Add(rds3);

                    //ReportDataSource rds4 = new ReportDataSource();
                    //rds4.Name = "dsGC_LandDet";//This refers to the dataset name in the RDLC file
                    //rds4.Value = dsResult.Tables["LandDet"];
                    //report.DataSources.Add(rds4);


                    //ReportViewer reportViewer = new ReportViewer();
                    //reportViewer.LocalReport.ReportPath = Server.MapPath("~/PDFReport/") + "RDLCGc.rdlc";
                    //reportViewer.LocalReport.DataSources.Clear();

                    //reportViewer.LocalReport.DataSources.Add(rds);
                    //reportViewer.LocalReport.DataSources.Add(rds2);
                    //reportViewer.LocalReport.DataSources.Add(rds3);
                    //reportViewer.LocalReport.DataSources.Add(rds4);



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

                    //#endregion

                    //var doc = new GenerateGCDoc(Server, folderPath + FileName, applicationId)
                    //{
                    //    EMP_NAME = dsResult.Tables["empDet"].Rows[0]["EMP_NAME"].ToString(),
                    //    Date = DateTime.Now.ToString(format: "dd/MM/yyyy"),
                    //    Add_correspondence = dsResult.Tables[0].Rows[0]["ADDRESS_FOR_CORRESPONDENCE"].ToString(),
                    //    MsgData = dsResult.Tables["RDLCData"].Rows[0]["MsgData"].ToString(),
                    //    MsgData2 = dsResult.Tables["RDLCData"].Rows[0]["MsgData2"].ToString(),
                    //    Quantum_power_injected_MW = dsResult.Tables[0].Rows[0]["Quantum_power_injected_MW"].ToString(),
                    //    SPV_Name = dsResult.Tables[0].Rows[0]["SPV_Name"].ToString(),
                    //    STU_INJECTION_VOLTAGE = dsResult.Tables[0].Rows[0]["STU_INJECTION_VOLTAGE"].ToString(),
                    //    STU_POINT_OF_INJECT = dsResult.Tables[0].Rows[0]["STU_POINT_OF_INJECT"].ToString(),
                    //    Subject = dsResult.Tables["RDLCData"].Rows[0]["subject"].ToString(),
                    //    ZONE = dsResult.Tables[0].Rows[0]["ZONE"].ToString(),
                    //    ZONE_NAME = dsResult.Tables["ZoneData"].Rows[0]["zone_name"].ToString(),
                    //    AnnexureB = dsResult.Tables["LandDet"]
                    //};


                    //var result = doc.GeneratePdf();

                    string fileName = "GC" + DateTime.Today.ToString("dd-MMM-yy").Replace("-", "_") + DateTime.Now.ToString("hh:mm:ss").Replace(":", "_") + ".pdf";

                    strQuery = "INSERT INTO mskvy_upload_doc_spd ( Application_No, FileType, FieName, CreateBy) VALUES ( '" + applicationId + "', 7, '" + fileName + "',  '" + applicationId + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = "INSERT INTO GC_file(Application_no, file_name, created_by) VALUES ('" + applicationId + "','" + fileName + "','" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = $"DELETE FROM proposalapproval_spd WHERE APPLICATION_NO = '{applicationId}'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);

                    strQuery = $"insert into proposalapproval_spd(APPLICATION_NO , roleid , createDT ,createBy) values ('{applicationId}','51', now() , '{Session["SAPID"]}')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment


                    strQuery = "insert into proposalapprovaltxn_spd(APPLICATION_NO , isAppr_Rej_Ret, Aprove_Reject_Return_by,roleid,createDT ,createBy) values ('" + applicationId + "','Y','" + Session["SAPID"].ToString() + "', '51', now() , '" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    strQuery = "Update mskvy_applicantdetails_spd  set WF_STATUS_CD_C=12, app_status='Grid connectivity proposal created.' ,APP_STATUS_DT=CURDATE() where APPLICATION_NO='" + applicationId + "'";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment

                    #region Application Status tracking date
                    strQuery = "insert into APPLICANT_STATUS_TRACKING(APPLICATION_NO, STATUS, STATUS_DT, Created_By) values('" + applicationId + "','Grid connectivity proposal created.',now(),'" + Session["SAPID"].ToString() + "')";
                    SQLHelper.ExecuteNonQuery(conString, CommandType.Text, strQuery);
                    //RemoveComment
                    #endregion

                    //DataSet dsEmailMob = SQLHelper.ExecuteDataset(conString, CommandType.Text, "select * from empmaster where role_id in (2,51,52,53)");
                    //var mobileNumbers = new List<string>() { dsResult.Tables[0].Rows[0]["CONT_PER_MOBILE_1"].ToString() };

                    //mobileNumbers.AddRange(dsEmailMob.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpMobile"].ToString())).Select(a => a["EmpMobile"].ToString()).Distinct());

                    //SMS.Send(message: SMSTemplates.GCApproved(applicationNo: applicationId, spvName: dsResult.Tables[0].Rows[0]["SPV_Name"].ToString()), string.Join(",", mobileNumbers), MethodBase.GetCurrentMethod(), log);

                    if (row.FindControl("btnReturn") is Button btnReturn)
                    {
                        btnReturn.Enabled = false;
                        btnReturn.BackColor = System.Drawing.Color.Red;
                    }
                    if (row.FindControl("btnVerify") is Button btnVerify)
                    {
                        btnVerify.Enabled = false;
                        btnVerify.BackColor = System.Drawing.Color.Red;
                    }

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "VerifyAndDownloadGC", $"fnDownloadGC('{applicationId}','{fileName}');", true);

                    //Response.Clear();
                    //Response.Buffer = true;
                    //Response.Charset = "";
                    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    //Response.ContentType = "PDF";
                    //Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    //Response.BinaryWrite(result.Data);
                    //Response.Flush();
                    //Response.SuppressContent = true;
                    //HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
                catch (Exception ex)
                {
                    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                    //   log.Error(ErrorMessage);

                }
                //}

            }

            if (e.CommandName == "ReturnDraft")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text;
                Response.Redirect("~/UI/Emp/AppReturn.aspx?application=" + applicationId + "&ProjectType=SPV2", false);

            }

            if (e.CommandName == "AppCON")
            {

                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/MSSPDPRAccept.aspx?application=" + applicationId, false);

            }

            if (e.CommandName == "UploadGC")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                //Reference the GridView Row.
                GridViewRow row = GVApplications.Rows[rowIndex];

                //Fetch value of Name.
                //string name = (row.FindControl("txtName") as TextBox).Text;

                //Fetch value of Country
                string applicationId = row.Cells[0].Text;
                Session["APPID"] = row.Cells[0].Text; ;
                Session["PROJID"] = row.Cells[1].Text;
                Response.Redirect("~/UI/Emp/MSSPDUpGC.aspx?application=" + applicationId, false);

            }
        }

        protected void GVApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Session["AppDet"] is DataSet ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[e.Row.RowIndex];
                        Button btnReturn = e.Row.FindControl("btnReturn") as Button;
                        Button btnVerify = e.Row.FindControl("btnVerify") as Button;

                        string isAppApproved = row["isAppApproved"].ToString();
                        int wfStatus = row["WF_STATUS_CD_C"].ToInt();

                        System.Drawing.Color PrimaryColor = System.Drawing.ColorTranslator.FromHtml("#008CBA");

                        if (isAppApproved == "N")
                        {
                            btnReturn.Enabled = false;
                            btnReturn.BackColor = System.Drawing.Color.Red;

                            btnVerify.Enabled = false;
                            btnVerify.BackColor = System.Drawing.Color.Red;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                string ErrorMessage = "Row Bound Exception Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + exception.Message + " " + exception.InnerException;
                log.Error(ErrorMessage);
            }
        }
    }
}