using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;

namespace RielAp.Domain.Models
{
    /*public class WallMaterial : IModel
    {
        public WallMaterial()
        {
            this.Apartments = new List<LivingAnnouncement>();
        }

        public int WallMaterialID { get; set; }
        public string WallMaterialName { get; set; }
        public virtual ICollection<LivingAnnouncement> Apartments { get; set; }
    }*/
    public enum WallMaterial 
    {
        [LocalizationText("WallMaterialUnknownLabel", "UnimportantLabel")]        
        Unknown,
        [LocalizationText("WallMaterialPanelLabel")]
        Panel,
        [LocalizationText("WallMaterialCeglaLabel")]
        Cegla,
        [LocalizationText("WallMaterialGasBetonLabel")]
        GasBeton
    }
}
