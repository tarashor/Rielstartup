using HtmlAgilityPack;
using RielAp.Domain.Models;
using RielAp.Parser.Helper;
using RielAp.Parser.Loader;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RielAp.Parser.Parsers
{
    public class VashmagazinParser : IPhonesParser
    {
        protected readonly ILoader _pageLoader;
        protected readonly ILoger _loger;

        public VashmagazinParser(ILoader pageLoader, ILoger loger)
        {
            _pageLoader = pageLoader;
            _loger = loger;
        }

        public const string xpathRows = "/html[1]/body[1]/div[1]/div[3]/div[4]/table[1]/tr[1]/td[3]/table[2]/tbody[1]";
            //"/html[1]/body[1]/div[1]/div[1]/div[4]/table[1]/tr[1]/td[3]/table[2]/tbody[1]"; 
                                     //" /html[1]/body[1]/div[1]/div[1]/div[1]/table[3]/tr[1]/td[3]/table[2]/tbody[1]";

        public const string xpathErrorBlock = "/html[1]/body[1]/div[1]/div[3]/div[4]/table[1]/tbody/tr[1]/td[3]/table[2]/tbody[1]";
            //"/html[1]/body[1]/div[1]/table[3]/tr[1]/td[3]/table[3]/tr[1]/td[1]";
        protected const string xpathStreetCell = "/td";
        protected const string tdclassStreet = "top_border_table";
        public const string classAttribute = "class";
        public const string tagClassSeparator = "r_top_r";
        public const string classStreetCell = "top_border_table";
        public const string classErrorBlock = "error_block";




        protected bool tryGetPhone(HtmlNode phoneRow, out string phone)
        {
            bool res = false;
            phone = string.Empty;
            if (phoneRow != null)
            {
                phone = HtmlConverter.GetPlainTextFromNode(phoneRow);
                res = true;
            }
            return res;
        }

        protected bool isNewRow(HtmlNode row)
        {
            bool res = row.Attributes[classAttribute].Value == tagClassSeparator;
            return res;
        }

        protected bool containErrorBlock(HtmlDocument page)
        {
            bool containError = false;
            if (page != null)
            {
                HtmlNode errorBlock = page.DocumentNode.SelectSingleNode(xpathErrorBlock);
                if (errorBlock != null)
                {
                    HtmlAttribute valueClassAttribute = errorBlock.Attributes[classAttribute];
                    if (valueClassAttribute != null)
                    {
                        containError = valueClassAttribute.Value == classErrorBlock;
                    }
                }
            }
            return containError;
        }



        public IDictionary<VashmagazinPhone, int> GetPhones()
        {
            ConcurrentDictionary<VashmagazinPhone, int> agents = new ConcurrentDictionary<VashmagazinPhone, int>();

            IEnumerable<Rubrics> rubrics = Enum.GetValues(typeof(Rubrics)).Cast<Rubrics>();

            foreach (Rubrics rubric in rubrics)
            {
                bool hasRubricError = false;
                IEnumerable<Subrubrics> subrubrics = Enum.GetValues(typeof(Subrubrics)).Cast<Subrubrics>();
                foreach(Subrubrics subrubric in subrubrics){
                    int page = 1;
                    bool hasPageError = false;
                    do
                    {
                        string url = _pageLoader.GetUrl(rubric, subrubric, page);
                        string pageHtml = _pageLoader.LoadPageHtml(url);
                        ICollection<VashmagazinPhone> phones = parseHtmlPageForGetPhone(pageHtml, rubric, out hasPageError);
                        if (page == 1)
                        {
                            hasRubricError = hasPageError;
                        }

                        if (!hasPageError)
                        {
                            foreach (VashmagazinPhone phone in phones)
                            {
                                int amount;
                                if (agents.TryGetValue(phone, out amount))
                                {
                                    agents.TryUpdate(phone, amount+1, amount);
                                }
                                else
                                {
                                    agents.TryAdd(phone, 1);
                                }
                            }

                            _loger.Log(url);
                            _loger.Log("Total phones: " + agents.Count);
                            page++;
                        }
                    } while (!hasPageError);
                    if (hasRubricError) break;
                    
                } 

            }
            return agents;
        }

        protected ICollection<VashmagazinPhone> parseHtmlPageForGetPhone(string pageHtml, Rubrics rubric, out bool hasError)
        {
            ICollection<VashmagazinPhone> phones = new List<VashmagazinPhone>();

            HtmlDocument page = new HtmlDocument();
            page.LoadHtml(pageHtml);
            hasError = containErrorBlock(page);
            if (!hasError)
            {
                HtmlNode table = page.DocumentNode.SelectSingleNode(xpathRows);//xpathRows
                IEnumerable<HtmlNode> rows = table.Elements("tr");
                hasError = rows.Count() == 0;
                if (!hasError)
                {
                    IEnumerator<HtmlNode> currentRow = rows.GetEnumerator();
                    while (currentRow.MoveNext())
                    {
                        if (isNewRow(currentRow.Current))
                        {
                            if (currentRow.MoveNext())
                            {
                                string phone = getPhoneFromRow(currentRow);
                                VashmagazinPhone vashmagazinPhone = new VashmagazinPhone();
                                vashmagazinPhone.Phone = phone;
                                vashmagazinPhone.Rubric = rubric;
                                phones.Add(vashmagazinPhone);
                                currentRow.MoveNext();
                                currentRow.MoveNext();
                                currentRow.MoveNext();
                            }
                        }
                    }
                }
            }

            return phones;
        }

        protected string getPhoneFromRow(IEnumerator<HtmlNode> rowPointer)
        {
            string phone = null;


            if (rowPointer.MoveNext())
            {

                if (rowPointer.MoveNext())
                {
                    HtmlNode phoneRow = rowPointer.Current;
                    tryGetPhone(phoneRow, out phone);
                }
            }
            return phone;
        }
    }
}
