using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class VashmagazinApartment: IModel
    {
        public VashmagazinApartment()
        {
            this.Address = new Address();
            Created = DateTime.Now;
            TotalSquare = new Square();
            Type = AnnouncementType.Buy;
            IsOld = false;
        }
        public int VashmagazinApartmentID { get; set; }
        public  Address Address { get; set; }

        public int RoomsCount { get; set; }
        public AnnouncementType Type { get; set; }
        public Square TotalSquare { get; set; }
        
        public decimal Price { get; set; }
        public Currency Currency { get; set; }
        
        public System.DateTime Created { get; set; }

        public string Phone { get; set; }

        public int Floor { get; set; }
        public int MaxFloor { get; set; }

        public bool IsOld { get; set; }
        
    }
}
