using System;
using System.Collections.Generic;

namespace RielAp.Domain.Models
{
    public class District : IModel
    {
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
    }
}
