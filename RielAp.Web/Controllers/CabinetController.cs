using ImageResizer;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Infrastructure;
using RielAp.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Controllers
{
    
    public class CabinetController : Controller
    {
        private IAnnouncementsRepository _announcementRepository;
        private IUsersRepository _userRepository;
        private IPhotosRepository _photoRepository;
        private IFeedbacksRepository _feedbackRepository;

        public CabinetController(IAnnouncementsRepository announcementRepository, IUsersRepository userRepository, IPhotosRepository photoRepository, IFeedbacksRepository feedbackRepository)
        {
            _announcementRepository = announcementRepository;
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _feedbackRepository = feedbackRepository;
        }

        public ActionResult Announcements(string phone)
        {
            User user = _userRepository.GetUserByPhone(phone);
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            IEnumerable<Announcement> announcements = null;
            if (user != null)
            {
                announcements = user.Apartments;
                ViewBag.UserPhone = phone;
                ViewBag.IsMyCabinet = authentacatedUser == user;
                ViewBag.LeftAnnouncements = user.Profile.MaxAnnouncements - user.Apartments.Count;
            }

            return View(announcements);
        }

        public ActionResult Feedbacks(string phone)
        {
            User user = _userRepository.GetUserByPhone(phone);
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            IEnumerable<Feedback> feedbacks = null;
            if (user != null)
            {
                feedbacks = user.Feedbacks.OrderBy(f => f.Created);
                ViewBag.UserPhone = phone;
                ViewBag.IsMyCabinet = authentacatedUser == user;
            }

            return View(feedbacks);
        }

        public ActionResult UserProfile(string phone)
        {
            User user = _userRepository.GetUserByPhone(phone);
            User authentacatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (user != null)
            {
                ViewBag.UserPhone = phone;
                ViewBag.IsMyCabinet = authentacatedUser == user;
            }

            return View(user);
        }

        #region -Apartment-

        [Authorize]
        [AssignShareProfile("Middle", "ShareTillDate", "ShareProfileExpires")]
        public ActionResult AddApartment()
        {
            string message = Translation.Translation.AccessIsDeniedMessage;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                if (authenticatedUser.Apartments.Count < authenticatedUser.Profile.MaxAnnouncements)
                {
                    ApartmentAnnouncement apartment = new ApartmentAnnouncement();
                    apartment.User = authenticatedUser;
                    _announcementRepository.Add(apartment);
                    _announcementRepository.SaveChanges();
                    return RedirectToAction("EditApartment", new { id = apartment.AnnouncementID });
                }
                else {
                    message = Translation.Translation.ProfileAnnouncementsLimitMessage;
                }
            }

            TempData["message"] = message;

            return RedirectToAction("Announcements", new { phone = User.Identity.Name });

        }

        [Authorize]
        public ActionResult EditApartment(int id)
        {
            ApartmentAnnouncement apartment = _announcementRepository.GetById(id) as ApartmentAnnouncement;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser == apartment.User) {
                ViewBag.User = authenticatedUser;
                return View(apartment); 
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditApartment(ApartmentAnnouncement apartment)
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            string errormessage = Translation.Translation.InvalidAnnouncementMessage;
            if (ModelState.IsValid )
            {
                if (apartment.Address.HasValue)
                {
                    if (saveAnnouncement(apartment, authenticatedUser))
                    {
                        TempData["message"] = string.Format(Translation.Translation.CabinetDataSavedMessage);
                        return RedirectToAction("Announcements", new { phone = authenticatedUser.Phone });
                    }
                    else
                    {
                        throw new Exception(Translation.Translation.AccessIsDeniedMessage);
                    }
                }
                else {
                    errormessage = Translation.Translation.InvalidAddressMessage;
                }
            }
            ViewBag.User = authenticatedUser;
            ModelState.AddModelError("", errormessage);
            return View(apartment);
        }

        #endregion

        #region -Land-


        [Authorize]
        [AssignShareProfile("Middle", "ShareTillDate", "ShareProfileExpires")]
        public ActionResult AddLand()
        {
            string message = Translation.Translation.AccessIsDeniedMessage;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                if (authenticatedUser.Apartments.Count < authenticatedUser.Profile.MaxAnnouncements)
                {
                    LandAnnouncement land = new LandAnnouncement();
                    land.User = authenticatedUser;
                    _announcementRepository.Add(land);
                    _announcementRepository.SaveChanges();
                    return RedirectToAction("EditLand", new { id = land.AnnouncementID });
                }
                else
                {
                    message = Translation.Translation.ProfileAnnouncementsLimitMessage;
                }
            }

            TempData["message"] = message;

            return RedirectToAction("Announcements", new { phone = User.Identity.Name });
        }

        [Authorize]
        public ActionResult EditLand(int id)
        {
            LandAnnouncement land = _announcementRepository.GetById(id) as LandAnnouncement;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser == land.User)
            {
                ViewBag.User = authenticatedUser;
                return View(land);
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditLand(LandAnnouncement land)
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            string errormessage = Translation.Translation.InvalidAnnouncementMessage;
            if (ModelState.IsValid)
            {
                if (land.Address.HasValue)
                {
                    if (saveAnnouncement(land, authenticatedUser))
                    {
                        TempData["message"] = string.Format(Translation.Translation.CabinetDataSavedMessage);
                        return RedirectToAction("Announcements", new { phone = authenticatedUser.Phone });
                    }
                    else
                    {
                        throw new Exception(Translation.Translation.AccessIsDeniedMessage);
                    }
                }
                else
                {
                    errormessage = Translation.Translation.InvalidAddressMessage;
                }
            }
            ViewBag.User = authenticatedUser;
            ModelState.AddModelError("", errormessage);
            return View(land);

            
        }

        #endregion

        #region -House-

        [Authorize]
        [AssignShareProfile("Middle", "ShareTillDate", "ShareProfileExpires")]
        public ActionResult AddHouse()
        {
            string message = Translation.Translation.AccessIsDeniedMessage;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                if (authenticatedUser.Apartments.Count < authenticatedUser.Profile.MaxAnnouncements)
                {
                    HouseAnnouncement house = new HouseAnnouncement();
                    house.User = authenticatedUser;
                    _announcementRepository.Add(house);
                    _announcementRepository.SaveChanges();
                    return RedirectToAction("EditHouse", new { id = house.AnnouncementID });
                }
                else
                {
                    message = Translation.Translation.ProfileAnnouncementsLimitMessage;
                }
            }

            TempData["message"] = message;

            return RedirectToAction("Announcements", new { phone = User.Identity.Name });
        }

        [Authorize]
        public ActionResult EditHouse(int id)
        {
            HouseAnnouncement house = _announcementRepository.GetById(id) as HouseAnnouncement;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser == house.User) {
                ViewBag.User = authenticatedUser;
                return View(house); 
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditHouse(HouseAnnouncement house)
        {
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            string errormessage = Translation.Translation.InvalidAnnouncementMessage;
            if (ModelState.IsValid)
            {
                if (house.Address.HasValue)
                {
                    if (saveAnnouncement(house, authenticatedUser))
                    {
                        TempData["message"] = string.Format(Translation.Translation.CabinetDataSavedMessage);
                        return RedirectToAction("Announcements", new { phone = authenticatedUser.Phone });
                    }
                    else
                    {
                        throw new Exception(Translation.Translation.AccessIsDeniedMessage);
                    }
                }
                else
                {
                    errormessage = Translation.Translation.InvalidAddressMessage;
                }
            }
            ViewBag.User = authenticatedUser;
            ModelState.AddModelError("", errormessage);
            return View(house);


        }

        #endregion

        [Authorize]
        public ActionResult DeleteAnnouncement(int id)
        {
            Announcement announcement = _announcementRepository.GetById(id);
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser == announcement.User)
            {
                foreach (Photo photo in announcement.Photos)
                {
                    deletePhoto(photo.Url);
                }

                _announcementRepository.Delete(announcement);
                _announcementRepository.SaveChanges();
                TempData["message"] = string.Format(Translation.Translation.CabinetDataDeleteMessage);
                return RedirectToAction("Announcements", new { phone = authenticatedUser.Phone });
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
        }

        #region -Feedback-

        public ActionResult AddFeedback(string phone) {
            User user = _userRepository.GetUserByPhone(phone);
            if (user!= null){
                Feedback feedback = new Feedback();
                feedback.UserId = user.UserID;
                return View(feedback);
            }
            else
            {
                throw new Exception(Translation.Translation.AccessIsDeniedMessage);
            }
        }

        [HttpPost]
        public ActionResult AddFeedback(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.Created = DateTime.Now;
                _feedbackRepository.Add(feedback);
                _feedbackRepository.SaveChanges();
                User user = _userRepository.GetById(feedback.UserId);
                TempData["message"] = string.Format(Translation.Translation.CabinetDataSavedMessage);
                return RedirectToAction("Feedbacks", new { phone = user.Phone});
            }
            else
            {
                return View(feedback);
            }
        }
        #endregion

        #region -Photo-

        [Authorize]
        [HttpPost]
        public ActionResult SetMainPhoto(int announcementId, int photoId)
        {
            bool res = false;
            Announcement announcement = _announcementRepository.GetById(announcementId);

            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser == announcement.User)
            {
                foreach (Photo photo in announcement.Photos)
                {
                    if (photo.PhotoId == photoId)
                    {
                        photo.IsMain = true;
                    }
                    else 
                    {
                        photo.IsMain = false;
                    }
                }
                _photoRepository.SaveChanges();
                res = true;
            }

            return Json(new { Done = res }, "text/json");
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddPhoto(int announcementId, object qqfile)
        {
            bool res = false;

            string fileName;
            Photo photo = new Photo();

            Stream stream = getFileStream(qqfile, out fileName);
            string message = string.Empty;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser != null)
            {
                
                Announcement announcement = _announcementRepository.GetById(announcementId);
                if (announcement != null)
                {
                    if (announcement.Photos.Count < authenticatedUser.Profile.MaxPhotosPerAnnouncements)
                    {
                        photo.Url = savePhoto(stream, fileName);
                        photo.Owner = authenticatedUser;
                        photo.AnnouncementId = announcementId;
                        _photoRepository.Add(photo);
                        _photoRepository.SaveChanges();
                        res = true;
                    }

                    if (announcement.Photos.Count < authenticatedUser.Profile.MaxPhotosPerAnnouncements)
                    {
                        message = string.Format(Translation.Translation.PhotosLeft, authenticatedUser.Profile.MaxPhotosPerAnnouncements - announcement.Photos.Count);
                    }
                    else
                    {
                        message = string.Format(Translation.Translation.LimitForPhotosIsReachedLabel, Url.Action("UpdateLimit", "Account"));
                    }
                }
            }

            return Json(new { Done = res, PhotoId = photo.PhotoId, PhotoUrl = photo.Url, Message = message }, "text/json");
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeletePhoto(int photoId)
        {
            bool res = false;
            string message = string.Empty;
            Photo photo = _photoRepository.GetById(photoId);
            Announcement announcement = photo.Announcement;
            User owner = photo.Owner;
            User authenticatedUser = _userRepository.GetUserByPhone(User.Identity.Name);
            if (authenticatedUser == photo.Owner)
            {
                deletePhoto(photo.Url);
                _photoRepository.Delete(photo);
                _photoRepository.SaveChanges();
                res = true;
            }

            if (announcement.Photos.Count < owner.Profile.MaxPhotosPerAnnouncements)
            {
                message = string.Format(Translation.Translation.PhotosLeft, owner.Profile.MaxPhotosPerAnnouncements - announcement.Photos.Count);
            }
            else {
                message = string.Format(Translation.Translation.LimitForPhotosIsReachedLabel, "/");
            }

            return Json(new { Done = res, Message = message }, "text/json");

        }

        #endregion

        #region - private methods -

        private Stream getFileStream(object qqfile, out string fileName)
        {
            Stream stream = null;
            if (qqfile is HttpPostedFileBase[])
            {
                var input = qqfile as HttpPostedFileBase[];
                string[] partsOfTheName = input[0].FileName.Split('\\');
                fileName = partsOfTheName.Last();
                stream = input[0].InputStream;
            }
            else
            {
                var input = qqfile as string[];
                fileName = input[0];
                stream = HttpContext.Request.InputStream;
            }
            return stream;
        }

        private string savePhoto(Stream fileStream, string filename)
        {
            string virtualPath = Path.Combine(ConfigurationManager.AppSettings["PhotoFolder"], filename);
            string absolutePath = Server.MapPath(virtualPath);
            int i = 1;
            string pureFilename = Path.GetFileNameWithoutExtension(filename);
            string fileExtension = Path.GetExtension(filename);
            while (System.IO.File.Exists(absolutePath)) {
                virtualPath = Path.Combine(ConfigurationManager.AppSettings["PhotoFolder"], pureFilename + "(" + i.ToString() + ")" + fileExtension);
                absolutePath = Server.MapPath(virtualPath);
                i++;
            }

            resizePhoto(fileStream, absolutePath);

            //int fileLength = (int)fileStream.Length;
            //byte[] fileContent = new byte[fileLength];

            //Request.InputStream.Read(fileContent, 0, fileLength);
            //System.IO.File.WriteAllBytes(absolutePath, fileContent);
            return virtualPath;
        }

        private void resizePhoto(Stream fileStream, string path) {
            using (FileStream destStream = new FileStream(path, System.IO.FileMode.Create))
            {
                ResizeSettings rs = new ResizeSettings("maxwidth=1024&maxheight=768");
                ImageResizer.ImageJob job = new ImageJob(fileStream, destStream, rs);
                job.Build();
            }
        }

        private void deletePhoto(string url)
        {
            System.IO.File.Delete(Server.MapPath(url));
        }

        private bool saveAnnouncement(Announcement announcement, User authenticatedUser)
        {
            bool result = false;

            if (authenticatedUser.UserID == announcement.UserID)
            {
                _announcementRepository.SaveAnnouncement(announcement);
                result = true;
            }
            return result;
        }

        #endregion
    }
}
