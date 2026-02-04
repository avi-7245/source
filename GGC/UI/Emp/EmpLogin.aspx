<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmpLogin.aspx.cs" Inherits="GGC.UI.Emp.EmpLogin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Home</title>
    <%--<link rel="shortcut icon" type="image/icon" href="../../assets/images/favicon.ico"/>--%>
    <!-- Font Awesome -->
    <link href="../../assets/css/font-awesome.css" rel="stylesheet" />
    <!-- Bootstrap -->
    <link href="../../assets/css/bootstrap.css" rel="stylesheet" />
    <!-- Slick slider -->
    <link rel="stylesheet" type="text/css" href="../../assets/css/slick.css" />
    <!-- Fancybox slider -->
    <%--<link rel="stylesheet" href="../../assets/css/jquery.fancybox.css" type="text/css"
        media="screen" />--%>
    <!-- Animate css -->
    <link rel="stylesheet" type="text/css" href="../../assets/css/animate.css" />
    <!-- Progress bar  -->
    <link rel="stylesheet" type="text/css" href="../../assets/css/bootstrap-progressbar-3.3.4.css" />
    <!-- Theme color -->
    <link id="switcher" href="../../assets/css/theme-color/default-theme.css" rel="stylesheet">
    <!-- Main Style -->
    <link href="../../Styles/style4.css" rel="stylesheet" />
    <!-- Fonts -->
    <!-- Open Sans for body font -->
    <link href='https://fonts.googleapis.com/css?family=Open+Sans' rel='stylesheet' type='text/css'>
    <!-- Lato for Title -->
    <link href='https://fonts.googleapis.com/css?family=Lato' rel='stylesheet' type='text/css'>
    <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
      <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <style>
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
    <script type="text/javascript">
        function disableButton() {
            document.getElementById('<%= btnSendOTP.ClientID %>').disabled = true;
            document.getElementById('<%= btnSendOTP.ClientID %>').style.backgroundColor = 'Grey';
            setTimeout(function () {
                document.getElementById('<%= btnSendOTP.ClientID %>').disabled = false;
                document.getElementById('<%= btnSendOTP.ClientID %>').style.backgroundColor = '#008CBA';
            }, 60000);
        }
    </script>
