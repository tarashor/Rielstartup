using RielAp.Web.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RielAp.Web.Models
{
    public class HousesViewModel : AnnouncementsViewModel
    {
        public HouseFilterCriteria FilterCriteria { get; set; }
    }
}
