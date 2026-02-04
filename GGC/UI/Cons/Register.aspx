<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="GGC.UI.Cons.Register" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Application</title>
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
    <style>
   
   /* Modal Header */
.modal-header {
  padding: 2px 16px;
  background-color: #5cb85c;
  color: white;
}

/* Modal Body */
.modal-body {padding: 2px 16px;}

/* Modal Footer */
.modal-footer {
  padding: 2px 16px;
  background-color: #5cb85c;
  color: white;
}

/* Modal Content */
.modal-content {
  position: relative;
  background-color: #fefefe;
  margin: auto;
  padding: 0;
  border: 1px solid #888;
  width: 80%;
  box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2),0 6px 20px 0 rgba(0,0,0,0.19);
  animation-name: animatetop;
  animation-duration: 0.4s
}

/* Add Animation */
@keyframes animatetop {
  from {top: -300px; opacity: 0}
  to {top: 0; opacity: 1}
}
        .mainbody
        {
            width: 900px;
            margin: 0px auto;
        }
        #customers
        {
            font-family: "Trebuchet MS" , Arial, Helvetica, sans-serif;
            border-collapse: collapse;
            width: 100%;
        }
        .button
        {
            background-color: #008CBA; /* Blue */
            border: none;
            color: white;
            padding: 5px 20px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 14px;
        }
        #customers td, #customers th
        {
            border: 1px solid #ddd;
            padding: 8px;
        }
        
        #customers tr:nth-child(even)
        {
            background-color: #f2f2f2;
        }
        
        #customers tr:hover
        {
            background-color: #ddd;
        }
        
        #customers th
        {
            padding-top: 12px;
            padding-bottom: 12px;
            text-align: left;
            background-color: #4CAF50;
            color: white;
        }
        .form-control
        {
            display: block;
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
    </style>
    <script type="text/javascript">
        function setUploadGSTIN() {

            var maxFileSize = 4194304; // 4MB -> 4 * 1024 * 1024
            var fileUpload = $('#FUGSTIN');

            if (fileUpload.val() == '') {
                return false;
            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {
                    $('#btnReg').prop('disabled', false);
                    return true;
                } else {
                    $('#lblGSTINStatus').text('File too big !')
                    return false;
                }
            }
        }

        function checkUploadFile(elem) {
            if (checkFileExtension(elem)) {
                ValidateFileName(elem);
            }
        }
        function checkFileExtension(elem) {
            var filePath = elem.value;
            if (filePath.indexOf('.') == -1)
                return false;

            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
            //Add valid extentions in this array
            //            validExtensions[0] = 'JPEG';
            //            validExtensions[1] = 'JPG';
            //            validExtensions[2] = 'PNG';
            //            validExtensions[3] = 'jpeg';
            //            validExtensions[4] = 'jpg';
            //            validExtensions[5] = 'png';
            validExtensions[0] = 'PDF';
            validExtensions[1] = 'pdf';
            validExtensions[2] = 'Pdf';

            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }
            alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
            elem.value = '';
            return false;
        }
        function ValidateFileName(elem) {


            var myfile = elem.value;
            if (myfile.search(/[<>'+\"\/;`%]/) > 0) {
                alert('please upload the file without special characters and SPACES');
                elem.value = '';
                return false;
            }
            else {
                //alert('valid Format');
                return true;
            }

        }
//        function setUploadButtonState() {

//            var maxFileSize = 2097152; // 2MB -> 2 * 1024 * 1024
//            var fileUpload = $('#FUGSTIN');

//            if (fileUpload.val() == '') {
//                return false;
//            }
//            else {
//                if (fileUpload[0].files[0].size < maxFileSize) {
//                    $('#button_fileUpload').prop('disabled', false);
//                    return true;
//                } else {
//                    $('#lblGSTINStatus').text('File too big !')
//                    return false;
//                }
//            }
//        }
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
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission
                        LTD.</font></span>
                </div>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="Home.aspx">Home</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <%--<table width="90%" align="center" cellspacing="10">
        <tr>
            <td width="20%">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/assets/images/logo.jpg" />
            </td>
            <td width="80%">
                <h1>
                    MAHARASHTRA STATE ELECTRICITY TRANSMISSION LTD</h1>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <h2>
                   Organization Registration</h2>
            </td>
        </tr>
    </table>--%>
        <asp:UpdatePanel ID="updatepnl" runat="server">
            <ContentTemplate>
                <div class="mainbody">
                    <center>
                        <h3>
                            Organization Registration</h3>
                    </center>
                    <table width="100%" align="center" cellspacing="0">
                        <tr>
                            <td>
                                Is GSTIN registered?
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbISGSTIN" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="rbISGSTIN_SelectedIndexChanged"
                                    AutoPostBack="true" RepeatLayout="Flow">
                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td colspan="2">
                                <div id="dvProvGSTIN" runat="server" visible="false">
                                    Specify Provisional GSTIN :
                                    <asp:TextBox ID="txtProvGSTIN" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                                        Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProvGSTIN" ValidationGroup="Submit"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                        </tr>
                        <tr style="background-color: white">
                            <td>
                                GSTIN :
                            </td>
                            <td>
                                <asp:TextBox ID="txtGSTIN" CssClass="form-control" runat="server" MaxLength="15"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtGSTIN" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                                PAN :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPAN" CssClass="form-control" runat="server" MaxLength="10"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPAN" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtPAN"
                                    Display="Dynamic" ForeColor="Red" ErrorMessage="InValid PAN" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}"
                                    ValidationGroup="Submit"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr style="background-color: black">
                            <td>
                                Organization Name :
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtOrgName" Width="350" CssClass="form-control" runat="server" MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtOrgName" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtAddress1" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress2" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtAddress2" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress3" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtAddress3" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                PinCode :
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPin" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPin" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtPin"
                                    Display="Dynamic" ForeColor="Red" ErrorMessage="InValid PIN" ValidationExpression="[A-Z]{5}\d{4}[A-Z]{1}"
                                    ValidationGroup="Submit"></asp:RegularExpressionValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                State :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlState" runat="server">
                                    <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                    <asp:ListItem Value="Andaman and Nicobar Islands">Andaman and Nicobar Islands</asp:ListItem>
                                    <asp:ListItem Value="Andhra Pradesh">Andhra Pradesh</asp:ListItem>
                                    <asp:ListItem Value="Arunachal Pradesh">Arunachal Pradesh</asp:ListItem>
                                    <asp:ListItem Value="Assam">Assam</asp:ListItem>
                                    <asp:ListItem Value="Bihar">Bihar</asp:ListItem>
                                    <asp:ListItem Value="Chandigarh">Chandigarh</asp:ListItem>
                                    <asp:ListItem Value="Chhattisgarh">Chhattisgarh</asp:ListItem>
                                    <asp:ListItem Value="Dadra and Nagar Haveli">Dadra and Nagar Haveli</asp:ListItem>
                                    <asp:ListItem Value="Daman and Diu">Daman and Diu</asp:ListItem>
                                    <asp:ListItem Value="Delhi">Delhi</asp:ListItem>
                                    <asp:ListItem Value="Goa">Goa</asp:ListItem>
                                    <asp:ListItem Value="Gujarat">Gujarat</asp:ListItem>
                                    <asp:ListItem Value="Haryana">Haryana</asp:ListItem>
                                    <asp:ListItem Value="Himachal Pradesh">Himachal Pradesh</asp:ListItem>
                                    <asp:ListItem Value="Jammu and Kashmir">Jammu and Kashmir</asp:ListItem>
                                    <asp:ListItem Value="Jharkhand">Jharkhand</asp:ListItem>
                                    <asp:ListItem Value="Karnataka">Karnataka</asp:ListItem>
                                    <asp:ListItem Value="Kerala">Kerala</asp:ListItem>
                                    <asp:ListItem Value="Lakshadweep">Lakshadweep</asp:ListItem>
                                    <asp:ListItem Value="Madhya Pradesh">Madhya Pradesh</asp:ListItem>
                                    <asp:ListItem Value="Maharashtra">Maharashtra</asp:ListItem>
                                    <asp:ListItem Value="Manipur">Manipur</asp:ListItem>
                                    <asp:ListItem Value="Meghalaya">Meghalaya</asp:ListItem>
                                    <asp:ListItem Value="Mizoram">Mizoram</asp:ListItem>
                                    <asp:ListItem Value="Nagaland">Nagaland</asp:ListItem>
                                    <asp:ListItem Value="Orissa">Orissa</asp:ListItem>
                                    <asp:ListItem Value="Pondicherry">Pondicherry</asp:ListItem>
                                    <asp:ListItem Value="Punjab">Punjab</asp:ListItem>
                                    <asp:ListItem Value="Rajasthan">Rajasthan</asp:ListItem>
                                    <asp:ListItem Value="Sikkim">Sikkim</asp:ListItem>
                                    <asp:ListItem Value="Tamil Nadu">Tamil Nadu</asp:ListItem>
                                    <asp:ListItem Value="Tripura">Tripura</asp:ListItem>
                                    <asp:ListItem Value="Uttaranchal">Uttaranchal</asp:ListItem>
                                    <asp:ListItem Value="Uttar Pradesh">Uttar Pradesh</asp:ListItem>
                                    <asp:ListItem Value="West Bengal">West Bengal</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ForeColor="Red"
                                    ErrorMessage="Select State!" InitialValue="-1" ControlToValidate="ddlState" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                Country :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server">
                                    <asp:ListItem>India</asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor="Red"
                                    ErrorMessage="Select Coutry!" InitialValue="-1" ControlToValidate="ddlCountry"
                                    ValidationGroup="Submit"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Phone :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPhone" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="STDCode-Number Format"
                                    ControlToValidate="txtPhone" Display="Dynamic" ForeColor="#FF3300" SetFocusOnError="True"
                                    ValidationExpression="\d{3}([- ]*)\d{6}"></asp:RegularExpressionValidator>--%>
                            </td>
                            <td>
                                Mobile :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMob" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Enter proper Mobile No!"
                                    ControlToValidate="txtMob" Display="Dynamic" ForeColor="#FF3300" SetFocusOnError="True"
                                    ValidationExpression="\d{10}"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                                    MaxLength="40"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtEmail" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Enter proper Email ID!!"
                                    ControlToValidate="txtEmail" Display="Dynamic" ForeColor="#FF3300" SetFocusOnError="True"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="Submit"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                Fax :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td>
                                User Name :
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtUName" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                                <asp:LinkButton ID="lnkChkUName" runat="server" OnClick="lnkChkUName_Click">Check availability</asp:LinkButton>
                                <asp:Label ID="lblLoginStatus" runat="server" Text="Label"></asp:Label>
                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtUName" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtUName" ValidationGroup="checkuname"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Password :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPass" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPass" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegExp1" runat="server" ErrorMessage="Password length must be between 7 to 16 characters"
                                    ControlToValidate="txtPass" ValidationExpression="^[a-zA-Z0-9'@&#.\s]{7,16}$"
                                    Display="Dynamic" ForeColor="Red" />
                            </td>
                            <td>
                                Confirm Password :
                            </td>
                            <td>
                                <asp:TextBox ID="txtCPass" TextMode="Password" CssClass="form-control" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ForeColor="Red"
                                    Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtCPass" ValidationGroup="Submit"
                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtPass"
                                    ControlToValidate="txtCPass" Display="Dynamic" ErrorMessage="Password do not match."
                                    ForeColor="Red" ValidationGroup="Submit"></asp:CompareValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="Password length must be between 7 to 16 characters"
                                    ControlToValidate="txtCPass" ValidationExpression="^[a-zA-Z0-9'@&#.\s]{7,16}$" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Upload GSTIN or Provisional GSTIN :
                            </td>
                            <td colspan="3">
                                <asp:FileUpload ID="FUGSTIN" runat="server" onchange="checkUploadFile(this);" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FUGSTIN"
                                    Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <%--<asp:CustomValidator ID="customGSTINValidate" runat="server" OnServerValidate="ValidateFileSize"  ValidationGroup="Submit"/>--%>
                                    <asp:CustomValidator ID="customValidatorUpload" Display="Dynamic" ForeColor="Red" runat="server" ErrorMessage="" ControlToValidate="FUGSTIN" ClientValidationFunction="setUploadGSTIN();" />
                                <asp:Label ID="lblGSTINStatus" runat="server" Text="" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Upload PAN :
                            </td>
                            <td colspan="3">
                                <asp:FileUpload ID="FUPAN" runat="server" onchange="checkUploadFile(this);" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="FUPAN"
                                    Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                                    <asp:CustomValidator ID="customPANValidate" runat="server" OnServerValidate="ValidateFileSize"  ValidationGroup="Submit"/>
                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btnReg" CssClass="button" runat="server" Text="Register" OnClick="btnReg_Click"
                                    ValidationGroup="Submit" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" align="center">
                                <asp:Label ID="lblResult" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <div id="content_footer">
                    </div>
                </div>
                <div id="footer">
                    <p>
                        Design and Developed by IT Department, MSETCL.</p>
                </div>
                <%-- <asp:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="Panel1" TargetControlID="btnReg"
    BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" style = "display:none">
    This is an ASP.Net AJAX ModalPopupExtender Example<br />
    <asp:Button ID="btnClose" runat="server" Text="Close" />
</asp:Panel>     --%>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnReg" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="modal-content" id="divModal" runat="server" visible="false">
            <div class="modal-header">
                <span class="close">&times;</span>
                <h2>
                    Modal Header</h2>
            </div>
            <div class="modal-body">
                <p>
                    Some text in the Modal Body</p>
                <p>
                    Some other text...</p>
            </div>
            <div class="modal-footer">
                <h3>
                    Modal Footer</h3>
            </div>
        </div>
        </form>
    </div>
</body>
</html>
