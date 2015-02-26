using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models.Filter
{
    [Serializable]
    public class ApartmentFilterCriteria : FilterCriteria<ApartmentAnnouncement>
    {
        public ApartmentFilterCriteria()
        {
            NotFirstFloor = false;
            NotLastFloor = false;
            RoomsCount = 0;
        }
        public int RoomsCount { get; set; }

        public HeatingType HeatingType { get; set; }
        public HotWaterType HotWaterType { get; set; }
        public PresentTypeValue PlaceForCar { get; set; }
        public PresentTypeValue Balcony { get; set; }
        public EntranceFrom EntranceFrom { get; set; }
        public EntranceIn EntranceIn { get; set; }

        public bool NotLastFloor { get; set; }
        public bool NotFirstFloor { get; set; }

        protected override bool IsAnnouncementValid(ApartmentAnnouncement announcement)
        {
        
            return (base.IsAnnouncementValid(announcement) && (RoomsCount == 0 || (announcement.RoomsNumber == RoomsCount))
                     && (!NotLastFloor || (announcement.Floor != announcement.MaxFloor))
                     && (!NotFirstFloor || (announcement.Floor > 1))
                     && (HeatingType == HeatingType.Unknown || (announcement.HeatingType == HeatingType))
                     && (HotWaterType == HotWaterType.Unknown || (announcement.HotWaterType == HotWaterType))
                     && (PlaceForCar == PresentTypeValue.Unknown || announcement.PlaceForCar == PlaceForCar)
                     && (Balcony == PresentTypeValue.Unknown || announcement.Balcony == Balcony)
                     && (EntranceFrom == EntranceFrom.Unknown || (announcement.EntranceFrom == EntranceFrom))
                     && (EntranceIn == EntranceIn.Unknown || (announcement.EntranceIn == EntranceIn)));
        }

        public override bool IsNotEmpty()
        {
            return base.IsNotEmpty() || (RoomsCount > 0)  || EntranceFrom != EntranceFrom.Unknown || EntranceIn != EntranceIn.Unknown
                || Balcony != PresentTypeValue.Unknown || PlaceForCar != PresentTypeValue.Unknown || HeatingType != HeatingType.Unknown || HotWaterType != HotWaterType.Unknown || NotFirstFloor || NotLastFloor;
        }

    }
}