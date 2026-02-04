<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="changepass.aspx.cs" Inherits="GGC.UI.Emp.changepass" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
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
            border-radius: 15px;
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
                <span style="position:absolute;bottom:0px;right:0px">Welcome 
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>  
            </div>
            
            
            
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected" id="home" runat="server" visible="false"><a href="EmpHome.aspx">Home</a></li>
                    <li class="selected" id="prList" runat="server" visible="false"><a href="ProposalApprovalList.aspx">Home</a></li>
                    <li class="selected" id="payList" runat="server" visible="false"><a href="../Finance/ApprovePayment.aspx">Home</a></li>
                    <li class="selected" id="liSignOut" runat="server"><a href="EmpLogin.aspx">SignOut</a></li>
                </ul>
                
            </div>
            
        </div>
        <%--<div id="content_header">
        
        </div>--%>
    <form id="form1" runat="server">
     <div class="mainbody">
            <center>
                <h3>
                    Change Password</h3>
            </center>
         
    <table width="100%" align="center" cellspacing="0">
        <tr>
            <td>
         New Password :
            </td>
            <td>
                <asp:TextBox ID="txtNewPass" runat="server"  MaxLength="16" TextMode="Password"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtNewPass"
    ForeColor="Red" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,16}$" 
            ErrorMessage="Password must contain: 8 to 16 characters atleast 1 UpperCase Alphabet, 1 LowerCase Alphabet , 1 Special character and 1 Number" 
            Display="Dynamic" />
            </td>
        </tr>
        <tr>
            <td>
         Confirm Password :
            </td>
            <td>
                <asp:TextBox ID="txtConfirmPass" runat="server" MaxLength="16" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ErrorMessage="Password does not match." ControlToValidate="txtConfirmPass" 
                    ControlToCompare="txtNewPass" ForeColor="Red"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
        <td colspan="2" >
        <asp:Button ID="btnChangePass" Text="Change Password" runat="server" 
                CssClass="button" onclick="btnChangePass_Click"/>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
            <%--<asp:LinkButton
                ID="lnkHome" runat="server" Visible="false" CausesValidation="False">Click here for Home page</asp:LinkButton>--%>
        </td>
        
        </tr>
    </table>
      </div>
        <div id="footer">
            <p>
                Design and Developed by IT Department, MSETCL.</p>
        </div>
    </form>
    </div>
</body>
</html>
