using RielAp.Domain.Managers;
using RielAp.Domain.Models;
using RielAp.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Models
{
    public class EnumsToSelectedListItems
    {
        public static string GetTextFromEnumValue<TEnum>(TEnum value, bool getAdditional = false)
        {
            string text = value.ToString();

            var type = typeof(TEnum);
            MemberInfo[] memInfos = type.GetMember(value.ToString());
            if ((memInfos != null) && (memInfos.Length > 0))
            {
                var attributes = memInfos[0].GetCustomAttributes(typeof(LocalizationTextAttribute), false);
                if ((attributes != null) && (attributes.Count() > 0))
                {
                    if (getAdditional)
                    {
                        text = ((LocalizationTextAttribute)attributes[0]).GetAdditionalText();
                    }
                    else {
                        text = ((LocalizationTextAttribute)attributes[0]).GetText();
                    }
                }
            }
            return text;
        }

        private static IEnumerable<SelectListItem> GetItemsForDropDown<TValue>(IEnumerable<TValue> values, bool getAdditional = false)
        {
            IEnumerable<SelectListItem> currencies =
                from value in values
                select new SelectListItem
                {
                    Text = GetTextFromEnumValue(value, getAdditional),
                    Value = value.ToString()
                };
            return currencies;
        }


        public static IEnumerable<SelectListItem> GetSquareUnitsList( bool getAdditional = false)
        {
            IEnumerable<SquareUnit> units = Enum.GetValues(typeof(SquareUnit)).Cast<SquareUnit>();
            return GetItemsForDropDown(units, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetRoomsNumbersList(int maxRoomNumber, bool getAdditional = false)
        {
            List<int> rooms = new List<int>();
            for (int roomsNumber = 0; roomsNumber <= maxRoomNumber; roomsNumber++)
            {
                rooms.Add(roomsNumber);
            }
            IEnumerable<SelectListItem> roomsNumbers = from value in rooms
                                                       select new SelectListItem
                                                       {
                                                           Text = value < 1 ? Translation.Translation.UnimportantLabel : value.ToString(),
                                                           Value = value.ToString()
                                                       };
            return roomsNumbers;
        }

        public static IEnumerable<SelectListItem> GetCurrenciesList(bool getAdditional = false)
        {
            IEnumerable<Currency> units = Enum.GetValues(typeof(Currency)).Cast<Currency>();
            return GetItemsForDropDown(units, getAdditional);
        }

        
        public static IEnumerable<SelectListItem> GetAnnouncementTypesList(bool getAdditional = false)
        {
            IEnumerable<AnnouncementType> values = Enum.GetValues(typeof(AnnouncementType)).Cast<AnnouncementType>().OrderBy(at => (int)at);
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetWallMaterialsList(bool getAdditional = false)
        {
            IEnumerable<WallMaterial> values = Enum.GetValues(typeof(WallMaterial)).Cast<WallMaterial>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetFloorMaterialsList(bool getAdditional = false)
        {
            IEnumerable<FloorMaterial> values = Enum.GetValues(typeof(FloorMaterial)).Cast<FloorMaterial>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetHeatingTypeList(bool getAdditional = false)
        {
            IEnumerable<HeatingType> values = Enum.GetValues(typeof(HeatingType)).Cast<HeatingType>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetHotWaterTypeList(bool getAdditional = false)
        {
            IEnumerable<HotWaterType> values = Enum.GetValues(typeof(HotWaterType)).Cast<HotWaterType>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetEntranceInList(bool getAdditional = false)
        {
            IEnumerable<EntranceIn> values = Enum.GetValues(typeof(EntranceIn)).Cast<EntranceIn>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetEntranceFromList(bool getAdditional = false)
        {
            IEnumerable<EntranceFrom> values = Enum.GetValues(typeof(EntranceFrom)).Cast<EntranceFrom>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetAddressTypeList(bool getAdditional = false)
        {
            IEnumerable<AddressType> values = Enum.GetValues(typeof(AddressType)).Cast<AddressType>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetPresentTypeValueList(bool getAdditional = false)
        {
            IEnumerable<PresentTypeValue> values = Enum.GetValues(typeof(PresentTypeValue)).Cast<PresentTypeValue>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetAddressTypes(bool getAdditional = false)
        {
            IEnumerable<AddressType> values = Enum.GetValues(typeof(AddressType)).Cast<AddressType>();
            return GetItemsForDropDown(values, getAdditional);
        }

        public static IEnumerable<SelectListItem> GetLandApplyTypeList(bool getAdditional = false)
        {
            IEnumerable<LandApplyType> values = Enum.GetValues(typeof(LandApplyType)).Cast<LandApplyType>();
            return GetItemsForDropDown(values, getAdditional);
        }


        public static IEnumerable<SelectListItem> GetTimePeriodsList(bool getAdditional = false)
        {
            IEnumerable<TimePeriods> units = Enum.GetValues(typeof(TimePeriods)).Cast<TimePeriods>();
            return GetItemsForDropDown(units, getAdditional);
        }
        
    }
}