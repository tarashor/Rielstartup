using System;
using System.Collections.Generic;

namespace RielAp.Domain.Models
{
    public class Note : IModel
    {
        public Note()
        {
            this.Announcements = new List<Announcement>();
            Created = DateTime.Now;
        }

        public int NoteId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public virtual ICollection<Announcement> Announcements { get; set; }
        public virtual User User { get; set; }
        public bool IsCurrent { get; set; }
    }
}
