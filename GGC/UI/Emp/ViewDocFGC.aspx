<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDocFGC.aspx.cs" Inherits="GGC.UI.Emp.ViewDocFGC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>View Documents</title>
     <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
    <style>
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
                <%--<span style="position:absolute;bottom:0px;right:0px">Welcome 
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>  --%>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="FGCHome.aspx">Home</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>

    <form id="form1" runat="server">
    <div class="mainbody">
            <center>
                <h3>
                    <asp:Label ID="lblAppNo" runat="server" Text="Document uploaded for Application No : "></asp:Label></h3>
            </center>
    <div>
    <table align="center" style="color: #333333;background-color: #F7F6F3;" width="80%">
    <tr>
    <td>
        <asp:Button ID="btnAppForm" runat="server" Text="Download Application Form" CssClass="button" 
            onclick="btnAppForm_Click" />
    </td>
    <td>
        <asp:Button ID="btnViewDocNext" runat="server" Text="Next document" CssClass="button" 
            onclick="btnViewDocNext_Click"  />
    </td>
    <td>
        <asp:Button ID="btnViewDocPrev" runat="server" Text="Previous document" 
            CssClass="button" onclick="btnViewDocPrev_Click"/>
    </td>
    
    </tr>
    <tr>
        <td colspan="4" align="center">
            <asp:Label ID="lblDocName" runat="server" Font-Bold="True"></asp:Label>
        </td>
    </tr>
    <tr>
    <td>
    Remark :
    </td>
    <td>
        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td align="right">
    <asp:Button ID="btnApp" runat="server" Text="Approve" 
            CssClass="button" onclick="btnApp_Click" />
        <asp:HiddenField ID="HdfDocType" runat="server" />
    </td>
    
    <td>
    <asp:Button ID="btnRet" runat="server" Text="Return" 
            CssClass="button" onclick="btnRet_Click" />
    </td>
    </tr>
    <tr>
    <td colspan="2">
        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
    </td>
    </tr>
    </table>
    </div>
    <div style="border-width: thin;border-color:Black">
        <asp:Literal ID="ltEmbed" runat="server" />
    </div>
    </div>
    </form>
    </div>
</body>
</html>
