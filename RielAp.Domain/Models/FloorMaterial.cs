using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;

namespace RielAp.Domain.Models
{
    /*public class FloorMaterial : IModel
    {
        public FloorMaterial()
        {
            this.Announcements = new List<LivingAnnouncement>();
        }

        public int FloorMaterialID { get; set; }
        public string FloorMaterialName { get; set; }
        public virtual ICollection<LivingAnnouncement> Announcements { get; set; }
    }*/
    public enum FloorMaterial {
        [LocalizationText("FloorMaterialUnknownLabel", "UnimportantLabel")]
        Unknown,
        [LocalizationText("FloorMaterialZalizoBetonLabel")]
        ZalizoBeton,
        [LocalizationText("FloorMaterialDerevoLabel")]
        Derevo,
        [LocalizationText("FloorMaterialZalizoBetonIDerevoLabel")]
        ZalizoBetonIDerevo
    }
}
