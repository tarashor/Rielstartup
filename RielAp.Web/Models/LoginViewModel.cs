using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class LoginViewModel
    {
        [LocalizedRequired("LoginPageUserPhone")]
        [Display(Name = "User phone number")]
        public string Phone { get; set; }

        [LocalizedRequired("LoginPagePassword")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}