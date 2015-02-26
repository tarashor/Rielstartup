using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Web.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Infrastructure
{
    public class AdminPasswordValidator : UserPasswordValidator
    {
        public AdminPasswordValidator(IPasswordEncrypter passwordEncrypter)
            : base(passwordEncrypter)
        { }

        public override bool HasRights(User user)
        {
            bool hasRights = false;
            if (user != null)
            {
                hasRights = base.HasRights(user) && user.Role.HasAdminAccess;
            }
            return hasRights;
        }


    }
}