using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class AnnouncementsViewModel
    {
        public IEnumerable<Announcement> Announcements { get; set; }
    }
}