using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Infrastructure
{
    public class UserPasswordValidator:IPasswordValidator
    {
        protected readonly IPasswordEncrypter _passwordEncrypter;
        public UserPasswordValidator(IPasswordEncrypter passwordEncrypter)
        {
            _passwordEncrypter = passwordEncrypter;
        }

        public virtual bool HasRights(User user)
        {
            bool hasRights = false;
            if (user != null)
            {
                hasRights = user.Role.HasBasicAccess;
            }
            return hasRights;
        }

        public bool IsPasswordValid(string password, User user)
        {
            string md5Password = _passwordEncrypter.EncryptPassword(password);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            bool isValid = (0 == comparer.Compare(md5Password, user.Password)) || (user.TemporaryPasswordExpires.HasValue && (user.TemporaryPasswordExpires.Value.CompareTo(DateTime.Now) > 0) && (0 == comparer.Compare(md5Password, user.TemporaryPassword)));
            return isValid;
        }


        public void Dispose()
        {
            _passwordEncrypter.Dispose();
        }


        
    }
}