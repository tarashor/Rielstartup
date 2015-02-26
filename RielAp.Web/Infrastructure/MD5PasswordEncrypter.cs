using RielAp.Core;
using RielAp.Web.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace RielAp.Web.Infrastructure
{
    public class MD5PasswordEncrypter:IPasswordEncrypter
    {
        private MD5 hasher = null;
        public MD5PasswordEncrypter() {
            hasher = MD5.Create();
        }
        public string EncryptPassword(string password)
        {
            return SecurityHelper.GetMd5Hash(hasher, password);
        }

        public void Dispose()
        {
            if (hasher != null) {
                hasher.Dispose();
            }
        }
    }
}