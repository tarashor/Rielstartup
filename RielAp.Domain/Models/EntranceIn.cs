using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public enum EntranceIn:byte
    {
        [LocalizationText("EntranceInUnknownLabel", "UnimportantLabel")]
        Unknown = 0,
        [LocalizationText("EntranceInCorridorLabel")]
        Corridor,
        [LocalizationText("EntranceInKitchenLabel")]
        Kitchen
    }
}
