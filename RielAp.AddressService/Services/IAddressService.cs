using RielAp.AddressService.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.AddressService.Services
{
    public interface IAddressService
    {
        string GetStreet(string street, string city);
        Coordinates GetCoordinates(int? buildingNumber, string street, string city);
    }
}
