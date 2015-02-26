using RielAp.Domain.Managers;
using RielAp.Domain.Models;
using RielAp.Web.Models.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Models
{
    public class ApartmentFilterCriteriaViewModel : FilterCriteriaViewModel
    {
        private const int MaxRoomNumber = 4;

        public ApartmentFilterCriteriaViewModel(ApartmentFilterCriteria filter):base()
        {
            Filter = filter;
            
            RoomsNumbers = EnumsToSelectedListItems.GetRoomsNumbersList(MaxRoomNumber);
            HeatingTypes = EnumsToSelectedListItems.GetHeatingTypeList(true);
            HotWaterTypes = EnumsToSelectedListItems.GetHotWaterTypeList(true);
            PlacesForCar = EnumsToSelectedListItems.GetPresentTypeValueList(true);
            BalconyAvailable = EnumsToSelectedListItems.GetPresentTypeValueList(true);
            EntranceInTypes = EnumsToSelectedListItems.GetEntranceInList(true);
            EntranceFromTypes = EnumsToSelectedListItems.GetEntranceFromList(true);


        }
        public ApartmentFilterCriteria Filter { get; set; }

        

        public IEnumerable<SelectListItem> RoomsNumbers { get; private set; }
        
        public IEnumerable<SelectListItem> HeatingTypes { get; private set; }
        public IEnumerable<SelectListItem> HotWaterTypes { get; private set; }
        public IEnumerable<SelectListItem> PlacesForCar { get; private set; }
        public IEnumerable<SelectListItem> BalconyAvailable { get; private set; }
        public IEnumerable<SelectListItem> EntranceInTypes { get; private set; }
        public IEnumerable<SelectListItem> EntranceFromTypes { get; private set; }

        
    }
}