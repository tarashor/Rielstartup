using RielAp.Domain.Models;
using RielAp.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace RielAp.Web.Utils
{
    public class ObjectToCookie
    {
        public static TObject GetObjectFromCookies<TObject>(HttpCookieCollection cookies, string cookiekey)
        {
            TObject filter = default(TObject);
            HttpCookie filterCookie = cookies[cookiekey];
            if (filterCookie != null)
            {
                byte[] buffer = Convert.FromBase64String(filterCookie.Value);
                Stream myStream = new MemoryStream(buffer);
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    filter = (TObject)formatter.Deserialize(myStream);
                }
                finally
                {
                    myStream.Close();
                }
            }
            return filter;
        }

        public static void SetCookieAsObject<TObject>(TObject obj, HttpCookieCollection cookies, string cookiekey)
        {
            HttpCookie cookie = new HttpCookie(cookiekey);
            Stream myStream = new MemoryStream();
            try
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(myStream, obj);
                myStream.Seek(0, SeekOrigin.Begin);
                byte[] buffer = new byte[myStream.Length];
                myStream.Read(buffer, 0, (int)myStream.Length);
                cookie.Value = Convert.ToBase64String(buffer);
                cookies.Add(cookie);
            }
            finally
            {
                myStream.Close();
            }
        }
    }
}