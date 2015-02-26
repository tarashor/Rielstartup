using RielAp.Domain.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RielAp.Domain.Models
{
    [ComplexType]
    [Serializable]
    public class Address
    {
        public string AdministrativeArea { get; set; }
        public string SubAdministrativeArea { get; set; }
        public string LocalityName { get; set; }
        public string District { get; set; }
        public string Street { get; set; }

        public AddressType AddressType { get; set; }

        public string Logitude { get; set; }
        public string Latitude { get; set; }

        public override string ToString()
        {
            string res = null;
            if (AddressType == Models.AddressType.City)
            {
                res = Street + ", " + LocalityName;
            }
            else {
                res = LocalityName + ", " + SubAdministrativeArea;
            }
            return res;
        }

        public bool HasValue {
            get {
                bool res = false;
                if (AddressType == Models.AddressType.City)
                {
                    res = !string.IsNullOrEmpty(Street);
                }
                else
                {
                    res = !string.IsNullOrEmpty(LocalityName);
                }
                return res;
            }
        }

        public bool IsEmpty
        {
            get
            {
                bool res = AddressType == Models.AddressType.Unknown && string.IsNullOrEmpty(AdministrativeArea) && string.IsNullOrEmpty(SubAdministrativeArea) && string.IsNullOrEmpty(LocalityName) && string.IsNullOrEmpty(District);
                return res;
            }
        }

        public bool Equals(Address address)
        {
            bool res = (AddressType == address.AddressType);
            if (res)
            {
                if (AddressType == Models.AddressType.Villarge)
                {
                    if (!string.IsNullOrEmpty(address.AdministrativeArea))
                    {
                        res = res && AdministrativeArea == address.AdministrativeArea;
                        if (res)
                        {
                            if (!string.IsNullOrEmpty(address.SubAdministrativeArea))
                            {
                                res = res && SubAdministrativeArea == address.SubAdministrativeArea;
                            }
                        }
                    }
                }

                if (res)
                {
                    if (!string.IsNullOrEmpty(address.LocalityName))
                    {
                        res = res && LocalityName == address.LocalityName;
                        if (res)
                        {
                            if (address.AddressType == Models.AddressType.City)
                            {
                                if (!string.IsNullOrEmpty(address.District))
                                {
                                    res = res && address.District == District;
                                    if (res)
                                    {
                                        if (!string.IsNullOrEmpty(address.Street))
                                        {
                                            res = res && address.Street == Street;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

            return res;

        }
    }

    public enum AddressType {
        [LocalizationText("AddressTypeUnknownLabel", "UnimportantLabel")]
        Unknown,
        [LocalizationText("AddressTypeCityLabel")]
        City,
        [LocalizationText("AddressTypeVillargeLabel")]
        Villarge
    }

}
