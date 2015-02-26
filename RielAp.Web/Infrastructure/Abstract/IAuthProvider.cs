using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Web.Infrastructure.Abstract
{
    public interface IAuthProvider:IDisposable
    {
        AuthResult Authenticate(string phoneNumber, string password);
        AuthResult Authenticate(User user);
    }
}
