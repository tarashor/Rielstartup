using RielAp.Domain.Managers;
using RielAp.Domain.Models;
using RielAp.Web.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Models
{
    public class LandFilterCriteriaViewModel : FilterCriteriaViewModel
    {
        public LandFilterCriteriaViewModel(LandFilterCriteria filter)
        {
            Filter = filter;
            ApplyTypes = EnumsToSelectedListItems.GetLandApplyTypeList(true);
        }
        public LandFilterCriteria Filter { get; set; }
        public IEnumerable<SelectListItem> ApplyTypes { get; private set; }
    }
}