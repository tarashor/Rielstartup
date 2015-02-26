using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class CabinetModel
    {
        public IEnumerable<Announcement> Announcements { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public string UserPhone { get; set; }
    }
}