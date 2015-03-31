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
    public class FormLoginUserService: ILoginService
    {
        private IAuthProvider authProvider;

        public string Login(LoginViewModel model)
        {
            /*string phone = StringHelper.GetOnlyNumbers(model.Phone);
            if (phone.Length == 12)
            {
                AuthResult authResult = _authProvider.Authenticate(phone, model.Password);
                if (authResult == AuthResult.Success)
                {
                    User user = _userRepository.GetUserByPhone(phone);
                    if (user != null)
                    {
                        if (user.ProfileExpires.HasValue)
                        {
                            if (user.ProfileExpires.Value.CompareTo(DateTime.Now) < 0)
                            {
                                user.Profile = _profilesRepository.GetBasicProfile();
                                user.ProfileExpires = null;
                            }
                        }

                        if (!string.IsNullOrEmpty(user.TemporaryPassword))
                        {
                            user.Password = user.TemporaryPassword;
                            user.TemporaryPasswordExpires = null;
                            user.TemporaryPassword = null;
                        }

                        _userRepository.SaveChanges();
                        return RedirectToLocal(returnUrl);
                    }
                }
                else if (authResult == AuthResult.NoRights)
                {

                    message = Translation.Translation.AccessIsDeniedMessage;
                }
            }*/
            return Translation.Translation.AccessIsDeniedMessage;
        }
    }
}