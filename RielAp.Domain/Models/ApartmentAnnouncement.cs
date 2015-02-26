using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public class ApartmentAnnouncement:LivingAnnouncement
    {
        public int Floor { get; set; }
        public int MaxFloor { get; set; }

        public HeatingType HeatingType { get; set; }
        public HotWaterType HotWaterType { get; set; }
        public PresentTypeValue PlaceForCar { get; set; }
        public PresentTypeValue Balcony { get; set; }
        public EntranceFrom EntranceFrom { get; set; }
        public EntranceIn EntranceIn { get; set; }
    }
}
