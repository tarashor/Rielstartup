using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public enum HotWaterType:byte
    {
        [LocalizationText("HotWaterTypeUnknownLabel", "UnimportantLabel")]
        Unknown = 0,
        [LocalizationText("HotWaterTypeCentralLabel")]
        Central,
        [LocalizationText("HotWaterTypeGeyserLabel")]
        Geyser,
        [LocalizationText("HotWaterTypeWaterBoilerLabel")]
        WaterBoiler,
        [LocalizationText("HotWaterTypeBoilerLabel")]
        Boiler
    }
}
