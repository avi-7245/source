<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectDetail.aspx.cs"
    Inherits="GGC.UI.MSKVY.ProjectDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Project Detail</title>
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;500;600&display=swap"
        rel="stylesheet" />
    <!--Stylesheet-->
    <style media="screen">
        *, *:before, *:after
        {
            padding: 0;
            margin: 0;
            box-sizing: border-box;
        }
        body
        {
            background-color: #f8f8f8;
        }
        .background
        {
            width: 430px;
            height: 520px;
            position: absolute;
            transform: translate(-50%,-50%);
            left: 50%;
            top: 50%;
        }
        .background .shape
        {
            height: 200px;
            width: 200px;
            position: absolute;
            border-radius: 50%;
        }
        .shape:first-child
        {
            background: linear-gradient(
        #1845ad,
        #23a2f6
    );
            left: -80px;
            top: -80px;
        }
        .shape:last-child
        {
            background: linear-gradient(
        to right,
        #ff512f,
        #f09819
    );
            right: -30px;
            bottom: -80px;
        }
        form
        {
            height: 700px;
            width: 1300px;
            background-color: #ffffff;
            position: absolute;
            transform: translate(-50%,-50%);
            top: 60%;
            left: 50%;
            border-radius: 10px;
            backdrop-filter: blur(10px);
            border: 2px solid rgba(255,255,255,0.1);
            box-shadow: 0 0 40px rgba(8,7,16,0.6);
        }
        form *
        {
            font-family: 'Poppins' ,sans-serif; /*color: Black;*/
            letter-spacing: 0.5px;
            outline: none;
            border: 0.1;
            font-size: 16px;
        }
        form h3
        {
            font-size: 28px;
            font-weight: 300;
            line-height: 42px;
            text-align: center;
        }
        label
        {
            /*display: block;*/
            margin-top: 1px;
            font-size: 18px;
            font-weight: 500;
            color: Black;
        }
        input
        {
            /*display: block;*/
            height: 30px;
            background-color: #f7f0f1;
            border-radius: 5px;
            border-color: #a8a0a4;
            padding: 0 10px;
            margin-top: 8px;
            font-size: 10px;
            font-weight: 100;
        }
        textarea
        {
            background-color: #f7f0f1;
            border-radius: 5px;
            border-color: #a8a0a4;
            padding: 0 10px;
            margin-top: 8px;
            font-size: 10px;
            font-weight: 100;
        }
        input[type="radio"]
        {
            height: 20px;
            border-radius: 5px;
            padding: 0 10px;
            margin-top: 8px;
            font-size: 18px;
            font-weight: 300;
        }
        ::placeholder
        {
            color: #e5e5e5;
        }
        button
        {
            margin-top: 50px;
            width: 50%;
            background-color: #ffffff;
            color: #080710;
            padding: 15px 0;
            font-size: 8px;
            font-weight: 600;
            border-radius: 5px;
            cursor: pointer;
        }
        .social
        {
            margin-top: 30px;
            display: flex;
        }
        .social div
        {
            background: red;
            width: 150px;
            border-radius: 3px;
            padding: 5px 10px 10px 5px;
            background-color: rgba(255,255,255,0.27);
            color: #eaf0fb;
            text-align: center;
        }
        .social div:hover
        {
            background-color: rgba(255,255,255,0.47);
        }
        .social .fb
        {
            margin-left: 25px;
        }
        .social i
        {
            margin-right: 4px;
        }
        .simpleshape1
        {
            color: #fff;
            background-color: #0f8bf7;
            height: 40px;
            width: 100px;
            padding: 2px;
            border: none 0px transparent;
            font-size: 10px;
            font-weight: lighter;
            webkit-border-radius: 2px 16px 16px 16px;
            -moz-border-radius: 2px 16px 16px 16px;
            border-radius: 2px 16px 16px 16px;
        }
        
        .simpleshape1:hover
        {
            background-color: #e74c3c;
            border: solid 1px #fff;
        }
        
        .simpleshape1:focus
        {
            color: #383838;
            background-color: #fff;
            border: solid 3px rgba(98,176,255,0.3);
        }
        .simplebutton1
        {
            color: #fff;
            background-color: #9b59b6;
            height: 40px;
            width: 100px;
            padding: 2px;
            padding: 10px;
            border: none 0px transparent;
            font-size: 15px;
            font-weight: lighter;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2px 2px;
            border-radius: 2px 2px 2px 2px;
        }
        .simplebutton1:hover
        {
            background-color: #3498db;
            border: solid 1px #fff;
        }
        
        .simplebutton1:focus
        {
            color: #383838;
            background-color: #fff;
            border: solid 3px rgba(98,176,255,0.3);
        }
        .simplebutton2
        {
            color: Black;
            background-color: #3498db;
            height: 34px;
            width: 180px;
            border: none 0px transparent;
            font-size: 14px;
            font-weight: 800;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2px 2px;
            border-radius: 4px 4px 4px 4px;
        }
        .simplebutton2:hover
        {
            background-color: #46e85e;
            border: solid 1px #fff;
        }
        
        .simplebutton2:focus
        {
            color: #383838;
            background-color: #fff;
            border: solid 3px rgba(98,176,255,0.3);
        }
        #menubar
        {
            width: 1300px;
            height: 70px;
            padding-right: 1px;
        }
        
        ul#menu
        {
            float: right;
            margin: 0;
        }
        
        ul#menu li
        {
            float: left;
            padding: 0 0 0 9px;
            list-style: none;
            margin: 4px 4px 0 4px;
        }
        
        ul#menu li a
        {
            font: normal 100% 'trebuchet ms' , sans-serif;
            display: block;
            float: left;
            height: 60px;
            padding: 6px 20px 5px 20px;
            text-align: center;
            color: #FFF;
            text-decoration: none;
            background: #BBB;
        }
        
        ul#menu li.selected a
        {
            height: 30px;
            padding: 6px 20px 5px 11px;
        }
        
        ul#menu li.selected
        {
            margin: 8px 4px 0 13px;
            background: #00C6F0;
        }
        
        ul#menu li.selected a, ul#menu li.selected a:hover
        {
            background: #00C6F0;
            color: #FFF;
        }
        
        ul#menu li a:hover
        {
            color: #323232;
        }
        form select
        {
            color: black;
            font-size: 12px;
            padding: 5px 1px;
            border-radius: 5px;
            background-color: #f7f0f1;
            font-weight: bold;
        }
        td, th
        {
            border: .1px solid black;
        }
    </style>
