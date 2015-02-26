using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Controllers
{
    public class AddressController : Controller
    {
        private IAnnouncementsRepository _announcementRepository;
        public AddressController(IAnnouncementsRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }


        #region AdministrativeArea
        [HttpPost]
        public JsonResult GetApartmentsAdministrativeAreas(AddressType addresstype)
        {
            IEnumerable<ApartmentAnnouncement> announcements = _announcementRepository.GetListOfApartments();
            return Json(GetAdministrativeAreas(announcements, addresstype));
        }

        [HttpPost]
        public JsonResult GetLandsAdministrativeAreas(AddressType addresstype)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfLands();
            return Json(GetAdministrativeAreas(announcements, addresstype));
        }

        [HttpPost]
        public JsonResult GetHousesAdministrativeAreas(AddressType addresstype)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfHouses();
            return Json(GetAdministrativeAreas(announcements, addresstype));
        }

        private AddressUnitsResults GetAdministrativeAreas(IEnumerable<Announcement> announcements, AddressType addresstype) {
            AddressUnitsResults res = new AddressUnitsResults();
            if (announcements != null)
            {
                IEnumerable<Announcement> filteredAnnouncements = announcements.Where(an => an.Address.AddressType == addresstype);
                res.Done = true;
                res.Items = _announcementRepository.GetDistinctAddressAdministrativeAreas(filteredAnnouncements);
            }
            return res;
        }
        #endregion

        #region SubAdministrativeArea
        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetApartmentsSubAdministrativeAreas(string adminArea)
        {
            IEnumerable<ApartmentAnnouncement> announcements = _announcementRepository.GetListOfApartments();
            adminArea = HttpUtility.HtmlDecode(adminArea);
            return Json(GetSubAdministrativeAreas(announcements, adminArea));
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetLandsSubAdministrativeAreas(string adminArea)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfLands();
            adminArea = HttpUtility.HtmlDecode(adminArea);
            return Json(GetSubAdministrativeAreas(announcements, adminArea));
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetHousesSubAdministrativeAreas(string adminArea)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfHouses();
            adminArea = HttpUtility.HtmlDecode(adminArea);
            return Json(GetSubAdministrativeAreas(announcements, adminArea));
        }

        private AddressUnitsResults GetSubAdministrativeAreas(IEnumerable<Announcement> announcements, string adminArea)
        {
            AddressUnitsResults res = new AddressUnitsResults();
            if (announcements != null && !string.IsNullOrEmpty(adminArea))
            {
                IEnumerable<Announcement> filteredAnnouncements = _announcementRepository.GetListOfApartments().Where(an => (an.Address.AddressType == AddressType.Villarge) && (an.Address.AdministrativeArea == adminArea));
                res.Done = true;
                res.Items = _announcementRepository.GetDistinctAddressSubAdministrativeAreas(filteredAnnouncements);
            }
            return res;
        }
        #endregion

        #region Locality
        
        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetApartmentsLocalities(AddressType addresstype, string adminArea, string subadminArea)
        {
            IEnumerable<ApartmentAnnouncement> announcements = _announcementRepository.GetListOfApartments();
            adminArea = HttpUtility.HtmlDecode(adminArea);
            subadminArea = HttpUtility.HtmlDecode(subadminArea);
            return Json(GetLocalities(announcements, addresstype, adminArea, subadminArea));
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetLandsLocalities(AddressType addresstype, string adminArea, string subadminArea)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfLands();
            adminArea = HttpUtility.HtmlDecode(adminArea);
            subadminArea = HttpUtility.HtmlDecode(subadminArea);
            return Json(GetLocalities(announcements, addresstype, adminArea, subadminArea));
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetHousesLocalities(AddressType addresstype, string adminArea, string subadminArea)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfHouses();
            adminArea = HttpUtility.HtmlDecode(adminArea);
            subadminArea = HttpUtility.HtmlDecode(subadminArea);
            return Json(GetLocalities(announcements, addresstype, adminArea, subadminArea));
        }

        private AddressUnitsResults GetLocalities(IEnumerable<Announcement> announcements, AddressType addresstype, string adminArea, string subadminArea)
        {
            AddressUnitsResults res = new AddressUnitsResults();
            IEnumerable<Announcement> filteredAnnouncements  =  announcements.Where(an => (an.Address.AddressType == addresstype));
            if (addresstype == AddressType.Villarge)
            {

                if (!string.IsNullOrEmpty(adminArea))
                {
                    filteredAnnouncements = filteredAnnouncements.Where(an => (an.Address.AdministrativeArea == adminArea));
                    if (!string.IsNullOrEmpty(subadminArea))
                    {
                        filteredAnnouncements = filteredAnnouncements.Where(an => (an.Address.SubAdministrativeArea == subadminArea));
                    }
                }
            }

            if (filteredAnnouncements != null)
            {
                res.Done = true;
                res.Items = _announcementRepository.GetDistinctLocalities(filteredAnnouncements);
            }
            return res;
        }
        #endregion

        #region Districts
        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetApartmentsDistricts(string locality)
        {
            IEnumerable<ApartmentAnnouncement> announcements = _announcementRepository.GetListOfApartments();
            locality = HttpUtility.HtmlDecode(locality);
            return Json(GetDistricts(announcements, locality));
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetLandsDistricts(string locality)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfLands();
            locality = HttpUtility.HtmlDecode(locality);
            return Json(GetDistricts(announcements, locality));
        }

        [HttpPost]
        [ValidateInput(false)] 
        public JsonResult GetHousesDistricts(string locality)
        {
            IEnumerable<Announcement> announcements = _announcementRepository.GetListOfHouses();
            locality = HttpUtility.HtmlDecode(locality);
            return Json(GetDistricts(announcements, locality));
        }


        private AddressUnitsResults GetDistricts(IEnumerable<Announcement> announcements, string locality)
        {
            AddressUnitsResults res = new AddressUnitsResults();
            IEnumerable<Announcement> filteredAnnouncements = null;
                if (!string.IsNullOrEmpty(locality))
                {
                    filteredAnnouncements = announcements.Where(an => (an.Address.AddressType == AddressType.City) && (an.Address.LocalityName == locality));
                }

            if (filteredAnnouncements != null)
            {
                res.Done = true;
                res.Items = _announcementRepository.GetDistinctDistricts(filteredAnnouncements);
            }
            return res;
        }

        #endregion
    }
}
