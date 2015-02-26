using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public enum EntranceFrom:byte
    {
        [LocalizationText("EntranceFromUnknownLabel", "UnimportantLabel")]
        Unknown = 0,
        [LocalizationText("EntranceFromStairCaseLabel")]
        StairCase, //Парадний
        [LocalizationText("EntranceFromBalconyLabel")]
        Balcony,
        [LocalizationText("EntranceFromYardLabel")]
        Yard
    }
}
