using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace RielAp.Web.Utils
{
    public class LiqPay
    {
        public static string Merch_id = "i4997382528";
        public static string Merch_sign = "5Hsl6MdAJ1UOVf8oaioVl0zTrc5aW";

        public static XmlDocument GetXmlDocument(
            decimal sum,
            string currency,
            string url_CSuccess,
            string url_PSuccess,
            string order_id,
            string description,
            string default_phone,
            string pay_way,
            int goods_id
            )
        {
            return GetXmlDocument(
                Merch_id,
                sum,
                currency,
                url_CSuccess,
                url_PSuccess,
                order_id,
                description,
                default_phone,
                pay_way,
                goods_id
                );
        }

        public static XmlDocument GetXmlDocument(
            string merch_id,
            decimal sum,
            string currency,
            string url_CSuccess,
            string url_PSuccess,
            string order_id,
            string description,
            string default_phone,
            string pay_way,
            int goods_id
            )
        {
            XmlDocument doc = new XmlDocument();
            XmlNode item;
            XmlNode request = doc.CreateNode(XmlNodeType.Element, "request", "");
            doc.AppendChild(request);

            item = doc.CreateNode(XmlNodeType.Element, "version", "");
            item.InnerText = "1.2";
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "merchant_id", "");
            item.InnerText = merch_id;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "result_url", "");
            item.InnerText = url_CSuccess;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "server_url", "");
            item.InnerText = url_PSuccess;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "order_id", "");
            item.InnerText = order_id;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "amount", "");
            item.InnerText = sum.ToString().Replace(",", ".");
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "currency", "");
            item.InnerText = currency;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "description", "");
            item.InnerText = description;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "default_phone", "");
            item.InnerText = default_phone;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "pay_way", "");
            item.InnerText = pay_way;
            request.AppendChild(item);

            item = doc.CreateNode(XmlNodeType.Element, "goods_id", "");
            item.InnerText = goods_id.ToString();
            request.AppendChild(item);

            return doc;
        }


        public static string GetOperation_xml(XmlDocument doc)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(doc.InnerXml));
        }


        public static string GetSignature(XmlDocument doc)
        {
            return GetSignature(doc, Merch_sign);
        }

        public static string GetSignature(string doc)
        {
            return GetSignature(doc, Merch_sign);
        }

        public static string GetSignature(XmlDocument doc, string merch_sign)
        {
            return GetSignature(doc.InnerXml, merch_sign);
        }

        public static string GetSignature(string doc, string merch_sign)
        {
            string id = merch_sign + doc + merch_sign;

            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            byte[] b = cryptoTransformSHA1.ComputeHash(Encoding.UTF8.GetBytes(id));
            string hash = Convert.ToBase64String(b);

            return hash;
        }

        public static string GetOperationXmlFromAnswer(string operation_xml)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(operation_xml));
        }

        public static string GetSignFromAnswer(string operation_xml)
        {
            return LiqPay.GetSignature(Encoding.UTF8.GetString(Convert.FromBase64String(operation_xml)));
        }
    }
}