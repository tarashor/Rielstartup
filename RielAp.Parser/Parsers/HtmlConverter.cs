using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RielAp.Parser.Parsers
{
    public class HtmlConverter
    {
        public static string GetPlainTextFromNode(HtmlNode node)
        {
            return Regex.Replace(WebUtility.HtmlDecode(node.InnerText), "<!--.*?-->", String.Empty, RegexOptions.Singleline).Trim();
        }

        public static string RemoveWhiteSpaces(string str)
        {
            return str.Replace(((char)160).ToString(), string.Empty);
        }

        public static string RemoveSpaces(string str)
        {
            return str.Replace(((char)32).ToString(), string.Empty);
        }
    }
}
