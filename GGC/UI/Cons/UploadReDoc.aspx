<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadReDoc.aspx.cs" Inherits="GGC.UI.Cons.UploadReDoc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reupload Documents</title>
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
                    <li class="selected"><a href="AppHome.aspx">Home</a></li>
                    <li class="selected"><a href="Home.aspx">SignOut</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div class="mainbody">
            <center>
                <h3>
                    Re-Upload Documents</h3>
            </center>
            <br />
            

            <table width="100%" align="center" align="center" cellspacing="5">
                <%--<tr>
                    <td colspan="4">
                        <b>Upload MEDA Letter</b>
                    </td>
                </tr>--%>                <%--<tr>
                    <td>
                        <asp:FileUpload ID="FUMEDADoc" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="lblLetterUploadStatus" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnUploadMEDADoc" Text="Upload" runat="server" ValidationGroup="Photo"  OnClick="UploadMEDALetter"  
                            CssClass="button"  />
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="Photo"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FUMEDADoc"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="Photo"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td colspan="4"  style="color: #333333;background-color: #F7F6F3;">
                        <b>Upload Substation Single Line diagram</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:FileUpload ID="FUSLD" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="lblSLDResult" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnUploadSLD" Text="Upload" runat="server" ValidationGroup="SLD" 
                            CssClass="button" onclick="btnUploadSLD_Click"  />
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="SLD"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FUSLD"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="SLD"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblSLD" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="color: #333333;background-color: #F7F6F3;">
                        <b>Upload Other Documents </b>(Upload all Project related other documents in single PDF.)
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:FileUpload ID="FUOther" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="lblOtherResult" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnOther" Text="Upload" runat="server" ValidationGroup="other"  
                            CssClass="button" onclick="btnOther_Click"  />
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidationGroup="other"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FUOther"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="other"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        <asp:Label ID="lblOther" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" visible="false" id="trother3" >
                    <td>
                        <asp:FileUpload ID="FileUpload3" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="Button3" Text="Upload" runat="server" ValidationGroup="other"  
                            CssClass="button" onclick="Button3_Click"  />
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="other"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                      
                    </td>
                    <td>
                        <asp:Label ID="lblOther3" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr runat="server" visible="false" id="trother4" >
                    <td>
                        <asp:FileUpload ID="FileUpload4" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="Button4" Text="Upload" runat="server" ValidationGroup="other"  
                            CssClass="button" onclick="Button4_Click"  />
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidationGroup="other"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                      
                    </td>
                    <td>
                        <asp:Label ID="lblOther4" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td colspan="4">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" 
                        onclick="btnAdd_Click"/>
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
