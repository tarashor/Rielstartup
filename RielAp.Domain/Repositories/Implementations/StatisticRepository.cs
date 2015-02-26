using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;
using RielAp.Domain.Managers;

namespace RielAp.Domain.Repositories.Implementations
{
    public class StatisticRepository: Repository<Statistic>, IStatisticRepository
    {
        public StatisticRepository(RealtyDBContext context)
            : base(context)
        { }

        public decimal GetPricePerMeterForDistrict(string district)
        {
            decimal? res = 0;
            var prices = Context.Statistics.Where(a => a.District == district && !a.IsOld).Select(a=>(decimal?)a.PricePerMeter);
            if (prices != null){
                res = prices.Average();
            }
            return res ?? 0;
        }

        public IEnumerable<Statistic> GetStatisticForDistrict(string district)
        {
            return Context.Statistics.Where(a => a.District == district).OrderBy(a => a.Date);
        }

        public IEnumerable<Statistic> GetAllStatistics()
        {
            return Context.Statistics;
        }


        public IEnumerable<Statistic> GetLastStatisticData()
        {
            return Context.Statistics.Where(a => !a.IsOld);
        }
    }
}
