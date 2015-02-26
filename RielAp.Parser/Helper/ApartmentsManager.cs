using RielAp.AddressService.Services;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RielAp.Parser.Helper
{
    public class ApartmentsManager : IApartmentsManager
    {
        private string CITY_NAME = "Львів";
        private IUsersRepository _userRepository;
        private IAnnouncementsRepository _apartmentRepository;
        private IAddressService _addressService;

        public ApartmentsManager(IUsersRepository userRepository, IAnnouncementsRepository apartmentRepository, IAddressService addressService)
        {
            _userRepository = userRepository;
            _apartmentRepository = apartmentRepository;
            _addressService = addressService;
            NewApartments = 0;
        }

        private Address GetAddressFromStreetName(string streetName)
        {
            Address address = new Address();
            address.LocalityName = CITY_NAME;
            string street = _addressService.GetStreet(streetName, CITY_NAME);
            if (street != null)
            {
                address.Street = _addressService.GetStreet(streetName, CITY_NAME);
            }
            
            return address;
        }

        public void SetStreet(ApartmentAnnouncement apartment, string streetName)
        {
            Address address = GetAddressFromStreetName(streetName);
            apartment.Address = address;
        }

        public void SetPhone(ApartmentAnnouncement apartment, string phone)
        {
            User user = _userRepository.GetUserByPhone(phone);
            if (user == null)
            {
                user = new User() { Phone = phone };
                _userRepository.Add(user);
                _userRepository.SaveChanges();
            }
            apartment.User = user;
        }

        public void AddApartment(ApartmentAnnouncement apartment)
        {
            ApartmentAnnouncement existingApartment = FindApartment(apartment);
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

        private ApartmentAnnouncement FindApartment(ApartmentAnnouncement apartment)
        {
            ApartmentAnnouncement res = null;
            if (apartment.Address != null)
            {
                User user = _userRepository.GetUserByPhone(apartment.User.Phone);

                Address address = GetAddressFromStreetName(apartment.Address.Street);
                if ((user != null) && (address != null))
                {
                    res = _apartmentRepository.FindApartment(address.Street, user.UserID, apartment.Floor, apartment.LivingSquare);
                }
            }
            return res;
        }

        public IEnumerable<ApartmentAnnouncement> GetListOfApartments()
        {
            return _apartmentRepository.GetListOfApartments();
        }


        public int NewApartments
        {
            get;
            set;
        }

    }
}
