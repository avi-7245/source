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
using Mysqlx.Crud;
using GGC.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using iTextSharp.tool.xml.html;
using static System.Net.Mime.MediaTypeNames;

namespace GGC.UI.Cons
{
    public partial class NewGCTerms : System.Web.UI.Page
    {

        bool valid = false;
        //protected static readonly ILog log = LogManager.GetLogger(typeof(PayConfirm));

        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!Page.IsPostBack)
        //    {
        //        ViewState["payApplicationId"] = Request.QueryString["appid"].ToString();
        //        ViewState["payPAN"] = Request.QueryString["PAN"].ToString();
        //        ViewState["payName"] = Request.QueryString["orgName"].ToString();
        //       // lblOrgName.Text = Request.QueryString["orgName"].ToString();

        //    }
        //}
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Proceed with your logic here
                //if (Session["NewApplication"].ToString() == "true")
                string appId = Session["appid"] as string;
                // Check if JavaScript validation passes (but also validate server-side as a fallback)
                if (IsValidCombination() &&  appId.Equals(""))
                {
                    // Proceed with the submission logic, such as saving data or displaying a success message
                    Response.Write("Valid successfully.");
                    Session["NewApplication"] = "true";
                    bool govtAuthorization = chkGovtAuthorization.Checked;
                    bool proofOfOwnership = chkProofOfOwnership.Checked;
                    bool bankGuarantee = chkBankGuarantee.Checked;
                    bool loAorPPA = chkLOAorPPA.Checked;
                    bool reProofoOwn = CheckBox1.Checked;
                    bool reBankGuarantee = CheckBox2.Checked;
                    bool solarWindRenewable = false;
                    bool renewableEnergy = false;
                    string selectedApplicationType = Request.Form["applicationType"];

                    if (!string.IsNullOrEmpty(selectedApplicationType))
                    {
                        if (selectedApplicationType == "solarWindRenewable")
                        {
                            // Handle the solarWindRenewable case
                            solarWindRenewable = true;
                        }
                        else if (selectedApplicationType == "renewableEnergy")
                        {
                            // Handle the renewableEnergy case
                            renewableEnergy = true;
                        }
                    }
                    else
                    {
                        // Handle the case where no radio button is selected
                        Response.Write("No selection made.");
                    }

                    MySqlConnection mySqlConnection = new MySqlConnection();
                    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                    string strPostName = string.Empty;
                    string insertQuery = @"
            INSERT INTO new_terms_condition 
            ( notaff, boardresul, momnaoa, solarWindRenewable, renewableEnergy, govtAuthorization, 
            proofOfOwnership, bankGuarantee, loAorPPA, reProofoOwn, reBankGuarantee)
            VALUES 
            ( @notaff, @boardresul, @momnaoa, @solarWindRenewable, @renewableEnergy, 
            @govtAuthorization, @proofOfOwnership, @bankGuarantee, @loAorPPA, @reProofoOwn, @reBankGuarantee);
        ";

                    using (MySqlConnection connection = new MySqlConnection(mySqlConnection.ConnectionString))
                    {
                        MySqlCommand command = new MySqlCommand(insertQuery, connection);

                        // Add parameters to the command

                        command.Parameters.AddWithValue("@notaff", 1);
                        command.Parameters.AddWithValue("@boardresul", 1);
                        command.Parameters.AddWithValue("@momnaoa", 1);
                        command.Parameters.AddWithValue("@solarWindRenewable", solarWindRenewable);
                        command.Parameters.AddWithValue("@renewableEnergy", renewableEnergy);
                        command.Parameters.AddWithValue("@govtAuthorization", govtAuthorization);
                        command.Parameters.AddWithValue("@proofOfOwnership", proofOfOwnership);
                        command.Parameters.AddWithValue("@bankGuarantee", bankGuarantee);
                        command.Parameters.AddWithValue("@loAorPPA", loAorPPA);
                        command.Parameters.AddWithValue("@reProofoOwn", reProofoOwn);
                        command.Parameters.AddWithValue("@reBankGuarantee", reBankGuarantee);

                        // Open the connection
                        connection.Open();

                        // Execute the insert query
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();
                        // Output how many rows were affected
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                    Response.Redirect("~/UI/Cons/ConsumerDetail.aspx", false);
                }
                else
                {
                    string applicationId = Session["appid"] as string;
                    string strPAN = Session["PAN"] as string;

                    bool govtAuthorization = chkGovtAuthorization.Checked;
                    bool proofOfOwnership = chkProofOfOwnership.Checked;
                    bool bankGuarantee = chkBankGuarantee.Checked;
                    bool loAorPPA = chkLOAorPPA.Checked;
                    bool reProofoOwn = CheckBox1.Checked;
                    bool reBankGuarantee = CheckBox2.Checked;
                    bool solarWindRenewable = false;
                    bool renewableEnergy = false;
                    string selectedApplicationType = Request.Form["applicationType"];

                    if (!string.IsNullOrEmpty(selectedApplicationType))
                    {
                        if (selectedApplicationType == "solarWindRenewable")
                        {
                            // Handle the solarWindRenewable case
                            solarWindRenewable = true;
                             loAorPPA =false;
                             reProofoOwn = false;
                             reBankGuarantee = false;
                        }
                        else if (selectedApplicationType == "renewableEnergy")
                        {
                            // Handle the renewableEnergy case
                            renewableEnergy = true;
                             govtAuthorization =false;
                             proofOfOwnership = false;
                             bankGuarantee = false;
                        }
                    }
                    else
                    {
                        // Handle the case where no radio button is selected
                        Response.Write("No selection made.");
                    }

                    MySqlConnection mySqlConnection = new MySqlConnection();
                    mySqlConnection.ConnectionString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();
                    string strPostName = string.Empty;
                    string insertQuery = @"
            INSERT INTO new_terms_condition 
            ( notaff, boardresul, momnaoa, solarWindRenewable, renewableEnergy, govtAuthorization, 
            proofOfOwnership, bankGuarantee, loAorPPA, reProofoOwn, reBankGuarantee)
            VALUES 
            ( @notaff, @boardresul, @momnaoa, @solarWindRenewable, @renewableEnergy, 
            @govtAuthorization, @proofOfOwnership, @bankGuarantee, @loAorPPA, @reProofoOwn, @reBankGuarantee);
        ";

                    using (MySqlConnection connection = new MySqlConnection(mySqlConnection.ConnectionString))
                    {
                        MySqlCommand command = new MySqlCommand(insertQuery, connection);

                        // Add parameters to the command

                        command.Parameters.AddWithValue("@notaff", 1);
                        command.Parameters.AddWithValue("@boardresul", 1);
                        command.Parameters.AddWithValue("@momnaoa", 1);
                        command.Parameters.AddWithValue("@solarWindRenewable", solarWindRenewable);
                        command.Parameters.AddWithValue("@renewableEnergy", renewableEnergy);
                        command.Parameters.AddWithValue("@govtAuthorization", govtAuthorization);
                        command.Parameters.AddWithValue("@proofOfOwnership", proofOfOwnership);
                        command.Parameters.AddWithValue("@bankGuarantee", bankGuarantee);
                        command.Parameters.AddWithValue("@loAorPPA", loAorPPA);
                        command.Parameters.AddWithValue("@reProofoOwn", reProofoOwn);
                        command.Parameters.AddWithValue("@reBankGuarantee", reBankGuarantee);

                        // Open the connection
                        connection.Open();

                        // Execute the insert query
                        int rowsAffected = command.ExecuteNonQuery();
                        connection.Close();
                        // Output how many rows were affected
                        Console.WriteLine($"{rowsAffected} row(s) inserted.");
                    }
                    if (!string.IsNullOrEmpty(applicationId) && !string.IsNullOrEmpty(strPAN))
                    {
                        Response.Redirect("~/UI/Cons/ConsumerDetail.aspx?appid=" + applicationId + "&PAN=" + strPAN, false);
                    }
                }

                
            }
            else
            {
                // If validation fails, inform the user
                Response.Write("Please select a valid combination of documents.");
            }
            }

            
            
            


        
        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (IsValidCombination())
            {
                // Proceed with the submission logic, such as saving data or displaying a success message
                Response.Write("Documents submitted successfully.");
            }
            else
            {
                // If validation fails, inform the user
                Response.Write("Please select a valid combination of documents.");
            }

           ;

        }
        private bool IsValidCombination()
        {
            // Server-side validation for Solar/Wind/Renewable Power Park
            bool govtAuthorization = chkGovtAuthorization.Checked;
            bool proofOfOwnership = chkProofOfOwnership.Checked;
            bool bankGuarantee = chkBankGuarantee.Checked;

            bool loAorPPA = chkLOAorPPA.Checked;
            bool reProofoOwn = CheckBox1.Checked;
            bool reBankGuarantee = CheckBox2.Checked;
            // Validation for Solar, Wind, Renewable Power Park Developers
            if ((govtAuthorization && proofOfOwnership) || (govtAuthorization && bankGuarantee))
            {
                valid = true;

                return valid; // Valid combination
            }

            // Validation for Renewable Energy Projects or ESS
            if (loAorPPA || reProofoOwn || reBankGuarantee)
            {
                valid = true;
                return valid; // Valid selection
            }
            
            
            return valid; // Invalid combination
        }
        
       

    }
}