using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class ChangePasswordModel
    {
        private const int MinimumPasswordLength = 4;

        [LocalizedRequired("UserProfileOldPasswordLabel")]
        [DataType(DataType.Password)]
        [Display(Name = "Old password")]
        public string OldPassword { get; set; }

        [LocalizedRequired("UserProfileNewPasswordLabel")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        public string ConfirmPassword { get; set; }

        public bool IsValid(out string errorMessage)
        {
            errorMessage = null;
            bool res = true;
            if (Password.Length < MinimumPasswordLength)
            {
                res = false;
                errorMessage = Translation.Translation.RegisterPagePasswordTooShortError;
            }
            else
            {
                if (Password != ConfirmPassword)
                {
                    res = false;
                    errorMessage = Translation.Translation.RegisterPageComparePasswordsError;
                }
            }

            return res;
        }
    }
}