using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public enum TimePeriods
    {
        [LocalizationText("TimePeriodLastMonth")]
        LastMonth,
        [LocalizationText("TimePeriodLast2Months")]
        Last2Months,
        [LocalizationText("TimePeriodAllTime")]
        AllPeriod
    }

    public static class TimePeriodsUtil
    {
        public static DateTime GetStartDate(TimePeriods period) { 
            DateTime res = GetEndDate(period);
            switch (period) { 
                case TimePeriods.LastMonth:
                    res = res.AddMonths(-1);
                    break;
                case TimePeriods.Last2Months:
                    res = res.AddMonths(-2);
                    break;
                case TimePeriods.AllPeriod:
                    res = res.AddYears(-5);
                    break;
            }
            return res;
        }
        public static DateTime GetEndDate(TimePeriods period)
        {
            return DateTime.Now;
        }
    }
}