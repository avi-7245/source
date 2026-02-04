using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for UserAuthClass
/// </summary>

[WebService(Namespace = "http://gridconn.mahatransco.in/")]
public class UserAuthClass
{
    public checkUser User;
	public UserAuthClass()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    [WebMethod]
   // [SoapHeader("User", Required = true)]
    public bool checklogin()
    {
        //if (User != null)
        //{
            if (User.isValid()>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        //}
        //else
        //{
        //    return false;
        //}
    }
}