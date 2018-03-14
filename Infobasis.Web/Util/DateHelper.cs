using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infobasis.Web.Util
{
    public class DateHelper
    {
        private DateHelper() { }

        public static int GetTotalDays(DateTime date1, DateTime date2)
        {
            TimeSpan ts = date2.Subtract(date1);
            int totalDays = ts.Days;

            return totalDays;
        }

        public static int GetClientTraceDays(DateTime? date1, DateTime date2)
        {
            if (!date1.HasValue || date1.Value == DateTime.MinValue)
                return 0;

            TimeSpan ts = date2.Subtract(date1.Value);
            double diffDays = ts.TotalDays;
            if (diffDays > 0 && diffDays <= 1)
                return 1;

            return (int)diffDays;
        }

        public static int GetWeekendDays(DateTime date1, DateTime date2)
        {
            int totalDays = GetTotalDays(date1, date2);
            int workDays = GetWorkDays(date1, date2);
            return totalDays - workDays;
        }

        public static int GetWorkDays(DateTime date1, DateTime date2)
        {
            int totalDays = GetTotalDays(date1, date2);
            int workDays = 0;
            for (int i = 0; i < totalDays; i++)
            {
                DateTime tempdt = date1.Date.AddDays(i);
                if (tempdt.DayOfWeek != System.DayOfWeek.Saturday && tempdt.DayOfWeek != System.DayOfWeek.Sunday)
                {
                    workDays++;
                }
            }

            return workDays;
        }

        public static int GetDiffMonths(DateTime startDate, DateTime endDate)
        {
            return endDate.Year * 12 + endDate.Month - startDate.Year * 12 - startDate.Month;
        }

    }
}