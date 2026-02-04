<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewGCTerms.aspx.cs" Inherits="GGC.UI.Cons.NewGCTerms" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <style type="text/css">
        .form-control
        {
            display: block;
            width: 85%;
            height: 16px;
            padding: 2px 2px;
            font-size: 13px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 2px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }
        .form-control:focus
        {
            border-color: #66afe9;
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
        }
        .form-control::-moz-placeholder
        {
            color: #999;
            opacity: 1;
        }
        .form-control:-ms-input-placeholder
        {
            color: #999;
        }
        .form-control::-webkit-input-placeholder
        {
            color: #999;
        }
        
        .form-control-textarea
        {
            display: block;
            width: 50%;
            height: 26px;
            padding: 2px 2px;
            font-size: 13px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            background-image: none;
            border: 1px solid #ccc;
            border-radius: 2px;
            -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            -webkit-transition: border-color ease-in-out .15s, -webkit-box-shadow ease-in-out .15s;
            -o-transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }
        .form-control-textarea :focus
        {
            border-color: #66afe9;
            outline: 0;
            -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
        }
        .form-control-textarea ::-moz-placeholder
        {
            color: #999;
            opacity: 1;
        }
        .form-control-textarea :-ms-input-placeholder
        {
            color: #999;
        }
        .form-control-textarea ::-webkit-input-placeholder
        {
            color: #999;
        }
        .mainbody
        {
            width: 1060px;
            margin: 0px auto;
        }
        
        .button
        {
            background-color: #008CBA; /* Blue */
            border: none;
            color: white;
            padding: 2px 10px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 13px;
        }
    
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 20px;
            padding: 20px;
            background-color: #f4f4f9;
            color: #333;
        }
        header {
            text-align: center;
            margin-bottom: 20px;
        }
       
        body {
            font-family: Arial, sans-serif;
            margin: 20px;
            line-height: 1.6;
        }
        .h1 {
            color: red;
        }
        h2, h3 {
            color: #2c3e50;
        }
        ul {
            margin: 10px 0;
            padding-left: 20px;
        }
        .highlight {
            color: #e74c3c;
            font-weight: bold;
        }
        .button {
            display: inline-block;
            padding: 10px 15px;
            margin-top: 20px;
            background-color: #3498db;
            color: white;
            text-decoration: none;
            border-radius: 5px;
        }
        .button:hover {
            background-color: #2980b9;
        }
        .p {
    margin: 0;
    padding: 0;
}
    </style>
        
   
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
   
    <script type="text/javascript">
        
        // Function to handle checkbox visibility on change
        function handleCheckboxChange() {
            var govtAuthorization = document.getElementById('<%=chkGovtAuthorization.ClientID%>');
        var proofOfOwnership = document.getElementById('<%=chkProofOfOwnership.ClientID%>');
        var bankGuarantee = document.getElementById('<%=chkBankGuarantee.ClientID%>');
        var loAorPPA = document.getElementById('<%=chkLOAorPPA.ClientID%>');

        // Toggle visibility based on the state of checkboxes
        if (govtAuthorization.checked) {
            proofOfOwnership.style.display = "inline-block"; // Show Proof of Ownership if Govt Authorization is checked
            bankGuarantee.style.display = "inline-block"; // Show Bank Guarantee if Govt Authorization is checked
        } else {
            proofOfOwnership.style.display = "none"; // Hide Proof of Ownership if Govt Authorization is unchecked
            bankGuarantee.style.display = "none"; // Hide Bank Guarantee if Govt Authorization is unchecked
        }

        // Show LOA or PPA only if Bank Guarantee is checked
        if (bankGuarantee.checked) {
            loAorPPA.style.display = "inline-block"; // Show LOA or PPA
        } else {
            loAorPPA.style.display = "none"; // Hide LOA or PPA if Bank Guarantee is unchecked
        }
    }
        // Get all checkboxes with the name "chkGovtAuthorization"
        const checkboxes = document.querySelectorAll('input[name="chkGovtAuthorization"]');

        // Add an event listener to each checkbox
        checkboxes.forEach(checkbox => {
            checkbox.addEventListener('change', () => {
                // Count how many checkboxes are checked
                const checkedCount = document.querySelectorAll('input[name="chkGovtAuthorization"]:checked').length;

                // If more than 2 checkboxes are checked, uncheck the last one that was clicked
                if (checkedCount > 2) {
                    checkbox.checked = false;
                    alert("You can only check 2 checkboxes.");
                }
            });
        });

    // Function to validate form submission
    function validateForm() {
        var routeSelected = document.querySelector('input[name="applicationType"]:checked');
        if (!routeSelected) {
            alert("Please select the application type (Solar, Wind, Renewable Power Park, or Renewable Energy Project).");
            return false;
        }
        if (routeSelected.value === "solarWindRenewable" || routeSelected.value === "renewableEnergy") {
            return validateDocumentSelection(routeSelected.value);
        }
       
        return false;
    }

    // Validate document selection based on selected route
    function validateDocumentSelection(route) {
        var govtAuthorization = document.getElementById('<%=chkGovtAuthorization.ClientID%>').checked;
        var proofOfOwnership = document.getElementById('<%=chkProofOfOwnership.ClientID%>').checked;
        var bankGuarantee = document.getElementById('<%=chkBankGuarantee.ClientID%>').checked;

        var loAorPPA = document.getElementById('<%=chkLOAorPPA.ClientID%>').checked;
        var reProofoOwn = document.getElementById('<%=CheckBox1.ClientID%>').checked;
        var reBankGuarantee = document.getElementById('<%=CheckBox2.ClientID%>').checked;
    
      


        // Count the number of checked checkboxes
        var checkedCount = 0;
       
        if (route === "solarWindRenewable") {
           
            if (govtAuthorization) checkedCount++;
            if (proofOfOwnership) checkedCount++;
            if (bankGuarantee) checkedCount++;

            
            // If more than 2 checkboxes are checked, return false
            if (checkedCount > 2) {
                alert("Submit documents as per combinations of (a) and (b) or combination of clauses(a) and(c) ");
                return false;
            }
            // Solar, Wind, Renewable Power Park Route Validation: (a & b) or (a & c)
            if ((govtAuthorization && proofOfOwnership) || (govtAuthorization && bankGuarantee)) {
                if (document.getElementById("<%=chkIAgree.ClientID %>").checked == true) {
                     return true;
                } else {
                    alert("Check I Agree to Terms and Conditions ");
                    return false;
                }
                return true; // If validation passes, return true to allow form submission
            } else {
                alert("Please select a valid combination of documents for Solar/Wind/Renewable Power Park.");
                return false;
            }
        }

        if (route === "renewableEnergy") {
          
            
            if (loAorPPA) checkedCount++;
            if (reProofoOwn) checkedCount++;
            if (reBankGuarantee) checkedCount++;

            console.log(loAorPPA, "loAorPPA");
            console.log(reProofoOwn, "reProofoOwn");
            console.log(reBankGuarantee, "reBankGuarantee");

            if (checkedCount > 1) {
                alert("Select anyone and submit relevant document accordingly.");
                return false;
            }
            if (loAorPPA || reProofoOwn || reBankGuarantee) {
            // Renewable Energy Projects or ESS Route Validation: Select anyone from a, b, or c
                if (document.getElementById("<%=chkIAgree.ClientID %>").checked == true) {
                   
                    return true;
                } else {
                    alert("Check I Agree to Terms and Conditions ");
                    return false;
                }
                return true; // If validation passes, return true to allow form submission {
                   
            } else {
                alert("Please select at least one document for Renewable Energy Projects/ESS.");
                return false;
            }
        }
    
        }

        
        
    </script>
    <script type="text/javascript">
        function ValidateCheckBox(sender, args) {
            if (document.getElementById("<%=chkIAgree.ClientID %>").checked == true) {
                args.IsValid = true;
                return true;
            } else {
                args.IsValid = false;
                return false;
            }
        }

    </script>

    </head>
    <body>
    <div id="main">
        <div id="header">
            <div id="logo">
                <div id="logo_text">
                    <!-- class="logo_colour", allows you to change the colour of the text -->
                    <%--<h1><a href="index.html">simple<span class="logo_colour">style_blue_trees</span></a></h1>--%>
                    <img src="../../assets/images/logo.jpg" height="100" align="middle" />
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission Company 
                        LTD.</font></span>
                </div>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="AppHome.aspx">Home</a></li>
                    <li class="selected"><a href="Home.aspx">SignOut</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">

             <header>
        <h1 class="h1">Important Notification!</h1>
    </header>
