<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppDetail.aspx.cs" Inherits="GGC.UI.MSKVY.AppDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSKVY Application Details</title>
    <link rel="preconnect" href="https://fonts.gstatic.com" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;500;600&display=swap"
        rel="stylesheet" />
    <!--Stylesheet-->
    <style media="screen">
        *, *:before, *:after {
            padding: 0;
            margin: 0;
            box-sizing: border-box;
        }

        body {
            background-color: #f8f8f8;
        }

        .background {
            width: 430px;
            height: 520px;
            position: absolute;
            transform: translate(-50%,-50%);
            left: 50%;
            top: 50%;
        }

            .background .shape {
                height: 200px;
                width: 200px;
                position: absolute;
                border-radius: 50%;
            }

        .shape:first-child {
            background: linear-gradient( #1845ad, #23a2f6 );
            left: -80px;
            top: -80px;
        }

        .shape:last-child {
            background: linear-gradient( to right, #ff512f, #f09819 );
            right: -30px;
            bottom: -80px;
        }

        form {
            height: 850px;
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

            form * {
                font-family: 'Poppins',sans-serif; /*color: Black;*/
                letter-spacing: 0.5px;
                outline: none;
                border: 0.1;
                font-size: 13px;
            }

            form h3 {
                font-size: 28px;
                font-weight: 300;
                line-height: 42px;
                text-align: center;
            }

        label {
            /*display: block;*/
            margin-top: 1px;
            font-size: 18px;
            font-weight: 500;
            color: Black;
        }

        input {
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

        textarea {
            background-color: #f7f0f1;
            border-radius: 5px;
            border-color: #a8a0a4;
            padding: 0 10px;
            margin-top: 8px;
            font-size: 10px;
            font-weight: 100;
        }

        input[type="radio"] {
            height: 20px;
            border-radius: 5px;
            padding: 0 10px;
            margin-top: 8px;
            font-size: 18px;
            font-weight: 300;
        }

        ::placeholder {
            color: #e5e5e5;
        }

        button {
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

        .social {
            margin-top: 30px;
            display: flex;
        }

            .social div {
                background: red;
                width: 150px;
                border-radius: 3px;
                padding: 5px 10px 10px 5px;
                background-color: rgba(255,255,255,0.27);
                color: #eaf0fb;
                text-align: center;
            }

                .social div:hover {
                    background-color: rgba(255,255,255,0.47);
                }

            .social .fb {
                margin-left: 25px;
            }

            .social i {
                margin-right: 4px;
            }

        .simpleshape1 {
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

            .simpleshape1:hover {
                background-color: #e74c3c;
                border: solid 1px #fff;
            }

            .simpleshape1:focus {
                color: #383838;
                background-color: #fff;
                border: solid 3px rgba(98,176,255,0.3);
            }

        .simplebutton1 {
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

            .simplebutton1:hover {
                background-color: #3498db;
                border: solid 1px #fff;
            }

            .simplebutton1:focus {
                color: #383838;
                background-color: #fff;
                border: solid 3px rgba(98,176,255,0.3);
            }

        .simplebutton2 {
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

            .simplebutton2:hover {
                background-color: #46e85e;
                border: solid 1px #fff;
            }

            .simplebutton2:focus {
                color: #383838;
                background-color: #fff;
                border: solid 3px rgba(98,176,255,0.3);
            }

        .simplebutton3 {
            color: Black;
            background-color: #3498db;
            height: 34px;
            width: 80px;
            border: none 0px transparent;
            font-size: 14px;
            font-weight: 800;
            webkit-border-radius: 2px 2px 2px 2px;
            -moz-border-radius: 2px 2px 2px 2px;
            border-radius: 4px 4px 4px 4px;
        }

            .simplebutton3:hover {
                background-color: #46e85e;
                border: solid 1px #fff;
            }

            .simplebutton3:focus {
                color: #383838;
                background-color: #fff;
                border: solid 3px rgba(98,176,255,0.3);
            }

        #menubar {
            width: 1300px;
            height: 70px;
            padding-right: 1px;
        }

        ul#menu {
            float: right;
            margin: 0;
        }

            ul#menu li {
                float: left;
                padding: 0 0 0 9px;
                list-style: none;
                margin: 4px 4px 0 4px;
            }

                ul#menu li a {
                    font: normal 100% 'trebuchet ms', sans-serif;
                    display: block;
                    float: left;
                    height: 60px;
                    padding: 6px 20px 5px 20px;
                    text-align: center;
                    color: #FFF;
                    text-decoration: none;
                    background: #BBB;
                }

                ul#menu li.selected a {
                    height: 30px;
                    padding: 6px 20px 5px 11px;
                }

                ul#menu li.selected {
                    margin: 8px 4px 0 13px;
                    background: #00C6F0;
                }

                    ul#menu li.selected a, ul#menu li.selected a:hover {
                        background: #00C6F0;
                        color: #FFF;
                    }

                ul#menu li a:hover {
                    color: #323232;
                }

        form select {
            color: black;
            font-size: 12px;
            padding: 5px 1px;
            border-radius: 5px;
            background-color: #f7f0f1;
            font-weight: bold;
        }

        td, th {
            border: .1px solid black;
        }

        div.scroll {
            margin: 4px, 4px;
            padding: 4px;
            /*width: 500px;*/
            height: 110px;
            overflow-x: hidden;
            overflow-y: auto;
            text-align: justify;
        }

        .auto-style1 {
            color: #FF3300;
            background-color: #FFFFFF;
        }

        .auto-style2 {
            color: #FF3300;
        }
    </style>
    <%--      <script type="text/javascript">
        function Validate(sender, args) {
            var gridView = document.getElementById("<%=GVApplications.ClientID %>");
            var checkBoxes = gridView.getElementsByTagName("input");
            for (var i = 0; i < checkBoxes.length; i++) {
                if (checkBoxes[i].type == "checkbox" && checkBoxes[i].checked) {
                    args.IsValid = true;
                    return;
                }
            }
            args.IsValid = false;
        }
