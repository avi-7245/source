<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalGCDraftUpload.aspx.cs" Inherits="GGC.UI.Emp.FinalGCDraftUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Final GC draft Letter</title>
    <style type="text/css">
        .form-control {
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

            .form-control:focus {
                border-color: #66afe9;
                outline: 0;
                -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
                box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
            }

            .form-control::-moz-placeholder {
                color: #999;
                opacity: 1;
            }

            .form-control:-ms-input-placeholder {
                color: #999;
            }

            .form-control::-webkit-input-placeholder {
                color: #999;
            }

        .form-control-textarea {
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

            .form-control-textarea :focus {
                border-color: #66afe9;
                outline: 0;
                -webkit-box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
                box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
            }

            .form-control-textarea ::-moz-placeholder {
                color: #999;
                opacity: 1;
            }

            .form-control-textarea :-ms-input-placeholder {
                color: #999;
            }

            .form-control-textarea ::-webkit-input-placeholder {
                color: #999;
            }

        .mainbody {
            width: 900px;
            margin: 0px auto;
        }

        .button {
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
            validExtensions[0] = 'pdf';
            validExtensions[1] = 'PDF';
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
                <span style="position: absolute; bottom: 0px; right: 0px">Welcome 
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="EmpHome.aspx">Home</a></li>
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
                    <h3>Upload 
                    Final Grid Connectivity Issued Letter.</h3>
                </center>
                <table width="100%" align="center" cellspacing="5">

                    <tr>
                        <td>Upload Final GC draft proposal :
                        </td>
                        <td>
                            <asp:FileUpload ID="FUDoc" runat="server" onchange="checkUploadFile(this);" Height="20px" Width="207px" />
                            <asp:Label ID="lblTechUploadStatus" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnUploadDoc" Text="Upload" runat="server" ValidationGroup="UploadIL"
                                CssClass="button" OnClick="btnUploadDoc_Click" />
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="UploadIL"
                                OnServerValidate="ValidateFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                                Display="Dynamic"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FUDoc"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="UploadIL"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblResult" runat="server" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Upload Final GC draft letter :
                        </td>
                        <td>
                            <asp:FileUpload ID="FUUploadDraftGC" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnUploadDraftGC" Text="Upload" runat="server" ValidationGroup="UploadLetter"
                                CssClass="button" OnClick="btnUploadDraftGC_Click" />
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="UploadLetter"
                                OnServerValidate="ValidateFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                                Display="Dynamic"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FUUploadDraftGC"
                                Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="UploadLetter"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblDraftMessage" runat="server" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5" align="center">
                            <asp:Button ID="btnFinalSubmit" runat="server" Text="Final Submit" ValidationGroup="FinalSubmit"
                                CssClass="button"
                                OnClick="btnFinalSubmit_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Green"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="footer">
                <p>
                    Design and Developed by IT Department, MSETCL.
                </p>
            </div>
        </form>
    </div>


</body>
</html>
