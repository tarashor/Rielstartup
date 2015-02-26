using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Infrastructure
{
    public class CurrentUser
    {
        public const string NAMECOOKIEKEY = "username";
        private static CurrentUser _instance;
        private CurrentUser() { }
        public static CurrentUser Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CurrentUser();
                }
                return _instance;
            }
        }

        public string Name
        {
            get
            {
                string name = null;
                HttpCookie nameCookie = HttpContext.Current.Request.Cookies[NAMECOOKIEKEY];
                if (nameCookie != null)
                {
                    name = convertASCIIToUnicode(nameCookie.Value);
                }
                return name;
            }

            set {
                setCookie(NAMECOOKIEKEY, convertUnicodeToASCII(value));
            }
        }

        private void setCookie(string key, string value)
        {
            HttpCookie cookie = new HttpCookie(key)
            {
                Value = value,
            };
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        private string convertUnicodeToASCII(string value)
        {
            System.Text.ASCIIEncoding encodingASCII = new System.Text.ASCIIEncoding();
            System.Text.UnicodeEncoding encodingUNICODE = new System.Text.UnicodeEncoding();
            byte[] sampleTextEncoded = encodingUNICODE.GetBytes(value);
            return HttpUtility.UrlEncode(encodingASCII.GetString(sampleTextEncoded));
        }

        private string convertASCIIToUnicode(string value)
        {
            System.Text.ASCIIEncoding encodingASCII = new System.Text.ASCIIEncoding();
            System.Text.UnicodeEncoding encodingUNICODE = new System.Text.UnicodeEncoding();

            byte[] sampleTextEncoded = encodingASCII.GetBytes(HttpUtility.UrlDecode(value));
            return encodingUNICODE.GetString(sampleTextEncoded);
        }
    }
}