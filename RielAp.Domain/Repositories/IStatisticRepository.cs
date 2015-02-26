using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface IStatisticRepository:IRepository<Statistic>
    {
        IEnumerable<Statistic> GetAllStatistics();

        decimal GetPricePerMeterForDistrict(string district);
        IEnumerable<Statistic> GetStatisticForDistrict(string district);
        IEnumerable<Statistic> GetLastStatisticData();
    }
}
