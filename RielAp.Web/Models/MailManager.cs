using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class MailManager
    {
        private IAnnouncementsRepository _announcementsRepository;
        private IUsersRepository _usersRepository;

        public MailManager(IAnnouncementsRepository announcementsRepository, IUsersRepository usersRepository) 
        {
            _announcementsRepository = announcementsRepository;
            _usersRepository = usersRepository;

        }

        public void SendAnnouncementsNDaysAgo(int days){
            DateTime twoDaysAgo = DateTime.Now.AddDays((-1)*days);
            IEnumerable<Announcement> newAnnouncements = _announcementsRepository.GetListOfApartmentsSince(twoDaysAgo);
            if (newAnnouncements.Count() > 0)
            {
                IEnumerable<User> users = _usersRepository.GetAllUsers().Where(u => u.ReceiveNewAnnouncements);
                IEnumerable<string> emails = from u in users select u.EmailAddress;
                IEnumerable<AnnouncementHtmlDescription> descriptions = from a in newAnnouncements select new ApartmentHtmlDescription() { Announcement = a };
                MailService.SendAnnouncements(emails, descriptions);
            }
        }

    }
}