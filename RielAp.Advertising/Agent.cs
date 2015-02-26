using RielAp.Parser.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Advertising
{
    public class Agent
    {
        public List<Phone> Phones { get; set; }
        public int AnnouncementsAmount { get; set; }
        public Rubrics Rubric { get; set; }

        public Agent()
        {
            Phones = new List<Phone>();
            AnnouncementsAmount = 0;
        }
    }

    public class Phone
    {
        private const int NumberLength = 7;
        private const int RegionCodeLength = 3;
        private const int CountryCodeLength = 3;

        public string Number { get; set; }
        public string RegionCode { get; set; }
        public string CountryCode { get; set; }

        public static string UkraineCode
        {
            get
            {
                return "+38";
            }
        }

        public Phone()
        {
            Number = null;
            RegionCode = null;
            CountryCode = UkraineCode;
        }

        public Phone(string number, string regionCode)
            : this()
        {
            Number = number;
            RegionCode = regionCode;
        }

        public bool IsValid() {
            bool isValid = (!string.IsNullOrEmpty(Number) && Number.Length == NumberLength) 
                && (!string.IsNullOrEmpty(RegionCode) && RegionCode.Length == RegionCodeLength) 
                && (!string.IsNullOrEmpty(CountryCode) && CountryCode.Length == CountryCodeLength);
            return isValid;
        }

        public string GetRawNumbers() {
            return CountryCode + RegionCode + Number;
        }

    }
}
