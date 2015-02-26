using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class Role:IModel
    {
        public Role() {
            Users = new List<User>();
        }

        [Key]
        public string RoleName { get; set; }
        public bool HasAdminAccess { get; set; }
        public bool HasBasicAccess { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
