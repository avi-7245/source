<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadGCLetter.aspx.cs"
    Inherits="GGC.UI.Cons.UploadGCLetter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload GC Letters</title>
    <style type="text/css">
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
    <script type="text/javascript">
        function checkUploadFile(elem) {
            if (checkFileExtension(elem)) {
                ValidateFileName(elem);
            }
        }
        function checkFileExtension(elem) {
            var filePath = elem.value;
            if (filePath.indexOf('.') == -1)
                return false;

            var validExtensions = new Array();
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
            //Add valid extentions in this array
            //            validExtensions[0] = 'JPEG';
            //            validExtensions[1] = 'JPG';
            //            validExtensions[2] = 'PNG';
            //            validExtensions[3] = 'jpeg';
            //            validExtensions[4] = 'jpg';
            //            validExtensions[5] = 'png';
            validExtensions[0] = 'PDF';
            validExtensions[1] = 'pdf';
            validExtensions[2] = 'Pdf';

            for (var i = 0; i < validExtensions.length; i++) {
                if (ext == validExtensions[i])
                    return true;
            }
            alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
            elem.value = '';
            return false;
        }
        function ValidateFileName(elem) {


            var myfile = elem.value;
            if (myfile.search(/[<>'\s+\"\/;`%]/) > 0) {
                alert('please upload the file without special characters and SPACES');
                elem.value = '';
                return false;
            }
            else {
                //alert('valid Format');
                return true;
            }

        }
    </script>
    <script>       
    function chkDate()
    {
        t1 = document.getElementById("txtAppliedDt").value;

        t2 = document.getElementById("txtIssueDt").value;

            var one_day=1000*60*60*24;

            var x=t1.split("/");

            var y=t2.split("/");

            var date1=new Date(x[2],(x[1]-1),x[0]);

            var date2=new Date(y[2],(y[1]-1),y[0])

            var month1=x[1]-1;
            var month2=y[1]-1;

            _Diff=Math.ceil((date2.getTime()-date1.getTime())/(one_day));
            if(_Diff<0)

            {
            alert("select correct date");

            }

    }

//    function onChange(sender, txt, departed) {
//        var txtArrival = $("#txtAppliedDt");
//        var txtArrivalDate = $(txtArrival).val(); //Value from arrival
//        var txtDate = $(txt).val(); //Value from departed

//        var departureDate = new Date(txtDate); //Converting string to date
//        var arrivalDate = new Date(txtArrivalDate); //Converting string to date

//        alert(arrivalDate);

//        if (departureDate.getTime() < arrivalDate.getTime()) {
//            //txt.val(txtArrivalDate); //Does not work, value is not updated
//            alert("Issue date must be Applied Date.");
//        }
//    }

//    $("#txtAppliedDt").change(function (event) {
//        onChange(event, $("#txtIssueDt"));
//    });

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
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div class="mainbody">
            <center>
                <h3>
                    Upload Documents 
                    of Old GC</h3>
            </center>
            <table width="100%" align="center" align="center" cellspacing="5">
                <%--<tr>
                    <td colspan="4">
                        <b>Upload MEDA Letter</b>
                    </td>
                </tr>--%>
                <%--<tr>
                    <td>
                        <asp:FileUpload ID="FUMEDADoc" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="lblLetterUploadStatus" runat="server" Text=""></asp:Label>
                    </td>
                    <td>
                        <asp:Button ID="btnUploadMEDADoc" Text="Upload" runat="server" ValidationGroup="Photo"  OnClick="UploadMEDALetter"  
                            CssClass="button"  />
                    </td>
                    <td>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="Photo"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FUMEDADoc"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="Photo"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td>
                        <b>Enter Application applied date to MSETCL physically</b>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAppliedDt" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ibtappliedDT" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtAppliedDt"
                            PopupButtonID="ibtappliedDT" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtAppliedDt" ValidationGroup="GCLetter"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                    </td>
                    

                </tr>
                <tr>
                    <td>
                        <b>Upload Grid connectivity issued letter</b>
                    </td>
                    <td colspan="3">
                        <asp:FileUpload ID="FUGCLetter" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="lblGCLetter" runat="server" Text=""></asp:Label>
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="GCLetter"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FUGCLetter"
                            Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="GCLetter"></asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                <td>
                    Enter Grid connectivity issued letter Date
                        
                    </td>
                    <td>
                    <asp:TextBox ID="txtIssueDt" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtIssueDt"
                            PopupButtonID="ImageButton1" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtIssueDt" ValidationGroup="GCLetter"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>

                        <asp:Label ID="lblGCLetter1" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <%--<asp:CompareValidator ID="CompareValidator1" runat="server" 
                            ControlToCompare="txtAppliedDt" ControlToValidate="txtIssueDt" 
                            Display="Dynamic" ErrorMessage="Issued date must be greater than apply date!" 
                            ForeColor="Red" Operator="GreaterThan" Type="Date" ></asp:CompareValidator>--%>
                    </td>
                    <td>
                    Enter Grid connectivity issued letter validity Date
                    </td>
                    <td>
                    <asp:TextBox ID="txtValidityDt" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtValidityDt"
                            PopupButtonID="ImageButton4" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            Display="Dynamic" ErrorMessage="*Required" ControlToValidate="txtValidityDt" ValidationGroup="GCLetter"
                            SetFocusOnError="True"></asp:RequiredFieldValidator>

                        
                        <%--<asp:CompareValidator ID="CompareValidator4" runat="server" 
                            ControlToCompare="txtIssueDt" ControlToValidate="txtValidityDt" 
                            Display="Dynamic" ErrorMessage="Validity date must be greater than issue date!" 
                            ForeColor="Red" Operator="GreaterThan" Type="Date" ValidationGroup="GCLetter"></asp:CompareValidator>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Upload Other Documents </b>(Upload all Project related other documents in single
                        PDF file)
                    </td>
                    <td colspan="3">
                        <asp:FileUpload ID="FUOther" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="lblOtherResult" runat="server" Text=""></asp:Label>
                        <asp:CustomValidator ID="CustomValidator3" runat="server" ValidationGroup="GCLetter"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                        <asp:Label ID="lblOther" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                    
                </tr>
                <tr>
                    <td>
                        <b>Upload First Extension letter (if any)</b></td>
                    <td>
                        <asp:FileUpload ID="FUExt1" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="GCLetter"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>

                    </td>
                    <td>
                    Enter First Extension validy Date
                        
                    </td>
                    <td>
                    <asp:TextBox ID="txtExt1" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtExt1"
                            PopupButtonID="ImageButton2" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <%--<asp:CompareValidator ID="CompareValidator2" runat="server" 
                            ControlToCompare="txtValidityDt" ControlToValidate="txtExt1" 
                            Display="Dynamic" ErrorMessage="Extension date must be greater than issued date!" 
                            ForeColor="Red" Operator="GreaterThan" Type="Date" ValidationGroup="GCLetter"></asp:CompareValidator>--%>
                        <asp:Label ID="lblExt1" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td>
                        <b>Upload second Extension letter  (if any) </b>
                    </td>
                    <td>
                        <asp:FileUpload ID="FUExt2" runat="server" onchange="checkUploadFile(this);" />
                        <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                        <asp:CustomValidator ID="CustomValidator4" runat="server" ValidationGroup="GCLetter"
                            OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator"
                            Display="Dynamic"></asp:CustomValidator>
                    </td>
                    <td>
                    
                        
                        Enter Second Extension validy Date</td>
                    <td>
                    <asp:TextBox ID="txtExt2" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" ImageUrl="~/assets/images/calendar.jpg"
                            ImageAlign="absbottom"></asp:ImageButton>
                        <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtExt2"
                            PopupButtonID="ImageButton3" Format="dd-MM-yyyy" PopupPosition="Right">
                        </asp:CalendarExtender>
                        <%--<asp:CompareValidator ID="CompareValidator3" runat="server" 
                            ControlToCompare="txtExt1" ControlToValidate="txtExt2" 
                            Display="Dynamic" ErrorMessage="Second Extension date must be greater than First Extension date!" 
                            ForeColor="Red" Operator="GreaterThan" Type="Date" ValidationGroup="GCLetter"></asp:CompareValidator>--%>
                        <asp:Label ID="lblExt2" runat="server" Text="" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btnSubmit" Text="Submit" runat="server" ValidationGroup="GCLetter"
                            CssClass="button" Enabled="False" onclick="btnSubmit_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
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
