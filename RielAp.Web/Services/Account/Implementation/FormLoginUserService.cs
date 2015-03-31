using RielAp.Core;
using RielAp.Domain.Models;
using RielAp.Web.Infrastructure;
using RielAp.Web.Infrastructure.Abstract;
using RielAp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Services.Account.Implementation
{
    public class FormLoginUserService : ILoginService
    {
        private IAuthProvider authProvider;
        private IUserValidator userValidator;

        public bool Login(string userPhone, string password)
        {
            string phone = StringHelper.GetOnlyNumbers(userPhone);
            if (phone.Length == 12)
            {
                AuthResult authResult = authProvider.Authenticate(phone, password);
                if (authResult == AuthResult.Success)
                {
                    return userValidator.Validate(phone);
                }
                /*
                    if (!string.IsNullOrEmpty(user.TemporaryPassword))
                    {
                        user.Password = user.TemporaryPassword;
                        user.TemporaryPasswordExpires = null;
                        user.TemporaryPassword = null;
                    }
                    _userRepository.SaveChanges();
                */
            }
            return false;
        }


        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }

}