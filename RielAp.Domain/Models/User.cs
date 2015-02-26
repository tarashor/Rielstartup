using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RielAp.Domain.Models
{
    public class User : IModel
    {
        public User()
        {
            this.Apartments = new List<Announcement>();
            this.Photos = new List<Photo>();
            this.Feedbacks = new List<Feedback>();
            this.AdditionalNumbers = new List<MobileNumber>();
            this.ReceiveNewAnnouncements = false;
            this.IsConfirmed = false;
        }

        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TemporaryPassword { get; set; }
        public DateTime? TemporaryPasswordExpires { get; set; }

        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public bool ReceiveNewAnnouncements { get; set; }
        public DateTime? RegisterDate { get; set; }

        public bool IsConfirmed { get; set; }
        public Guid ConfirmationCode { get; set; }

        [ForeignKey("Role")]
        public string RoleName { get; set; }
        public virtual Role Role { get; set; }
        
        [ForeignKey("Profile")]
        public int ProfileId { get; set; }
        public virtual Profile Profile { get; set; }

        public DateTime? ProfileExpires { get; set; }

        public virtual ICollection<Announcement> Apartments { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<MobileNumber> AdditionalNumbers { get; set; }

    }
}
