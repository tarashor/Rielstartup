using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public enum LandApplyType:int
    {
        [LocalizationText("LandApplyTypeUnknownLabel", "UnimportantLabel")]
        Unknown,
        [LocalizationText("LandApplyTypeSummerHouseLabel")]
        SummerHouse,
        [LocalizationText("LandApplyTypeForBuildingLabel")]
        ForBuilding
    }
}
