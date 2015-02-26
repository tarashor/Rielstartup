using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Managers
{
    public class AddressComparer:IComparer<Address>
    {
        public int Compare(Address x, Address y)
        {
            string addressStringX = x.ToString();
            string addressStringY = y.ToString();
            return addressStringX.CompareTo(addressStringY);
        }
    }
}
