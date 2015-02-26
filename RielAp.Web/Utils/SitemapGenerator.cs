using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Controllers;
using RielAp.Web.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RielAp.Web.Utils
{
    public class SitemapGenerator : ISitemapGenerator
    {
        private readonly IAnnouncementsRepository _announcementsRepository; 
        private readonly IUsersRepository _usersRepository;
        private readonly UrlHelper urlHelper;

        public SitemapGenerator(IAnnouncementsRepository announcementsRepository, IUsersRepository usersRepository)
        {
            _announcementsRepository = announcementsRepository;
            _usersRepository = usersRepository;
            urlHelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public void GenerateSitemap()
        {
            IEnumerable<string> urls = generateUrls();
            IEnumerable<string> sitemaps = saveUrlsToSitemaps(urls);
            saveGeneralSitemap(sitemaps);
        }

        private IEnumerable<string> generateUrls()
        {
            List<string> urls = new List<string>();

            urls.Add(string.Empty);

            IEnumerable<ApartmentAnnouncement> apartments = _announcementsRepository.GetListOfApartments();
            
            /*IEnumerable<ApartmentAnnouncement> buyApartments = apartments.Where(an => an.Type == AnnouncementType.Buy);
            IEnumerable<ApartmentAnnouncement> rentApartments = apartments.Where(an => an.Type == AnnouncementType.Rent);
            IEnumerable<ApartmentAnnouncement> rentshortApartments = apartments.Where(an => an.Type == AnnouncementType.RentShort);

            int pagesBuyApartments = AnnouncementsController.PageSize;*/

            foreach (ApartmentAnnouncement apartment in apartments) {
                urls.Add(urlHelper.Action("ApartmentDetails", "Announcements", new { id=apartment.AnnouncementID, area=string.Empty }));
            }


            IEnumerable<LandAnnouncement> lands = _announcementsRepository.GetListOfLands();
            foreach (LandAnnouncement land in lands)
            {
                urls.Add(urlHelper.Action("LandDetails", "Announcements", new { id = land.AnnouncementID, area = string.Empty }));
            }

            IEnumerable<HouseAnnouncement> houses = _announcementsRepository.GetListOfHouses();
            foreach (HouseAnnouncement house in houses)
            {
                urls.Add(urlHelper.Action("HouseDetails", "Announcements", new { id = house.AnnouncementID, area = string.Empty }));
            }

            IEnumerable<User> users = _usersRepository.GetAllUsers();
            foreach (User user in users) {
                urls.Add(urlHelper.Action("UserProfile", "Cabinet", new { phone = user.Phone, area = string.Empty }));
                urls.Add(urlHelper.Action("Announcements", "Cabinet", new { phone = user.Phone, area = string.Empty }));
            }

            
            return urls;
        }

        private IEnumerable<string> saveUrlsToSitemaps(IEnumerable<string> urls)
        {
            List<string> sitemaps = new List<string>();
            int currentSiteMapXmlLines = 0;
            StringBuilder currentSitemapXml = new StringBuilder();
            int sitemapIndex = 0;

            foreach (string url in urls)
            {
                currentSiteMapXmlLines++;
                currentSitemapXml.AppendLine(generateSitemapLine(url));
                if (currentSiteMapXmlLines > 5000)
                {
                    string sitemap = saveSiteMap(currentSitemapXml, sitemapIndex);
                    if (!string.IsNullOrEmpty(sitemap))
                    {
                        sitemaps.Add(sitemap);
                    }
                    currentSitemapXml.Clear();
                    currentSiteMapXmlLines = 0;
                    sitemapIndex++;
                }
            }
            string lastsitemap = saveSiteMap(currentSitemapXml, sitemapIndex);
            if (!string.IsNullOrEmpty(lastsitemap))
            {
                sitemaps.Add(lastsitemap);
            }

            return sitemaps;
        }


        private string saveSiteMap(StringBuilder currentSitemapXml, int sitemapIndex)
        {
            string sitemapContent = currentSitemapXml.ToString();
            string sitemapUrl = null;
            if (!string.IsNullOrEmpty(sitemapContent))
            {
                sitemapUrl = "/sitemap/sitemap" + (sitemapIndex + 1) + ".xml.gz";
                using (var stream = new FileStream(HttpContext.Current.Server.MapPath(sitemapUrl), FileMode.OpenOrCreate))
                {
                    using (var hgs = new GZipStream(stream, CompressionMode.Compress, true))
                    {
                        var encoding = new UTF8Encoding();
                        byte[] data = encoding.GetBytes(generateSitemapPartial(sitemapContent));
                        hgs.Write(data, 0, data.Length);
                    }
                }
            }
            return sitemapUrl;

        }

        private void saveGeneralSitemap(IEnumerable<string> sitemaps)
        {
            string generalSitemapContent = generateSitemap(sitemaps);
            File.WriteAllText(HttpContext.Current.Server.MapPath("/sitemap/sitemap.xml"), generalSitemapContent);
        }


        private string generateSitemapLine(string url)
        {
            return " <url>" + "<loc> " + ConfigurationManager.AppSettings["websiteurl"] + url + "</loc>"
                                    + "<lastmod>" + DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod>"
                                    + "<changefreq>daily</changefreq>" +
                                 "</url>";
        }

        private string generateSitemapPartial(string sitemapContent)
        {
            return "<?xml version = \"1.0\" encoding = \"UTF-8\"?> " +
                                                                    "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\"> " +
                                                                    sitemapContent + "</urlset>";
        }
        private string generateSitemap(IEnumerable<string> sitemaps)
        {
            var rootXml =
                new StringBuilder(
                    "<?xml version = \"1.0\" encoding=\"UTF-8\"?><sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");
            foreach (string sitemap in sitemaps)
            {
                rootXml.Append("<sitemap><loc>" + ConfigurationManager.AppSettings["websiteurl"] + sitemap +
                               "</loc><lastmod>" + DateTime.Now.ToString("yyyy-MM-dd") + "</lastmod></sitemap>");
            }


            rootXml.Append("</sitemapindex>");
            return rootXml.ToString();
        }
    }
}
