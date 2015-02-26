using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;
using RielAp.Domain.Managers;

namespace RielAp.Domain.Repositories.Implementations
{
    public class VashmagazinRepository: Repository<VashmagazinApartment>, IVashmagazinRepository
    {
        public VashmagazinRepository(RealtyDBContext context)
            : base(context)
        { }

        public IEnumerable<string> GetDistinctDistricts(IEnumerable<VashmagazinApartment> announcements)
        {
            IEnumerable<string> districts = from a in announcements select a.Address.District;
            return districts.Distinct().OrderBy(s => s);
        }

        public IEnumerable<VashmagazinApartment> GetListOfLastApartments()
        {
            return Context.VashmagazinApartments.Where(a => !a.IsOld);
        }

        public IEnumerable<VashmagazinApartment> GetListOfApartmentsForDate(DateTime date)
        {
            return Context.VashmagazinApartments.Where(a => a.Created.CompareTo(date.Date)>=0);
        }

        public IEnumerable<VashmagazinApartment> GetListOfApartmentsWithRoomsCount(int roomsCount)
        {
            return Context.VashmagazinApartments.Where(a => a.RoomsCount == roomsCount);
        }


        public IEnumerable<int> GetDistinctRoomsCount(IEnumerable<VashmagazinApartment> announcements)
        {
            IEnumerable<int> districts = from a in announcements select a.RoomsCount;
            return districts.Distinct().OrderBy(s => s);
        }
    }
}
