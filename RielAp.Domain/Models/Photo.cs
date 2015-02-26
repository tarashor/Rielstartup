using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class Photo:IModel
    {
        public Photo() {
            IsVisible = true;
            Created = DateTime.Now;
        }

        public int PhotoId { get; set; }
        public string Url { get; set; }
        public DateTime Created { get; set; }
        public bool IsVisible { get; set; }

        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }
        
        public virtual Announcement Announcement { get; set; }
        public int? AnnouncementId { get; set; }

        public bool IsMain { get; set; }
    }
}
