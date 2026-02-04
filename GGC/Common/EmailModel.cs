using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public class EmailModel
    {
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string MailAttachment { get; set; }
        
    }
}