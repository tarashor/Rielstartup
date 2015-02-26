using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Areas.Admin.Models;
using RielAp.Web.Infrastructure;
using RielAp.Web.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class DefaultController : Controller
    {
        private readonly IUsersRepository _userRepository;
        private readonly IProfilesRepository _profilesRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly ISitemapGenerator _sitemapGenerator;

        public DefaultController(IUsersRepository userRepository, IProfilesRepository profilesRepository, IRolesRepository rolesRepository, ISitemapGenerator sitemapGenerator)
        {
            _userRepository = userRepository;
            _profilesRepository = profilesRepository;
            _rolesRepository = rolesRepository;
            _sitemapGenerator = sitemapGenerator;

        }

        public ActionResult Users()
        {
            UsersViewModel model = new UsersViewModel();
            model.Users = _userRepository.GetAllUsers().OrderByDescending(u => u.RegisterDate);
            return View(model);
        }

        public ActionResult Operations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GenerateSitemap()
        {
            _sitemapGenerator.GenerateSitemap();
            return Json(new { Done = true });
        }

        [HttpPost]
        public ActionResult GenerateRegisterDates()
        {
            IEnumerable<User> users = _userRepository.GetAllUsers();
            foreach (User user in users) { 
                Announcement announcement = user.Apartments.OrderBy(a=>a.Created).FirstOrDefault();
                if (announcement != null)
                {
                    user.RegisterDate = announcement.Created;
                }
                else
                {
                    user.RegisterDate = DateTime.Now;
                }
                user.Role = _rolesRepository.GetById("User");
            }
            _userRepository.SaveChanges();
            return Json(new { Done = true });
        }

        [HttpPost]
        public ActionResult GenerateData()
        {
            int usersCount = 100;
            int announcementsPerUser = 100;
            for (int i = 0; i < usersCount; i++) {
                User user = createUser(i, announcementsPerUser);
                _userRepository.Add(user);
                _userRepository.SaveChanges();
            }
            return Json(new { Done = true });
        }

        private User createUser(int userNumber, int announcementsPerUser)
        {
            User user = new User();
            user.Username = userNumber.ToString();
            user.RoleName = "User";
            user.RegisterDate = DateTime.Now;
            user.Password = userNumber.ToString();
            user.IsConfirmed = true;
            user.EmailAddress = userNumber.ToString() + "@aparts.in.ua";
            user.ConfirmationCode = Guid.NewGuid();
            user.Phone = getUser(userNumber);
            user.Profile = _profilesRepository.GetBasicProfile();
            

            for (int j = 0; j < announcementsPerUser; j++)
            {
                user.Apartments.Add(createAnnouncement(j, user));
            }

            return user;
        }

        private Announcement createAnnouncement(int j, User user)
        {
            Announcement an = null;
            if (j % 3 == 0)
            {
                ApartmentAnnouncement apartment = new ApartmentAnnouncement();
                apartment.Balcony = PresentTypeValue.Present;
                apartment.Floor = j;
                apartment.MaxFloor = j * 3;
                apartment.RoomsNumber = j % 4;
                apartment.TotalSquare = new Square() { Value = j % 100, Unit = SquareUnit.SquareMeters };

                an = apartment;
            }
            else {
                if (j % 3 == 1)
                {
                    HouseAnnouncement house = new HouseAnnouncement();
                    house.TotalSquare = new Square() { Value = j % 20, Unit = SquareUnit.Are };
                    an = house;
                }
                else
                {
                    LandAnnouncement land = new LandAnnouncement();
                    land.TotalSquare = new Square() { Value = j % 20, Unit = SquareUnit.Are };
                    an = land;
                }
            }
            an.Type = AnnouncementType.Buy;
            an.Currency = Currency.Hryvna;
            an.Price = j * 1000;
            an.IsVisible = true;
            an.Sold = false;
            an.User = user;
            an.Address.AddressType = AddressType.City;
            an.Address.AdministrativeArea = j.ToString();
            an.Address.LocalityName = j.ToString();
            an.Address.District = j.ToString();
            an.Address.Street = j.ToString();
            
            return an;

        }

        private string getUser(int userNumber)
        {
            int phoneLength = 12;
            string phone = "380" + userNumber.ToString();
            for (int i = 0; i < phoneLength - phone.Length; i++)
            {
                phone += "0";
            }
            return phone;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _userRepository.Dispose();
            }
        }

    }
}
