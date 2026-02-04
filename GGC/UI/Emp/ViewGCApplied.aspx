<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewGCApplied.aspx.cs" Inherits="GGC.UI.Emp.ViewGCApplied" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View</title>

    
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
                <span style="position:absolute;bottom:0px;right:0px">Welcome 
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>  
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="GCApplied.aspx">Back</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>

    <form id="form1" runat="server">
    <div class="mainbody">
            <center>
                <h3>
                    <asp:Label ID="lblAppNo" runat="server" Text="Application No : "></asp:Label></h3>
            </center>
            <br />

    <div>
    <table align="center" border="1">
    <tr>
    <td>Applied Date :</td>
    <td>
        <asp:TextBox ID="txtAppDt" runat="server" Enabled="false"></asp:TextBox>

    </td>
    </tr>
    <tr>
    <td>
    Remark :
    </td>
    <td>
        <asp:TextBox ID="txtRemark" runat="server" MaxLength="500" Rows="4" 
            TextMode="MultiLine"></asp:TextBox>
    </td>
    </tr>
    <tr>
    <td colspan="2" align="center">
    <asp:Button ID="btnApproved" Text="Approved" runat="server" CssClass="button" 
            onclick="btnApproved_Click" Enabled="False"/> &nbsp;
    <asp:Button ID="btnReject" Text="Reject" runat="server" CssClass="button" 
            onclick="btnReject_Click" Enabled="False"/>

    </td>
    </tr>
    <td colspan="2" align="center">
    <tr>
    <td>
        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
    </td>
    </tr>
    </table>
    </div>
    <div>
        <table>
            
            <tr>
                <td>
                    <asp:Button ID="btnApplication" runat="server" Text="View Application"
                        CssClass="button" onclick="btnApplication_Click" />
                </td>
                <td>
                    <asp:Button ID="btnGCLetter" runat="server" Text="GC Letter" 
                        CssClass="button" onclick="btnGCLetter_Click" />
                </td>
                <td>
                    <asp:Button ID="btnOther" runat="server" Text="Other Document"
                        CssClass="button" onclick="btnOther_Click" />
                </td>
                <td>
                    <asp:Button ID="btnExtension1" runat="server" Text="Extension 1" 
                        CssClass="button" onclick="btnExtension1_Click" />
                </td>
                <td>
                    <asp:Button ID="btnExtension2" runat="server" Text="Extension 2"
                        CssClass="button" onclick="btnExtension2_Click" />
                </td>
                
            </tr>
        </table>
    </div>
    <div>
        <asp:Literal ID="ltEmbed" runat="server" />
    </div>
    </div>
    </form>
    </div>
</body>
</html>