</script>--%>

    <script language="javascript">
        function limitText(limitField, limitCount, limitNum) {
            if (txtFax1.value.length > limitNum) {
                txtFax1.value = txtFax1.value.substring(0, limitNum);
            } else {
                txtFax1.value = limitNum - txtFax1.value.length;
            }
        }
    </script>

</head>
<body>
    <div style="background-color: #5D7B9D">
        <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px; padding-left: 40px; border: 1px" />
        <font size="+5" style="color: white;">Maharashtra State Electricity Transmission
                Company LTD.</font>
    </div>
    <div id="menubar">
    </div>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
            
<%--&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>

        </asp:ToolkitScriptManager>
        <div class="mainbody">
            <center>
                <h3>MSKVY Application Details</h3>
            </center>
            <table width="100%" align="center" cellpadding="5px" style="color: #333333;">
                <tr>
                    <td>
                        <font color="red">*</font>Enter Cluster ID:
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjCode" runat="server" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator119" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjCode" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="3">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="simplebutton2" Text="Get Project Details"
                            OnClick="btnSubmit_Click" />
                        <asp:Label ID="lblProjIDResult" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Name of Applicant/Developer :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtNameOfPromoter" runat="server" MaxLength="100" Width="300"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Nature of Applicant :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNatureOfApp" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlNatureOfApp_SelectedIndexChanged">
                            <%--<asp:ListItem Value="-1">--Select--</asp:ListItem>
                                <asp:ListItem Value="1">Generator</asp:ListItem>
                                <asp:ListItem Value="2">EHV Consumer</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    <td>Project Type :
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlProjectType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlProjectType_SelectedIndexChanged">
                            <%--<asp:ListItem Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem Value="9">BAGASSE</asp:ListItem>
                            <asp:ListItem Value="10">BIOMASS</asp:ListItem>
                            <asp:ListItem Value="19">HYBRID</asp:ListItem>
                            <asp:ListItem Value="12">INDUSTRIAL WASTE</asp:ListItem>
                            <asp:ListItem Value="13">MUNICIPAL SOLID WASTE</asp:ListItem>
                            <asp:ListItem Value="11">SMALL HYDRO</asp:ListItem>--%>
                            <asp:ListItem Value="7">SOLAR</asp:ListItem>
                            <%--<asp:ListItem Value="18">SOLAR PARK</asp:ListItem>
                            <asp:ListItem Value="8">WIND</asp:ListItem>--%>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hdnProjectType" runat="server" />
                        <%--<asp:RequiredFieldValidator ID="RQFVProjectType" runat="server" ForeColor="Red" ErrorMessage="Select Project Type!"
                                InitialValue="-1" ControlToValidate="ddlProjectType" Display="Dynamic" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Cluster Capacity (MW):
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectCapacity" runat="server" MaxLength="4"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjectCapacity"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtProjectCapacity"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>--%>
                    </td>
                    <td>Cluster Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtClusterName" runat="server" Width="300"></asp:TextBox>
                    </td>
                </tr>
                <tr runat="server" id="trssDetails" visible="false">
                    <td colspan="4" align="center">
                        <div class="scroll">
                            <asp:GridView ID="GVApplications" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                ForeColor="#333333" GridLines="Both" HeaderStyle-BackColor="#5D7B9D" HeaderStyle-ForeColor="White"
                                HeaderStyle-Font-Size="8" OnRowCommand="GVApplications_RowCommand">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Select">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="True"
                                                OnCheckedChanged="chkSelect_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ssNo" HeaderText="Substaion No">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ssName" HeaderText="Substaion Name">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="distName" HeaderText="District">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="ssSolarCapacity" HeaderText="Sub-Station Solar Capacity (MW)">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="subProjectCode11" HeaderText="SubProject Code 11 kV">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="solarCap11" HeaderText="Solar Capacity 11kV">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="subProjectCode22" HeaderText="SubProject Code 22 kV">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="solarCap22" HeaderText="Solar Capacity 22kV">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="subProjectCode33" HeaderText="SubProject Code 33 kV">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="solarCap33" HeaderText="Solar Capacity 33kV">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="LANDREQUIRED" HeaderText="Land Required">
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Land Details">
                                        <ItemTemplate>
                                            <asp:Button ID="btnShow" Text="View" runat="server" CssClass="simplebutton3" CommandName="ViewLandDetails"
                                                CommandArgument="<%# Container.DataItemIndex %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>

                                <HeaderStyle BackColor="#5D7B9D" Font-Size="8pt" ForeColor="White"></HeaderStyle>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr runat="server" id="trLandDetails" visible="false">
                    <td colspan="4" align="center">

                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Contact Person Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContPer1" runat="server" MaxLength="30"></asp:TextBox>
                        <%--                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtContPer1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Contact Person Designation :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesign1" runat="server" MaxLength="30"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDesign1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Phone :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone1" runat="server" MaxLength="20"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtPhone1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        <font color="red">*</font> Mobile :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMob1" runat="server" MaxLength="10"></asp:TextBox>
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
                        <font color="red">*</font> Email Address :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail1" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtEmail1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail1"
                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            ValidationGroup="Submit" Display="Dynamic" ErrorMessage="Invalid email address" />
                    </td>
                    <td>Address for correspondence:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax1" runat="server" MaxLength="200" Rows="3"
                            TextMode="MultiLine" Width="319px"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>Contact Person 2 Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContPer2" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                    <td>Contact Person 2 Designation :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDesign2" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>Phone :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone2" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>Mobile :
                    </td>
                    <td>
                        <asp:TextBox ID="txtMob2" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ValidationGroup="Submit"
                            ControlToValidate="txtMob2" ErrorMessage="Invalid Mobile No!!" ValidationExpression="[0-9]{10}"
                            ForeColor="Red"></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>Email Address :
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail2" runat="server" MaxLength="40"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmail2"
                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                            ValidationGroup="Submit" Display="Dynamic" ErrorMessage="Invalid email address" />
                    </td>
                    <td>FAX :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax2" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr runat="server" id="trConsTypeOfLoad" visible="false">
                    <td>
                        <%--<font color="red">*</font>--%>
                    Type of Load (Industrial/Commercial etc.)
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtConsTypeOfLoad" runat="server"  MaxLength="30"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlConsTypeOfLoad" runat="server">
                            <asp:ListItem Selected="True" Value="-1">--Select--</asp:ListItem>
                            <asp:ListItem>Industrial</asp:ListItem>
                            <asp:ListItem>Data Centre</asp:ListItem>
                            <asp:ListItem>Commercial</asp:ListItem>
                            <asp:ListItem>Residential</asp:ListItem>
                        </asp:DropDownList>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ForeColor="Red"
                            ErrorMessage="Select Type of Load." Display="Dynamic" InitialValue="-1" ControlToValidate="ddlConsTypeOfLoad"
                            ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Quantum of power to be drawn (MVA) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtConsQuantumPowerDrawn" runat="server" MaxLength="5"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtConsQuantumPowerDrawn"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr runat="server" id="trTypeOfGen">
                    <td class="style1">
                        <%--<font color="red">*</font>--%>
                    Type of Generation
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtTypeOfGen" runat="server" MaxLength="30"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtTypeOfGen" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td class="style1">
                        <font color="red">*</font>Project Capacity (MW) :
                    </td>
                    <td class="style1">
                        <asp:TextBox ID="txtQuantumeOfPower" runat="server" MaxLength="5"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtQuantumeOfPower" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Invalid Number."
                            ControlToValidate="txtQuantumeOfPower" Display="Dynamic" ForeColor="Red" Operator="DataTypeCheck"
                            Type="Double" ValidationGroup="Submit">Invalid Number format should be (9999.99).</asp:CompareValidator>
                    </td>
                </tr>
                <%--<tr>
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
                        <asp:TextBox ID="txtGCIssueDt" runat="server"  ></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGCIssueDt"
                            PopupButtonID="ImageButton1" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                    </td>
                </tr>--%>

                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Project Location (Village/Area):
                    </td>
                    <td colspan="3">

                        <asp:GridView ID="GVLandDetails" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Both" HeaderStyle-BackColor="#5D7B9D" HeaderStyle-ForeColor="White"
                            HeaderStyle-Font-Size="8" Width="100%">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>

                                <asp:BoundField DataField="landSurveyNo" HeaderText="Land Survey No">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landSubSurveyNo" HeaderText="SubSurvey No">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landArea" HeaderText="Land Area">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landDistanceFromSs" HeaderText="Land Distance From S/S">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landVillageName" HeaderText="Land Village">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landPanchayatName" HeaderText="Land Panchayat">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landTalukaName" HeaderText="Land Taluka">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="landDistrictName" HeaderText="Land District">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>

                            </Columns>
                        </asp:GridView>
                        <asp:TextBox ID="txtProjectLocation" TextMode="MultiLine" runat="server" CssClass="input"
                            MaxLength="100"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjectLocation"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>

                </tr>
                <tr>
                    <td>Taluka :
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaluka" runat="server" CssClass="input"
                            MaxLength="50"></asp:TextBox>
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
                    <td>Latitude :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLatitude" runat="server" MaxLength="15"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLatitude" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>Longitude :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLongitude" runat="server" MaxLength="15"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                                Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtLongitude" ValidationGroup="Submit"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>Whether additional capacity to existing or sanction project?
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbIsAddCapacity" runat="server" AutoPostBack="true"
                            RepeatDirection="Horizontal" onclick="EnableDisableTextBox()"
                            OnSelectedIndexChanged="rbIsAddCapacity_SelectedIndexChanged">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
                    <td>Grid Connectivity Letter Number :
                    </td>
                    <td>
                        <asp:TextBox ID="txtGCLtrNo" runat="server" MaxLength="15" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Grid Connectivity Letter Date :
                    </td>
                    <td>
                        <asp:TextBox ID="txtGCLtrDt" runat="server" Enabled="false"></asp:TextBox>
                        <asp:ImageButton ID="ibtGCLtrDt" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom" Enabled="false"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGCLtrDt"
                            PopupButtonID="ibtGCLtrDt" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Scheduled Commissioning Date<span class="auto-style2">*</span> :
                    </td>
                    <td>
                        <asp:TextBox ID="txtSchCommDt" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ibtSchCommDT" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtSchCommDt"
                            PopupButtonID="ibtSchCommDT" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtSchCommDt" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <%--<asp:CompareValidator ID="CompareScheduledCommissioningValidator" Operator="LessThan" type="String" ControltoValidate="txtSchCommDt"
                                 ErrorMessage="Scheduled Commissioning date must be greater than today" runat="server" ValidationGroup="Submit"
                                SetFocusOnError="True" />--%>
                    </td>
                </tr>
                <tr>
                    <td>Name of Transmission Licensee(s) whose Transmission Network will be used for Grid connection :
                            <asp:TextBox ID="txtNameofTransmission" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                    <td>Point of Injection: (Name of EHV Substation/Line) :

                    </td>
                    <td>
                        <asp:TextBox ID="txtPointOfInj" runat="server" MaxLength="100"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td>Injection Voltage Level 
                (kV) <span class="auto-style1">*</span>:
                    </td>
                    <td>
                        <asp:TextBox ID="txtInjectionVlot" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>Approximate distance from proposed plant 
                (In Km.):
                    </td>

                    <td>
                        <asp:TextBox ID="txtApproximate" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none">
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
                    <td>
                        <%--<font color="red">*</font>--%>
                    Specify the step-up generation Voltage :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtStepUpGen" runat="server" MaxLength="30"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtStepUpGen" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtStepUpGen"  
                    Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$" ValidationGroup="Submit"></asp:RegularExpressionValidator> --%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%-- <font color="red">*</font>--%>
                    Is it a captive Power Plant :
                    </td>
                    <td colspan="3" align="left">
                        <asp:RadioButtonList ID="RbCaptivePower" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0" Selected>No</asp:ListItem>
                        </asp:RadioButtonList>
                        <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ErrorMessage="Please select captive Power Plant."
                            ControlToValidate="RbCaptivePower" runat="server" ForeColor="Red" Display="Dynamic"
                            ValidationGroup="Submit" />--%>
                    </td>
                </tr>
                <tr runat="server" id="trConsLoadDemand" visible="false">
                    <td>
                        <%--<font color="red">*</font>--%>
                    Specify the drawal Voltage level. :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDrawalVoltLevel" runat="server" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtDrawalVoltLevel"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>
                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtDrawalVoltLevel"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="InValid Capacity" ValidationExpression="((\d+)((\.\d{1,2})?))$"
                            ValidationGroup="Submit"></asp:RegularExpressionValidator>--%>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>
                    Reactive Power requirement of the Project (MVAr) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtReactivePower" runat="server" MaxLength="5"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtReactivePower"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save & Next" CssClass="simplebutton2"
                            ValidationGroup="Submit" OnClick="btnSave_Click" />
                    </td>
                </tr>
                <%--<tr>
                    <td colspan="4" align="center">
                        
                    </td>
                </tr>--%>
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
            <div>
                <asp:Label ID="lblResult" runat="server" ForeColor="#FF3300"></asp:Label>
            </div>
        </div>
        <script type="text/javascript">
            function EnableDisableTextBox() {
            //alert(<%=rbIsAddCapacity.ClientID %>);
                var chkYes = document.getElementById("rbIsAddCapacity_0");

                var txtGCLtrNo = document.getElementById("txtGCLtrNo.ClientID");
                var txtGCLtrDt = document.getElementById("txtGCLtrDt.ClientID");
                txtGCLtrNo.en = chkYes.checked ? disabled : false;
                txtGCLtrDt.disabled = chkYes.checked ? disabled : false;

            }
        </script>

    </form>
</body>
</html>
