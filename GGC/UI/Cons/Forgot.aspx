<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="GGC.UI.Cons.Forgot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
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
    </style>
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
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
            <%--<div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="AppHome.aspx">Home</a></li>
                    <li class="selected"><a href="Home.aspx">SignOut</a></li>
                </ul>
            </div>--%>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">
        <div class="mainbody">
            <center>
                <h3>
                    Forgot Password</h3>
            </center>
            <table width="40%" align="center" cellspacing="0">
                <tr>
                    <td>
                       User ID : 
                    </td>
                    <td>
                        <asp:TextBox ID="txtUID" runat="server" ValidationGroup="sendOTP" MaxLength="12"></asp:TextBox>
                        <asp:Button ID="btnSendOTP" runat="server" Text="Send OTP" CssClass="button" ValidationGroup="sendOTP"
                            onclick="btnSendOTP_Click" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtUID" ErrorMessage="*Required"  
                            ValidationGroup="sendOTP" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr runat="server" id="trSubmitOTP" visible="false">
                <td>
                Enter OTP : 
                </td>
                <td>
                 <asp:TextBox ID="txtOTP" runat="server" ValidationGroup="submitOTP" TextMode="Password"  MaxLength="6"></asp:TextBox>
                 <asp:Button ID="btnSubmit" runat="server" Text="Submit OTP" CssClass="button" 
                        ValidationGroup="submitOTP" onclick="btnSubmit_Click"
                            />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="submitOTP" 
                            ControlToValidate="txtOTP" ErrorMessage="*Required" ForeColor="Red" 
                        Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                </tr>
                </table>
                <table  width="40%" align="center" cellspacing="0" runat="server" id="tblNewPass" visible="false">
                <tr>
                <td>
                Enter New Password : 
                </td>
                <td>
                 <asp:TextBox ID="txtNewPassword" runat="server" ValidationGroup="submitPass" TextMode="Password" MaxLength="16"></asp:TextBox>
                
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="submitPass" 
                            ControlToValidate="txtNewPassword" ErrorMessage="*Required" 
                        ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                </td>
                </tr>
                <tr>
                <td>
                Confirm Password : 
                </td>
                <td>
                 <asp:TextBox ID="txtConfirmPass" runat="server" ValidationGroup="submitPass" TextMode="Password"  MaxLength="16"></asp:TextBox>
                 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="submitPass" 
                            ControlToValidate="txtConfirmPass" ErrorMessage="*Required" 
                        ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" Display="Dynamic" 
                        ForeColor="Red" ErrorMessage="Both password doesnot match." 
                        ControlToCompare="txtNewPassword" ControlToValidate="txtConfirmPass" ValidationGroup="submitPass"></asp:CompareValidator>
                </td>
                </tr>
                <tr>
                <td colspan="2">
                <asp:Button ID="btnUpdatePass" runat="server" Text="Submit" CssClass="button" 
                        ValidationGroup="submitPass" onclick="btnUpdatePass_Click"
                            />
                </td>
                </tr>
            </table>
        </div>
        </form>
        <div id="footer">
            <p>
                Designed and Developed by IT Department, MSETCL.</p>
        </div>
    </div>
</body>
</html>
