using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public enum HeatingType:byte
    {
        [LocalizationText("HeatingTypeUnknownLabel", "UnimportantLabel")]
        Unknown = 0,
        [LocalizationText("HeatingTypeGasBoilerLabel")]
        GasBoiler,
        [LocalizationText("HeatingTypeCentralLabel")]
        Central,
        [LocalizationText("HeatingTypeStoveLabel")]
        Stove
    }
}
