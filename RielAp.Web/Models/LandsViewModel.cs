using RielAp.Web.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RielAp.Web.Models
{
        public class LandsViewModel : AnnouncementsViewModel
        {
            public LandFilterCriteria FilterCriteria { get; set; }
        }
}
