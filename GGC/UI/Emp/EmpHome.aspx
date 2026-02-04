<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpHome.aspx.cs" Inherits="GGC.UI.Emp.EmpHome" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home</title>
    <style type="text/css">
        /* The dropdown container */
        .dropdown
        {
            float: left;
            overflow: hidden;
        }
        
        /* Dropdown button */
        .dropdown .dropbtn
        {
            font-size: 16px;
            border: none;
            outline: none;
            color: white;
            padding: 14px 16px;
            background-color: inherit;
            font-family: inherit; /* Important for vertical align on mobile phones */
            margin: 0; /* Important for vertical align on mobile phones */
        }
        
        /* Add a red background color to navbar links on hover */
        .navbar a:hover, .dropdown:hover .dropbtn
        {
            background-color: red;
        }
        
        /* Dropdown content (hidden by default) */
        .dropdown-content
        {
            display: none;
            position: absolute;
            background-color: #f9f9f9;
            min-width: 160px;
            box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
            z-index: 1;
        }
        
        /* Links inside the dropdown */
        .dropdown-content a
        {
            float: none;
            color: black;
            padding: 12px 16px;
            text-decoration: none;
            display: block;
            text-align: left;
        }
        
        /* Add a grey background color to dropdown links on hover */
        .dropdown-content a:hover
        {
            background-color: #ddd;
        }
        
        /* Show the dropdown menu on hover */
        .dropdown:hover .dropdown-content
        {
            display: block;
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
            width: 1060px;
            margin: 0px auto;
        }
        
        .button
        {
            background-color: #008CBA; /* Blue */
            border: none;
            color: white;
            padding: 2px 10px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 13px;
            border-radius: 15px;
        }
        
        #mskvy-items {
  display: none;
}

#mskvy:hover #mskvy-items {
  display: block;
}
    </style>
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
    
