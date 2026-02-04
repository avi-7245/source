<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpLogin1.aspx.cs" Inherits="GGC.UI.Emp.EmpLogin1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Home</title>
   
    <link rel="preconnect" href="https://fonts.gstatic.com">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css">
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;500;600&display=swap" rel="stylesheet">
    <!--Stylesheet-->
    <style media="screen">
      *,
*:before,
*:after{
    padding: 0;
    margin: 0;
    box-sizing: border-box;
}
body{
    background-color: #f8f8f8;
}
.background{
    width: 430px;
    height: 520px;
    position: absolute;
    transform: translate(-50%,-50%);
    left: 50%;
    top: 50%;
}
.background .shape{
    height: 200px;
    width: 200px;
    position: absolute;
    border-radius: 50%;
}
.shape:first-child{
    background: linear-gradient(
        #1845ad,
        #23a2f6
    );
    left: -80px;
    top: -80px;
}
.shape:last-child{
    background: linear-gradient(
        to right,
        #ff512f,
        #f09819
    );
    right: -30px;
    bottom: -80px;
}
form{
    height: 520px;
    width: 400px;
    background-color: #fff;
    position: absolute;
    transform: translate(-50%,-50%);
    top: 50%;
    left: 50%;
    border-radius: 10px;
    backdrop-filter: blur(10px);
    border: 2px solid rgba(255,255,255,0.1);
    box-shadow: 0 0 40px rgba(8,7,16,0.6);
    padding: 50px 35px;
}
form *{
    font-family: 'Poppins',sans-serif;
    color: Black;
    letter-spacing: 0.5px;
    outline: none;
    border: none;
}
form h3{
    font-size: 32px;
    font-weight: 500;
    line-height: 42px;
    text-align: center;
}

label{
    display: block;
    margin-top: 30px;
    font-size: 16px;
    font-weight: 500;
    color:Black;
}
input{
    display: block;
    height: 50px;
    width: 100%;
    background-color: #f7f0f1;
    border-radius: 5px;
    padding: 0 10px;
    margin-top: 8px;
    font-size: 14px;
    font-weight: 300;
}
::placeholder{
    color: #e5e5e5;
}
button{
    margin-top: 50px;
    width: 100%;
    background-color: #ffffff;
    color: #080710;
    padding: 15px 0;
    font-size: 18px;
    font-weight: 600;
    border-radius: 5px;
    cursor: pointer;
}
.social{
  margin-top: 30px;
  display: flex;
}
.social div{
  background: red;
  width: 150px;
  border-radius: 3px;
  padding: 5px 10px 10px 5px;
  background-color: rgba(255,255,255,0.27);
  color: #eaf0fb;
  text-align: center;
}
.social div:hover{
  background-color: rgba(255,255,255,0.47);
}
.social .fb{
  margin-left: 25px;
}
.social i{
  margin-right: 4px;
}
  .simpleshape1
{
	color: #fff;
	background-color:#0f8bf7;
	height: 40px;
	width: 100px;
	padding:2px;
	border:none 0px transparent;
	font-size: 10px;
	font-weight: lighter;
	webkit-border-radius: 2px 16px 16px 16px;
	-moz-border-radius:  2px 16px 16px 16px;
	border-radius:  2px 16px 16px 16px;
}

.simpleshape1:hover
{
	background-color: #e74c3c;
	border:solid 1px #fff;
}

.simpleshape1:focus
{
	color: #383838;
	background-color: #fff;
	border:solid 3px rgba(98,176,255,0.3);
}

.simplebutton1
{
	color: #fff;
	background-color:#9b59b6;
	height: 40px;
	width: 100px;
	padding:2px;
	padding:10px;
	border:none 0px transparent;
	font-size: 15px;
	font-weight: lighter;
	webkit-border-radius: 2px 2px 2px 2px;
	-moz-border-radius:  2px 2px 2px 2px;
	border-radius:  2px 2px 2px 2px;
}
.simplebutton1:hover
{
	background-color:#3498db;
	border:solid 1px #fff;
}

.simplebutton1:focus
{
	color: #383838;
	background-color: #fff;
	border:solid 3px rgba(98,176,255,0.3);
}

.simplebutton2
{
	color: #fff;
	background-color:#46e85e;
	height: 40px;
	width: 100px;
	padding:2px;
	padding:10px;
	border:none 0px transparent;
	font-size: 15px;
	font-weight: lighter;
	webkit-border-radius: 2px 2px 2px 2px;
	-moz-border-radius:  2px 2px 2px 2px;
	border-radius:  4px 4px 4px 4px;
}
.simplebutton2:hover
{
	background-color:#3498db;
	border:solid 1px #fff;
}

