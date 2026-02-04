<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowLetter.aspx.cs" Inherits="GGC.UI.Emp.ShowLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Document Verification</title>
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
                    <li class="selected"><a href="EmpHome.aspx">Home</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
    <form id="form1" runat="server">
     <div class="mainbody">
            <center>
                <h3>
                    <asp:Label ID="lblAppNo" runat="server" Text="Document approval/rejection for Application No : "></asp:Label></h3>
            </center>
           
         <div>
        <table align="center">
            <tr>
                <td>
                Comments If any : 
                </td>
                <td>
                    <asp:TextBox ID="txtComments" runat="server" MaxLength="60" Rows="3" 
                        TextMode="MultiLine"></asp:TextBox>

                </td>
            </tr>
            <tr>
            <td>
            Whether additional capacity to existing or sanction project?
            </td>
            <td>
                <asp:RadioButtonList ID="rbAddCap" runat="server" RepeatDirection="Horizontal">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Selected="True" Value="2">No</asp:ListItem>
                </asp:RadioButtonList>
            </td>
            </tr>
            <tr>
                <td align="center" colspan="4">
                    <asp:Button ID="btnApprove" runat="server" Text="Confirm" OnClick="btnApprove_Click"
                        CssClass="button" /> &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnReject" runat="server" Text="Return" OnClick="btnReject_Click"
                        CssClass="button" />
                </td>
            </tr>
        </table>
    </div>           
    <div>
    <asp:Literal ID="ltEmbed" runat="server" />
       <%-- <asp:Image ID="imgMedaLetter" runat="server" Height="400px" Width="800px" />--%>
    
    </div>
    <%--<div>
        <asp:Button ID="btnApprove" runat="server" Text="Approve" 
            onclick="btnApprove_Click" CssClass="button" />
        <asp:Button ID="btnReject"
            runat="server" Text="Reject" onclick="btnReject_Click" CssClass="button" />
    </div>--%>
    </div>
    </form>
    </div>
</body>
</html>
