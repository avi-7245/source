using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GGC.Common
{
    public static class ListExtensions
    {
        public static string JoinStrings(this List<string> strings, string separator = ",")
        {
            return string.Join(separator, strings);
        }
    }
}