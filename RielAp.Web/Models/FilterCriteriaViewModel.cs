using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace RielAp.Web.Models
{
    public class FilterCriteriaViewModel
    {
        public FilterCriteriaViewModel() {
            SquareUnits = EnumsToSelectedListItems.GetSquareUnitsList(true);
            Currencies = EnumsToSelectedListItems.GetCurrenciesList(true);
            AddressTypes = EnumsToSelectedListItems.GetAddressTypeList(true);
        }

        public IEnumerable<SelectListItem> AddressTypes { get; private set; }
        

        public IEnumerable<SelectListItem> SquareUnits { get; private set; }
        public IEnumerable<SelectListItem> Currencies { get; private set; }
    }
}
