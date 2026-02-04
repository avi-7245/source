<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectDetail.aspx.cs"
    Inherits="GGC.UI.Cons.ProjectDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
                tr:nth-child(odd) {
  /*background-color: #f2f2f2*/
  background-color: #f2f2f2
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
                    <%--<li class="selected"><a href="Home.aspx">SignOut</a></li>--%>
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
                    &nbsp;Project Detail</h3>
            </center>
            <table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td colspan="4">
                        <b>*Details of Proposed Interconnection Point in Intra-State Transmission System</b></td>
                </tr>
                <tr>
                    <td width="25%">
                        <font color="red">*</font>Name of Transmission Licensee(s) whose Transmission Network
                        will be used for Grid connection :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNameOfTransmission" class="form-control" runat="server" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtNameOfTransmission"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <%--<td width="25%">
                        <font color="red">*</font> Generation Voltage Level
                        (kV) :</td>
                    <td width="25%">
                        <asp:TextBox ID="txtGenVolt" runat="server" class="form-control" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtGenVolt" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtGenVolt"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Voltage level" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Injection Voltage Level (kV) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtInjVolt" runat="server" class="form-control" MaxLength="3"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtInjVolt" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtInjVolt"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Voltage level" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                        <font color="red">*</font> Point of Injection (Name of EHV Substation /Line):
                    </td>
                    <td>
                        <asp:TextBox ID="txtPointInj" runat="server" class="form-control" 
                            MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPointInj" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font> Approximate distance from proposed Plant (In KM):
                    </td>
                    <td>
                        <asp:TextBox ID="txtDistancePlant" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDistancePlant"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ErrorMessage="Invalid Number." ControlToValidate="txtDistancePlant" 
                                Display="Dynamic" ForeColor="Red" Operator="DataTypeCheck" Type="Double" 
                                ValidationGroup="Submit">Invalid Number format should be (9999.99).</asp:CompareValidator>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtDistancePlant"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>--%>
                    </td>
                    <td>
                        <font color="red">*</font>Voltage level of Sub-Station 
                        (KV):
                    </td>
                    <td>
                        <asp:TextBox ID="txtVoltLevelSubstn" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtVoltLevelSubstn"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtVoltLevelSubstn"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Voltage level" ValidationExpression="\d*([\/]?\d+)"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="display:none">
                    <td colspan="4">
                        <b>Details of PPA/MoU/Sale of Power</b>
                    </td>
                </tr>
                <tr style="display:none">
                    <td width="25%">
                        Name of Transmission Licensee(s) whose Transmission Network will be used for Grid
                        connection :
                    </td>
                    <td width="25%">
                        <asp:DropDownList ID="ddlNatureOfTransLic" runat="server">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem>Discom</asp:ListItem>
                            <asp:ListItem>Captive</asp:ListItem>
                            <asp:ListItem>Third Party</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="25%">
                        InterState :
                    </td>
                    <td width="25%">
                        <asp:DropDownList ID="ddlInterState" runat="server">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem>Discom</asp:ListItem>
                            <asp:ListItem>Captive</asp:ListItem>
                            <asp:ListItem>Third Party</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Details of PPAs / Contracts and MOU :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtDetailPPA" class="form-control" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDetailPPA"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <%--<td>
                        Agreement with traders if any in above transaction. :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAgreement" class="form-control" runat="server" MaxLength="30"></asp:TextBox>
                    </td>--%>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>Status of Project</b>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <%--<font color="red">*</font>--%>Total Land required For project (Hectors) :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtTotReqLand" class="form-control" runat="server"></asp:TextBox>
                         <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtTotReqLand"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    
                    <td width="25%">
                        <%--<font color="red">*</font>--%>Whether Project (and/or
Substation) is in Forest Land?(Hectors) :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtTotForestLnd" runat="server" class="form-control"></asp:TextBox>
                         <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtTotForestLnd"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr runat="server" id="trFuel" visible="false">
                    <td width="25%">
                        Status of Fuel linkages (For Thermal & Gas Projects). :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtStatusFuelLinkages" class="form-control" runat="server"></asp:TextBox>
                    </td>
                    <td width="25%">
                        Status of Water Supply (For Thermal & Hydro Projects) :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtStatusWaterSupp" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        Status of forest land diversion:
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtStatusOfFoestLnd" class="form-control" runat="server"></asp:TextBox>
                    </td>
                    <td width="25%">
                        Whether route of tentative evacuation lines passes through forest/bird sanctuary 
                        etc.:
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtIsSanctury" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save & Next" OnClick="btnSave_Click"
                            CssClass="button" ValidationGroup="Submit" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
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
