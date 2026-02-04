using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace GGC.Common
{
    public static class MasterPageExtenations
    {
        public static T GetMasterPageObject<T>(this MasterPage master) where T : MasterPage
        {
            return (T)master;
        }
    }
}