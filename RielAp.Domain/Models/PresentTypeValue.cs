using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Models
{
    public enum PresentTypeValue:byte
    {
        [LocalizationText("UnknownLabel", "UnimportantLabel")]
        Unknown = 0,
        [LocalizationText("PresentTypeValuePresentLabel")]
        Present,
        [LocalizationText("PresentTypeValueAbsentLabel")]
        Absent
        
    }
}
