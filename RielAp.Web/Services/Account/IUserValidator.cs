using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RielAp.Web.Services.Account
{
    public interface IUserValidator
    {
        bool Validate(string phone);
    }
}
