using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Helper
{
    public interface IApartmentsManager
    {
        void AddApartment(ApartmentAnnouncement apartment);
        IEnumerable<ApartmentAnnouncement> GetListOfApartments();
        void SetPhone(ApartmentAnnouncement apartment, string phone);
        void SetStreet(ApartmentAnnouncement apartment, string streetName);
        int NewApartments { get; set; }
    }
}
