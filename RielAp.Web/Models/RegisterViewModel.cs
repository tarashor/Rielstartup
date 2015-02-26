using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class RegisterViewModel
    {
        private const int MinimumPasswordLength = 4;

        [LocalizedRequired("RegisterPageTelephoneNumberLabel")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "User phone number")]
        public string Phone { get; set; }

        [LocalizedRequired("RegisterPageUserNameLabel")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [LocalizedRequired("RegisterPagePassword")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [LocalizedRequired("RegisterPageEmailLabel")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Receive emails")]
        public bool CanReceiveEmail { get; set; }

        public bool IsValid(out string errorMessage) {
            errorMessage = null;
            bool res = true;
            if (Password.Length < MinimumPasswordLength)
            {
                res = false;
                errorMessage = Translation.Translation.RegisterPagePasswordTooShortError;
            }
            else {
                if (Password != ConfirmPassword)
                {
                    res = false;
                    errorMessage = Translation.Translation.RegisterPageComparePasswordsError;
                }
            }

            if (res) {
                if (!MailService.IsValid(Email))
                {
                    res = false;
                    errorMessage = Translation.Translation.RegisterPageBadEmailError;
                }
            }
            return res;
        }
    }
}