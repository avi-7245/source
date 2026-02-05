using System;
using System.Security.Cryptography;
using System.Text;
using log4net;


namespace GGC.Common
{
    public class APICall
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(APICall));
        public string hmacSHA256Checksum(String key, String data)
        {
            //string str = string.Empty;
            //byte[] secretkey;
            //byte[] byteData ;
            //byte[] byteData1;
            UTF8Encoding encoder = new UTF8Encoding();

            byte[] hashValue;
            byte[] keybyt = encoder.GetBytes(key);
            byte[] message = encoder.GetBytes(data);

            HMACSHA256 hashString = new HMACSHA256(keybyt);
            string hex = "";
            try
            {

                ////RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                //secretkey = Encoding.UTF8.GetBytes(key);
                //byteData = Encoding.UTF8.GetBytes(data);
                //HMACSHA256 hmac = new HMACSHA256(secretkey);
                //str= Convert.ToBase64String(hmac.ComputeHash(byteData));
                ////rng.GetBytes(secretkey);
                ////str = Encoding.ASCII.GetString(byteData1);  



                hashValue = hashString.ComputeHash(message);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                //                return hex;
            }
            catch (Exception ex)
            {
                // string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + mySqlException.Message + " " + mySqlException.InnerException;
                //log.Error(ErrorMessage);
            }
            return hex;
        }
    }
}