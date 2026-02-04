<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PayRegConfirm.aspx.cs" Inherits="GGC.UI.MSKVY.PayRegConfirm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Payment confirmation</title>
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
            height: 500px;
            width: 1100px;
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
        /*#menubar
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
        }*/
        form select
        {
            color: black;
            font-size: 12px;
            padding: 5px 1px;
            border-radius: 5px;
            background-color: #f7f0f1;
            font-weight: bold;
        }
        
    </style>
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
<div style="background-color: #5D7B9D">
        <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px;
            padding-left: 40px; border: 1px" />
        <center>
            <b><font size="+5" style="color: white;">Maharashtra State Electricity Transmission
                Company LTD.</font></b></center>
    </div>
    
    <form id="form1" runat="server">
     <%--<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>--%>
    <div class="mainbody">
        <center>
            <h3>
                Read Instructions to pay Registration charges for MSKVY project.</h3>
        </center>
        <table width="100%" align="center" cellspacing="0">
                <tr>
                <td>
                I 
                    <asp:Label ID="lblOrgName" runat="server" Text=""></asp:Label>&nbsp;the undersigned hereby undertake that :
                </td>
                
                </tr>
                <tr>
                    <td>
                    
                    1. I have read and understood all the terms and conditions mentioned in the procedures laid down by the STU towards the Grant of Grid Connectivity, Open Access which are uploaded on its website: www.mahatransco.in <br/>
                    2. I have read and understood all the terms and conditions mentioned in the Government of Maharashtra's Renewable Energy Policy-2020 and the methodology for establishment of RE project therein. <br/>
                    3. I have not obtained or applied for any other grid connectivity for this plant from any other transmission licensee<br/>
                    4. This connectivity stands cancelled, in case I opt for grid connectivity from any other transmission licensee<br/>
                    5. As per the clause No. 5.3 of the MERC(Transmission Open Access) Regulations 2016, if in future there is any material change in the location of the project or change, by more than ten percent(10%) in the quantum of power to be interchanged with the Intra-state transmission system, I shall make a fresh application.<br/>
                    6. This application and the grid connectivity obtained shall be governed by the prevailing MERC (Transmission Open Access) Regulation and GoM Renewable Energy policy.<br/>
                    
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
                <asp:Button ID="btnPay" runat="server" Text="Pay Now" CssClass="simplebutton2" 
                        onclick="btnPay_Click"/>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
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
