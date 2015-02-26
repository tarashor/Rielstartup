using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace RielAp.Web.Utils
{
    public class MailService
    {
        public static void SendAnnouncements(IEnumerable<string> addresses, IEnumerable<AnnouncementHtmlDescription> announcements)
        {
            StringBuilder mailBody = new StringBuilder();
            foreach(AnnouncementHtmlDescription announcement in announcements){
                mailBody.Append(announcement.GetHtmlDescription());
            }

            string mail = mailBody.ToString();

            foreach (string address in addresses) {
                SendEmail(address, Translation.Translation.MailServiceMailSubject, mail);
            }
        }

        public static void SendConfirmationEmail(string userEmail, string confirmUrl)
        {
            string body = string.Format(Translation.Translation.ConfirmationMailBody, ConfigurationManager.AppSettings["websiteurl"], confirmUrl);
            MailService.SendEmail(userEmail, Translation.Translation.ConfirmationMailTitle, body);
        }

        public static void SendTemporaryPassword(string userEmail, string temporaryPassword, DateTime temporaryPasswordExpires)
        {
            string body = string.Format(Translation.Translation.SendPasswordMailBody, ConfigurationManager.AppSettings["websiteurl"], temporaryPassword, temporaryPasswordExpires);
            MailService.SendEmail(userEmail, Translation.Translation.SendPasswordMailTitle, body);
        }

        public static bool SendEmail(string address, string subject, string body)
        {
            bool res = false;
            if (IsValid(address))
            {
                var fromAddress = new MailAddress("info@aparts.in.ua", Translation.Translation.WebsiteTitle);
                var toAddress = new MailAddress(address);

                var smtp = new SmtpClient();

                using (var mail = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(mail);
                }
            }
            return res;
        }

        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}