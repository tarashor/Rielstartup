using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class Profile:IModel
    {
        public Profile() {
            this.Users = new List<User>();
        }

        public int ProfileId { get; set; }
        public string ProfileName { get; set; }
        public int MaxAnnouncements { get; set; }
        public int MaxPhotosPerAnnouncements { get; set; }
        public bool IsBasic { get; set; }
        public int Priority { get; set; }
        public decimal Price { get; set; }

        public ICollection<User> Users { get;set; }
    }
}
