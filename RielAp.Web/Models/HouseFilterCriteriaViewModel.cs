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
    public class HouseFilterCriteriaViewModel:FilterCriteriaViewModel
    {

        public HouseFilterCriteriaViewModel(HouseFilterCriteria filter)
        {
            Filter = filter;

        }
        public HouseFilterCriteria Filter { get; set; }
        
    }
}