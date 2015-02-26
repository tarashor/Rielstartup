using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface IApartmentsRepository:IRepository<ApartmentAnnouncement>
    {
        IEnumerable<ApartmentAnnouncement> GetListOfApartments();
        ApartmentAnnouncement FindApartment(string street, int userId, int floor, Square square);
    }
}
