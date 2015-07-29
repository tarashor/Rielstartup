using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Parser.Parsers;
using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RielAp.Web.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        private readonly IVashmagazinRepository _vashmagazinRepository;
        private readonly IStatisticRepository _statisticRepository;
        private readonly IAnnouncementsParser _announcementsParser;

        public MainController(IVashmagazinRepository vashmagazinRepository, IStatisticRepository statisticRepository, IAnnouncementsParser announcementsParser)
        {
            _vashmagazinRepository = vashmagazinRepository;
            _statisticRepository = statisticRepository;
            _announcementsParser = announcementsParser;
        }

        public ActionResult Index()
        {
            ViewBag.CurrentPage = NavigationPage.Main;
            
            return View();
        }

        [HttpPost]
        public JsonResult GetStatisticPerDistrict()
        {
            IEnumerable<Statistic> districtsStatistic = _statisticRepository.GetLastStatisticData();
            IDictionary<string, decimal> statistic = new SortedDictionary<string, decimal>();
            foreach (Statistic district in districtsStatistic)
            {
                statistic.Add(district.District, district.PricePerMeter);
            }
            return Json(statistic);
        }

        public ActionResult GenerateStatistic()
        {
            Task.Run(() =>
            {
                _announcementsParser.ParseApartments();
                generateStatisticData();
            });
            return RedirectToAction("Index");
        }

        public ActionResult GenerateStatisticData()
        {
            generateStatisticData();
            return RedirectToAction("Index");
        }

        private void generateStatisticData()
        {
            IEnumerable<VashmagazinApartment> apartments = _vashmagazinRepository.GetListOfLastApartments();
            IEnumerable<string> districts = _vashmagazinRepository.GetDistinctDistricts(apartments);
            IEnumerable<Statistic> statistics = _statisticRepository.GetAllStatistics();
            foreach (Statistic statistic in statistics)
            {
                statistic.IsOld = true;
            }
            _statisticRepository.SaveChanges();

            foreach (string district in districts)
            {
                IEnumerable<VashmagazinApartment> apartmentsInDistrict = apartments.Where(a => (a.Address.District == district) && (a.Price > 0) && (a.TotalSquare.Value > 0) && (a.TotalSquare.Unit == SquareUnit.SquareMeters));
                double p = apartmentsInDistrict.Average(a => (double)a.Price / a.TotalSquare.Value);
                decimal avaragePricePerMeter = (decimal)p;
                Statistic stat = new Statistic();
                stat.District = district;
                stat.Date = DateTime.Today;
                stat.IsOld = false;
                stat.PricePerMeter = avaragePricePerMeter;
                _statisticRepository.Add(stat);
            }
            _statisticRepository.SaveChanges();
        }



        [HttpPost]
        public JsonResult GetStatisticForDistrict(string district, int timeperiod)
        {
            IEnumerable<Statistic> statisticForDistrict = _statisticRepository.GetStatisticForDistrict(district);
            
            Dictionary<string, decimal> stat = new Dictionary<string, decimal>();
            foreach (Statistic statistic in statisticForDistrict)
            {
                string date = statistic.Date.ToShortDateString();
                if (!stat.ContainsKey(date))
                {
                    stat.Add(date, statistic.PricePerMeter);
                }
            }
            
            return Json(new { Statistic = stat, Title = string.Format(RielAp.Translation.Translation.MainIndexTitleDistrictHistoryChart, district)});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                _vashmagazinRepository.Dispose();
                _statisticRepository.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
