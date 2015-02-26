using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RielAp.Domain.Models;

namespace RielAp.Web.Utils
{
    public static class ApartmentHelper
    {
        private const string HryvnaString = "грн";
        private const string DollarString = "дол";

        public static string GetCurrency(this Announcement announcement)
        {
            string res = DollarString;
            switch (announcement.Currency)
            {
                case Currency.Hryvna:
                    {
                        res = HryvnaString;
                    }
                    break;
                case Currency.Dollar:
                    {
                        res = DollarString;
                    } 
                    break;
            }
            return res;
        }
    }
}