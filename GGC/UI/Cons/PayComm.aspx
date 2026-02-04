<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayComm.aspx.cs" Inherits="GGC.UI.Cons.PayComm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Payment Confirmation</title>
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
        }
    </style>
    <link rel="stylesheet" type="text/css" href="../../Styles/style.css" />
    <script type = "text/javascript">
        function ValidateCheckBox(sender, args) {
            if (document.getElementById("<%=chkIAgree.ClientID %>").checked == true) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
        }
    </script>
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
            <center>
                <h3>
                    Read Instructions to pay Committment charges.</h3>
            </center>
            <div>
            </div>
            <table width="100%" align="center" cellspacing="0">
                <tr>
                <td>
                I 
                    <asp:Label ID="lblOrgName" runat="server" Text=""></asp:Label>&nbsp;the undersigned hereby undertake that :
                </td>
                
                </tr>
                <tr>
                    <td>
                    <ol>
                    <li>I have read and understood all the terms and conditions mentioned in the procedures laid down by the STU towards the Grant of Grid Connectivity, Open Access which are uploaded on its website: www.mahatransco.in </li>
                    <li>I have read and understood all the terms and conditions mentioned in the Government of Maharashtra's Renewable Energy Policy-2020 and the methodology for establishment of RE project therein. </li>
                    <li>I have not obtained or applied for any other grid connectivity for this plant from any other transmission licensee</li>
                    <li>This connectivity stands cancelled, in case I opt for grid connectivity from any other transmission licensee</li>
                    <li>As per the clause No. 5.3 of the MERC(Transmission Open Access) Regulations 2016, if in future there is any material change in the location of the project or change, by more than ten percent(10%) in the quantum of power to be interchanged with the Intra-state transmission system, I shall make a fresh application.</li>
                    <li>This application and the grid connectivity obtained shall be governed by the prevailing MERC (Transmission Open Access) Regulation and GoM Renewable Energy policy.</li>
                    </ol>
                    </td>
                </tr>
                <tr>
                <td>
                <asp:CheckBox ID="chkIAgree" runat="server" /> I Agree above Terms & Conditions.
                <asp:CustomValidator ID="CustomValidator1" ForeColor="Red" runat="server" ErrorMessage="*Required" ClientValidationFunction = "ValidateCheckBox"></asp:CustomValidator>
                </td>
                </tr>
                <tr>
                <td>
                <asp:Button ID="btnPay" runat="server" Text="Pay Now" CssClass="button" 
                        onclick="btnPay_Click"/>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                </td>
                </tr>
            </table>
        </div>
        </form>
    </div>
</body>
</html>
