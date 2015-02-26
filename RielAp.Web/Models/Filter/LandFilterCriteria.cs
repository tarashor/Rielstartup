using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models.Filter
{
    [Serializable]
    public class LandFilterCriteria : FilterCriteria<LandAnnouncement>
    {
        public LandFilterCriteria()
        {
        }
        public LandApplyType ApplyType { get; set; }

        protected override bool IsAnnouncementValid(LandAnnouncement announcement)
        {
            return (base.IsAnnouncementValid(announcement)
                     && (ApplyType == LandApplyType.Unknown || (announcement.ApplyType != this.ApplyType)));
        }

        public override bool IsNotEmpty()
        {
            return base.IsNotEmpty() || (ApplyType != LandApplyType.Unknown);
        }
    }
}