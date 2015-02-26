using System;
using RielAp.Domain.Models;

namespace RielAp.Domain.Managers
{
    public interface IApartmentManager
    {
        void AddApartment(Apartment apartment);
        //System.Collections.Generic.IEnumerable<RielAp.Domain.Models.Apartment> GetListOfApartments();
        void SetPhone(Apartment apartment, string phone);
        void SetStreet(Apartment apartment, string streetName);
        void SetWallMaterial(Apartment apartment, string wallMaterial);
        //int NewApartments { get; set; }
    }
}
