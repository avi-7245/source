using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public class SessionConst
    {
        public const string EmpRole = "EmpRole";
        public const string SAPID = "SAPID";
        public const string ReportingSAPID = "ReportingSAPID";
        public const string EmpZone = "EmpZone";
        public const string EmpDesignation = "EmpDesignation";
        public const string EmpName = "EmpName";
        public const string EKYC_ID = "ekycid";
        public const string ApplicationNo = "AppID";
    }

    public class ButtonCssClassConst
    {
        public const string Primary = "btn btn-primary";
        public const string Danger = "btn btn-danger";
    }

    public class RoleConst
    {
        //STU Admin
        public const int STU = 2;

        //FD - Finance Department
        public const int FD = 31;
        
        //EE - Executive Engineer
        public const int EE = 51;
        
        //SE - Superintending Engineer
        public const int SE = 52;
        
        //CE - Chief Engineer
        public const int CE = 53;

        //DirOP - Director of Operations
        public const int DirOP = 54;

        //CMD
        public const int CMD = 55;
    }
}