.simplebutton2:focus
{
	color: #383838;
	background-color: #fff;
	border:solid 3px rgba(98,176,255,0.3);
}
    </style>
    <script type="text/javascript">
        function disableButton() {
            document.getElementById('<%= btnSendOTP.ClientID %>').disabled = true;
            document.getElementById('<%= btnSendOTP.ClientID %>').style.backgroundColor = 'Grey';  
            setTimeout(function () {
                document.getElementById('<%= btnSendOTP.ClientID %>').disabled = false;
                document.getElementById('<%= btnSendOTP.ClientID %>').style.backgroundColor = '#46e85e';
            }, 60000);
        }
</script>
</head>
<body>
   
    
   <div style="background-color:#5D7B9D">
    <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px;
        padding-left: 40px ;border:1px" />
    <b><font size="+3" style="color: white;">Maharashtra State Electricity Transmission Company
        LTD.</font></b>
        
            </div>
            
            <br />
           <%-- <center>
        <font size="+2" style="color: Black">Welcome to Online Grid Connectivity Application
            Employee portal</font></center>--%>
    <form id="form2" runat="server">
     <center>
        <font size="+1" style="color: Blue;padding-top:20px;">Grid Connectivity Employee portal</font></center>
<%--    <table width="30%" align="center" border="1px" cellpadding="3px" cellspacing="10px" style="border-radius: 5px;">--%>
<label for="username">SAP ID:</label>
<asp:TextBox ID="txtSAPID" runat="server" placeholder="SAP ID"></asp:TextBox>
<label for="password">Password</label>
<asp:TextBox ID="txtPass" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
<asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtPass" Display="Dynamic"
                    ValidationGroup="Login" ErrorMessage="Spaces are not allowed!" ValidationExpression="[^\s]+" />
                <asp:RequiredFieldValidator ID="rfv" runat="server" Display="Dynamic" ControlToValidate="txtPass" ErrorMessage="Blank is not allowed!" />
                 <asp:Button ID="btnSendOTP" runat="server" CssClass="simplebutton2" ValidationGroup="OTP" 
                    Text="Get OTP" OnClick="btnSendOTP_Click" />
                <asp:TextBox ID="txtOTP" runat="server" TextMode="Password" placeholder="Enter OTP"></asp:TextBox>
               
                    <asp:Button ID="btnLogin" runat="server" CssClass="simplebutton1" ValidationGroup="Login"
                    Text="Login" OnClick="btnLogin_Click" />
                    <%--<asp:Button ID="btnForgot" runat="server" CssClass="button" ValidationGroup="Forgott"
                    Text="Forgott Password" OnClick="btnForgot_Click" />--%>
                <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                <table width="70%" align="center" cellspacing="0" style="border: none">
            <tr>
            <td>
            Zone :
            </td>
            <td>
                <asp:DropDownList ID="ddlZone" runat="server">
                </asp:DropDownList>
            </td>
           
            <td>
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" 
                    onclick="btnSubmit_Click" />
            </td>
            </tr>
            </table>
            <table width="90%" align="center" cellspacing="0" style="border: none">
            
                <tr>
                    <td align="center">
                        
                        <asp:Chart ID="Chart1" runat="server" Height="435px" Width="600px" BackColor="InactiveCaption"
                            BorderlineWidth="0" Palette="None" PaletteCustomColors="Maroon" BorderlineColor="64, 0, 64">
                            <Series>
                                <asp:Series Name="Series1" XValueMember="stages" YValueMembers="cnt">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BackColor="#FFFFCC">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </td>
                    <td align="center">
                        <asp:Chart ID="Chart2" runat="server" Height="435px" Width="600px" BackColor="InactiveCaption"
                            BorderlineWidth="0" Palette="None" PaletteCustomColors="Maroon" BorderlineColor="64, 0, 64">
                            <Series>
                                <asp:Series Name="Series1" XValueMember="stages" YValueMembers="cnt">
                                    
                                </asp:Series>
                            </Series>
                            <Legends>
                                <asp:Legend Alignment="Center" Docking="Bottom" IsTextAutoFit="true" Name="Default"
                                    LegendStyle="Column" />
                            </Legends>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BackColor="#FFFFCC" Area3DStyle-LightStyle="Realistic">
                                    <Area3DStyle Enable3D="True" />
                                </asp:ChartArea>
                            </ChartAreas>
                        </asp:Chart>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:GridView ID="GVApplications" runat="server" AutoGenerateColumns="False" CellPadding="4"
                            ForeColor="#333333" GridLines="Both">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:BoundField DataField="stages" HeaderText="APPLICATION STAGES">
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                </asp:BoundField>
                                <asp:BoundField DataField="cnt" HeaderText="TOTAL">
                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                </asp:BoundField>
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
    </form>
   
    <br />
    <br />
    <br />
    <br />
    
    
</body>
</html>
