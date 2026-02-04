<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppHome.aspx.cs" Inherits="GGC.UI.MSKVYSPD.AppHome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
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
            width: 1160px;
            margin: 0px auto;
        }

        .button {
            background-color: #008CBA; /* Blue */
            border: none;
            color: white;
            padding: 2px 10px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 13px;
        }

        a.Linkbutton {
            border: 1px solid white;
            border-radius: 10px;
            box-shadow: 0 0 34px #008CBA inset;
            color: black;
            text-align: center;
            text-decoration: none;
            text-shadow: 0 1px 2px white;
            text-transform: uppercase;
            font: lighter 12px Helvetica,sans-serif;
            transition: color 0.25s ease-in-out 0s;
            margin: 0 auto;
            padding: 7px 2%;
        }

        .rounded-corners {
            border: 1px solid #A1DCF2;
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
            overflow: hidden;
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
            <div class="mainbody">

                <div style="float: right;">
                    <h4>
                        <asp:Label ID="txtOrganizationName" runat="server" Text=""></asp:Label>
                    </h4>
                </div>
                <center>
                    <h3>MSKVY-SPV
                    Application List</h3>
                </center>

                <div>
                    <asp:LinkButton ID="lnkCons" runat="server" CssClass="Linkbutton"
                        OnClick="lnkCons_Click">
                        Create New Application</asp:LinkButton>
                </div>
                <br />
                <div class="rounded-corners">
                    <asp:GridView ID="GVApplications" runat="server" AutoGenerateColumns="False" CellPadding="4"
                        ForeColor="#333333" GridLines="Both"
                        OnRowCommand="GVApplications_RowCommand"
                        OnRowDataBound="GVApplications_RowDataBound">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:BoundField DataField="APPLICATION_NO" HeaderText="Application NO">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MEDAProjectID" HeaderText="Cluster ID">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PAN_TAN_NO" HeaderText="PAN" Visible="false">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GSTIN_NO" HeaderText="GSTIN" Visible="false">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="MEDA_REC_LETTER_NO" HeaderText="MEDA RECOMMENDATION LETTER NO"
                                Visible="false" />
                            <asp:BoundField DataField="MEDADateF" HeaderText="MEDA RECOMMENDATION LETTER DATE"
                                Visible="false" />
                            <asp:BoundField DataField="CONT_PER_NAME_1" HeaderText="CONTACT PERSON" Visible="false" />
                            <asp:BoundField DataField="NATURE_OF_APP" HeaderText="Nature Of Application" Visible="false">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="PROJECT_TYPE" HeaderText="Project Type" />
                            <asp:BoundField DataField="PROJECT_TYPE" HeaderText="SUBSTATION NAME" />
                            <asp:BoundField DataField="PROJECT_CAPACITY_MW" HeaderText="Project Capacity(MW)">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="app_status" HeaderText="Application Status">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="GENERATION_VOLTAGE" HeaderText="GENERATION VOLTAGE" Visible="false" />
                            <asp:BoundField DataField="INJECTION_VOLTAGE" HeaderText="INJECTION VOLTAGE" Visible="false" />
                            <asp:BoundField DataField="APP_STATUS_DT" HeaderText="Status Date" HeaderStyle-Width="100px" DataFormatString="{0:dd-MMM-yyyy}">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:BoundField DataField="days" HeaderText="Days After Payment" Visible="false">
                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Edit Form">
                                <ItemTemplate>

                                    <asp:ImageButton ID="btnSelect" ImageUrl="~/assets/images/editsquare.png" BorderWidth="0px"
                                        Text="Edit" runat="server" ToolTip="Edit" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>"
                                        Width="40px" Height="25px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Download Application Form">
                                <ItemTemplate>
                                    <%--<asp:Button ID="btnDownload" Text="Application Form" CssClass="button" runat="server"
                                            CommandName="Download" CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                    <center>
                                        <asp:ImageButton ID="btnDownload" Text="Application Form" ToolTip="Download Application Form"
                                            ImageUrl="~/assets/images/download.png" BorderWidth="0px" Width="30px" Height="30px"
                                            runat="server" CommandName="Download" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Upload Application Form">
                                <ItemTemplate>

                                    <center>
                                        <asp:ImageButton ID="btnUploadForm" Text="Upload Form" ToolTip="Upload Application Form"
                                            ImageUrl="~/assets/images/upload.png" BorderWidth="0px" Width="30px" Height="30px" Enabled="false"
                                            runat="server" CommandName="UploadForm" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Pay Registration Fees" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%--<asp:Button ID="btnPayNow" Text="Pay Registration Fees" ToolTip="Pay Registration Fees" CssClass="button" runat="server"
                                            CommandName="PayNow" CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                    <center>
                                        <asp:ImageButton ID="btnPayNow" Text="Pay Registration Fees" ToolTip="Pay Registration Fees"
                                            ImageUrl="~/assets/images/rupee.png" BorderWidth="0px" Width="30px" Height="25px"
                                            runat="server" CommandName="PayNow" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="WF_STATUS_CD_C" HeaderText="WF_STATUS_CD_C" Visible="false" />
                            <asp:TemplateField HeaderText="Download Receipt">
                                <ItemTemplate>
                                    <%--<asp:Button ID="btnRecDownload" Text="Receipt" CssClass="button" runat="server" CommandName="ReceiptDownload"
                                            CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                    <center>
                                        <asp:ImageButton ID="btnRecDownload" Text="Receipt" ToolTip="Receipt"
                                            ImageUrl="~/assets/images/receipt.jpg" BorderWidth="0px" Width="30px" Height="30px"
                                            runat="server" CommandName="ReceiptDownload" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Pay Committment Fees" Visible="false">
                                <ItemTemplate>
                                    <%--<asp:Button ID="btnCommFees" Text="Committment Fees" CssClass="button" runat="server"
                                            CommandName="CommittmentFees" CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                    <center>
                                        <asp:ImageButton ID="btnCommFees" Text="Pay Committment Fees" ToolTip="Pay Committment Fees"
                                            ImageUrl="~/assets/images/rupee.png" BorderWidth="0px" Width="30px" Height="25px" Enabled="false"
                                            runat="server" CommandName="CommittmentFees" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Download GC">
                                <ItemTemplate>
                                    <%--<asp:Button ID="btnRecDownload" Text="Receipt" CssClass="button" runat="server" CommandName="ReceiptDownload"
                                            CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                    <center>
                                        <asp:ImageButton ID="btnDownloadGC" Text="Download" ToolTip="Download"
                                            ImageUrl="~/assets/images/receipt.jpg" BorderWidth="0px" Width="30px" Height="30px" Enabled="true"
                                            runat="server" CommandName="DownloadGC" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                              <asp:TemplateField HeaderText="Download FGC">
                              <ItemTemplate>
                                  <%--<asp:Button ID="btnRecDownload" Text="Receipt" CssClass="button" runat="server" CommandName="ReceiptDownload"
                                          CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                  <center>
                                      <asp:ImageButton ID="btnDownloadFGC" Text="Download" ToolTip="Download"
                                          ImageUrl="~/assets/images/receipt.jpg" BorderWidth="0px" Width="30px" Height="30px" Enabled="true"
                                          runat="server" CommandName="DownloadFGC" CommandArgument="<%# Container.DataItemIndex %>" />
                                  </center>
                              </ItemTemplate>
                          </asp:TemplateField>
                            <asp:TemplateField HeaderText="Committment Fees Receipt" Visible="false">
                                <ItemTemplate>
                                    <%--<asp:Button ID="btnRecDownload" Text="Receipt" CssClass="button" runat="server" CommandName="ReceiptDownload"
                                            CommandArgument="<%# Container.DataItemIndex %>" />--%>
                                    <center>
                                        <asp:ImageButton ID="btnCommittmentDownload" Text="Receipt" ToolTip="Committment Fees Receipt"
                                            ImageUrl="~/assets/images/receipt.jpg" BorderWidth="0px" Width="30px" Height="30px" Enabled="false"
                                            runat="server" CommandName="CommittmentReceiptDownload" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="View Documents">
                                <ItemTemplate>

                                    <center>
                                        <asp:ImageButton ID="btnViewDoc" Text="View Documents" ToolTip="View Documents"
                                            ImageUrl="~/assets/images/view.png" BorderWidth="0px" Width="30px" Height="25px" Enabled="false"
                                            runat="server" CommandName="ViewDoc" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </center>
                                </ItemTemplate>
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
                </div>
            </div>
            <div id="footer">
                <p>
                    Designed and Developed by IT Department, MSETCL.
                </p>
            </div>
        </form>
    </div>
</body>
</html>
