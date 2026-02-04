<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayOffline.aspx.cs" Inherits="GGC.UI.Cons.PayOffline" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Pay Offline</title>
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
    <script type="text/javascript">
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
            validExtensions[0] = 'JPEG';
            validExtensions[1] = 'JPG';
            validExtensions[2] = 'PNG';
            validExtensions[3] = 'jpeg';
            validExtensions[4] = 'jpg';
            validExtensions[5] = 'png';
            validExtensions[6] = 'PDF';
            validExtensions[7] = 'pdf';
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
            if (myfile.search(/[<>'\s+\"\/;`%]/) > 0) {
                alert('please upload the file without special characters and SPACES');
                elem.value = '';
                return false;
            }
            else {
                //alert('valid Format');
                return true;
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
        <div class="mainbody">
            <center>
                <h3>
                    Organization Registration Process.</h3>
            </center>
            <table width="100%" align="center" cellspacing="0">
                <tr>
                    <td>
                        Application No :
                    </td>
                    <td>
                        <asp:Label ID="lblAppNo" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        DD No :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDDNo" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Bank Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtBankName" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        DD Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDDDate" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        DD Scan Copy :
                    </td>
                    <td>
                        <asp:FileUpload ID="FUDDCopy" runat="server" onchange="checkUploadFile(this);" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="Submit"
                            OnServerValidate="ValidateFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FUDDCopy"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        DD No :
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server" MaxLength="15"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                <td colspan="2">
                <asp:Button ID="btnSubmit" Text="Submit" runat="server" ValidationGroup="Submit"  
                            CssClass="button" onclick="btnSubmit_Click"  />
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