</head>
<body>
    <div id="main">
        <div id="header">
            <div id="logo">
                
                
                <div id="logo_text">
                <img src="../../assets/images/logo.jpg" height="100" align="middle" />
                    <!-- class="logo_colour", allows you to change the colour of the text -->
                    <%--<h1><a href="index.html">simple<span class="logo_colour">style_blue_trees</span></a></h1>--%>
                   
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission
                        Company LTD.</font></span>
                        
                </div>
                <span style="position: absolute; bottom: 0px; right: 0px">Welcome
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>
            </div>
            
        </div>

        <div id="menubar">
                <ul id="menu">
                    
                    <%--<li class="selected" id="mskvy">  <a href="#">MSKVY Home</a>
                    <ul id="mskvy-items">
                    <li id="MSKVYHome" runat="server"><a href="MSKVYHome.aspx">MSKVY</a></li>
                    <li id="MSSPD" runat="server"><a href="MSSPDDraft.aspx">MS-SPD</a></li>
                    <li id="MSFGC" runat="server"><a href="FGCHome.aspx">FGC</a></li>
                    </ul>
                    </li>
                    --%>
                    

                    
                    
                    <li class="selected" id="SWPFGC" runat="server"><a href="FinalGC.aspx">SWP-FGC</a></li>
                    <li class="selected" id="MSKVYHome" runat="server"><a href="MSKVYHome.aspx">MSKVY</a></li>
                    <li class="selected" id="MSSPD" runat="server"><a href="MSSPDDraft.aspx">MS-SPD</a></li>
                    <li class="selected" id="MSFGC" runat="server"><a href="FGCHome.aspx">MSKVY FGC</a></li>
                    
                    
                    <li class="selected" id="gcIssued" runat="server"><a href="GCIssued.aspx">GC Issued</a></li>
                    <li class="selected" id="gcCancelled" runat="server"><a href="AppCancelList.aspx">GC Cancelled</a></li>
                    <li class="selected" id="dashboard" runat="server"><a href="Dashboard.aspx">Dashboard</a></li>
                    <li class="selected" id="RegistrationDone" runat="server"><a href="StatusReport.aspx">
                        Report</a></li>
                    <li class="selected" id="Usrmgt" runat="server"><a href="UsrMgt.aspx">Users</a></li>
                    
                    <li class="selected" id="chkStatus" runat="server"><a href="CheckStatus.aspx">Check
                        status</a></li>
                    <li class="selected" id="pending" runat="server"><a href="PendingApplication.aspx">Pending
                        Application</a></li>
                    <li class="selected" id="lfs" runat="server"><a href="LoadFlowStudy.aspx">LFS</a></li>
                    <li class="selected" id="viewDoc" runat="server"><a href="GCApplied.aspx">View Docs</a></li>
                    <li class="selected" id="changePass" runat="server"><a href="changepass.aspx">Change
                        Password</a></li>
                    <li class="selected" id="liSignOut" runat="server" onclick="signout()"><a href="EmpLogin.aspx">SignOut</a></li>
                    
                </ul>
                
            </div>
        
        <form id="form1" runat="server">
        
        <div class="mainbody">
            <center>
                <h3>
                    Application List.</h3>
            </center>
            <%--  <div>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="1">Complete</asp:ListItem>
                    <asp:ListItem Value="2">InComplete</asp:ListItem>
                </asp:RadioButtonList>
    </div>--%>
            <%--<table width="100%" align="center" cellspacing="0">
        <tr>
            <td>--%>
            <div style="width:100%">
            <asp:GridView ID="GVApplications" runat="server" AutoGenerateColumns="False" CellPadding="4"
                ForeColor="#333333" GridLines="Both" OnRowCommand="GVApplications_RowCommand"
                OnRowDataBound="GVApplications_RowDataBound" OnRowCreated="GVApplications_RowCreated">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" Width="100%" />
                <Columns>
                    <asp:BoundField DataField="APPLICATION_NO" HeaderText="APPLICATION NO">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="MEDAProjectID" HeaderText="Project ID">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GSTIN_NO" HeaderText="GSTIN" Visible="false">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PROJECT_LOC" HeaderText="Project Location">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <%--<asp:BoundField DataField="MEDALettername" HeaderText="MEDALettername" Visible="false"/>
                        <asp:BoundField DataField="MEDA_REC_LETTER_NO" HeaderText="MEDA RECOMMENDATION LETTER NO" Visible="false" />
                        <asp:BoundField DataField="MEDADateF" HeaderText="MEDA RECOMMENDATION LETTER DATE" Visible="false"/>
                    --%>
                    <asp:BoundField DataField="CONT_PER_NAME_1" HeaderText="CONTACT PERSON" Visible="false" />
                    <asp:BoundField DataField="ORGANIZATION_NAME" HeaderText="ORGANIZATION NAME">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PROJECT_TYPE" HeaderText="PROJECT TYPE" ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="PROJECT_CAPACITY_MW" HeaderText="PROJECT CAPACITY(MW)">
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="app_status" HeaderText="APPLICATION STATUS">
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                    </asp:BoundField>
                    <asp:BoundField DataField="GENERATION_VOLTAGE" HeaderText="GENERATION VOLTAGE" Visible="false" />
                    <asp:BoundField DataField="INJECTION_VOLTAGE" HeaderText="INJECTION VOLTAGE" Visible="false" />
                    <%--<asp:BoundField DataField="APP_STATUS_DT" HeaderText="Status Date" DataFormatString="{0:dd-MMM-yyyy}" />--%>
                    <asp:TemplateField HeaderText="View Application" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnViewAppZone" Text="View Application" runat="server" CssClass="button"
                                CommandName="ViewAppZone" CommandArgument="<%# Container.DataItemIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Application verification" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnViewApp" Text="Verify" runat="server" CssClass="button" CommandName="ViewApp"
                                CommandArgument="<%# Container.DataItemIndex %>" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Document verification" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnView" Text="Verify" runat="server" CssClass="button" CommandName="View"
                                CommandArgument="<%# Container.DataItemIndex %>" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Approve MEDA Report" Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="btnAppMEDA" Text="Approve" runat="server" CssClass="button"  CommandName="ApproveMEDA" CommandArgument="<%# Container.DataItemIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    <asp:TemplateField HeaderText="Technical Feasibility" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnUpload" Text="Upload" runat="server" CssClass="button" CommandName="Upload"
                                CommandArgument="<%# Container.DataItemIndex %>" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="View Feasibility" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnViewTech" Text="View" runat="server" CssClass="button" CommandName="ViewTech"
                                CommandArgument="<%# Container.DataItemIndex %>" Enabled="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cancel Application" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="button" CommandName="CancelApp" Visible="true"
                                CommandArgument="<%# Container.DataItemIndex %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Put up to commitee" Visible="false">
                            <ItemTemplate>
                                <asp:Button ID="btnPutUp" Text="Put Up" runat="server" CssClass="button"  CommandName="PutUp" CommandArgument="<%# Container.DataItemIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Approved by commitee">
                            <ItemTemplate>
                                <asp:Button ID="btnApprByComm" Text="Approved" runat="server" CssClass="button"  CommandName="Approvedbycommitee" CommandArgument="<%# Container.DataItemIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
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
            <%--</td>
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
