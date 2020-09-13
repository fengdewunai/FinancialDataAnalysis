using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web.Common.Helper
{
    public static class CommonHelper
    {
        public static string GetFormatStr(this DateTime dateTime)
        {
            if(dateTime == default(DateTime))
            {
                return "";
            }
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}