using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class EditPhonesModel
    {
        public int MaxPhonesNumber { get { return int.Parse(ConfigurationManager.AppSettings["MaxAdditionalMobileNumbers"]); } }

        public string Phone { get; set; }

        public IEnumerable<MobileNumber> UserMobileNumbers { get; private set; }
        public IEnumerable<MobileNumber> EmptyMobileNumbers { get; private set; }

        public EditPhonesModel(string phone, IEnumerable<MobileNumber> userMobileNumbers)
        {
            Phone = phone;

            int userMobileNumbersCount = userMobileNumbers.Count();

            if (userMobileNumbersCount > MaxPhonesNumber) {
                userMobileNumbers = userMobileNumbers.Take(MaxPhonesNumber);
            }

            UserMobileNumbers = userMobileNumbers;

            int emptyMobileNumbersCount = MaxPhonesNumber - UserMobileNumbers.Count();
            List<MobileNumber> emptyMobileNumbers = new List<MobileNumber>();
            for (int i = 0; i < emptyMobileNumbersCount; i++) {
                emptyMobileNumbers.Add(new MobileNumber());
            }

            EmptyMobileNumbers = emptyMobileNumbers;


        }
    }
}