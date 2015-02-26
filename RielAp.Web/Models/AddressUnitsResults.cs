using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class AddressUnitsResults
    {
        public AddressUnitsResults() {
            Done = false;
            Items = null;
        }
        public bool Done { get; set; }
        public IEnumerable<string> Items { get; set; }
    }
}