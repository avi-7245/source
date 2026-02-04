<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalGCDoc.aspx.cs" Inherits="GGC.UI.Emp.FinalGCDoc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../assets/css/bootstrap.css" />
    <style>
        #main, #logo, #menubar, #site_content, #footer {
            margin-left: auto;
            margin-right: auto;
        }

        #header {
            background: #CCC;
            border-bottom: 1px solid #CCC;
            height: 156px;
        }

        #logo {
            width: 1100px;
            position: relative;
            height: 110px;
            background: #F5F5F5;
        }

            #logo #logo_text {
                position: absolute;
                top: 10px;
                left: 0;
            }

            #logo h1, #logo h2 {
                font: normal 300% 'century gothic', arial, sans-serif;
                border-bottom: 0;
                text-transform: none;
                margin: 0 0 0 9px;
            }

        #logo_text h1, #logo_text h1 a, #logo_text h1 a:hover {
            padding: 10px 0 0 0;
            color: #323232;
            letter-spacing: 0.1em;
            text-decoration: none;
        }

            #logo_text h1 a .logo_colour {
                color: #00C6F0;
            }

        #logo_text a:hover .logo_colour {
            color: #DDD;
        }

        #logo_text h2 {
            font-size: 120%;
            padding: 4px 0 0 0;
            color: #999;
        }

        #menubar {
            width: 1100px;
            height: 45px;
            padding-right: 8px;
            background: #CCC;
            border-top: 1px solid #CCC;
        }

        ul#menu {
            float: right;
            margin: 0;
        }

            ul#menu li {
                float: left;
                padding: 0 0 0 9px;
                list-style: none;
                margin: 8px 4px 0 4px;
            }

                ul#menu li a {
                    font: normal 100% 'trebuchet ms', sans-serif;
                    display: block;
                    float: left;
                    height: 30px;
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

        .auto-style1 {
            color: #FF0000;
            font-size: x-large;
        }
    </style>
    <%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>

<script type="text/javascript">
        $(document).on("click", "[id*=lnkView]", function () {
    $("#id").html($(".Id", $(this).closest("tr")).html());
    $("#FileName").html($(".FileName", $(this).closest("tr")).html());
    $("#description").html($(".Description", $(this).closest("tr")).html());
    var fileName = $(".FileName", $(this).closest("tr")).html();
    $("#dialog").dialog({
        title: "View Details",
        buttons: {
            Ok: function () {
                $(this).dialog('close');
            }
        },
        open: function () {
            var object = "<object data=\"{FileName}\" type=\"application/pdf\" width=\"300px\" height=\"200px\">";
            object += "If you are unable to view file, you can download from <a href=\"{FileName}\">here</a>";
            object += " or download <a target = \"_blank\" href = \"http://get.adobe.com/reader/\">Adobe PDF Reader</a> to view the file.";
            object += "</object>";
            object = object.replace(/{FileName}/g, "Files/FinalGC/20100100000006/" + fileName);
            $("#dialog").html(object);
        },
        modal: true
    });
    return false;
});
</script>--%>
    <script type="text/javascript">
