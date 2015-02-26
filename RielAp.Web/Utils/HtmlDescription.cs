using RielAp.Domain.Models;
using RielAp.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace RielAp.Web.Utils
{
    public abstract class AnnouncementHtmlDescription
    {
        public Announcement Announcement { get; set; }
        public virtual string GetHtmlDescription()
        {
            if (Announcement != null)
            {
                return string.Format("<a href=\"{0}\">{1}</a>", AnnouncementUrl(), Announcement.Address.ToString());
            }
            return string.Empty;
        }

        private  string AnnouncementUrl()
        {
            return  ConfigurationManager.AppSettings["websiteurl"] + "/Announcements/" + getDetailsAction() + "/" + Announcement.AnnouncementID.ToString();
        }

        protected abstract string getDetailsAction();

    }

    public class ApartmentHtmlDescription : AnnouncementHtmlDescription
    {
        public override string GetHtmlDescription()
        {
            ApartmentAnnouncement apartment = Announcement as ApartmentAnnouncement;
            string description = string.Empty;
            if (apartment!= null){
                description = string.Format("{0}: {1}<br/>", Translation.Translation.ApartmentDetailsPageRoomsCountLabel, apartment.RoomsNumber);
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.ApartmentDetailsPageSquareLabel, apartment.TotalSquare.Value, EnumsToSelectedListItems.GetTextFromEnumValue(apartment.TotalSquare.Unit));
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.ApartmentDetailsPagePriceLabel, apartment.Price, EnumsToSelectedListItems.GetTextFromEnumValue(apartment.Currency));
            }
            return Translation.Translation.ApartmentAnnouncementLabel + " " +base.GetHtmlDescription() + "<br/>" + description;
        }

        protected override string getDetailsAction()
        {
            return "ApartmentDetails";
        }


    }

    public class LandHtmlDescription : AnnouncementHtmlDescription
    {
        public override string GetHtmlDescription()
        {
            LandAnnouncement land = Announcement as LandAnnouncement;
            string description = string.Empty;
            if (land != null)
            {
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.LandsDetailsPageSquareLabel, land.TotalSquare.Value, EnumsToSelectedListItems.GetTextFromEnumValue(land.TotalSquare.Unit));
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.ApartmentDetailsPagePriceLabel, land.Price, EnumsToSelectedListItems.GetTextFromEnumValue(land.Currency));
            }
            return Translation.Translation.LandAnnouncementLabel + " "+ base.GetHtmlDescription() + "<br/>" + description;
        }
        protected override string getDetailsAction()
        {
            return "LandDetails";
        }

    }

    public class HouseHtmlDescription : AnnouncementHtmlDescription
    {
        public override string GetHtmlDescription()
        {
            HouseAnnouncement house = Announcement as HouseAnnouncement;
            string description = string.Empty;
            if (house != null)
            {
                description = string.Format("{0}: {1}<br/>", Translation.Translation.ApartmentDetailsPageRoomsCountLabel, house.RoomsNumber);
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.LandsDetailsPageSquareLabel, house.TotalSquare.Value, EnumsToSelectedListItems.GetTextFromEnumValue(house.TotalSquare.Unit));
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.HousesDetailsPageSquareLabel, house.LivingSquare.Value, EnumsToSelectedListItems.GetTextFromEnumValue(house.LivingSquare.Unit));
                description += string.Format("{0}: {1} {2}<br/>", Translation.Translation.ApartmentDetailsPagePriceLabel, house.Price, EnumsToSelectedListItems.GetTextFromEnumValue(house.Currency));
            }
            return Translation.Translation.LandAnnouncementLabel + " " + base.GetHtmlDescription() + "<br/>" + description;
        }
        protected override string getDetailsAction()
        {
            return "HouseDetails";
        }

    } 
}