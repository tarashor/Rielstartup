using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface IVashmagazinRepository:IRepository<VashmagazinApartment>
    {
        IEnumerable<VashmagazinApartment> GetListOfLastApartments();
        IEnumerable<VashmagazinApartment> GetListOfApartmentsForDate(DateTime date);
        IEnumerable<VashmagazinApartment> GetListOfApartmentsWithRoomsCount(int roomsCount);

        IEnumerable<string> GetDistinctDistricts(IEnumerable<VashmagazinApartment> announcements);
        IEnumerable<int> GetDistinctRoomsCount(IEnumerable<VashmagazinApartment> announcements);
    }
}
