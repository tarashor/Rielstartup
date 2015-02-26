using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Web.Models
{
    public class NoteViewModel
    {
        public IEnumerable<Announcement> Announcements { get; set; }
        public PageInfo Page { get; set; }
    }
}
