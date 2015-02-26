using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Utils
{
    public enum AnnouncementOrder
    {
        [LocalizationText("AnnouncementOrderAddressLabel")]
        Address,
        [LocalizationText("AnnouncementOrderAddressDescendingLabel")]
        AddressDescending,
        [LocalizationText("AnnouncementOrderDateLabel")]
        Date,
        [LocalizationText("AnnouncementOrderDateDescendingLabel")]
        DateDescending,
    }
}