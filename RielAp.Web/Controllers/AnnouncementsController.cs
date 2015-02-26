using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Infrastructure;
using RielAp.Web.Models;
using RielAp.Web.Models.Filter;
using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace RielAp.Web.Controllers
{
    public class AnnouncementsController : Controller
    {
        public const int PageSize = 20;
        private readonly string COOKIE_KEY_APARTMENTFILTER = "ApartmentFilter";
        private readonly string COOKIE_KEY_LANDFILTER = "LandFilter";
        private readonly string COOKIE_KEY_HOUSEFILTER = "HouseFilter";

        private IAnnouncementsRepository _announcementRepository;
        private IUsersRepository _userRepository;

        public AnnouncementsController(IAnnouncementsRepository apartmentRepository, IUsersRepository userRepository)
        {
            _announcementRepository = apartmentRepository;
            _userRepository = userRepository;   
        }

        //[NoCache]
        public ActionResult Apartments(ApartmentFilterCriteria filter, AnnouncementType type = AnnouncementType.Buy, AnnouncementOrder order = AnnouncementOrder.Address, NavigationViewType viewtype = NavigationViewType.List, int page = 1, bool considerEmpty = true)
        {
            IEnumerable<ApartmentAnnouncement> apartments = _announcementRepository.GetListOfApartments().Where(an => an.Type == type && an.Address.HasValue);

            switch (order)
            {
                case AnnouncementOrder.Address: apartments = apartments.OrderBy(a => a.Address.ToString()); break;
                case AnnouncementOrder.AddressDescending: apartments = apartments.OrderByDescending(a => a.Address.ToString()); break;
                case AnnouncementOrder.Date: apartments = apartments.OrderBy(a => a.Created); break;
                case AnnouncementOrder.DateDescending: apartments = apartments.OrderByDescending(a => a.Created); break;
                default: apartments = apartments.OrderBy(a => a.Address.ToString()); break;
            }

            if (ModelState.IsValid)
            {
                if (filter != null)
                {
                    if (filter.IsEmpty() && considerEmpty)
                    {
                        ApartmentFilterCriteria newFilter = ObjectToCookie.GetObjectFromCookies<ApartmentFilterCriteria>(Request.Cookies, COOKIE_KEY_APARTMENTFILTER);
                        if (newFilter != null)
                        {
                            filter = newFilter;
                        }
                    }
                    apartments = filter.Filter(apartments);
                    ObjectToCookie.SetCookieAsObject<ApartmentFilterCriteria>(filter, Response.Cookies, COOKIE_KEY_APARTMENTFILTER);
                }
            }
            else {
                filter = new ApartmentFilterCriteria();
            }
            
            ViewBag.ViewType = viewtype;
            ViewBag.Type = type;
            ViewBag.OrderType = order;

            if (viewtype == NavigationViewType.List)
            {
                ApartmentsListViewModel viewModel = new ApartmentsListViewModel();
                viewModel.Page = createPageInfo(page, apartments.Count());
                setViewModel(viewModel, announcementsForPage(apartments, page));
                viewModel.FilterCriteria = filter;
                
                return View("Apartments", viewModel);
            }
            else {
                ApartmentsViewModel viewModel = new ApartmentsViewModel();
                setViewModel(viewModel, apartments);
                viewModel.FilterCriteria = filter;
                return View("ApartmentsMap", viewModel);
            }
        }
        
        [NoCache]
        public ActionResult Lands(LandFilterCriteria filter, AnnouncementType type = AnnouncementType.Buy, AnnouncementOrder order = AnnouncementOrder.Address, NavigationViewType viewtype = NavigationViewType.List, int page = 1, bool considerEmpty = true)
        {
            IEnumerable<LandAnnouncement> lands = _announcementRepository.GetListOfLands().ToList().Where(an => an.Type == type && an.Address.HasValue);

            switch (order)
            {
                case AnnouncementOrder.Address: lands = lands.OrderBy(a => a.Address.ToString()); break;
                case AnnouncementOrder.AddressDescending: lands = lands.OrderByDescending(a => a.Address.ToString()); break;
                case AnnouncementOrder.Date: lands = lands.OrderBy(a => a.Created); break;
                case AnnouncementOrder.DateDescending: lands = lands.OrderByDescending(a => a.Created); break;
                default: lands = lands.OrderBy(a => a.Address.ToString()); break;
            }
            if (ModelState.IsValid)
            {
                if (filter != null)
                {
                    if (filter.IsEmpty() && considerEmpty)
                    {
                        LandFilterCriteria newFilter = ObjectToCookie.GetObjectFromCookies<LandFilterCriteria>(Request.Cookies, COOKIE_KEY_LANDFILTER);
                        if (newFilter != null)
                        {
                            filter = newFilter;
                        }
                    }
                    lands = filter.Filter(lands);
                    ObjectToCookie.SetCookieAsObject<LandFilterCriteria>(filter, Response.Cookies, COOKIE_KEY_LANDFILTER);
                }
            }
            else
            {
                filter = new LandFilterCriteria();
            }

            ViewBag.ViewType = viewtype;
            ViewBag.Type = type;
            ViewBag.OrderType = order;

            if (viewtype == NavigationViewType.List)
            {
                LandsListViewModel viewModel = new LandsListViewModel();
                viewModel.Page = createPageInfo(page, lands.Count());
                setViewModel(viewModel, announcementsForPage(lands, page));
                viewModel.FilterCriteria = filter;
                return View("Lands", viewModel);
            }
            else
            {
                LandsViewModel viewModel = new LandsViewModel();
                setViewModel(viewModel, lands);
                viewModel.FilterCriteria = filter;
                return View("LandsMap", viewModel);
            }
        }

        [NoCache]
        public ActionResult Houses(HouseFilterCriteria filter, AnnouncementType type = AnnouncementType.Buy, AnnouncementOrder order = AnnouncementOrder.Address, NavigationViewType viewtype = NavigationViewType.List, int page = 1, bool considerEmpty = true)
        {
            IEnumerable<HouseAnnouncement> houses = _announcementRepository.GetListOfHouses().ToList().Where(an => an.Type == type && an.Address.HasValue);

            switch (order)
            {
                case AnnouncementOrder.Address: houses = houses.OrderBy(a => a.Address.ToString()); break;
                case AnnouncementOrder.AddressDescending: houses = houses.OrderByDescending(a => a.Address.ToString()); break;
                case AnnouncementOrder.Date: houses = houses.OrderBy(a => a.Created); break;
                case AnnouncementOrder.DateDescending: houses = houses.OrderByDescending(a => a.Created); break;
                default: houses = houses.OrderBy(a => a.Address.ToString()); break;
            }

            if (ModelState.IsValid)
            {
                if (filter != null)
                {
                    if (filter.IsEmpty() && considerEmpty)
                    {
                        HouseFilterCriteria newFilter = ObjectToCookie.GetObjectFromCookies<HouseFilterCriteria>(Request.Cookies, COOKIE_KEY_HOUSEFILTER);
                        if (newFilter != null)
                        {
                            filter = newFilter;
                        }
                    }
                    houses = filter.Filter(houses);
                    ObjectToCookie.SetCookieAsObject<HouseFilterCriteria>(filter, Response.Cookies, COOKIE_KEY_HOUSEFILTER);
                }
            }
            else
            {
                filter = new HouseFilterCriteria();
            }


            ViewBag.ViewType = viewtype;
            ViewBag.Type = type;
            ViewBag.OrderType = order;

            if (viewtype == NavigationViewType.List)
            {
                HousesListViewModel viewModel = new HousesListViewModel();
                viewModel.Page = createPageInfo(page, houses.Count());
                setViewModel(viewModel, announcementsForPage(houses, page));
                viewModel.FilterCriteria = filter;
                return View("Houses", viewModel);
            }
            else
            {
                HousesViewModel viewModel = new HousesViewModel();
                setViewModel(viewModel, houses);
                viewModel.FilterCriteria = filter;
                return View("HousesMap", viewModel);
            }
        }

        public ActionResult ApartmentDetails(int id)
        {
            ApartmentAnnouncement apartment = _announcementRepository.GetById(id) as ApartmentAnnouncement;
            return View(apartment);
        }

        public ActionResult LandDetails(int id)
        {
            LandAnnouncement land = _announcementRepository.GetById(id) as LandAnnouncement;
            return View(land);
        }

        public ActionResult HouseDetails(int id)
        {
            HouseAnnouncement house = _announcementRepository.GetById(id) as HouseAnnouncement;
            return View(house);
        }

        public ActionResult ClearApartmentFilter(AnnouncementType type = AnnouncementType.Buy, NavigationViewType viewtype = NavigationViewType.List, int page = 1)
        {
            if (Request.Cookies[COOKIE_KEY_APARTMENTFILTER] != null)
            {
                var c = new HttpCookie(COOKIE_KEY_APARTMENTFILTER);
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Apartments", new { type = type, viewtype = viewtype, page = page });
        }
        
        public ActionResult ClearLandFilter(AnnouncementType type = AnnouncementType.Buy, NavigationViewType viewtype = NavigationViewType.List, int page = 1)
        {
            if (Request.Cookies[COOKIE_KEY_LANDFILTER] != null)
            {
                var c = new HttpCookie(COOKIE_KEY_LANDFILTER);
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Lands", new { type = type, viewtype = viewtype, page = page });
        }

        public ActionResult ClearHouseFilter(AnnouncementType type = AnnouncementType.Buy, NavigationViewType viewtype = NavigationViewType.List, int page = 1)
        {
            if (Request.Cookies[COOKIE_KEY_HOUSEFILTER] != null)
            {
                var c = new HttpCookie(COOKIE_KEY_HOUSEFILTER);
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Houses", new { type = type, viewtype = viewtype, page = page });
        }

        [HttpPost]
        public ActionResult GetApartmentDescription(int id)
        {
            ApartmentHtmlDescription descriptor = new ApartmentHtmlDescription();
            descriptor.Announcement = _announcementRepository.GetById(id);
            return Json(new {Html = descriptor.GetHtmlDescription()});
        }

        [HttpPost]
        public ActionResult GetLandDescription(int id)
        {
            LandHtmlDescription descriptor = new LandHtmlDescription();
            descriptor.Announcement = _announcementRepository.GetById(id);
            return Json(new { Html = descriptor.GetHtmlDescription() });
        }

        [HttpPost]
        public ActionResult GetHouseDescription(int id)
        {
            HouseHtmlDescription descriptor = new HouseHtmlDescription();
            descriptor.Announcement = _announcementRepository.GetById(id);
            return Json(new { Html = descriptor.GetHtmlDescription() });
        }

        #region Private Methods

        private IEnumerable<Announcement> announcementsForPage(IEnumerable<Announcement> announcements, int page)
        {
            return announcements.Skip((page - 1) * PageSize).Take(PageSize);
        }

        private PageInfo createPageInfo(int page, int announcementCount)
        {
            PageInfo pageinfo = new PageInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = announcementCount
            };
            return pageinfo;
        }

        private void setViewModel(AnnouncementsViewModel viewModel, IEnumerable<Announcement> announcements)
        {
            viewModel.Announcements = announcements;
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _announcementRepository.Dispose();
                _userRepository.Dispose();
                //_notesRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        
    }
}
