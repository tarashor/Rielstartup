using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;
using RielAp.Domain.Managers;

namespace RielAp.Domain.Repositories.Implementations
{
    public class AnnouncementsRepository: Repository<Announcement>, IAnnouncementsRepository
    {
        public AnnouncementsRepository(RealtyDBContext context)
            : base(context)
        { }

        public IEnumerable<ApartmentAnnouncement> GetListOfApartments()
        {
            return Context.Announcements.OfType<ApartmentAnnouncement>().Where(a => a.IsVisible);
        }

        public IEnumerable<HouseAnnouncement> GetListOfHouses()
        {
            return Context.Announcements.OfType<HouseAnnouncement>().Where(a => a.IsVisible);
        }

        public IEnumerable<LandAnnouncement> GetListOfLands()
        {
            return Context.Announcements.OfType<LandAnnouncement>().Where(a => a.IsVisible);
        }

        public IEnumerable<ApartmentAnnouncement> GetListOfApartmentsSince(DateTime date)
        {
            return GetListOfApartments().Where(ap=>ap.Created.CompareTo(date)>0);
        }

        public ApartmentAnnouncement FindApartment(string street, int userId, int floor, Square square)
        {
            ApartmentAnnouncement apartment = null;
            IQueryable<ApartmentAnnouncement> apartments = SearchFor(an => isApartmentValid(an, street, userId, floor, square)).Select(an => an as ApartmentAnnouncement);

            apartment = apartments.FirstOrDefault();
            return apartment;
        }

        private bool isApartmentValid(Announcement announcement, string street, int userId, int floor, Square square) 
        {
            bool res = false;
            ApartmentAnnouncement apartment = announcement as ApartmentAnnouncement;
            if (apartment != null){
                res = ((apartment.Address.Street == street) && (apartment.Floor == floor) && (apartment.LivingSquare.Value == square.Value) && (apartment.UserID == userId));
            }
            return res;
        }

        

        public void SaveAnnouncement(Announcement announcement)
        {
            if (announcement == null)
            {
                throw new ArgumentException("Cannot add a null entity.");
            }

            var entry = Context.Entry(announcement);

            if (entry.State == System.Data.EntityState.Detached)
            {
                Announcement attachedEntity = this.GetById(announcement.AnnouncementID);  // You need to have access to key

                if (attachedEntity != null)
                {
                    var attachedEntry = Context.Entry(attachedEntity);
                    attachedEntry.CurrentValues.SetValues(announcement);
                }
                else
                {
                    Attach(announcement);
                }
            }
            Context.SaveChanges();
        }


        public IEnumerable<string> GetDistinctAddressAdministrativeAreas(IEnumerable<Announcement> announcements)
        {
            IEnumerable<string> administrativeAreas = from a in announcements select a.Address.AdministrativeArea;
            return administrativeAreas.Distinct().OrderBy(s=>s);
        }

        public IEnumerable<string> GetDistinctAddressSubAdministrativeAreas(IEnumerable<Announcement> announcements)
        {
            IEnumerable<string> subadministrativeAreas = from a in announcements select a.Address.SubAdministrativeArea;
            return subadministrativeAreas.Distinct().OrderBy(s => s);
        }


        public IEnumerable<string> GetDistinctLocalities(IEnumerable<Announcement> announcements)
        {
            IEnumerable<string> localities = from a in announcements select a.Address.LocalityName;
            return localities.Distinct().OrderBy(s => s);
        }


        public IEnumerable<string> GetDistinctDistricts(IEnumerable<Announcement> announcements)
        {
            IEnumerable<string> districts = from a in announcements select a.Address.District;
            return districts.Distinct().OrderBy(s => s);
        }
    }
}