</head>
<body>
    <<div style="background-color: #5D7B9D">
        <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px;
            padding-left: 40px; border: 1px" />
        <center>
            <b><font size="+5" style="color: white;">Maharashtra State Electricity Transmission
                Company LTD.</font></b></center>
    </div>
    <div id="menubar">
    </div>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="mainbody">
        <center>
            <h3>
                MSKVY Project Details</h3>
        </center>
        <table width="100%" align="center" cellpadding="5px" style="color: #333333;">
            <tr>
                    <td colspan="4">
                        <b>*Details of Intrastate Transmission System</b>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <%--<font color="red">*</font>--%>
                        Name of Transmission Licensee(s) whose Transmission Network
                        will be used for Grid connection :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtNameOfTransmission" class="form-control" runat="server" MaxLength="30"></asp:TextBox>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtNameOfTransmission"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td width="25%">
                       <%-- <font color="red">*</font>--%> 
                        Generation Voltage Level
                        (kV) :</td>
                    <td width="25%">
                        <asp:TextBox ID="txtGenVolt" runat="server" class="form-control" MaxLength="3"></asp:TextBox>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtGenVolt" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtGenVolt"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Voltage level" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>
                        Injection Voltage Level (kV) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtInjVolt" runat="server" class="form-control" MaxLength="3"></asp:TextBox>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtInjVolt" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtInjVolt"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Voltage level" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>
                        Point of Injection (Name of EHV Substation /Line):
                    </td>
                    <td>
                        <asp:TextBox ID="txtPointInj" runat="server" class="form-control" 
                            MaxLength="50"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPointInj" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        --%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>
                         Approximate distance from proposed Plant (In KM):
                    </td>
                    <td>
                        <asp:TextBox ID="txtDistancePlant" runat="server" class="form-control"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDistancePlant"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ErrorMessage="Invalid Number." ControlToValidate="txtDistancePlant" 
                                Display="Dynamic" ForeColor="Red" Operator="DataTypeCheck" Type="Double" 
                                ValidationGroup="Submit">Invalid Number format should be (9999.99).</asp:CompareValidator>
                       <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtDistancePlant"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>--%>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>
                        Voltage level of Sub-Station 
                        (KV):
                    </td>
                    <td>
                        <asp:TextBox ID="txtVoltLevelSubstn" runat="server" class="form-control"></asp:TextBox>
                       <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtVoltLevelSubstn"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
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
                    <td>
                        <asp:TextBox ID="txtDetailPPA" class="form-control" runat="server"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDetailPPA"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        Agreement with traders if any in above transaction. :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAgreement" class="form-control" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>*Status of Project</b>
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
                        <%--<font color="red">*</font>--%> Land in Possession (Hectors) :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtLandInPoss" runat="server" class="form-control"></asp:TextBox>
                         <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLandInPoss"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <%--<font color="red">*</font>--%>Total private land (Hectors) :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtTotPvtLnd" class="form-control" runat="server"></asp:TextBox>
                         <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtTotPvtLnd"
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
                        Weather route of tentative evaculation lines passes through forest / bird sanctury
                        etc. :
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtIsSanctury" runat="server" class="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save &amp; make Payment" 
                            CssClass="simplebutton2" ValidationGroup="Submit" 
                            onclick="btnSave_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
        </table>
        <div>
            <asp:Label ID="lblResult" runat="server" ForeColor="#FF3300"></asp:Label>
        </div>
    </div>
    </form>
</body>
</html>
