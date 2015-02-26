using RielAp.AddressService.Results;
using RielAp.AddressService.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RielAp.AddressService.YandexServices
{
    public class YandexAddressService:IAddressService
    {
        public static readonly Encoding Encoding = Encoding.GetEncoding("windows-1251");
        public const string MapsUrlPattern = "http://geocode-maps.yandex.ru/1.x/?lang=uk-UA&geocode={0}";
        public const string AddressWithBuldingPattern = "{0},+{1}+вулиця,+дім+{2}";
        public const string AddressPattern = "{0},+{1}+вулиця";
        
        private Stream loadResponse(string url)
        {
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            //req.UserAgent = "MOZILLA/5.0 (WINDOWS NT 6.1; WOW64) APPLEWEBKIT/537.1 (KHTML, LIKE GECKO) CHROME/21.0.1180.75 SAFARI/537.1";
            //req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //req.Headers.Add("Accept-Encoding", "gzip,deflate");

            //GZipStream zip = new GZipStream(req.GetResponse().GetResponseStream(), CompressionMode.Decompress);
            return req.GetResponse().GetResponseStream();//zip;
        }

        private XmlDocument loadXMLResponse(string url)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(loadResponse(url));
            return xmlDocument;

        }

        private string formatAddress(int? buildingNumber, string street, string city){
            string res = string.Empty;
            if (buildingNumber.HasValue)
            {
                res = String.Format(AddressWithBuldingPattern, city, street, buildingNumber.Value);
            }
            else {
                res = String.Format(AddressPattern, city, street);
            }
            return res;
        }

        private string generateMapServiceUrl(int? buildingNumber, string street, string city)
        {
            return String.Format(MapsUrlPattern, formatAddress(buildingNumber, street, city));
        }

        public string GetStreet(string street, string city)
        {
            string streetNameYandex = null;
            if (!string.IsNullOrEmpty(street))
            {
                XmlDocument xmlDocument = loadXMLResponse(generateMapServiceUrl(null, street, city));
                XmlNodeList nodeList = xmlDocument.GetElementsByTagName("ThoroughfareName");
                
                if (nodeList.Count > 0)
                {
                    streetNameYandex = nodeList[0].InnerText;
                }
            }
            return streetNameYandex;
        }

        public Coordinates GetCoordinates(int? buildingNumber, string street, string city)
        {
            throw new NotImplementedException();
        }
    }
}
