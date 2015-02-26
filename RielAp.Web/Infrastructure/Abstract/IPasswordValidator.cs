using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Web.Infrastructure.Abstract
{
    public interface IPasswordValidator:IDisposable
    {
        bool IsPasswordValid(string password, User user);
        bool HasRights(User user);
    }
}
