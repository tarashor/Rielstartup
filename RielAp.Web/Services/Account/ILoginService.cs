using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Web.Models;
using System.Threading.Tasks;

namespace RielAp.Web.Services.Account
{
    interface ILoginService
    {
        bool Login(string userPhone, string password);

        void Logout();
    }
}
