<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppDetail.aspx.cs" Inherits="GGC.UI.MSKVYSPD.AppDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSKVY SPD Application Details</title>
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
            width: 190px;
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
    </style>
</head>
<body>
    <div style="background-color: #5D7B9D">
        <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px; padding-left: 40px; border: 1px" />
        <font size="+5" style="color: white;">Maharashtra State Electricity Transmission
                Company LTD.</font>
    </div>

    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div class="mainbody">
            <center>
                <h3>MSKVY-SPV Application Details</h3>
            </center>
            <table width="100%" align="center" cellpadding="5px" style="color: #333333;">
                <tr>
                    <td>
                        <font color="red">*</font>Enter Application No:
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjCode" runat="server" MaxLength="20"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator119" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjCode" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="3">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="simplebutton2"
                            Text="Get Application Details" OnClick="btnSubmit_Click" />
                        <asp:Label ID="lblProjIDResult" runat="server" ForeColor="#FF3300"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>Name of Applicant/SPV :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNameOfPromoter" runat="server" MaxLength="100" Width="300"></asp:TextBox></td>
                    <td>Project Id :
                    </td>
                    <td>
                        <asp:TextBox ID="txtAppNo" runat="server" MaxLength="100"></asp:TextBox></td>

                </tr>
                <tr>
                    <td>PAN Number :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPANNO" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    <td>GSTIN : 
                    </td>
                    <td>
                        <asp:TextBox ID="txtGSTIN" runat="server" MaxLength="4"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RQFVProjectType" runat="server" ForeColor="Red" ErrorMessage="Select Project Type!"
                                InitialValue="-1" ControlToValidate="ddlProjectType" Display="Dynamic" ValidationGroup="Submit"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>Cluster Capacity (MW):
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

                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>Contact Person Name :
                    </td>
                    <td>
                        <asp:TextBox ID="txtContPer1" runat="server" MaxLength="30"></asp:TextBox>
                        <%--                      <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtContPer1" ValidationGroup="Submit"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                    </td>
                    <td>
                        <%--<font color="red">*</font>--%>Contact Person Designation :
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
                        <%--<font color="red">*</font>--%>Phone :
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
                    <td>Fax :
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax1" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>Name of the SPV (Name of the firm in whose name Grid connectivity is issued) :
                    </td>
                    <td>
                        <asp:TextBox ID="txtNameofSPV" runat="server" Width="300"></asp:TextBox>
                    </td>
                    <td>Address for correspondence :
                    </td>
                    <td>
                        <asp:TextBox ID="txtCorrAdd" TextMode="MultiLine" runat="server" CssClass="input"
                            MaxLength="100" Height="56px" Width="238px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Is there any change in project location from Deemed GC?</td>
                    <td align="left">
                        <asp:RadioButtonList ID="RbIsChangeLoc" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="True" OnSelectedIndexChanged="RbIsChangeLoc_SelectedIndexChanged">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>Sub Project Code
                    </td>
                    <td>
                        <asp:Label ID="lblSubProjCode" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--<font color="red">*</font>--%>Project Location :
                    </td>
                    <td>
                        <asp:TextBox ID="txtProjectLocation" TextMode="MultiLine" runat="server" CssClass="input"
                            MaxLength="100" Height="56px" Width="238px"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtProjectLocation"
                            ValidationGroup="Submit" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
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
                <tr runat="server">
                    <td>Interconnection at :</td>
                    <td>
                        <asp:TextBox ID="txtInterconnection" runat="server" MaxLength="15" Width="300px"></asp:TextBox>

                    </td>
                    <td>Deemed GC Letter Number :</td>
                    <td>
                        <asp:TextBox ID="txtGCLtrNo" runat="server" MaxLength="15" ReadOnly="True"></asp:TextBox>

                    </td>
                </tr>
                <tr>

                    <td>Deemed GC Letter Date :
                    </td>
                    <td>

                        <asp:TextBox ID="txtGCLtrDt" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ibtGCLtrDt" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtGCLtrDt"
                            PopupButtonID="ibtGCLtrDt" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                    </td>
                    <td>Deemed GC validity Date : 
                    </td>
                    <td>
                        <asp:TextBox ID="txtGCValDt" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ibtGCValDt" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom" Enabled="False"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtGCValDt"
                            PopupButtonID="ibtGCValDt" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                    </td>
                </tr>

                <tr>
                    <td>MSEDCL Tender No :
                    </td>
                    <td>
                        <asp:TextBox ID="txtDCLTenderNo" runat="server" MaxLength="100"></asp:TextBox>

                    </td>
                    <td>PPA Details :
                    </td>
                    <td>
                        <asp:TextBox ID="txtPPADet" runat="server" MaxLength="100"></asp:TextBox>

                    </td>
                </tr>

                <tr>
                    <td>Total Land Required for Project :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLandReq" runat="server" MaxLength="100"></asp:TextBox>

                    </td>
                    <td>Land in possession :
                    </td>
                    <td>
                        <asp:TextBox ID="txtLandPoss" runat="server" MaxLength="100"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td>Whether Project (and/or Substation) is in Forest Land? :
                    </td>
                    <td>
                        <asp:RadioButtonList ID="RbIsForestLand" runat="server"
                            RepeatDirection="Horizontal" AutoPostBack="True">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                        </asp:RadioButtonList></td>
                    <td>Status of forest Land diversion :
                    </td>
                    <td>
                        <asp:TextBox ID="txtStatusofForest" runat="server" MaxLength="100"></asp:TextBox>

                    </td>
                </tr>
                <tr>
                    <td>Whether route of tentative evacuation lines passes through forest/bird sanctuary etc. :
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtLinesthrForest" runat="server" MaxLength="100"></asp:TextBox>

                    </td>

                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSave" runat="server" Text="Save & Next" CssClass="simplebutton2"
                            ValidationGroup="Submit" OnClick="btnSave_Click" />
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
