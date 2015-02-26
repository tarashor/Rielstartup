using HtmlAgilityPack;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Parser.Helper;
using RielAp.Parser.Loader;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Parsers
{
    public class VashmagazinAnnouncementsParserParallel : VashmagazinAnnouncementsParser, IAnnouncementsParser
    {
        protected BlockingCollection<VashmagazinApartment> apartmetsToSave;
        protected List<Task> savingTasks;

        public VashmagazinAnnouncementsParserParallel(ILoader pageLoader, IVashmagazinRepository vashmagazinRepository, ILoger loger)
            : base(pageLoader, vashmagazinRepository, loger)
        {
            apartmetsToSave = new BlockingCollection<VashmagazinApartment>();
        }


        public int ParseApartments()
        {
            IEnumerable<VashmagazinApartment> apartments = _vashmagazinRepository.GetListOfLastApartments();
            foreach (VashmagazinApartment apartment in apartments)
            {
                apartment.IsOld = true;
            }
            _vashmagazinRepository.SaveChanges();

            savingTasks = new List<Task>();
            Stopwatch time = new Stopwatch();
            time.Start();
            Task savingTask = saveParsedApartments();
            Rubrics rubric = Rubrics.Apartments;
            IEnumerable<Subrubrics> subrubrics = Enum.GetValues(typeof(Subrubrics)).Cast<Subrubrics>();
            foreach (Subrubrics subrubric in subrubrics)
            {
                int page = 1;
                bool hasError = false;
                do
                {
                    string url = _pageLoader.GetUrl(rubric, subrubric, page);
                    _loger.Log(url);
                    string pageHtml = _pageLoader.LoadPageHtml(url);
                    hasError = parseHtmlPage(pageHtml, CITY, getDistrictFromSubrubric(subrubric));
                    page++;
                } while (!hasError);
            }
            
            Task.Factory.ContinueWhenAll(savingTasks.ToArray(), (tasks) => {
                apartmetsToSave.CompleteAdding();
            });

            savingTask.Wait();
            time.Stop();
            _loger.Log("Time elapsed: " + time.Elapsed.ToString());

            return _vashmagazinRepository.GetListOfLastApartments().Count();
        }

        protected Task saveParsedApartments()
        {
            Task savingTask = Task.Run(() =>
            {
                VashmagazinApartment apartment = null;
                while (!apartmetsToSave.IsCompleted)
                {
                    try
                    {
                        apartment = apartmetsToSave.Take();
                        _vashmagazinRepository.Add(apartment);
                    }
                    catch (InvalidOperationException)
                    {
                        _loger.Log("Adding was compeleted!");
                        break;
                    }
                }
                _vashmagazinRepository.SaveChanges();
            });
            return savingTask;
        }

        

        protected bool parseHtmlPage(string pageHtml, string localityName, string district)
        {
            IEnumerable<HtmlNode> rows = getRowsWithApartmentsData(pageHtml);
            bool hasError = (rows == null) || (rows.Count() < 1);
            if (!hasError)
            {
                savingTasks.Add(Task.Run(() =>
                {
                    extractApartments(localityName, district, rows);
                }));
            }
            return hasError;

        }

        protected void extractApartments(string localityName, string district, IEnumerable<HtmlNode> rows)
        {
            int i = 0;
            IEnumerator<HtmlNode> currentRow = rows.GetEnumerator();
            while (currentRow.MoveNext())
            {
                if (isNewRow(currentRow.Current))
                {
                    VashmagazinApartment apartment = new VashmagazinApartment();
                    apartment.Address.LocalityName = localityName;
                    apartment.Address.District = district;
                    if (currentRow.MoveNext())
                    {
                        setApartmentProperties(currentRow, apartment);
                    }
                    apartmetsToSave.Add(apartment);
                    i++;
                }
            }
            _loger.Log("Added: " + i.ToString());
        }
    }
}
