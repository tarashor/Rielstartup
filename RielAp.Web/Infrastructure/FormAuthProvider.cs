using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using RielAp.Web.Infrastructure.Abstract;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Core;

namespace RielAp.Web.Infrastructure
{
    public class FormAuthProvider : IAuthProvider
    {
        private readonly IPasswordValidator _passwordValidator;
        private readonly IUsersRepository _usersRepository;

        public FormAuthProvider(IPasswordValidator passwordValidator, IUsersRepository usersRepository)
        {
            _passwordValidator = passwordValidator;
            _usersRepository = usersRepository;
        }

        public AuthResult Authenticate(string phoneNumber, string password)
        {
            AuthResult result = AuthResult.WrongCreadentials;
            User user = _usersRepository.GetUserByPhone(phoneNumber);
            if (user != null)
            {
                bool isPasswordValid = _passwordValidator.IsPasswordValid(password, user);
                bool hasRights = _passwordValidator.HasRights(user);
                if (isPasswordValid && hasRights)
                {
                    FormsAuthentication.SetAuthCookie(phoneNumber, false);
                    CurrentUser.Instance.Name = user.Username;
                    result = AuthResult.Success;
                }
                else {
                    if (isPasswordValid) {
                        result = AuthResult.NoRights;
                    }
                }
            }
            return result;
        }


        public AuthResult Authenticate(User user)
        {
            AuthResult result = AuthResult.WrongCreadentials;
            if (user != null)
            {
                bool hasRights = _passwordValidator.HasRights(user);
                if (hasRights)
                {
                    FormsAuthentication.SetAuthCookie(user.Phone, false);
                    CurrentUser.Instance.Name = user.Username;
                    result = AuthResult.Success;
                }
                else
                {
                    result = AuthResult.NoRights;
                }
            }
            return result;
        }

        public void Dispose()
        {
            _passwordValidator.Dispose();
            _usersRepository.Dispose();
        }



        
    }
}