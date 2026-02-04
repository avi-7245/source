using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using GGC.Scheduler;
using MySql.Data.MySqlClient;

namespace GGC.Common
{
    public class BasePage : System.Web.UI.Page
    {
        string _conString = ConfigurationManager.AppSettings["DevpGridConnectivity"].ToString();

        protected IEnumerable<int> GetReturnRoleByCurrentUser(bool isApproved = true)
        {
            var roleId = int.Parse(Session[SessionConst.EmpRole].ToString());

            var roleIds = new List<int>();

            if (roleId == RoleConst.STU)
            {
                roleIds.Add(RoleConst.STU);
            }
            else if (roleId == RoleConst.FD)
            {
                roleIds.Add(RoleConst.STU);
                roleIds.Add(RoleConst.FD);
                if (isApproved)
                {
                    roleIds.Add(RoleConst.EE);
                }
            }
            else if (roleId == RoleConst.EE)
            {
                roleIds.Add(RoleConst.STU);
                roleIds.Add(RoleConst.EE);
            }
            else if (roleId == RoleConst.SE)
            {
                roleIds.Add(RoleConst.STU);
                roleIds.Add(RoleConst.EE);
                roleIds.Add(RoleConst.SE);
            }
            else if (roleId == RoleConst.CE)
            {
                roleIds.Add(RoleConst.STU);
                roleIds.Add(RoleConst.EE);
                roleIds.Add(RoleConst.SE);
                roleIds.Add(RoleConst.CE);
            }
            return roleIds;
        }

        protected List<string> GetMobileNumbers(DataSet empDataSet)
        {

            var mobileNumbers = new List<string>();
            mobileNumbers.AddRange(empDataSet.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpMobile"].ToString())).Select(a => a["EmpMobile"].ToString().Trim()).Distinct());
            return mobileNumbers;
        }

        protected List<string> GetEmailIds(DataSet empDataSet)
        {
            var emailIds = new List<string>();
            emailIds.AddRange(empDataSet.Tables[0].AsEnumerable().Where(a => !string.IsNullOrEmpty(a["EmpEmailID"].ToString())).Select(a => a["EmpEmailID"].ToString().Trim()).Distinct());
            return emailIds;
        }

        protected NotificationData GetNotificationData(bool isApproved = true)
        {
            var nd = new NotificationData();

            var roleIds = GetReturnRoleByCurrentUser(isApproved);

            if (roleIds.Any())
            {

                string query = $"SELECT e.SRNO, e.SAPID, e.EMP_NAME, e.EmpEmailID, e.EmpMobile, e.ROLE_ID FROM empmaster e WHERE e.ROLE_ID IN ({string.Join(",", roleIds)})";

                DataSet empDataSet = SQLHelper.ExecuteDataset(_conString, CommandType.Text, query);

                nd.MobileNumbers = GetMobileNumbers(empDataSet);
                nd.EmailIds = GetEmailIds(empDataSet);
            }
            return nd;
        }

        protected string GetAttachmentFilePath(string strAppID)
        {
            using (var mySqlConnection = new MySqlConnection(_conString))
            {
                mySqlConnection.Open();
                var strQuery = "SELECT FieName FROM mskvy_upload_doc_spd WHERE Application_No = @APPLICATION_NO AND FileType=3;";
                using (var cmd = new MySqlCommand(strQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@APPLICATION_NO", strAppID);
                    string serverFilePath = Server.MapPath("~/Files/MSKVY/" + strAppID + "/" + cmd.ExecuteScalar().ToString()); ;
                    return serverFilePath;
                }
            }
        }

        protected string GetProjectDistrictEmail(string distname)
        {
            using (var mySqlConnection = new MySqlConnection(_conString))
            {
                mySqlConnection.Open();
                var strQuery = "SELECT cmz.zone_email FROM zone_circle_district_map zcdm, corresponding_msedcl_zone cmz WHERE LOWER(zcdm.zone_name) LIKE CONCAT('%', LOWER(cmz.zone_name), '%') AND LOWER(zcdm.dist_name) = LOWER(@DIST_NAME) LIMIT 1;";
                using (var cmd = new MySqlCommand(strQuery, mySqlConnection))
                {
                    cmd.Parameters.AddWithValue("@DIST_NAME", distname);
                    return cmd.ExecuteScalar().ToString(); ;
                }
            }
        }
    }
}