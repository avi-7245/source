<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadCon.aspx.cs" Inherits="GGC.UI.Cons.UploadCon" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Upload Documents</title>
    <style type="text/css">
        .form-control {
            display: block;
            width: 85%;
            height: 16px;
            padding: 2px 2px;
            font-size: 13px;
            line-height: 1.42857143;
            color: #555;
            background-color: #fff;
            border: 1px solid #ccc;
            border-radius: 2px;
            box-shadow: inset 0 1px 1px rgba(0, 0, 0, .075);
            transition: border-color ease-in-out .15s, box-shadow ease-in-out .15s;
        }
        .form-control:focus {
            border-color: #66afe9;
            outline: 0;
            box-shadow: inset 0 1px 1px rgba(0,0,0,.075), 0 0 8px rgba(102, 175, 233, .6);
        }
        .mainbody {
            width: 900px;
            margin: 0 auto;
        }
        .button {
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
    <script>
        function checkUploadFile(input) {
            const maxSizeInBytes = 10 * 1024 * 1024; // 10MB
            const file = input.files[0]; // Get the first file

            if (file) {
                if (file.size > maxSizeInBytes) {
                    alert("File size exceeds 10MB. Please choose a smaller file.");
                    input.value = ""; // Clear the input
                }
            }
        }

        function validateAllFiles() {
            const fileInputs = document.querySelectorAll('input[type="file"]');
            let allUploaded = true;

            fileInputs.forEach((input) => {
                // Check if the file input (or its parent) is visible
                if (input.offsetParent !== null) {
                    if (!input.value) {
                        allUploaded = false;
                        // Display an alert for the label or associated text next to the file input
                        const label = input.closest('tr')?.querySelector('td:nth-child(2)')?.textContent.trim() || "a required file";
                        alert(`Please upload the file for: ${label}`);
                    }
                }
            });

            return allUploaded; // Prevent form submission if false
        }
    </script>
    <script type="text/javascript">
        //function checkUploadFile(elem) {
        //    if (checkFileExtension(elem)) {
        //        ValidateFileName(elem);
        //    }
        //}

        function checkFileExtension(elem) {
            var filePath = elem.value;
            if (filePath.indexOf('.') === -1) return false;

            var validExtensions = ['pdf'];
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();

            if (validExtensions.includes(ext)) return true;

            alert('The file extension ' + ext.toUpperCase() + ' is not allowed!');
            elem.value = '';
            return false;
        }

        function ValidateFileName(elem) {
            var myfile = elem.value;
            if (myfile.search(/[<>'\s+\"\/;`%]/) > 0) {
                alert('Please upload the file without special characters and spaces.');
                elem.value = '';
                return false;
            }
            return true;
        }

        function validateFileSize(fileInput, maxSizeMB) {
            const files = fileInput.files;

            // Check if any file is selected
            if (files.length === 0) {
                alert("Please select a file.");
                return false;
            }

            // Convert max size to bytes (1 MB = 1024 * 1024 bytes)
            const maxSizeBytes = maxSizeMB * 1024 * 1024;

            // Loop through each file and check its size
            for (let i = 0; i < files.length; i++) {
                const file = files[i];

                // Check if the file size is larger than the max size
                if (file.size > maxSizeBytes) {
                    alert("The file '" + file.name + "' is too large. Please upload a file smaller than " + maxSizeMB + " MB.");
                    return false;
                }
            }

            // If all file sizes are within the limit
            alert("File size is valid.");
            return true;
        }


    </script>
</head>
<body>
    <div id="main">
        <div id="header">
            <div id="logo">
                <img src="../../assets/images/logo.jpg" height="100" align="middle" />
                <span class="logo_colour"><font size="+2">Maharashtra State Electricity Transmission Company LTD.</font></span>
            </div>
            <div id="menubar">
                <ul id="menu">
                    <li class="selected"><a href="AppHome.aspx">Home</a></li>
                    <li class="selected"><a href="Home.aspx">SignOut</a></li>
                </ul>
            </div>
        </div>
        <form id="form1" runat="server">
            <div class="mainbody">
                <center>
                    <h3>Upload Relevant Documents</h3>
                </center>
                <br />
                <div>
                    <font size="+2" color="red">
                        <marquee>Kindly after all documents upload, Download application form and upload with seal and sign from home page.</marquee>
                    </font>
                </div>

                <table width="100%" align="center" cellspacing="5">
                 
                     <tr>
     <td>
         <asp:FileUpload ID="FUSLD" runat="server" onchange="checkUploadFile(this);" />
         <asp:Label ID="lblSLDResult" runat="server" Text="Substation Single Line diagram"></asp:Label>
     </td>
     <td>
         <asp:Button ID="Button9" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadSLD" CssClass="button"  OnClientClick="return validateFileSize(fileUpload, 10);" />
     </td>
     <td>
         <asp:CustomValidator ID="cvFUMEDADoc" runat="server"      ControlToValidate="FUSLD"            ErrorMessage="File size is too large!" ForeColor="Red"  />
     </td>
     <td>
         <asp:Label ID="lblSLD" runat="server" Text="" ForeColor="Red"></asp:Label>
     </td>
 </tr>
                    <%-- <tr>
                       <td >
                            <asp:FileUpload ID="FUSLD" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblSLDResult" runat="server" Text=""></asp:Label>
                        </td>
                        <td colspan="4" style="color: #333333;background-color: #F7F6F3;">
                            <asp:Button ID="btnUploadSLD" Text="Upload" runat="server" ValidationGroup="SLD" OnClick="UploadSLD" CssClass="button" />
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="SLD" OnServerValidate="ValidateMEDAFileSize" ForeColor="Red" ErrorMessage="CustomValidator" Display="Dynamic"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="FUSLD" Display="Dynamic" ForeColor="Red" ErrorMessage="*Required" ValidationGroup="SLD"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lblSLD" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td colspan="4" style="color: #333333;background-color: #F7F6F3;">
                            <b>Upload Other Documents</b> (Upload all Project related other documents in single PDF.)
                        </td>
                    </tr>--%>
                    <tr runat="server" id="row1" visible="true">
                        <td>
                            <asp:FileUpload ID="FUOther" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblNotrizeAff" runat="server" Text="Notarized affidavit"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnOther" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther" CssClass="button"  OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                             <asp:CustomValidator ID="CustomValidator18" runat="server"      ControlToValidate="FUOther"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="lblOther" runat="server" Text="Notarized affidavit" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row2">
                        <td>
                            <asp:FileUpload ID="fuBoardResolution" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblBoardResolution" runat="server" Text="Board Resolution"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button3" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther3" CssClass="button"  OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                          <asp:CustomValidator ID="CustomValidator19" runat="server"      ControlToValidate="fuBoardResolution"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="lblOther3" runat="server" Text="Board Resolution" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row3">
                        <td>
                            <asp:FileUpload ID="fuMemorandumAndArticlesofAssociation" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblMemorandumAndArticlesofAssociation" runat="server" Text="Memorandum And Articles of Association"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button4" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther4" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator20" runat="server"   ControlToValidate="fuMemorandumAndArticlesofAssociation"     ErrorMessage="File size is too large!" ForeColor="Red"  />

                            </td>
                        <td>
                            <asp:Label ID="lblOther4" runat="server" Text="Memorandum AnArticle of Association" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                   <tr runat="server" visible="true" id="row4">
                        <td>
                            <asp:FileUpload ID="fuGovernmentAuthorization" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblGovernmentAuthorization" runat="server" Text="Government Authorization"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button1" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther5" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                                                              <asp:CustomValidator ID="CustomValidator21" runat="server"      ControlToValidate="fuGovernmentAuthorization"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Government Authorization" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row5">
                        <td>
                            <asp:FileUpload ID="fuLeaseRightsorOwnershipProof" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblLeaseRightsorOwnershipProof" runat="server" Text="Lease Rights or Ownership Proof"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button2" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther6" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                                                            <asp:CustomValidator ID="CustomValidator22" runat="server"      ControlToValidate="fuLeaseRightsorOwnershipProof"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                            </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Lease Rights or Ownership Proof" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row6">
                        <td>
                            <asp:FileUpload ID="fuBankGuaranteeDocument" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblBankGuaranteeDocument" runat="server" Text="Bank Guarantee Document"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button5" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther7" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                                                              <asp:CustomValidator ID="CustomValidator23" runat="server"      ControlToValidate="fuBankGuaranteeDocument"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Bank Guarantee Document" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row7">
                        <td>
                            <asp:FileUpload ID="fulblLetterofAward" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblLetterofAward" runat="server" Text="Letter of Award (LOA) or PPA"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button6" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther8" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                                                                 <asp:CustomValidator ID="CustomValidator24" runat="server"      ControlToValidate="fulblLetterofAward"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="Label10" runat="server" Text="Letter of Award (LOA) or PPA" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row8">
                        <td>
                            <asp:FileUpload ID="fuProofofOwnership" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblProofofOwnership" runat="server" Text="Proof of Ownership/Lease for Land"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button7" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther9" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                                                              <asp:CustomValidator ID="CustomValidator25" runat="server"      ControlToValidate="fuProofofOwnership"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="Label12" runat="server" Text="Proof of Ownership/Lease for Land" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr runat="server" visible="true" id="row9">
                        <td>
                            <asp:FileUpload ID="fuBankGuarantee" runat="server" onchange="checkUploadFile(this);" />
                            <asp:Label ID="lblBankGuarantee" runat="server" Text="Bank Guarantee ₹10 Lakh/MW or ₹100 Crore"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="Button8" Text="Upload" runat="server" ValidationGroup="other" OnClick="UploadOther10" CssClass="button" OnClientClick="return validateFileSize(fileUpload, 10);"/>
                        </td>
                        <td>
                         <asp:CustomValidator ID="CustomValidator26" runat="server"      ControlToValidate="fuBankGuarantee"            ErrorMessage="File size is too large!" ForeColor="Red"  />

                        </td>
                        <td>
                            <asp:Label ID="Label14" runat="server" Text="Bank Guarantee ₹10 Lakh/MW or ₹100 Crore" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Button ID="btnAdd" runat="server" onclick="btnSubmit_Click" Text="Sumbit" CssClass="button" Visible="true" /> <%--OnClientClick="return validateAllFiles();"--%>
                           <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" Visible="true"></asp:Label>
                        </td>
                    </tr>
                </table>

                


<br />






            </div>
            <div id="footer">
                <p>Design and Developed by IT Department, MSETCL.</p>
            </div>
        </form>
    </div>
</body>
</html>