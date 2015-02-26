using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class Statistic : IModel
    {
        public Statistic()
        {
            Date = DateTime.Today;
            Type = AnnouncementType.Buy;
            IsOld = true;
        }
        public int StatisticID { get; set; }

        public string City { get; set; }
        public string District { get; set; }
        public int RoomsCount { get; set; }

        public AnnouncementType Type { get; set; }
        
        public decimal PricePerMeter { get; set; }
        public Currency Currency { get; set; }
        
        public DateTime Date { get; set; }

        public bool IsOld { get; set; }
        
    }
}
