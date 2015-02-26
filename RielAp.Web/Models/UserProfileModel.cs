using RielAp.Web.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace RielAp.Web.Models
{
    public class UserProfileModel
    {
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Receive emails")]
        public bool CanReceiveEmail { get; set; }

        public bool IsValid(out string errorMessage)
        {
            errorMessage = null;
            bool res = true;
            

            if (res)
            {
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
