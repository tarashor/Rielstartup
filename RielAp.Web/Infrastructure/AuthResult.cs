using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Infrastructure
{
    public enum AuthResult
    {
        Success,
        NoRights,
        WrongCreadentials
    }
}