<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApprComm.aspx.cs" Inherits="GGC.UI.Emp.ApprComm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approve</title>
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
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission Company 
                        LTD.</font></span>
                </div>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="EmpHome.aspx">Home</a></li>
                    <li class="selected" id="liSignOut" runat="server"><a href="EmpLogin.aspx">SignOut</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">
        <div class="mainbody">
            <center>
                <h3>
                    Commitee Approval.</h3>
            </center>
            <table width="100%" align="center" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Application No :"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblApplcationNo" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td>
                Approved ?
                </td>
                <td>
                    <asp:RadioButtonList ID="RBApprove" runat="server" 
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                        <asp:ListItem Value="N">No</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label2" runat="server" Text="Upload Letter :"></asp:Label>
                    </td>
                    <td>
                        <asp:FileUpload ID="FUApproval" runat="server" onchange="checkUploadFile(this);" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="Photo"
                            ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FUApproval"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="Photo"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                <td>
                Remark : 
                </td>
                <td>
                
                    <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine"></asp:TextBox>
                
                </td>
                </tr>
                <tr>
                <td colspan="2">
                                        <asp:Button ID="btnUploadMEDADoc" Text="Submit" runat="server" ValidationGroup="Photo" 
                            CssClass="button" onclick="btnUploadMEDADoc_Click"  />

                </td>
                </tr>
                <tr>
                <td colspan="2">
                    <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                </td>
                </tr>
            </table>
        </div>
        </form>
    </div>
</body>
</html>
