using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jose;
using System.Configuration;
using LitJson;

namespace GGC.Common
{
    public class BillDeskAPI
    {

        void req()
        {
            var payload = new
            {
                mercid = ConfigurationManager.ConnectionStrings["mercId"].ConnectionString,
                orderid = "ORD" + DateTimeOffset.Now.Millisecond.ToString(),
                amount = "300.00",
                order_date = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                currency = "356",
                ru = "https://merchant.com",
                additional_info = new
                {
                    additional_info1 = "Details1",
                    additional_info2 = "Details2"
                },
                itemcode = "DIRECT",
                device = new
                {
                    init_channel = "internet",
                    ip = "17.233.107.92",
                    mac = "11-AC-58-21-1B-AA",
                    imei = "990000112233445",
                    user_agent = "Mozilla/5.0",
                    accept_header = "text/html",
                    fingerprintid = "61b12c18b5d0cf901be34a23ca64bb19"
                }
            };
        }
    }
}