<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MSKVYCommList.aspx.cs" Inherits="GGC.UI.Emp.MSKVYCommList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                <span style="position:absolute;bottom:0px;right:0px">Welcome 
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>  
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    
                    <li class="selected"><a href="ProposalApprovalList.aspx">Proposal Approval</a></li>
                    <li class="selected"><a href="CommFeeList.aspx">Committment Fees</a></li>
                    <li class="selected"><a href="PaymentApproved.aspx">Committment Fees Approved </a></li>
                    
                    <%--<li class="selected"><a href="UploadLFS.aspx">Feasibility</a></li>--%>
                    
                    
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
                    Upload GC Letter List.</h3>
            </center>
          <%--  <div>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" 
                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                    <asp:ListItem Selected="True" Value="1">Complete</asp:ListItem>
                    <asp:ListItem Value="2">InComplete</asp:ListItem>
                </asp:RadioButtonList>
    </div>--%>
    <table width="100%" align="center" cellspacing="0">
        <tr>
            <td>
                <asp:GridView ID="GVApplications" runat="server" AutoGenerateColumns="False" 
                    CellPadding="4" ForeColor="#333333" GridLines="Both" 
                    onrowcommand="GVApplications_RowCommand" 
                    onrowdatabound="GVApplications_RowDataBound" >
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="APPLICATION_NO" HeaderText="APPLICATION NO"  >
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="MEDAProjectID" HeaderText="Project ID">
                        
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <%--<asp:BoundField DataField="MEDALettername" HeaderText="MEDALettername" Visible="false"/>
                        <asp:BoundField DataField="MEDA_REC_LETTER_NO" HeaderText="MEDA RECOMMENDATION LETTER NO" Visible="false" />
                        <asp:BoundField DataField="MEDADateF" HeaderText="MEDA RECOMMENDATION LETTER DATE" Visible="false"/>
--%>                        
                        <asp:BoundField DataField="PROMOTOR_NAME" HeaderText="DEVELOPER NAME" />
                        <asp:BoundField DataField="NATURE_OF_APP" HeaderText="NATURE OF APPLICATION " >
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="PROJECT_TYPE" HeaderText="PROJECT TYPE " />
                        <asp:BoundField DataField="PROJECT_CAPACITY_MW" HeaderText="PROJECT CAPACITY(MW)" >
                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="app_status" HeaderText="APPLICATION STATUS" >
                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                        </asp:BoundField>
                        <asp:BoundField DataField="GENERATION_VOLTAGE" HeaderText="GENERATION VOLTAGE" Visible="false" />
                        <asp:BoundField DataField="INJECTION_VOLTAGE" HeaderText="INJECTION VOLTAGE"  Visible="false"/>
                        <asp:BoundField DataField="APP_STATUS_DT" HeaderText="Status Date" DataFormatString="{0:dd-MMM-yyyy}" />
                        
                        
                        <asp:TemplateField HeaderText="Issue Demand Note" ItemStyle-HorizontalAlign="Center" Visible="false">
                            <ItemTemplate>

                                <asp:Button ID="btnIssueDemandNote" Text="Issue" Enabled="false" runat="server" CssClass="button"  CommandName="IssueDemandNote" CommandArgument="<%# Container.DataItemIndex %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Addded on dt 13.07.23--%>
                        <asp:TemplateField HeaderText="Upload GC Letter" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>

                                <asp:Button ID="btnUploadGCLetter" Text="Upload" runat="server" CssClass="button"  CommandName="UploadLetter" CommandArgument="<%# Container.DataItemIndex %>" />
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
