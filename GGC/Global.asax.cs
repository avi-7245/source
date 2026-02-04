using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net;

namespace GGC
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            log4net.Config.XmlConfigurator.Configure();
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)(768 | 3072);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }

        void Session_Start(object sender, EventArgs e)
        {
            //// Code that runs when a new session is started
            //if (Session["user_name"] != null)
            //{
            //    //Redirect to Welcome Page if Session is not null  
            //    Response.Redirect("/UI/Cons/AppHome.aspx");

            //}
            //else
            //{
            //    //Redirect to Login Page if Session is null & Expires   
            //    Response.Redirect("UI/Cons/Home.aspx");

            //}  
        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
