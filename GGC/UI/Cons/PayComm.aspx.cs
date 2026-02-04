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

namespace GGC.UI.Cons
{
    public partial class PayComm : System.Web.UI.Page
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(PayComm));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["payApplicationId"] = Request.QueryString["appid"].ToString();
                ViewState["payPAN"] = Request.QueryString["PAN"].ToString();
                ViewState["payName"] = Request.QueryString["orgName"].ToString();
                lblOrgName.Text = Request.QueryString["orgName"].ToString();

            }
        }
        protected void btnPay_Click(object sender, EventArgs e)
        {
            string applicationId = ViewState["payApplicationId"].ToString();
            string strProjectID = string.Empty;
            string PAN = ViewState["payPAN"].ToString();
            string strName = ViewState["payName"].ToString();

            string strMerchantCode = ConfigurationManager.AppSettings["MerchantID"].ToString();
            //string strCustomerID = "1";
            long strSrno = 0;
            string strPayAmt;
            string strCurrencyType = ConfigurationManager.AppSettings["CurrencyType"].ToString();
            string strSecurityID = ConfigurationManager.AppSettings["SecurityID"].ToString();
            string checkSumKey = ConfigurationManager.AppSettings["ChecksumKey"].ToString();
            string strPGURL = ConfigurationManager.AppSettings["PGURL"].ToString();
            string strRU = ConfigurationManager.AppSettings["RUCommit"].ToString();
            string strCon = ConfigurationManager.AppSettings["ConventionalFees"].ToString();
            string strNonCon = ConfigurationManager.AppSettings["NonConventionalFees"].ToString();
            log.Error("Session[AppType] " + Session["AppType"].ToString());
            //if (Session["AppType"].ToString() == "NonConventional Generator")
            //{
            //    strPayAmt = strNonCon;
            //}
            //else
            //{
            //    strPayAmt = strCon;
            //}

            MySqlConnection mySqlConnection = new MySqlConnection();
            mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
            string strPostName = string.Empty;

            try
            {

                mySqlConnection.Open();


                switch (mySqlConnection.State)
                {

                    case System.Data.ConnectionState.Open:
                        CultureInfo provider = CultureInfo.InvariantCulture;
                        System.Globalization.DateTimeStyles style = DateTimeStyles.None;
                        DateTime date1;
                        DateTime.TryParseExact(DateTime.Now.ToString(), "yyyy-MM-dd", provider, style, out date1);
                        //date1 = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss");
                        MySqlCommand cmd = new MySqlCommand("select count(1) cnt  from BillDesk_TXN where ApplicationNo='" + applicationId + "' and AuthStatus is not null and typeofpay='Committment'", mySqlConnection);
                        DataSet dsResult = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResult);

                        cmd = new MySqlCommand("select * from applicantdetails where Application_No='" + applicationId + "'", mySqlConnection);
                        DataSet dsResultCommFees = new DataSet();
                        da = new MySqlDataAdapter(cmd);
                        da.Fill(dsResultCommFees);
                        string strPROJECT_TYPE = dsResultCommFees.Tables[0].Rows[0]["PROJECT_TYPE"].ToString();
                        strProjectID = dsResultCommFees.Tables[0].Rows[0]["MEDAProjectID"].ToString();
                        strPayAmt = dsResultCommFees.Tables[0].Rows[0]["CommittmentFees"].ToString();

                        //if (strPROJECT_TYPE == "Solar Project" || strPROJECT_TYPE == "Wind" || strPROJECT_TYPE == "Solar Park" || strPROJECT_TYPE == "Hybrid")
                        //{
                        //    strPayAmt = dsResultCommFees.Tables[0].Rows[0]["CommittmentFees"].ToString();
                        //}
                        int cnt = Int16.Parse(dsResult.Tables[0].Rows[0]["cnt"].ToString());
                        if (cnt < 1)
                        {
                            string strQuery = "insert into BillDesk_TXN(ApplicationNo,MEDAProjectID, MerchantID, TxnAmount,ORIGINALSTATUS,  CreatedDt,typeofpay) " +
                                "values('" + applicationId + "','" + strProjectID + "','" + strMerchantCode + "','" + strPayAmt + "','" + "1','" + DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "','Committment')";
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                            log.Error(strQuery);
                            //    strSrno=cmd.LastInsertedId;
                            cmd = new MySqlCommand("select SRNO  from BillDesk_TXN where APPLICATIONNO='" + applicationId + "' order by srno desc", mySqlConnection);
                            dsResult = new DataSet();
                            da = new MySqlDataAdapter(cmd);
                            da.Fill(dsResult);
                            strSrno = long.Parse(dsResult.Tables[0].Rows[0]["SRNO"].ToString());

                            log.Error(strQuery + " SRNO : " + strSrno);
                            strQuery = "update BillDesk_TXN set CustomerID= " +
                                "'" + strSrno + "' where SRNO='" + strSrno + "' and APPLICATIONNO='" + applicationId + "'";
                            log.Error(strQuery + " SRNO : " + strSrno);
                            cmd = new MySqlCommand(strQuery, mySqlConnection);
                            cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            lblMessage.Text = "Payment already done.";
                            break;
                        }
 // Change on 31.05.23 for oragnization name having  brackets String data = strMerchantCode + "|" + strSrno + "|NA|" + strPayAmt + "|NA|NA|NA|" + strCurrencyType + "|NA|R|" + strSecurityID + "|NA|NA|F|" + strName + "|" + applicationId + "|Committment|NA|NA|NA|NA|" + strRU + "";
                        String data = strMerchantCode + "|" + strSrno + "|NA|" + strPayAmt + "|NA|NA|NA|" + strCurrencyType + "|NA|R|" + strSecurityID + "|NA|NA|F|" + applicationId + "|" + applicationId + "|Committment|NA|NA|NA|NA|" + strRU + "";
                        PaymentGateway dataprg = new PaymentGateway();
                        String hash = String.Empty;
                        hash = dataprg.GetHMACSHA256(data, checkSumKey).ToUpper();
                        log.Error(strPGURL + "?msg=" + data + "|" + hash);
                        Response.Redirect(strPGURL + "?msg=" + data + "|" + hash, false);
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

            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
                //  log.Error(strQuery);

            }

            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
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