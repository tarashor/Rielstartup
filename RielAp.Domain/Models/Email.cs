using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class Email : IModel
    {
        public Email() {
            Responses = new List<Email>();
        }

        public int Id { get; set; }
        public string Author { get; set; }
        public string AuthorEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsShown { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("ParentEmail")]
        public int? ParentEmailId { get; set; }
        public virtual Email ParentEmail { get; set; }

        public virtual ICollection<Email> Responses { get; set; }
    }
}
