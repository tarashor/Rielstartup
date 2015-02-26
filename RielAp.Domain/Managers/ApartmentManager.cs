using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;

namespace RielAp.Domain.Managers
{
    public class ApartmentManager : RielAp.Domain.Managers.IApartmentManager 
    {
        private IUserRepository _userRepository;
        private IWallMaterialRepository _wallMaterialRepository;
        private IApartmentRepository _apartmentRepository;

        public ApartmentManager(IUserRepository userRepository, IWallMaterialRepository wallMaterialRepository, IApartmentRepository apartmentRepository) 
        {
            _userRepository = userRepository;
            _wallMaterialRepository = wallMaterialRepository;
            _apartmentRepository = apartmentRepository;
            NewApartments = 0;
        }

        public void SetStreet(Apartment apartment, string streetName) {
            Street street = _streetRepository.GetStreetByName(streetName);
            if (street == null) { 
               street = new Street() { StreetName = streetName };
               _streetRepository.Add(street);
               _streetRepository.SaveChanges();
            }
            apartment.Street = street;
        }

        public void SetWallMaterial(Apartment apartment, string wallMaterial) {
            WallMaterial material = _wallMaterialRepository.GetWallMaterialByName(wallMaterial);
            if (material == null)
            {
                material = new WallMaterial() { WallMaterialName = wallMaterial };
                _wallMaterialRepository.Add(material);
                _wallMaterialRepository.SaveChanges();
            }
            apartment.WallMaterial = material;
        }
        public void SetPhone(Apartment apartment, string phone) {
            User user = _userRepository.GetUserByPhone(phone);
            if (user == null)
            {
                user = new User() { Phone = phone };
                _userRepository.Add(user);
                _userRepository.SaveChanges();
            }
            apartment.User = user;
        }

        public void AddApartment(Apartment apartment) {
            Apartment existingApartment = FindApartment(apartment);
            if (existingApartment == null)
            {
                apartment.Sold = false;
                _apartmentRepository.Add(apartment);
                NewApartments++;
            }
            else
            {
                existingApartment.Price = apartment.Price;
                existingApartment.Currency = apartment.Currency;
                existingApartment.Notice = apartment.Notice;
                existingApartment.Sold = false;

                _apartmentRepository.SetModified(existingApartment);
            }
            _apartmentRepository.SaveChanges();
        }

        private Apartment FindApartment(Apartment apartment)
        {
            Apartment res = null;
            User user = _userRepository.GetUserByPhone(apartment.User.Phone);
            Street street = _streetRepository.GetStreetByName(apartment.Address.Street);
            if ((user != null) && (street != null))
            {
                res = _apartmentRepository.FindApartment(street.StreetID, agent.AgentID, apartment.Floor, apartment.Square);
            }
            return res;
        }

        public IEnumerable<Apartment> GetListOfApartments() {
            return _apartmentRepository.GetListOfApartments();
        }


        public int NewApartments
        {
            get;
            set;
        }
    }
}
