<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsumerDetail.aspx.cs"
    Inherits="GGC.UI.Cons.ConsumerDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Applicant Details</title>
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
            height: 59px;
        }
        #fvpp-blackout
        {
            display: none;
            z-index: 499;
            position: fixed;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            background: #000;
            opacity: 0.5;
        }
        #my-welcome-message
        {
            display: none;
            z-index: 500;
            position: fixed;
            width: 40%;
            left: 30%;
            top: 20%;
            padding: 20px 2%;
            font-family: Calibri, Arial, sans-serif;
            background: #FFF;
        }
        #fvpp-close
        {
            position: absolute;
            top: 10px;
            right: 20px;
            cursor: pointer;
        }
        #fvpp-dialog h2
        {
            font-size: 2em;
            margin: 0;
        }
        #fvpp-dialog p
        {
            margin: 0;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
    <script src="../../assets/js/jquery.min.js"></script> 
    <script src="../../assets/js/jquery.firstVisitPopup.min.js"></script> 
    <script>        $(function () {
            $('#my-welcome-message').firstVisitPopup({
                cookieName: 'ConsumerDetail', showAgainSelector: '#show-message'
            });
        });    </script>
</head>
<body>
    <div id="my-welcome-message">        <img src="../../assets/images/Workflow.jpg" width="600px" height="550px"/>    </div>   
    <div id="main">
        <div id="header">
            <div id="logo">
                <div id="logo_text">
                    <!-- class="logo_colour", allows you to change the colour of the text -->
                    <%--<h1><a href="index.html">simple<span class="logo_colour">style_blue_trees</span></a></h1>--%>
                    <img src="../../assets/images/logo.jpg" height="100" align="middle" />
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission
                        Company LTD.</font></span>
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
        <%--<table width="90%" align="center" cellspacing="5">
        <tr>
            <td width="20%">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/assets/images/logo.jpg" />
            </td>
            <td width="80%">
                <h1>
                    MAHARASHTRA STATE ELECTRICITY TRANSMISSION LTD</h1>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <h2>
                    Applicant Details</h2>
            </td>
        </tr>
    </table>--%>
        <%--    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>  
        --%>
        <div class="mainbody">
            <center>
                <h3>
                    Applicant Detail</h3>
            </center>
            <table width="100%" align="center" cellspacing="0" style="color: #333333;">
                <tr>
                    <td>
                        <font color="red">*</font>Enter Project Code :
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjCode" runat="server" class="form-control" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator119" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjCode" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Get Project Details"
                            OnClick="btnSubmit_Click" />
                        <asp:Label ID="lblProjIDResult" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        Already GC Approved?
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList ID="rbOldYesNo" runat="server" RepeatDirection="Horizontal"
                            Width="100px" RepeatLayout="Flow">
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                            <asp:ListItem Value="N">No</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Please select Old GC or not."
                            ControlToValidate="rbOldYesNo" runat="server" ForeColor="Red" Display="Dynamic"
                            ValidationGroup="Submit" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font> Nature of Applicant :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNatureOfApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNatureOfApp_SelectedIndexChanged">
                            <%--<asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Generator</asp:ListItem>
                                <asp:ListItem Value="2">EHV Consumer</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ForeColor="Red"
                            ErrorMessage="Select Nature Of Application!" Display="Dynamic" InitialValue="-1"
                            ControlToValidate="ddlNatureOfApp" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Project Type :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProjectType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProjectType_SelectedIndexChanged">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="9">BAGASSE</asp:ListItem>
                            <asp:ListItem Value="10">BIOMASS</asp:ListItem>
                            <asp:ListItem Value="19">HYBRID (Colocated)</asp:ListItem>
                            <asp:ListItem Value="12">INDUSTRIAL WASTE</asp:ListItem>
                            <asp:ListItem Value="13">MUNICIPAL SOLID WASTE</asp:ListItem>
                            <asp:ListItem Value="11">SMALL HYDRO</asp:ListItem>
                            <asp:ListItem Value="7">SOLAR</asp:ListItem>
                            <asp:ListItem Value="18">SOLAR PARK</asp:ListItem>
                            <asp:ListItem Value="8">WIND</asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnProjectType" runat="server" />
                        <%--<asp:RequiredFieldValidator ID="RQFVProjectType" runat="server" ForeColor="Red" ErrorMessage="Select Project Type!"
                                InitialValue="-1" ControlToValidate="ddlProjectType" Display="Dynamic" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <%--      <tr runat="server" id="trMEDA" visible="false">
                        <td>
                           <font color="red">*</font>MEDA Recommendation Letter No :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLetterNo" class="form-control" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLetterNo" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                          <font color="red">*</font>MEDA Recommendation Letter Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtLetterDt" runat="server" class="form-control"></asp:TextBox>
                            <asp:ImageButton ID="ibtLetterDt" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                                ImageAlign="absbottom"></asp:ImageButton>
                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtLetterDt"
                                PopupButtonID="ibtLetterDt" Format="yyyy-MM-dd" PopupPosition="Right">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLetterDt" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                        </td>
                    </tr>--%>
                <tr runat="server" id="trStatusOfApp" visible="false">
                    <td>
                        <font color="red">*</font>Status of Applicant :
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList ID="rbStatusApplicant" runat="server" RepeatDirection="Horizontal"
                            Width="100px">
                            <asp:ListItem>Developer</asp:ListItem>
                            <asp:ListItem>Investor</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Contact Person Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContPer1" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtContPer1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Contact Person Designation :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesign1" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDesign1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Phone :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone1" runat="server" class="form-control" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPhone1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Mobile :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMob1" runat="server" class="form-control" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtMob1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ValidationGroup="Submit"
                            ControlToValidate="txtMob1" ErrorMessage="Invalid Mobile No!!" ValidationExpression="[0-9]{10}"
                            ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Email Address :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail1" runat="server" class="form-control" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtEmail1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail1"
                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            ValidationGroup="Submit" Display="Dynamic" ErrorMessage="Invalid email address" />
                    </td>
                    <td>
                        Fax :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax1" runat="server" class="form-control" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Contact Person 2 Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContPer2" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                    </td>
                    <td>
                        Contact Person 2 Designation :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesign2" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone2" runat="server" class="form-control" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>
                        Mobile :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMob2" runat="server" class="form-control" MaxLength="10"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationGroup="Submit"
                            ControlToValidate="txtMob2" ErrorMessage="Invalid Mobile No!!" ValidationExpression="[0-9]{10}"
                            ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email Address :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail2" runat="server" class="form-control" MaxLength="40"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail2"
                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            ValidationGroup="Submit" Display="Dynamic" ErrorMessage="Invalid email address" />
                    </td>
                    <td>
                        FAX :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax2" runat="server" class="form-control" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr runat="server" id="trConsTypeOfLoad" visible="false">
                    <td>
                        <font color="red">*</font>Type of Load (Industrial/Commercial etc.)
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtConsTypeOfLoad" runat="server" class="form-control" MaxLength="30"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlConsTypeOfLoad" runat="server">
                            <asp:ListItem Selected="True" Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem>Industrial</asp:ListItem>
                            <asp:ListItem>Data Centre</asp:ListItem>
                            <asp:ListItem>Commercial</asp:ListItem>
                            <asp:ListItem>Residential</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ForeColor="Red"
                            ErrorMessage="Select Type of Load." Display="Dynamic" InitialValue="-1" ControlToValidate="ddlConsTypeOfLoad"
                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Quantum of power to be drawn (MVA) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtConsQuantumPowerDrawn" runat="server" class="form-control" MaxLength="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtConsQuantumPowerDrawn"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr runat="server" id="trTypeOfGen">
                    <td class="style1">
                        <font color="red">*</font>Type of Generation
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtTypeOfGen" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtTypeOfGen" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style1">
                        <font color="red">*</font>Quantum of power to be injected (MW) :
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtQuantumeOfPower" runat="server" class="form-control" MaxLength="5"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtQuantumeOfPower" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Invalid Number."
                            ControlToValidate="txtQuantumeOfPower" Display="Dynamic" ForeColor="Red" Operator="DataTypeCheck"
                            Type="Double" ValidationGroup="Submit">Invalid Number format should be (9999.99).</asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Name of Applicant/Developer (GC will be issued in this
                        name):
                    </td>
                    <td>
                        <asp:TextBox ID="txtNameOfPromoter" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtNameOfPromoter"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        Address for correspondence :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCorAdd" runat="server" TextMode="MultiLine" Rows="3" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Whether applying for additional capacity to existing or sanction project?
                    </td>
                    <td align="left">
                        <asp:RadioButtonList ID="rbAddCap" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                            <asp:ListItem Selected="True" Value="N">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        GC issue Letter Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtGCIssueDt" runat="server" class="form-control"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGCIssueDt"
                            PopupButtonID="ImageButton1" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Project Capacity (MW):
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectCapacity" runat="server" class="form-control" MaxLength="4"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjectCapacity"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtProjectCapacity"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Scheduled Commissioning Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtSchCommDt" runat="server" class="form-control"></asp:TextBox>
                        <asp:ImageButton ID="ibtSchCommDT" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtSchCommDt"
                            PopupButtonID="ibtSchCommDT" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtSchCommDt" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <%--<asp:CompareValidator ID="CompareScheduledCommissioningValidator" Operator="LessThan" type="String" ControltoValidate="txtSchCommDt"
                                 ErrorMessage="Scheduled Commissioning date must be greater than today" runat="server" ValidationGroup="Submit"
                                SetFocusOnError="True" />--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Project Location :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtProjectLocation" TextMode="MultiLine" runat="server" class="form-control-textarea "
                            MaxLength="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjectLocation"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Taluka :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaluka" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtTaluka" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <%--<asp:DropDownList ID="ddlTaluka" runat="server">
         <asp:ListItem Value="-1">--Select--</asp:ListItem>
                    <asp:ListItem>Kalyan</asp:ListItem>
            </asp:DropDownList>--%>
                    </td>
                    <td>
                        <font color="red">*</font>District :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDistrict" runat="server">
                            <%--     <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem>Ahmadnagar</asp:ListItem>
                                <asp:ListItem>Amravati</asp:ListItem>
                                <asp:ListItem>Aurangabad</asp:ListItem>
                                <asp:ListItem>Bhandara</asp:ListItem>
                                <asp:ListItem>Beed</asp:ListItem>
                                <asp:ListItem>Buldana</asp:ListItem>
                                <asp:ListItem>Chandrapur</asp:ListItem>
                                <asp:ListItem>Dhule</asp:ListItem>
                                <asp:ListItem>Gadchiroli</asp:ListItem>
                                <asp:ListItem>Gondiya</asp:ListItem>
                                <asp:ListItem>Hingoli</asp:ListItem>
                                <asp:ListItem>Jalgaon</asp:ListItem>
                                <asp:ListItem>Jalna</asp:ListItem>
                                <asp:ListItem>Kolhapur</asp:ListItem>
                                <asp:ListItem>Latur</asp:ListItem>
                                <asp:ListItem>Mumbai City</asp:ListItem>
                                <asp:ListItem>Mumbai Suburban</asp:ListItem>
                                <asp:ListItem>Nagpur</asp:ListItem>
                                <asp:ListItem>Nanded</asp:ListItem>
                                <asp:ListItem>Nandurbar</asp:ListItem>
                                <asp:ListItem>Nashik</asp:ListItem>
                                <asp:ListItem>Osmanabad</asp:ListItem>
                                <asp:ListItem>Parbhani</asp:ListItem>
                                <asp:ListItem>Pune</asp:ListItem>
                                <asp:ListItem>Raigarh</asp:ListItem>
                                <asp:ListItem>Ratnagiri</asp:ListItem>
                                <asp:ListItem>Sangli</asp:ListItem>
                                <asp:ListItem>Satara</asp:ListItem>
                                <asp:ListItem>Sindhudurg</asp:ListItem>
                                <asp:ListItem>Solapur</asp:ListItem>
                                <asp:ListItem>Thane</asp:ListItem>
                                <asp:ListItem>Wardha</asp:ListItem>
                                <asp:ListItem>Washim</asp:ListItem>
                                <asp:ListItem>Yavatmal</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ForeColor="Red"
                            ErrorMessage="Select District!" InitialValue="-1" ControlToValidate="ddlDistrict"
                            ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr runat="server" id="trCoOrdinates">
                    <td>
                        Latitude :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLatitude" runat="server" class="form-control" MaxLength="15"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLatitude" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        Longitude :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLongitude" runat="server" class="form-control" MaxLength="15"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLongitude" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr style="display: none;">
                    <%-- <td>
                            <font color="red">*</font>Type of Fuel :
                        </td>
                        <td>
                            
                            <asp:DropDownList ID="ddlTypeOfFuel" runat="server">
                            <asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Solar</asp:ListItem>
                                <asp:ListItem Value="2">Wind</asp:ListItem>
                                <asp:ListItem Value="3">Bagasse</asp:ListItem>
                                <asp:ListItem Value="4">Biomass</asp:ListItem>
                                <asp:ListItem Value="5">Small Hydro</asp:ListItem>
                                <asp:ListItem Value="6">Solid Waste</asp:ListItem>
                                <asp:ListItem Value="7">Industrial Waste</asp:ListItem>
                            </asp:DropDownList>
                            
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ForeColor="Red"
                                ErrorMessage="Select Type of Fuel!" InitialValue="-1" Display="Dynamic" ControlToValidate="ddlTypeOfFuel"
                                ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>--%>
                    <%--<td>
                        <font color="red">*</font>Specify the step-up generation Voltage :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtStepUpGen" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtStepUpGen" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    --%>    <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtStepUpGen"  
                    Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$" ValidationGroup="Submit"></asp:RegularExpressionValidator> --%>
                    <%--</td>--%>
                </tr>
                <tr>
                    <td>
                        <font color="red">*</font>Is it a captive Power Plant :
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList ID="RbCaptivePower" runat="server" RepeatDirection="Horizontal"
                            RepeatLayout="Flow">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Please select captive Power Plant."
                            ControlToValidate="RbCaptivePower" runat="server" ForeColor="Red" Display="Dynamic"
                            ValidationGroup="Submit" />
                    </td>
                </tr>
                <tr runat="server" id="trConsLoadDemand" visible="false">
                    <td>
                        <font color="red">*</font>Specify the drawal Voltage level. :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDrawalVoltLevel" runat="server" class="form-control" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDrawalVoltLevel"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtDrawalVoltLevel"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>
                    </td>
                    <td>
                        <font color="red">*</font>Reactive Power requirement of the Project (MVAr) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtReactivePower" runat="server" class="form-control" MaxLength="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtReactivePower"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save & Next" CssClass="button" OnClick="btnSave_Click"
                            ValidationGroup="Submit" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblResult" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <%-- 
    <tr>
    <td>
    MEDA Recognition Letter No  :
    </td>
    <td>
     <asp:TextBox ID="txtMEDANo" runat="server"></asp:TextBox>
    
    </td>
    <td>
    MEDA Recognition Letter Date  :    
    </td>
    <td>
     <asp:TextBox ID="txtMEDADt" runat="server"></asp:TextBox>
    
    </td>
    </tr>--%>
            </table>
        </div>
        <div id="footer">
            <p>
                Design and Developed by IT Department, MSETCL.</p>
        </div>
        <%--            </ContentTemplate>  

        </asp:UpdatePanel>--%>
        </form>
    </div>
</body>
</html>
