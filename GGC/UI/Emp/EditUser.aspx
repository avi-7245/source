<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditUser.aspx.cs" Inherits="GGC.UI.Emp.EditUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit User</title>
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
        .style1
        {
            height: 34px;
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
                <span style="position: absolute; bottom: 0px; right: 0px">Welcome
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    
                    <li class="selected"><a href="EmpHome.aspx">Home</a></li>
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
                   Edit User Details.</h3>
            </center>
            <table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td>
                        <font color="red">*</font>SAP ID:
                    </td>
                    <td>
                        <asp:TextBox ID="txtSAPID" runat="server" MaxLength="5"></asp:TextBox>
                        <asp:Button ID="btnGetDet" runat="server" Text="Get Details" CssClass="button" 
                            onclick="btnGetDet_Click" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator119" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtSAPID" ValidationGroup="GetDet"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Emp Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmpName" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtEmpName" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        <font color="red">*</font>Designation:
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlDesgntn" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="style1">
                        <font color="red">*</font>Department/Office Name:
                    </td>
                    <td class="style1">
                        <asp:DropDownList ID="ddlDepartment" runat="server">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem>C. O.</asp:ListItem>
                            <asp:ListItem>EHV P.C. O&M Zone Aurangabad</asp:ListItem>
                            <asp:ListItem>EHV PC O & M Zone Nagpur</asp:ListItem>
                            <asp:ListItem>EHV PC O&M Zone Amravati</asp:ListItem>
                            <asp:ListItem>EHV PC O&M Zone Karad</asp:ListItem>
                            <asp:ListItem>EHV PC O&M Zone Nashik</asp:ListItem>
                            <asp:ListItem>EHV PC O&M Zone, Vashi</asp:ListItem>
                            <asp:ListItem>EHV Project O&M Zone  Pune</asp:ListItem>
                            <asp:ListItem>STU, C. O.</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="ddlDepartment"
                            InitialValue="-1" ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Zone :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlZone" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <font color="red">*</font>Reporting officer SAP ID :
                    </td>
                    <td>
                        <asp:TextBox ID="txtReportingSAPID" runat="server" MaxLength="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtReportingSAPID"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Email ID :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtReportingSAPID"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            ValidationGroup="Submit" Display="Dynamic" ErrorMessage="Invalid email address" />
                    </td>
                    <td>
                        <font color="red">*</font>Mobile Number :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMob" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtReportingSAPID"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ValidationGroup="Submit"
                            ControlToValidate="txtMob" ErrorMessage="Invalid Mobile No!!" ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                 <tr>
                 <td colspan="4" align="center">
                     <asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="button" 
                         ValidationGroup="Submit"/>
                 </td>
                 </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
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
