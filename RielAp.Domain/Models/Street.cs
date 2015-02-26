using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RielAp.Domain.Models
{
    [ComplexType]
    public class Address
    {
        public Nullable<int> BuildingNumber { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        /*public Nullable<int> DistrictID { get; set; }*/

        /*public virtual District District { get; set; }*/

        public bool HasValue
        {
            get
            {
                return (Street != null || City != null);// || DistrictID != null);
            }
        }
    }
}