<%--<asp:DynamicHyperLink ForeColor="Blue" runat="server"> https://www.mahatransco.in/information/details/maharashtra_stu </asp:DynamicHyperLink>--%>
    <section>
        <p>The State Transmission Utility (STU) has introduced a revised procedure for granting grid connectivity to renewable energy-based generating projects. This aligns with the Transmission Open Access (TOA) Regulation, RE Policy 2020 & methodology and CERC (Connectivity and General Network Access to the inter-State transmission System) Regulations, 2024 Dt. 19/06/2024. The revised procedure is effective immediately and has been published on the MSETCL website via notification dated 06/01/2024. (Click here to download revised grid connectivity (GC) procedure:- 
            <a href="https://www.mahatransco.in/information/details/maharashtra_stu">https://www.mahatransco.in/information/details/maharashtra_stu</a>
        </p> 
        <p> Developers are required to comply strictly with the guidelines outlined in the Revised procedure Grid Connectivity. </p>
<br />
<p style="color: blue;">As per clause No. 5.3 of Revised Procedure for grant of Grid Connectivity, gist of Documents to be submitted along with the Application are as follows:</p>
    <p class="p"><b>1.	Notarized affidavit:</b> as per Format – 1</p>
  <p class="p"><b>2.	Board Resolution: </b>for the proposed project.</p>
  <p class="p"><b>3.	Memorandum and Articles of Association </b>having provision to take up proposed business/ project.</p>
  <p class="p"><b>4.	For Solar/Wind/Renewable Power Park Developers: Submit documents as per combinations of (a) and (b) or combination of clauses (a) and (c):</b></p>
  <p class="p">&nbsp;a)	Government Authorization to undertake grid connectivity for solar/wind generators.</p>
  <p class="p">&nbsp;b)	Proof of ownership/lease rights for at least 50% of the required land.</p>
  <p class="p">&nbsp;c)	Bank Guarantee: ₹10 Lakh/MW for ≤1000 MW or ₹100 Crore + ₹5 Lakh/MW for capacity beyond 1000 MW.</p>
        <br />
  <p class="p"><b>5.	For Renewable Energy Projects or ESS (excluding Hydro/PSP):</b></p>
  <p class="p">&nbsp;a)	Letter of Award (LOA) or Power Purchase Agreement (PPA). <b>or</b></p>
  <p class="p">&nbsp;b)	Proof of ownership/lease rights for at least 50% of the required land. <b>or</b></p>
  <p class="p">&nbsp;c)	Bank Guarantee: ₹10 Lakh/MW for ≤1000 MW or ₹100 Crore + ₹5 Lakh/MW for capacity beyond 1000 MW</p>
        <br />

