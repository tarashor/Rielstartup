using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;

namespace RielAp.Domain.Models
{
    public enum AnnouncementType:int {
        [LocalizationText("RentLabel")]
        Rent,
        [LocalizationText("BuySellLabel")]
        Buy,
        [LocalizationText("RentShortLabel")]
        RentShort,
    }
}
        