unction ValidateRadioButton(sender, args)
        {
            var gv = document.getElementById("<%= grvDocuments.ClientID %>");
            var items = gv.getElementsByTagName('input');
            for (var i = 0; i < items.length; i++) {
                if (items[i].type == "radio") {
                    if (items[i].checked) {
                        args.IsValid = true;
                        return;
                    }
                    else {
                        args.IsValid = false;
                    }
                }
            }
        }

        var validateradios = function () {
            var icount = 0;
            var grid = document.getElementById('<%=grvDocuments.ClientID %>');
            alert(grid.rows.length);
            for (var i = 1; i < grid.rows.length; i++) {
                var row = grid.rows[i];
                var targetcell = row.cells[3];
                var inputs = targetcell.getElementsByTagName("input");
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].checked) {
                        icount++; break;
                    }
                }
            }
            alert(icount)
            if (icount == (grid.rows.length - 1))
                alert(' all rows checked');
            else
                alert('All verifications need to be checked.');
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
                    <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission
                        Company LTD.</font></span>
                </div>
                <span style="position: absolute; bottom: 0px; right: 0px">Welcome
                    <asp:Label ID="lblLoginname" runat="server" Text=""></asp:Label></span>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <!-- put class="selected" in the li tag for the selected page - to highlight which page you're on -->
                    <li class="selected"><a href="FinalGC.aspx">Back</a></li>
                    <%--<li class="selected"><a href="Home.aspx">SignOut</a></li>--%>
                </ul>
            </div>
        </div>
    </div>
    <form id="form1" runat="server">
        <div class="content-wrapper">
            <div class="content">
                <div class="connectedSortable" style="margin-left: 10px;">
                    <div class="col-sm-1">
                    </div>
                    <div class="col-md-10">
                        <div class="col-md-4 ">
                            <br />
                            <br />
                            <asp:Label ID="lblMessage" runat="server" Text="" Font-Names="Arial"></asp:Label>
                        </div>
                        <br />
                        <br />
                        <div class="modal-content">
                            <div class="modal-header" style="background-color: #4682B4; color: #fff">
                                <h4>
                                    <span class="fa fa-download"></span>&nbsp; "View & Submit Files</h4>
                            </div>
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-sm-12 ">
                                        <div class="table-responsive">
                                            <asp:GridView runat="server" ID="grvDocuments" Width="100%" Class="table table-striped table-bordered table-hover"
                                                AutoGenerateColumns="false" OnRowCommand="grvDocuments_RowCommand" OnRowDataBound="grvDocuments_RowDataBound" OnSelectedIndexChanged="grvDocuments_SelectedIndexChanged">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="S.No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                                                            <asp:HiddenField ID="HFSrno" runat="server" Value='<%#Eval("SrNo")%>' />
                                                            <asp:HiddenField ID="HdfFileName" runat="server" Value='<%#Eval("FileName")%>' />
                                                            <asp:HiddenField ID="HFFilesrno" runat="server" Value='<%#Eval("FileSrNo")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DocName" HeaderText="Document Type" />
                                                    <asp:TemplateField HeaderText="View">
                                                        <ItemTemplate>
                                                            <%--<asp:HyperLink ID="lnkView" Text="View" NavigateUrl='<%# Eval("Value", "~/ViewFinalGCDoc.aspx?fileName={0}") %>'
                                                            runat="server" Target="_blank" />--%>
                                                            <asp:ImageButton ID="imgView" ToolTip="View Document" ImageUrl="~/assets/images/download.png"
                                                                BorderWidth="0px" Width="30px" Height="30px" runat="server" CommandName="View"
                                                                CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="aspnetForm.target ='_blank';" />
                                                            <%--<asp:LinkButton Text="View" ID="lnkView" runat="server" />--%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Verified/Returned">
                                                        <ItemTemplate>
                                                            <asp:RadioButtonList ID="rbVR" runat="server" RepeatDirection="Horizontal" ViewStateMode="Enabled">
                                                                <asp:ListItem Value="Y">Verify</asp:ListItem>
                                                                <asp:ListItem Value="N">Return</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Remark">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtRemark" TextMode="MultiLine" Text='<%#Eval("Remark")%>' runat="server"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Add remark" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnAddRemark" runat="server" Text="Submit" CssClass="btn-info" ValidationGroup="Submit"
                                                                CommandName="AddRemark" CommandArgument="<%# Container.DataItemIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Want to changed?">
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnChange" runat="server" Text="Change" CssClass="btn-info" ValidationGroup="Submit"
                                                                CommandName="Change" CommandArgument="<%# Container.DataItemIndex %>" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div id="dialog" style="display: none">
                                                <b>Id:</b> <span id="id"></span>
                                                <br />
                                                <b>Name:</b> <span id="name"></span>
                                                <br />
                                                <b>Description:</b> <span id="description"></span>
                                            </div>
                                            <table align="center" cellspacing="10px" cellpadding="20px">
                                                <tr>
                                                    <td>Remarks :
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtRemark" runat="server" TextMode="MultiLine" Rows="5" Columns="100"
                                                            Width="461px"></asp:TextBox>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr id="trDeviation" runat="server">
                                                    <td>Is Deviation 
                                                    required? <span class="auto-style1">*</span> :
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rbDev" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow"
                                                            CellPadding="10" CellSpacing="10">
                                                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                                                            <asp:ListItem Value="N">No</asp:ListItem>
                                                        </asp:RadioButtonList>
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Button ID="btnVerified" runat="server" Text="Submit" CssClass="btn-info" ValidationGroup="Submit"
                                                            OnClick="btnVerified_Click" />
                                                        &nbsp;&nbsp;
                                                    <asp:Button ID="btnFinalSubmit" runat="server" Text="Final Submit" CssClass="btn-info"
                                                        ValidationGroup="FinalSubmit" OnClick="btnFinalSubmit_Click" Enabled="False"
                                                        Visible="False" />
                                                        <%--<asp:Button ID="btnReturned" runat="server" Text="Return" CssClass="btn-info" 
                                                        ValidationGroup="Submit" onclick="btnReturned_Click" />--%>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="center">
                                                        <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Green"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Please select all verify/return in the list."
                                                            ClientValidationFunction="ValidateRadioButton" ValidationGroup="Submit" Style="display: none"></asp:CustomValidator>
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Submit"
                                                            HeaderText="Error List:" DisplayMode="BulletList" ForeColor="Red" />
                                                    </td>
                                                </tr>
                                            </table>

                                            <br />
                                            <br />
                                            <br />
                                            <table align="center">
                                                <tr>
                                                    <td align="center">Deviation History</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="GVDeviationHistory" runat="server" AutoGenerateColumns="False" CellPadding="4"
                                                            ForeColor="#333333" GridLines="Both" EmptyDataText="No History Found.">
                                                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                                            <Columns>
                                                                <asp:BoundField DataField="APPLICATION_NO" HeaderText="Application No">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="isAppr_Rej_Ret" HeaderText="Confirm/Return">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="remark" HeaderText="Remark">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Aprove_Reject_Return_by" HeaderText="Verify_Return_By">
                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="createDT" HeaderText="Verify_Return_Date" DataFormatString="{0:dd-MM-yyyy}">
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
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
