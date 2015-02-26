using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface IAnnouncementsRepository:IRepository<Announcement>
    {
        IEnumerable<ApartmentAnnouncement> GetListOfApartments();
        IEnumerable<ApartmentAnnouncement> GetListOfApartmentsSince(DateTime date);
        IEnumerable<HouseAnnouncement> GetListOfHouses();
        IEnumerable<LandAnnouncement> GetListOfLands();
        ApartmentAnnouncement FindApartment(string street, int userId, int floor, Square square);

        void SaveAnnouncement(Announcement announcement);

        IEnumerable<string> GetDistinctAddressAdministrativeAreas(IEnumerable<Announcement> announcements);
        IEnumerable<string> GetDistinctAddressSubAdministrativeAreas(IEnumerable<Announcement> announcements);
        IEnumerable<string> GetDistinctLocalities(IEnumerable<Announcement> announcements);
        IEnumerable<string> GetDistinctDistricts(IEnumerable<Announcement> announcements);
    }
}
