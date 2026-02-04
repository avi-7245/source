using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using log4net;
using System.Reflection;
using System.Net;

namespace GGC.Common
{
    public class SendEmail
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(SendEmail));

        public void Send(string strTo,string strCC,string strSub,string strBody)
        {
            try
            {

                
                #region using MailMessage
                MailMessage Msg = new MailMessage();
                MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                Msg.From = fromMail;
                
                Msg.IsBodyHtml = true;
                string[] splittedTo = strTo.Split(';');
                
                
                //string[] splittedCC = strCC.Split(';');
                //Msg.To.Add(new MailAddress(strTo));
                //Msg.CC.Add(new MailAddress(strCC));
                //Msg.To.Add(new MailAddress(splittedTo[0].ToString()));
                //Msg.CC.Add(new MailAddress(splittedTo[1].ToString()));

                foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (part != "")
                        Msg.To.Add(new MailAddress(part.ToString()));
                    //Msg.To.Add(new MailAddress(part.ToString()));
                    log.Error("Part " + part);

                }
                if (strCC != "")
                {
                    string[] splittedCC = strCC.Split(',');
                    foreach (var part in strCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (part != "")
                            Msg.CC.Add(new MailAddress(part.ToString()));
                        //Msg.CC.Add(new MailAddress(part.ToString()));
                        log.Error("Part " + part);
                    }
                }
                Msg.Bcc.Add("progit4000@mahatransco.in");
                //  Msg.To.Add(new MailAddress(toAddress));

                Msg.Subject = strSub;
                Msg.Body = strBody;
                //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                a.EnableSsl = true;
                a.Port = 587;
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                a.Credentials = new NetworkCredential("donotreply@mahatransco.in", "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~");
                a.Send(Msg);

                Msg = null;
                fromMail = null;
                a = null;
                #endregion
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
                // throw ex;
            }
        }
        public void SendAttachment(string strTo, string strCC, string strSub, string strBody,string strAttach)
        {
            try
            {


                #region using MailMessage
                MailMessage Msg = new MailMessage();
                MailAddress fromMail = new MailAddress("donotreply@mahatransco.in");
                Msg.From = fromMail;

                Msg.IsBodyHtml = true;
                string[] splittedTo = strTo.Split(';');


                //string[] splittedCC = strCC.Split(';');
                //Msg.To.Add(new MailAddress(strTo));
                //Msg.CC.Add(new MailAddress(strCC));
                //Msg.To.Add(new MailAddress(splittedTo[0].ToString()));
                //Msg.CC.Add(new MailAddress(splittedTo[1].ToString()));

                foreach (var part in strTo.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (part != "")
                        Msg.To.Add(new MailAddress(part.ToString()));
                    //Msg.To.Add(new MailAddress(part.ToString()));
                    log.Error("Part " + part);

                }
                if (strCC != "")
                {
                    string[] splittedCC = strCC.Split(',');
                    foreach (var part in strCC.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (part != "")
                            Msg.CC.Add(new MailAddress(part.ToString()));
                        //Msg.CC.Add(new MailAddress(part.ToString()));
                        log.Error("Part " + part);
                    }
                }

                //  Msg.To.Add(new MailAddress(toAddress));

                Msg.Subject = strSub;
                Msg.Body = strBody;
                //SmtpClient a = new SmtpClient("mail.mahatransco.in");
                SmtpClient a = new SmtpClient(ConfigurationManager.AppSettings["EmailServer"].ToString());
                a.EnableSsl = true;
                a.Port = 587;
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;// SecurityProtocolType.Tls; // can you check now
                a.Credentials = new NetworkCredential("donotreply@mahatransco.in", "#6_!M0,p.9uV,2q8roMWg#9Xn'Ux;nK~");
                System.Net.Mail.Attachment attachment;

                attachment = new System.Net.Mail.Attachment(strAttach);
                a.Send(Msg);

                Msg = null;
                fromMail = null;
                a = null;
                #endregion
            }
            catch (Exception ex)
            {
                string ErrorMessage = "Method Name: " + MethodBase.GetCurrentMethod().Name + " | Description: " + ex.Message + " " + ex.InnerException;
                log.Error(ErrorMessage);
                // throw ex;
            }
        }
    }
}