using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;

namespace GGC.Common
{
    public static class StringExtenations
    {
        public static string MailtoTrimming(this string input)
        {
            if (input.StartsWith("mailto:"))
            {
                input = input.TrimStart("mailto:".ToCharArray());
            }
            return input;
        }
        
        public static string ToAddress(this string input, string separator = ",")
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }
            string[] parts = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(",<br />", parts);
        }
    }
}