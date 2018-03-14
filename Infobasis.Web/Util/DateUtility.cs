using System;
using System.Collections.Generic;
using System.Web;
using Infobasis.Web;

namespace Infobasis.Web.Util
{
    public class DateUtility
    {
        public static DateTime Parse(string dateString)
        {
            DateTime date;
            System.Globalization.CultureInfo parseCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            string parseDateFormat = Global.DateFormat;

            if (!DateTime.TryParseExact(dateString, parseDateFormat, parseCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out date))
            {
                DateTime.TryParse(dateString, parseCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out date);
            }

            return date;
        }
    }
}
