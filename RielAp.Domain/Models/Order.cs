using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class Order : IModel
    {
        public Guid OrderId { get; set; }
        public string Description { get; set; }
        public decimal Ammount { get; set; }
        public DateTime Created { get; set; }
        public string Status { get; set; }

        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }
}
