using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Utils
{
    public class DateTimeParser
    {
        public static DateTime? ParseDate(string stringDate)
        {
            DateTime? result = null;
            char dateSeparator = '.';
            if (!string.IsNullOrEmpty(stringDate))
            {
                string[] values = stringDate.Split(dateSeparator);
                if (values.Length == 3)
                {
                    int day;
                    int month;
                    int year;
                    if ((int.TryParse(values[0], out day)) && (int.TryParse(values[1], out month)) && (int.TryParse(values[2], out year)))
                    {
                        if ((month < 13) && (month > 0))
                        {
                            int maxDays = DateTime.DaysInMonth(year, month);
                            if ((day > 0) && (day <= maxDays))
                            {
                                result = new DateTime(year, month, day);
                            }
                        }
                    }

                }
            }
            return result;
        }

        public static bool isToday(DateTime date)
        {
            DateTime today = DateTime.Today;
            return ((Math.Abs(date.Day - today.Day) + Math.Abs(date.Month - today.Month) + Math.Abs(date.Year - today.Year)) == 0);
        }
    }
}