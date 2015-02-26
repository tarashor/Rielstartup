using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public abstract class LivingAnnouncement:Announcement
    {
        public LivingAnnouncement() {
            LivingSquare = new Square();
        }
        public int RoomsNumber { get; set; }
        public Square LivingSquare { get; set; }

        public WallMaterial WallMaterial { get; set; }
        public FloorMaterial FloorMaterial { get; set; }
    }
}