</head>
<body>
    <!-- Start login modal window -->
    <div aria-hidden="false" role="dialog" tabindex="-1" id="login-form" class="modal leread-modal fade in">
        <div class="modal-dialog">
            <!-- Start login section -->
            <div id="login-content" class="modal-content">
                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button">
                        <span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">
                        <i class="fa fa-unlock-alt"></i>Login</h4>
                </div>
                <%--<div class="modal-body">--%>
                <%-- </div>--%>
                <%--<div class="modal-footer footer-box">
          <a href="#">Forgot password ?</a>
         <%-- <span>No account ? <a id="btnSignUp" href="Register.aspx">Sign Up.</a></span>     
        </div>--%>
            </div>
            <!-- Start signup section -->
            <div id="signup-content" class="modal-content">
                <div class="modal-header">
                    <button aria-label="Close" data-dismiss="modal" class="close" type="button">
                        <span aria-hidden="true">×</span></button>
                    <h4 class="modal-title">
                        <i class="fa fa-lock"></i>Sign Up</h4>
                </div>
                <div class="modal-body">
                    <form>
                        <div class="form-group">
                            <input placeholder="Name" class="form-control">
                        </div>
                        <div class="form-group">
                            <input placeholder="Username" class="form-control">
                        </div>
                        <div class="form-group">
                            <input placeholder="Email" class="form-control">
                        </div>
                        <div class="form-group">
                            <input type="password" placeholder="Password" class="form-control">
                        </div>
                        <div class="signupbox">
                            <span>Already got account? <a id="login-btn" href="#">Sign In.</a></span>
                        </div>
                        <div class="loginbox">
                            <label>
                                <input type="checkbox"><span>Remember me</span><i class="fa"></i></label>
                            <button class="btn signin-btn" type="button">
                                SIGN UP</button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <!-- End login modal window -->
    <!-- BEGAIN PRELOADER -->
    <div id="preloader">
        <div id="status">
            &nbsp;
        </div>
    </div>
    <!-- END PRELOADER -->
    <!-- SCROLL TOP BUTTON -->
    <a class="scrollToTop" href="#"><i class="fa fa-angle-up"></i></a>
    <!-- END SCROLL TOP BUTTON -->
    <!-- Start header -->
    <header id="header">
        <!-- header top search -->
        <%--<div class="header-top">
      <div class="container">
        <form action="">
          <div id="search">
          <input type="text" placeholder="Type your search keyword here and hit Enter..." name="s" id="m_search" style="display: inline-block;">
          <button type="submit">
            <i class="fa fa-search"></i>
          </button>
        </div>
        </form>
      </div>
    </div>--%>
        <!-- header bottom -->
        <div class="header-bottom">
            <div class="container">
                <div class="row">
                    <div class="col-md-6 col-sm-6 col-xs-6">
                        <div class="header-contact">
                            <ul>
                                <%--<li>
                  <div class="phone">
                    <i class="fa fa-phone"></i>
                    
                  </div>
                </li>--%>
                                <li>
                                    <div class="mail">
                                        <i class="fa fa-envelope"></i>
                                        adee2sys@mahatransco.in
                                    </div>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <%--<div class="col-md-6 col-sm-6 col-xs-6">
            <div class="header-login">
              <a class="login modal-form" data-target="#login-form" data-toggle="modal" href="#">Login</a>
            </div>
          </div>--%>
                </div>
            </div>
        </div>
    </header>
    <!-- End header -->
    <!-- BEGIN MENU -->
    <%-- <section id="menu-area">      --%>
    <%-- <nav class="navbar navbar-default" role="navigation">  --%>
    <%--<div class="container">--%>
    <div>
        <!-- FOR MOBILE VIEW COLLAPSED BUTTON -->
        <%--<button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>--%>
        <!-- LOGO -->
        <!-- TEXT BASED LOGO -->
        <!--<a class="navbar-brand" href="index.html">Mahapareshan</a>      -->
        <!-- IMG BASED LOGO  -->
        <!-- <a class="navbar-brand" href="index.html"><img src="assets/images/logo.png" alt="logo"></a> -->
    </div>
    <%--<div id="navbar" class="navbar-collapse collapse">
          <ul id="top-menu" class="nav navbar-nav navbar-right main-nav">
            <li class="active"><a href="index.html">Home</a></li>
            <li><a href="feature.html">Feature</a></li>
            <li><a href="service.html">Service</a></li>
            <li><a href="portfolio.html">Portfolio</a></li>
            <li class="dropdown">
              <a href="#" class="dropdown-toggle" data-toggle="dropdown">Blog <span class="fa fa-angle-down"></span></a>
              <ul class="dropdown-menu" role="menu">
                <li><a href="blog-archive.html">Blog Archive</a></li>                
                <li><a href="blog-single-with-left-sidebar.html">Blog Single with Left Sidebar</a></li>
                <li><a href="blog-single-with-right-sidebar.html">Blog Single with Right Sidebar</a></li>
                <li><a href="blog-single-with-out-sidebar.html">Blog Single with out Sidebar</a></li>           
              </ul>
            </li>
            <li><a href="404.html">404 Page</a></li>               
            <li><a href="contact.html">Contact</a></li>
          </ul>                     
        </div>--%>
    <!--/.nav-collapse -->
    <%--<a href="#" id="search-icon">
          <i class="fa fa-search">            
          </i>
        </a>--%>
    <%--</div>     --%>
    <%--</nav>--%>
    <%--</section>--%>
    <!-- END MENU -->
    <img src="../../assets/images/logo.jpg" alt="logo" align="middle" style="padding-right: 150px; padding-left: 40px" />
    <b><font size="+3" style="color: Red">Maharashtra State Electricity Transmission Company
        LTD.</font></b>
    <center>
        <font size="+2" style="color: Black">Welcome to Online Grid Connectivity Application
            Employee portal</font></center>
    <br />
    <br />
    <br />
    <br />
    <br />
    <form id="form2" runat="server">
        <%--<div style="margin: auto;width:20%">
            <fieldset style="width: 100%;">
                <legend>Login</legend>--%>
        <table width="30%" align="center" border="1px" cellpadding="10px" cellspacing="10px" style="border-radius: 10px;">
            <%--<tr>
    <td colspan="2" align="center"><h4>Login</h4></td>
    </tr>--%>
            <tr>
                <td>SAP ID:</td>
                <td>
                    <%--<input type="text" placeholder="User name" class="form-control" runat="server" >--%>
                    <asp:TextBox ID="txtSAPID" runat="server" placeholder="SAP ID"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>Password: </td>
                <td>
                    <asp:TextBox ID="txtPass" runat="server" TextMode="SingleLine" placeholder="Password" ViewStateMode="Enabled"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rev" runat="server" ControlToValidate="txtPass" Display="Dynamic"
                        ValidationGroup="Login" ErrorMessage="Spaces are not allowed!" ValidationExpression="[^\s]+" />
                    <asp:RequiredFieldValidator ID="rfv" runat="server" Display="Dynamic" ControlToValidate="txtPass" ErrorMessage="Blank is not allowed!" />
                </td>
            </tr>
            <%--<asp:RegularExpressionValidator ID="Regex2" runat="server" ControlToValidate="txtPass"
    ValidationExpression="^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,}$"
    ErrorMessage="Incorrect user or password" ForeColor="Red" />--%>
            <tr>
                <td>
                    <asp:TextBox ID="txtOTP" runat="server" TextMode="Password" placeholder="OTP"></asp:TextBox>
                </td>
                <td align="center">
                    <asp:Button ID="btnSendOTP" runat="server" CssClass="button" ValidationGroup="OTP"
                        Text="Get OTP" OnClick="btnSendOTP_Click" />
                </td>
            </tr>
            <%--<label><input type="checkbox"><span>Remember me</span></label>--%>
            <%--<button class="btn signin-btn" type="button" runat="server" ID="btnLogin" onclick="btnLogin_Click" >SIGN IN</button>--%>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnLogin" runat="server" CssClass="button" ValidationGroup="Login"
                        Text="Login" OnClick="btnLogin_Click" />
                    <%--<asp:Button ID="btnForgot" runat="server" CssClass="button" ValidationGroup="Forgott"
                    Text="Forgott Password" OnClick="btnForgot_Click" />--%>
                    <asp:Label ID="lblResult" runat="server" Text="" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <%--</fieldset>
    </div>--%>
    </form>
    <!-- Start Feature -->
    <%-- <section id="feature">
    <div class="container">
      <div class="row">
        <div class="col-md-12">
          <div class="title-area">
            
            
            
          </div>
        </div>
        <hr width="100%" />
        <div class="col-md-12">
          <div class="feature-content">
            <div class="row">
              <div class="col-md-4 col-sm-6">
                <div class="single-feature wow zoomIn">
                  <i class="fa fa-leaf feature-icon"></i>
                  <h4 class="feat-title">Creative Design</h4>
                  <p>There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6">
                <div class="single-feature wow zoomIn">
                  <i class="fa fa-mobile feature-icon"></i>
                  <h4 class="feat-title">Responsive Layouts</h4>
                  <p>There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6">
                <div class="single-feature wow zoomIn">
                  <i class="fa fa-thumbs-o-up feature-icon"></i>
                  <h4 class="feat-title">Great Features</h4>
                  <p>There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6">
                <div class="single-feature wow zoomIn">
                  <i class="fa fa-gears feature-icon"></i>
                  <h4 class="feat-title">Multiple Options</h4>
                  <p>There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6">
                <div class="single-feature wow zoomIn">
                  <i class="fa fa-code feature-icon"></i>
                  <h4 class="feat-title">Quality Code</h4>
                  <p>There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.</p>
                </div>
              </div>
              <div class="col-md-4 col-sm-6">
                <div class="single-feature wow zoomIn">
                  <i class="fa fa-smile-o feature-icon"></i>
                  <h4 class="feat-title">Awesome Support</h4>
                  <p>There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable.</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>--%>
    <!-- End Feature -->
    <br />
    <br />
    <br />
    <br />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <!-- Bootstrap -->
    <script src="../../assets/js/bootstrap.js"></script>
    <!-- Slick Slider -->
    <script type="text/javascript" src="../../assets/js/slick.js"></script>
    <!-- mixit slider -->
    <script type="text/javascript" src="../../assets/js/jquery.mixitup.js"></script>
    <!-- Add fancyBox -->
    <script type="text/javascript" src="../../assets/js/jquery.fancybox.pack.js"></script>
    <!-- counter -->
    <script src="../../assets/js/waypoints.js"></script>
    <script src="../../assets/js/jquery.counterup.js"></script>
    <!-- Wow animation -->
    <script type="text/javascript" src="../../assets/js/wow.js"></script>
    <!-- progress bar   -->
    <script type="text/javascript" src="../../assets/js/bootstrap-progressbar.js"></script>
    <!-- Custom js -->
    <script type="text/javascript" src="../../assets/js/custom.js"></script>
</body>
</html>
