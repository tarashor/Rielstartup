using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Infrastructure.Abstract;
using RielAp.Web.Models;
using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Controllers
{
    public class GeneralController : Controller
    {
        private readonly int daysToNotifyProfile = 3;
        private readonly int daysToSendMail = 7;
        private readonly IAnnouncementsRepository _announcementsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProfilesRepository _profilesRepository;
        private readonly ISitemapGenerator _sitemapGenerator;

        public GeneralController(IAnnouncementsRepository announcementsRepository, IUsersRepository usersRepository, IProfilesRepository profilesRepository, ISitemapGenerator sitemapGenerator) 
        {
            _usersRepository = usersRepository;
            _announcementsRepository = announcementsRepository;
            _profilesRepository = profilesRepository;
            _sitemapGenerator = sitemapGenerator;
        }

        private bool checkParams(int a, int b) {
            return (a + b) == 10;
        }
        
        public ActionResult SendMails(int a, int b)
        {
            if (checkParams(a,b))
            {
                MailManager manager = new MailManager(_announcementsRepository, _usersRepository);
                manager.SendAnnouncementsNDaysAgo(daysToSendMail);
            }

            return new EmptyResult();
        }

        public ActionResult GenerateSitemaps(int a, int b)
        {
            if (checkParams(a, b))
            {
                _sitemapGenerator.GenerateSitemap();
            }

            return new EmptyResult();
        }

        public ActionResult VerifyUsers(int a, int b)
        {
            if (checkParams(a, b))
            {
                IEnumerable<User> users = _usersRepository.GetAllUsers();
                foreach (User user in users) {
                    if (user.ProfileExpires.HasValue) {
                        if (user.ProfileExpires.Value.CompareTo(DateTime.Now) <= 0)
                        {
                            user.Profile = _profilesRepository.GetBasicProfile();
                            IEnumerable<Announcement> removeAnnouncements =  user.Apartments.OrderByDescending(an => an.Created).Skip(user.Profile.MaxAnnouncements);
                            foreach (Announcement announcement in removeAnnouncements) {
                                announcement.IsVisible = false;
                            }

                            foreach (Announcement announcement in user.Apartments)
                            {
                                if (announcement.Photos.Count > user.Profile.MaxPhotosPerAnnouncements) {
                                    IEnumerable<Photo> removePhotos = announcement.Photos.OrderByDescending(p => p.Created).Skip(user.Profile.MaxPhotosPerAnnouncements);
                                    foreach (Photo photo in removePhotos) {
                                        photo.IsVisible = false;
                                    }
                                }
                            }
                        }
                        else {
                            if (user.ProfileExpires.Value.CompareTo(DateTime.Now.AddDays(-1 * daysToNotifyProfile)) <= 0)
                            {
                                string title = string.Format(Translation.Translation.EmailProfileWillBeExpiredTitle, daysToNotifyProfile);
                                string body = string.Format(Translation.Translation.EmailProfileWillBeExpiredBody, daysToNotifyProfile, Url.Action("UpdateLimit", "Account", new { }, this.Request.Url.Scheme));
                                MailService.SendEmail(user.EmailAddress, title, body);
                            }
                        }
                    }
                }
                _usersRepository.SaveChanges();
            }

            return new EmptyResult();
        }


    }
}
