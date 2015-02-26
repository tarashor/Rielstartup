using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RielAp.Domain.Models;
using RielAp.Web.Models.Filter;

namespace RielAp.Web.Models
{
    public class ApartmentsViewModel:AnnouncementsViewModel
    {
        public ApartmentFilterCriteria FilterCriteria { get; set; }
    }
}