using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class UpdateLimitModel
    {
        public IEnumerable<Profile> Profiles { get; set; }
        public User CurrentUser { get; set; }
    }
}