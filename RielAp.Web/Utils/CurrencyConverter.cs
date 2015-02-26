using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RielAp.Domain.Models;

namespace RielAp.Web.Utils
{
    public static class CurrencyConverter
    {
        private static decimal kurs = 12M;
        public static decimal ConvertToCurrency(this Announcement announcement, Currency currency)
        {
            decimal res = announcement.Price;
            if (announcement.Currency != currency)
            {
                if (currency == Currency.Dollar)
                {
                    res = announcement.Price / kurs;
                }
                else 
                {
                    res = announcement.Price * kurs;
                }
            }

            res = Math.Round(res, 2);
            return res;
        }
    }
}