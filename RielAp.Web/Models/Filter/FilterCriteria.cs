using RielAp.Domain.Managers;
using RielAp.Domain.Models;
using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace RielAp.Web.Models.Filter
{
    [Serializable]
    public abstract class FilterCriteria<TAnnouncement> where TAnnouncement : Announcement
    {
        public FilterCriteria() {
            Currency = Domain.Models.Currency.Dollar;
            TotalSquareUnit = SquareUnit.SquareMeters;
            Address = new Address();
        }
        public decimal? StartPrice { get; set; }
        public decimal? EndPrice { get; set; }
        public Currency Currency { get; set; }

        public float? StartTotalSquare { get; set; }
        public float? EndTotalSquare { get; set; }
        public SquareUnit TotalSquareUnit { get; set; }

        public Address Address { get; private set; }

        public IEnumerable<TAnnouncement> Filter(IEnumerable<TAnnouncement> announcements)
        {
            if (IsNotEmpty())
            {
                return announcements.Where(an => (IsAnnouncementValid(an)));
            }
            else {
                return announcements;
            }
        }

        protected virtual bool IsAnnouncementValid(TAnnouncement announcement)
        {
            Square startTotalSquare = null;
            if (StartTotalSquare.HasValue)
            {
                startTotalSquare = new Square()
                {
                    Value = StartTotalSquare.Value,
                    Unit = TotalSquareUnit
                };
            }

            Square endTotalSquare = null;
            if (EndTotalSquare.HasValue)
            {
                endTotalSquare = new Square()
                {
                    Value = EndTotalSquare.Value,
                    Unit = TotalSquareUnit
                };
            }

            decimal priceInFilterCurrency = announcement.ConvertToCurrency(Currency);

            bool res = ((!EndPrice.HasValue || (priceInFilterCurrency < EndPrice.Value))
                     && (!StartPrice.HasValue || (priceInFilterCurrency > StartPrice.Value))
                     && (startTotalSquare == null || (announcement.TotalSquare.CompareTo(startTotalSquare) > 0))
                     && (endTotalSquare == null || (announcement.TotalSquare.CompareTo(endTotalSquare) < 0))
                     && (!announcement.Sold.HasValue || !announcement.Sold.Value)
                     && (Address.IsEmpty || announcement.Address.Equals(Address)));
            return res;
        }

        public virtual bool IsNotEmpty()
        {
            return StartPrice.HasValue || EndPrice.HasValue || StartTotalSquare.HasValue || EndTotalSquare.HasValue || !Address.IsEmpty;
        }

        public bool IsEmpty()
        {
            return !IsNotEmpty();
        }

    }
    

   
    
}