using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RielAp.Domain.Models
{
    public abstract class Announcement : IModel
    {
        public Announcement()
        {
            this.Address = new Address();
            //this.Notes = new List<Note>();
            this.Photos = new List<Photo>();
            Created = DateTime.Now;
            Updated = DateTime.Now;
            TotalSquare = new Square();
            Type = AnnouncementType.Buy;
            IsVisible = true;
        }
        public int AnnouncementID { get; set; }
        public  Address Address { get; set; }

        public AnnouncementType Type { get; set; }
        public Square TotalSquare { get; set; }
        
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        public string Notice { get; set; }
        
        public System.DateTime Created { get; set; }
        public System.DateTime Updated { get; set; }

        public Nullable<bool> Sold { get; set; }

        public int UserID { get; set; }
        public virtual User User { get; set; }

        //public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

        public bool IsVisible { get; set; }
        
    }

    public enum Currency:int { 
        [LocalizationText("DollarLabel")]
        Dollar = 0,
        [LocalizationText("HryvnaLabel")]
        Hryvna = 1
    }
}
