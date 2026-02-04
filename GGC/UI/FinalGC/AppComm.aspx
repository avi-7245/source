<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppComm.aspx.cs" Inherits="GGC.UI.FinalGC.AppComm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MSKVY Final GC Application Details</title>
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
        form *
        {
            font-family: 'Poppins' ,sans-serif; /*color: Black;*/
            letter-spacing: 0.5px;
            outline: none;
            border: 0.1;
            font-size: 13px;
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
        .simplebutton3
        {
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
        .simplebutton3:hover
        {
            background-color: #46e85e;
            border: solid 1px #fff;
        }
        
        .simplebutton3:focus
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
        div.scroll
        {
            margin: 4px, 4px;
            padding: 4px; /*width: 500px;*/
            height: 110px;
            overflow-x: hidden;
            overflow-y: auto;
            text-align: justify;
        }
        .auto-style1 {
            color: #FF0000;
        }
        .auto-style2 {
            color: #CC0000;
        }
    </style>
</head>
<body>
    <div style="background-color: #5D7B9D">
        <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px;
            padding-left: 40px; border: 1px" />
        <font size="+5" style="color: white;">Maharashtra State Electricity Transmission Company
            LTD.</font>
    </div>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="mainbody">
        <center>
            <h3>
                MSKVY-Final GC Application Details</h3>
        </center>
        <table width="100%" align="center" cellpadding="5px" style="color: #333333;">
            <tr>
                <td colspan="8">
                    <h3>
                        COMMISSIONIG SCHEDULE & COD of units</h3>
                </td>
            </tr>
            <tr>
                <td style="width: 150px">
                    <strong>Unit No</strong><span class="auto-style1"><strong>*</strong></span><strong> :</strong><br />
                    <asp:TextBox ID="txtUnitno" runat="server" Width="140" />
                </td>
                <td style="width: 150px">
                    <strong>Unit Size (MW)</strong><span class="auto-style2"><strong>* </strong></span><strong>:</strong><br />
                    <asp:TextBox ID="txtUnitSize" runat="server" Width="140" />
                </td>
                <td style="width: 150px">
                    <strong>Date of Work Commencement</strong><span class="auto-style1"><strong>*</strong></span><strong> :</strong><br />
                    <asp:TextBox ID="txtDTWorkComm" runat="server" Width="140" />
                    <asp:ImageButton ID="ibtDTWorkComm" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                        ImageAlign="absbottom"></asp:ImageButton>
                    <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDTWorkComm"
                        PopupButtonID="ibtDTWorkComm" Format="yyyy-MM-dd" PopupPosition="Right">
                    </asp:CalendarExtender>
                </td>
                <td style="width: 150px">
                    <strong>Work Completion Date</strong><span class="auto-style1"><strong>* </strong></span><strong>:</strong><br />
                    <asp:TextBox ID="txtDTWorkCompletion" runat="server" Width="140" />
                    <asp:ImageButton ID="ibtDTWorkCompletion" runat="server" CausesValidation="false"
                        ImageUrl="~/assets/images/calendar.jpg" ImageAlign="absbottom"></asp:ImageButton>
                    <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDTWorkCompletion"
                        PopupButtonID="ibtDTWorkCompletion" Format="yyyy-MM-dd" PopupPosition="Right">
                    </asp:CalendarExtender>
                </td>
                <td style="width: 150px">
                    <strong>Date of Synchronization (Scheduled)<span class="auto-style1">*</span> :</strong><br />
                    <asp:TextBox ID="txtDTSynch" runat="server" Width="140" />
                    <asp:ImageButton ID="ibtDTSynch" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                        ImageAlign="absbottom"></asp:ImageButton>
                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtDTSynch"
                        PopupButtonID="ibtDTSynch" Format="yyyy-MM-dd" PopupPosition="Right">
                    </asp:CalendarExtender>
                </td>
                <td style="width: 150px">
                    <strong>Scheduled Commercial Operation Date(COD)<span class="auto-style1">*</span> :</strong><br />
                    <asp:TextBox ID="txtDTSCHCOD" runat="server" Width="140" />
                    <asp:ImageButton ID="ibtDTSCHCOD" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                        ImageAlign="absbottom"></asp:ImageButton>
                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtDTSCHCOD"
                        PopupButtonID="ibtDTSCHCOD" Format="yyyy-MM-dd" PopupPosition="Right">
                    </asp:CalendarExtender>
                </td>
                <td style="width: 70px">
                    <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="simplebutton1" OnClick="Insert" />
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <asp:GridView ID="GvComm" runat="server" AutoGenerateColumns="false" DataKeyNames="SrNo"
                        OnRowDataBound="OnRowDataBound" PageSize="3" AllowPaging="true" OnPageIndexChanging="OnPaging"
                        OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added.">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                            <asp:TemplateField HeaderText="Unit No" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblUnitNo" runat="server" Text='<%# Eval("UNIT_NO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUnitNo" runat="server" Text='<%# Eval("UNIT_NO") %>' Width="140"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Unit Size (MW)" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblUNIT_SIZE" runat="server" Text='<%# Eval("UNIT_SIZE") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtUNIT_SIZE" runat="server" Text='<%# Eval("UNIT_SIZE") %>' Width="140"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of Work Commencement" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDT_OF_WORK_COMMENCMENT" runat="server" Text='<%# Eval("DT_OF_WORK_COMMENCMENT","{0:dd-MM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDT_OF_WORK_COMMENCMENT" runat="server" Text='<%# Eval("DT_OF_WORK_COMMENCMENT","{0:dd-MM-yyyy}") %>'
                                        Width="140"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Work Completion Date" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDT_WORK_COMPLETE" runat="server" Text='<%# Eval("DT_WORK_COMPLETE","{0:dd-MM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDT_WORK_COMPLETE" runat="server" Text='<%# Eval("DT_WORK_COMPLETE") %>'
                                        Width="140"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date of Synchronization (Scheduled)" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDT_SYNCH" runat="server" Text='<%# Eval("DT_SYNCH","{0:dd-MM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDT_SYNCH" runat="server" Text='<%# Eval("DT_SYNCH") %>' Width="140"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Scheduled Commercial Operation Date(COD)" ItemStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Label ID="lblDT_SCH_COD" runat="server" Text='<%# Eval("DT_SCH_COD","{0:dd-MM-yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDT_SCH_COD" runat="server" Text='<%# Eval("DT_SCH_COD") %>' Width="140"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Link" ShowDeleteButton="true" ItemStyle-Width="150" />
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                    <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" />
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
            <tr>
                <td colspan="8" align="center">
                    <asp:Button ID="btnComFinalSave" runat="server" Text="Final Save" CssClass="simplebutton1"
                        OnClick="btnComFinalSave_Click" />
                </td>
            </tr>
        </table>
        <br />
        <table width="100%" align="center" cellpadding="5px" style="color: #333333;">
            <tr>
                <td>
                    <h3>
                        CERTIFICATION & CONFIRMATION</h3>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvCertification" runat="server" AutoGenerateColumns="false" EmptyDataText="No records has been added."
                        CellPadding="4" ForeColor="#333333" GridLines="Both">
                        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                        <Columns>
                        <asp:TemplateField HeaderText="Document SR.No">
                                <ItemTemplate>
                                    <asp:Label ID="lblsrno" runat="server" Text='<%# Eval("srno") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="certification_detail" HeaderText="Certification & Confirmation"/>
                            <asp:TemplateField HeaderText="Yes/No">
                                <ItemTemplate>
                                    <asp:RadioButtonList ID="rbYesNo" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1">Yes</asp:ListItem>
                                        <asp:ListItem Value="0">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#999999" />
                    <EmptyDataRowStyle Font-Bold="True" ForeColor="Red" />
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
            <tr>
            <td align="center">
            <asp:Button ID="btnSave_Next" runat="server" Text="Save & Next" 
                    CssClass="simplebutton1" onclick="btnSave_Next_Click"/>
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
