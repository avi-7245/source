using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;
using log4net;
using System.Reflection;


namespace GGC.Common
{
    public class SMS
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="mobileNumbers">MethodBase.GetCurrentMethod()</param>
        /// <param name="method"></param>
        /// <param name="log"></param>
        public static void Send(string message, string mobileNumbers, MethodBase method, ILog log)
        {
            if (string.IsNullOrEmpty(mobileNumbers))
            {
                return;
            }

            var queryParams = new Dictionary<string, string>{
                { "SenderId", ConfigurationManager.AppSettings["SMSSENDER"] },
                { "Message", Uri.EscapeDataString(message) },
                { "MobileNumbers", mobileNumbers },
                { "ApiKey", ConfigurationManager.AppSettings["SMSAPIKEY"] },
                { "ClientId", ConfigurationManager.AppSettings["SMSCLIENTID"] }
            };
            var strURL = ConfigurationManager.AppSettings["SMSURL"] + "?" + string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"));

            try
            {
                log.Error("INFO :: " + strURL);
                var request = WebRequest.Create(strURL);
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var readStream = new StreamReader(stream))
                        {
                            var dataString = readStream.ReadToEnd();
                            log.Error("INFO :: " + dataString);
                        }
                    }
                }
                //RemoveComment
            }
            catch (Exception ex)    
            {
                string ErrorMessage = "Method Name: " + method.Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
            }
        }

        public void send()
        {
            //try
            //{
            //    //                                string userAuthenticationURI = "http://regrid.mahadiscom.in/reGrid/getProjectDtls?projid=" + txtProjCode + "&entity=MSETCL&secKey=" + str;
            //    string userAuthenticationURI = "https://regrid.mahadiscom.in/reGrid/saveToMedaGcAppln";

            //    //WebRequest req = WebRequest.Create(@userAuthenticationURI);
            //    var req = (HttpWebRequest)WebRequest.Create(@userAuthenticationURI);


            //    req.Method = "POST";
            //    req.ContentType = "application/json";

            //    req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("mahatransco:Ashish@1234"));
            //    //req.Credentials = new NetworkCredential("username", "password");
            //    using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            //    {
            //        string json = JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None);

            //        streamWriter.Write(JsonConvert.SerializeObject(objGetStatusDTO, Formatting.None));
            //    }
            //    HttpWebResponse resp = req.GetResponse() as HttpWebResponse;

            //    StreamReader reader = new StreamReader(resp.GetResponseStream());

            //    string responseText = reader.ReadToEnd();
            //}
            //catch (Exception ex)
            //{
            //    string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
            //    log.Error(ErrorMessage);
            //}
        }
    }

    public static class SMSTemplates
    {
        /// <summary>
        /// Send Login Otp To User. 
        /// </summary>
        /// <param name="otp"></param>
        /// <returns></returns>
        public static string SmsOtp(string otp)
        {
            return $"Dear User, \nYour One Time Password (OTP) for GC login is {otp}.\nPlease do not share this OTP.\n\nRegards,\nCE STU, MSETCL";
        }

        /// <summary>
        /// Initial Processing Fee paid by SPV. 
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string ProcessingFeePaidForDeemGC(string applicationNo, string spvName)
        {
            return $"Payment towards Initial Processing fee of Rs.100/- against online Application No.{applicationNo} by M/s.{spvName} has been received.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// Finance section has approved the Initial Processing Fee payment
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string ProcessingApprovedForDeemGC(string applicationNo, string spvName)
        {
            return $"Payment towards initial Processing fee against online Application No.{applicationNo} by M/s.{spvName} has been approved.\n\nRegards, STU, MSETCL";
        }


        /// <summary>
        /// STU Admin has checked annexure A & Uploaded LFS. Deemed GC created. EE, SE & CE STU has verified application. Deemed GC approved 
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string DeemedGCApproved(string applicationNo, string spvName)
        {
            return $"Deemed Grid Connectivity against Online Application No.{applicationNo} by M/s.{spvName} has been approved.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// If returned by STU Admin/EE/SE/CE
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string DeemedGCReturned(string applicationNo, string spvName)
        {
            return $"Deemed Grid Connectivity proposal against Online Application No.{applicationNo} by M/s.{spvName} has been returned by CE-STU for compliance.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// Processing Fee paid by SPV.  
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string ProcessingFeePaidForGC(string applicationNo, string spvName)
        {
            return $"Payment towards Processing fee of Rs. 99,900/- against online Application No.{applicationNo} by M/s.{spvName} has been received.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// Finance section has approved the Processing Fee payment
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string ProcessingFeeApprovedForGC(string applicationNo, string spvName)
        {
            return $"Payment towards Processing fee against online Application No.{applicationNo} by M/s.{spvName} has been approved.\n\nRegards, \nSTU, MSETCL";
        }

        /// <summary>
        /// STU Admin has checked annexure B, PPA & other documents. GC created. EE, SE & CE STU has verified Documents. Deemed GC approved.
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string GCApproved(string applicationNo, string spvName)
        {
            return $"Grid Connectivity against Online Application No.{applicationNo} by M/s.{spvName} has been approved.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// If returned by STU Admin/EE/SE/CE
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string GCReturned(string applicationNo, string spvName)
        {
            return $"Grid Connectivity proposal against Online Application No.{applicationNo} by M/s.{spvName} has been returned by CE (STU) for compliance.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// Deemed Grid Connectivity Updated By SPV
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string DeemedGCUpdated(string applicationNo, string spvName)
        {
            return $"Deemed Grid Connectivity proposal against Online Application No. {applicationNo} by M/s {spvName} has been Updated.\n\nRegards, STU, MSETCL";
        }

        /// <summary>
        /// Grid Connectivity Updated By SPV
        /// </summary>
        /// <param name="applicationNo"></param>
        /// <param name="spvName"></param>
        /// <returns></returns>
        public static string GCUpdated(string applicationNo, string spvName)
        {
            return $"Grid Connectivity proposal against Online Application No. {applicationNo} by M/s {spvName} has been Updated.\n\nRegards, STU, MSETCL";
        }
    }
}