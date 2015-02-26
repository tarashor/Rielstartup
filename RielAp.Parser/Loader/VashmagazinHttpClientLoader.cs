using RielAp.Parser.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Loader
{
    public class VashmagazinHttpClientLoader : ILoader
    {
        public static readonly Encoding Encoding = Encoding.GetEncoding("windows-1251");

        public Stream LoadPage(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            req.UserAgent = "MOZILLA/5.0 (WINDOWS NT 6.1; WOW64) APPLEWEBKIT/537.1 (KHTML, LIKE GECKO) CHROME/21.0.1180.75 SAFARI/537.1";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            req.Headers.Add("Accept-Encoding", "gzip,deflate");

            GZipStream zip = new GZipStream(req.GetResponse().GetResponseStream(), CompressionMode.Decompress);
            return zip;
        }

        public string LoadPageHtml(string url)
        {
            string pageHtml = string.Empty;
            using (Stream pageStream = LoadPage(url))
            {
                using (var reader = new StreamReader(pageStream, Encoding))
                {
                    pageHtml = reader.ReadToEnd();
                }
            }
            return pageHtml;
        }


        public string GetUrl(Rubrics rubric, int page)
        {
            return GetUrl(rubric, Subrubrics.Frankivskyj, page);
        }

        public string GetUrl(Rubrics rubric, Subrubrics subrubric, int page)
        {
            return GetUrl(rubricToString(rubric), subrubricToString(subrubric), page);
        }

        private string subrubricToString(Subrubrics subrubric)
        {
            string res = string.Empty;
            switch (subrubric) {
                case Subrubrics.Frankivskyj: res = "frankivskyy-rayon"; break;
                case Subrubrics.Galyckyj: res = "halytskyy-rayon"; break;
                case Subrubrics.Lychakivskyj: res = "lychakivskyy-rayon"; break;
                case Subrubrics.Shevchenkivskyj: res = "shevchenkivskyy-rayon"; break;
                case Subrubrics.Syhivskyj: res = "zaliznychnyy-rayon"; break;
                case Subrubrics.Zaliznychnyj: res = "sykhivskyy-rayon"; break;
                default: break;
            }
            return res;
        }

        private string rubricToString(Rubrics rubric)
        {
            string res = string.Empty;
            switch (rubric) {
                case Rubrics.Apartments: res = "kvartyry"; break;
                default: res = "kvartyry"; break;
            }
            return res;
        }

        private string GetUrl(string rubrik, string subrubrik, int pageNumber)
        {
            return string.Format(VashmagazinUrlPattern, rubrik, subrubrik, pageNumber);
        }

        public const string VashmagazinUrlPattern = "http://vashmagazin.ua/nerukhomist/{0}/{1}//?item_price1=&item_price2=&page={2}";
    }
}
