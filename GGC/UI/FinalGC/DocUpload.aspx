<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocUpload.aspx.cs" Inherits="GGC.UI.FinalGC.DocUpload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FGC Upload doc</title>
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
            cursor: pointer;
        }

        .disabled-button {
            opacity: 0.5;
            cursor: not-allowed;

        }

        .modal {
            display: none;
            position: fixed;
            z-index: 1;
            left: 0;
            top: 0;
            width: 100%;
            height: 100%;
            overflow: hidden; /* Hide overflow to remove scrollbar */
            background-color: rgba(0, 0, 0, 0.4);
            transition: opacity 0.3s ease;
        }

        .modal-content {
            background-color: #fefefe;
            margin: 0; /* Remove margin */
            padding: 20px;
            border: 1px solid #888;
            width: 100%;
            height: 100%;
            overflow-y: auto;
            border-radius: 0; /* Remove border-radius */
            position: relative;
            box-sizing: border-box;
        }

        .right {
            position: absolute;
            bottom: 10px;
            right: 10px;
        }

        .modal.active {
            display: flex;
            justify-content: center;
            align-items: center;
        }

            .modal.active .modal-content {
                opacity: 1;
            }

        .close {
            color: #aaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
            transition: color 0.3s ease;
        }

            .close:hover,
            .close:focus {
                color: #333;
                text-decoration: none;
                cursor: pointer;
            }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
    <script type="text/javascript">
        class Modal {
            constructor(modalId) {
                this.modal = document.getElementById(modalId);
                this.btnOpen = document.getElementById("btn-open-" + modalId);
                this.btnClose = document.getElementById("btn-close-" + modalId);
                this.span = document.getElementById("span-" + modalId + "-close");

                if (this.btnOpen) this.btnOpen.addEventListener("click", () => this.open());

                if (this.btnClose) this.btnClose.addEventListener("click", () => this.close());

                if (this.span) this.span.addEventListener("click", () => this.close());

                window.addEventListener('click', (event) => {
                    if (event.target === this.modal) {
                        this.close();
                    }
                });
            }

            open() {
                this.modal.classList.add('active');
            }

            close() {
                this.modal.classList.remove('active');
            }
        }

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
        function ViewUploadedDocModal() {
            const modal = new Modal("doc-modal");
            modal.open();
        }

    </script>
</head>
<body>
    <div id="doc-modal" class="modal">
        <div class="modal-content" runat="server">
            <span id="span-doc-modal-close" class="close">&times;</span>
            <h2>
                <asp:Label ID="lblSelectedDocName" runat="server"></asp:Label></h2>
            <asp:Literal runat="server" ID="htmlLiteral" />
        </div>
    </div>
    <div id="main">
        <div id="header">
            <div id="logo">
                <div id="logo_text">
                    <!-- class="logo_colour", allows you to change the colour of the text -->
                    <%--<h1><a href="index.html">simple<span class="logo_colour">style_blue_trees</span></a></h1>--%>
                    <img src="../../assets/images/logo.jpg" height="100" align="middle" alt="Maharashtra State Electricity Transmission Company LTD" />
                    <span class="logo_colour"><font>Maharashtra State Electricity Transmission Company LTD.</font></span>
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
                    <h3>Upload Documents of Final GC</h3>
                </center>
                <br />
                <center>
                    <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Green"></asp:Label>
                </center>
                <div>
                    <asp:GridView ID="GVApplications" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" OnRowCommand="GVApplications_RowCommand">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="HFFileName" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Srno" HeaderText="SR.NO">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="DOC_NAME" HeaderText="DOCUMENT TYPE">
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                            </asp:BoundField>

                            <asp:TemplateField HeaderText="Select File" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:FileUpload ID="FUUpload" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:TemplateField>


                            <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Button ID="btnUpload" Text="Upload" runat="server" CssClass="button" CommandName="UploadDocs" CommandArgument="<%# Container.DataItemIndex %>" />
                                </ItemTemplate>

                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="View Document">
                                <ItemTemplate>
                                    <asp:Button ID="btnViewDocument" Text="View" runat="server" CssClass="button disabled-button" CommandName="ViewUploadedDocs" CommandArgument="<%# Container.DataItemIndex %>" Enabled="false" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Uploaded or Not" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Label ID="lblUploadStatus" runat="server" Text="" ForeColor="Green"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btnUpload" Text="Final Submit" runat="server" CssClass="button"
                        OnClick="btnUpload_Click" />
                </div>

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