Single-window portal is currently under development to streamline the revised GC process. In the meantime, as a temporary measure, developers are required to: <p class="p">1.	Upload the necessary documents to the existing single-window portal.</p>
  <p class="p">2.	Email the documents to cestu@mahatransco.in within 24 hours of making the application/processing fee payment.</p>
        <br />
Note: Failure to comply with these requirements will result in the application being rejected.
<p class="p"> I have read and understood the Revised Procedure for Grant of Grid Connectivity and I hereby confirm that I would like to apply under the following category (please select one option):</p>
       <br />
        </section>
            <div>

            <!-- Application Type Selection -->
            <label>Applying as:</label><br />
            <input type="radio" name="applicationType" value="solarWindRenewable" id="radioSolarWindRenewable" /> Solar, Wind, or Renewable Power Park Developers (Refer to Clause 5.3.6 of the Revised GC Procedure).<br />
            <input type="radio" name="applicationType" value="renewableEnergy" id="radioRenewableEnergy" /> Renewable Energy Projects or ESS (excluding Hydro/PSP) (Refer to Clause 5.3.7 of the Revised GC Procedure). (Upload anyone):<br />
            <div id="solarWindRenewableDocuments" style="display:none;">
                <p>Documents for Solar, Wind, or Renewable Power Park Developers (Combinations of (a) & (b) or (a) & (c))</p>
                <asp:CheckBox ID="chkGovtAuthorization" runat="server" Text="a) Government Authorization for Grid Connectivity" /><br />
                <asp:CheckBox ID="chkProofOfOwnership" runat="server" Text="b) Proof of Ownership/Lease Rights for at least 50% of the required land" /><br />
                <asp:CheckBox ID="chkBankGuarantee" runat="server" Text="c) Bank Guarantee: ₹10 Lakh/MW or ₹100 Crore + ₹5 Lakh/MW" /><br /><br />
            </div>

            <!-- Renewable Energy Projects or ESS Documents (Select any from a, b, or c) -->
            <div id="renewableEnergyDocuments" style="display:none;" >
                <p>Select anyone and submit relevant document accordingly.</p>
                <asp:CheckBox ID="chkLOAorPPA" runat="server" Text="a) Letter of Award (LOA) or Power Purchase Agreement (PPA)" /><br />
                <asp:CheckBox ID="CheckBox1" runat="server" Text="b) Proof of Ownership/Lease Rights for at least 50% of the required land" /><br />
                <asp:CheckBox ID="CheckBox2" runat="server" Text="c) Bank Guarantee: ₹10 Lakh/MW or ₹100 Crore + ₹5 Lakh/MW" /><br /><br />
            </div>

            <!-- Submit Button -->
          

<asp:CheckBox ID="chkIAgree" runat="server" /> I Agree above Terms & Conditions.

<asp:CustomValidator 
    runat="server" 
    ID="CheckBoxRequired" 
    EnableClientScript="true" 
    ClientValidationFunction="ValidateCheckBox" 
    ErrorMessage="You must select this box to proceed." 
    ForeColor="Red" 
    Display="Dynamic" />

            </div>
                <!-- Submit Button -->
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return validateForm()" OnClick="btnSubmit_Click" />
            </form>
             
        </div>
   
          <script type="text/javascript">
              // Show the relevant document section based on the selected application type
              document.getElementById('radioSolarWindRenewable').onclick = function () {
                  document.getElementById('solarWindRenewableDocuments').style.display = 'block';
                  document.getElementById('renewableEnergyDocuments').style.display = 'none';
              };
              document.getElementById('radioRenewableEnergy').onclick = function () {
                  document.getElementById('renewableEnergyDocuments').style.display = 'block';
                  document.getElementById('solarWindRenewableDocuments').style.display = 'none';
              };
    </script> 
                
                
</body>
</html>
