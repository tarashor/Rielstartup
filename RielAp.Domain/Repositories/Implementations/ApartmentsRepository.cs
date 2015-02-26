using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class ApartmentsRepository: Repository<ApartmentAnnouncement>, IApartmentsRepository
    {
        public ApartmentsRepository(RealtyDBContext context)
            : base(context)
        { }

        public IEnumerable<ApartmentAnnouncement> GetListOfApartments()
        {
            return Context.Announcements.OfType<ApartmentAnnouncement>().OrderBy(ap => ap.Address.Street);
        }

        public ApartmentAnnouncement FindApartment(string street, int userId, int floor, Square square)
        {
            ApartmentAnnouncement apartment = null;
            IQueryable<ApartmentAnnouncement> apartments = SearchFor(ap => ((ap.Address.Street == street) && (ap.Floor == floor) && (ap.LivingSquare.Value == square.Value) && (ap.UserID == userId)));
            if (apartments.Count() > 0)
            {
                apartment = apartments.First();
            }
            return apartment;
        }
    }
}
