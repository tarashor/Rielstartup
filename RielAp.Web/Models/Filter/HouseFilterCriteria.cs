using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models.Filter
{
    [Serializable]
    public class HouseFilterCriteria : FilterCriteria<HouseAnnouncement>
    {
        public HouseFilterCriteria()
        {
        }

        public float? StartLivingSquare { get; set; }
        public float? EndLivingSquare { get; set; }
        public SquareUnit LivingSquareUnit { get; set; }

        protected override bool IsAnnouncementValid(HouseAnnouncement announcement)
        {
            Square startLivingSquare = null;
            if (StartLivingSquare.HasValue)
            {
                startLivingSquare = new Square()
                {
                    Value = StartLivingSquare.Value,
                    Unit = LivingSquareUnit
                };
            }

            Square endLivingSquare = null;
            if (EndLivingSquare.HasValue)
            {
                endLivingSquare = new Square()
                {
                    Value = EndLivingSquare.Value,
                    Unit = LivingSquareUnit
                };
            }

            return (base.IsAnnouncementValid(announcement)
                     && (startLivingSquare == null || (announcement.TotalSquare.CompareTo(startLivingSquare) > 0))
                     && (endLivingSquare == null || (announcement.TotalSquare.CompareTo(endLivingSquare) < 0)));
        }

        public override bool IsNotEmpty()
        {
            return base.IsNotEmpty() || StartLivingSquare.HasValue || EndLivingSquare.HasValue;
        }

    }
}