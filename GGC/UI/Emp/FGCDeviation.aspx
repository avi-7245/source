<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FGCDeviation.aspx.cs" Inherits="GGC.UI.Emp.FGCDeviation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Confirmation of Deviation</title>
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
            width: 900px;
            margin: 0px auto;
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
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission
                        Company LTD.</font></span>
                </div>
                <span style="position: absolute; bottom: 0px; right: 0px">Welcome
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="FGCHome.aspx">Home</a></li>
                    <li class="selected"><a href="EmpLogin.aspx">SignOut</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">
        <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>--%>
        <div class="mainbody">
            <center>
                <h3>
                    Upload Final Grid Connectivity draft Document</h3>
            </center>
            <table width="100%" align="center" cellspacing="5">
                <tr>
                    <td>
                        Application No :
                    </td>
                    <td>
                        <asp:Label ID="lblAppID" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                    Is Deviation :
                    </td>
                    <td>
                    <asp:RadioButtonList ID="rbIsDeviation" runat="server" 
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                    </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                <td>
                Deviation Remark :
                </td>
                <td>
                    <asp:TextBox ID="txtDeviationTrmark" TextMode="MultiLine" runat="server"></asp:TextBox>
                </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btnConfirm" Text="Confirm" runat="server" ValidationGroup="UploadLFS"
                            CssClass="button" onclick="btnConfirm_Click"/>
                            <asp:Button ID="btnReturn" Text="Return" runat="server" ValidationGroup="UploadLFS"
                            CssClass="button" onclick="btnReturn_Click"/>
                        
                                <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Red"></asp:Label>
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
