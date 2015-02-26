using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Web.Infrastructure.Abstract
{
    public interface IPasswordEncrypter:IDisposable
    {
        string EncryptPassword(string password);
    }
}
