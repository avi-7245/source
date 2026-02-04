<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalGridCon.aspx.cs" Inherits="GGC.UI.Cons.FinalGridCon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Final Grid Connectivity</title>
    <style type="text/css">
        tr:nth-child(odd)
        {
            /*background-color: #f2f2f2*/
            background-color: #f2f2f2;
        }
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
            width: 950px;
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
        .style1
        {
            height: 43px;
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
                    <li class="selected"><a href="/UI/Cons/AppHome.aspx">Home</a></li>
                    <li class="selected"><a href="Home.aspx">SignOut</a></li>
                </ul>
            </div>
        </div>
        <div id="content_header">
        </div>
        <form id="form1" runat="server">
        <div class="mainbody">
            <center>
                <h3>
                    Final Grid Connectivity</h3>
            </center>
            <table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td class="style1">
                        <font color="red">*</font>Enter Project Code :
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtProjCode" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator119" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjCode" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2" class="style1">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Get Project Details"
                            OnClick="btnSubmit_Click" />
                        <asp:Label ID="lblProjIDResult" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Application No :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppNo" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Nature of Applicant :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNatureApp" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        Project Type :
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjType" runat="server" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Project Capacity (MW):
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectCapacity" runat="server" class="form-control" 
                            MaxLength="4" Enabled="False"></asp:TextBox>
                    </td>
                    <td>
                        Project Location :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtProjectLocation" TextMode="MultiLine" runat="server" class="form-control-textarea "
                            MaxLength="70" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Taluka :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaluka" runat="server" class="form-control" MaxLength="30" 
                            Enabled="False"></asp:TextBox>
                    </td>
                    <td>
                        District :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDistrict" runat="server" class="form-control" 
                            MaxLength="30" Enabled="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblResult" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
            </table>
            <table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td colspan="2">
                        Downloads and Enclosure
                    </td>
                </tr>
                <tr>
                    <td>
                        Application form for Final Grid connectivity.
                    </td>
                    <td>
                        <asp:ImageButton ID="imgAppForm" ToolTip="Download Application Form" ImageUrl="~/assets/images/download.png"
                            BorderWidth="0px" Width="30px" Height="30px" runat="server" 
                            onclick="imgAppForm_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Connection Aggreement
                    </td>
                    <td>
                        <asp:ImageButton ID="imgConnAgg" ToolTip="Download Application Form" ImageUrl="~/assets/images/download.png"
                            BorderWidth="0px" Width="30px" Height="30px" runat="server" 
                            onclick="imgConnAgg_Click" />
                    </td>
                </tr>
            </table>
            <table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td>
                        Upload dcouments
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="GVFiles" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ShowFooter="true" ForeColor="#333333" GridLines="Both" OnRowCommand="GVFiles_RowCommand"
                            OnRowDataBound="GVFiles_RowDataBound" 
                            onselectedindexchanged="GVFiles_SelectedIndexChanged">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="SrNo" HeaderText="SrNo" />
                                <%--<asp:BoundField DataField="DocName" HeaderText="Document Name" />--%>
                                <asp:TemplateField HeaderText="Document Description" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDocName" runat="server" Text='<%# Eval("DocName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Button ID="btnFinalSubmit" Text="Final Submit" runat="server" CssClass="button"
                                            CommandName="FinalSubmit" CommandArgument="<%# Container.DataItemIndex %>" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Choos File" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Upload" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Button ID="btnUpload" Text="Upload" runat="server" CssClass="button" CommandName="Upload"
                                            CommandArgument="<%# Container.DataItemIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remark" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemark" runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" VerticalAlign="Middle" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <%--<table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td colspan="3">
                    Documents to be uploaded.
                    </td>
            </tr>
            <tr>
            
            <td>
            Application for connection to Intra-State transmission system
            </td>

            <td>
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button1" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button1_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Work Completion Report for the works as per the Scope of Works mentioned in Grid Connectivity letter from concerned Superintending Engineer, EHV Projects Circle, MSETCL.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload2" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button2" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button2_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Work Completion Report for ABT Meters, SCADA/RTU-DC installation & commissioning report from concerned Superintending Engineer, PAC circle, MSETCL.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload3" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button3" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button3_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Connection Agreement to be executed with the Chief Engineer EHV PC O&M Zone, MSETCL
            </td>

            <td>
                <asp:FileUpload ID="FileUpload4" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button4" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button4_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Site Responsibility Schedule along with equipment details executed with MSETCL.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload5" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button5" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button5_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Approved Copy of Single Line Diagram and layout from the Chief Engineer (MSLDC), Airoli, Navi Mumbai and Chief Engineer, EHV PC O&M Zone, MSETCL.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload6" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button6" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button6_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Certificate from the Chief Engineer (MSLDC), Airoli, Navi Mumbai, towards confirmation of AMR facility integration control center at MSLDC.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload7" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button7" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button7_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Certificate from the Chief Engineer (MSLDC), Airoli, Navi Mumbai, towards visibility of Generation to MSLDC for real time monitoring.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload8" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button8" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button8_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Consent from the concerned Distribution Licensee for drawal of power required for Start-up and auxiliary consumption from the network.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload9" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button9" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button9_Click"/>
            </td>
            </tr>
            <tr>
            
            <td>
            Document showing unique registration number generated after registering the above generating unit with CEA online registration portal.
            </td>

            <td>
                <asp:FileUpload ID="FileUpload10" runat="server" />
            </td>
            <td>
            <asp:Button ID="Button10" runat="server" CssClass="button" 
                            Text="Upload" onclick="Button10_Click"/>
            </td>
            </tr>
            
            </table>--%>
        </div>
        <div id="footer">
            <p>
                Design and Developed by IT Department, MSETCL.</p>
        </div>
        </form>
    </div>
</body>
</html>
