using HtmlAgilityPack;
using RielAp.Domain.Models;
using RielAp.Domain.Repositories;
using RielAp.Parser.Helper;
using RielAp.Parser.Loader;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Parser.Parsers
{
    public abstract class VashmagazinAnnouncementsParser : VashmagazinParser
    {
        public const string CITY = "Львів";


        protected IVashmagazinRepository _vashmagazinRepository;

        public VashmagazinAnnouncementsParser(ILoader pageLoader, IVashmagazinRepository vashmagazinRepository, ILoger loger)
            : base(pageLoader, loger)
        {
            _vashmagazinRepository = vashmagazinRepository;
        }


        protected void setApartmentProperties(IEnumerator<HtmlNode> rowPointer, VashmagazinApartment apartment)
        {
            string streetName;

            if (tryGetStreetName(rowPointer.Current, out streetName))
            {
                apartment.Address.Street = streetName;
                if (rowPointer.MoveNext())
                {
                    HtmlNode infoRow = rowPointer.Current;
                    Square square;
                    int floor;
                    int maxFloor;
                    decimal price;
                    int roomsNumber;
                    Currency currency;
                    if (tryGetSquare(infoRow, out square))
                    {
                        apartment.TotalSquare = square;
                    }

                    if (tryGetRoomsNumber(infoRow, out roomsNumber))
                    {
                        apartment.RoomsCount = roomsNumber;
                    }
                    if (tryGetFloor(infoRow, out floor, out maxFloor))
                    {
                        apartment.Floor = floor;
                        apartment.MaxFloor = maxFloor;
                    }
                    if (tryGetPrice(infoRow, out price, out currency))
                    {
                        apartment.Price = price;
                        apartment.Currency = currency;
                    }
                    if (rowPointer.MoveNext())
                    {
                        HtmlNode phoneRow = rowPointer.Current;
                        string phone;
                        if (tryGetPhone(phoneRow, out phone))
                        {
                            apartment.Phone = phone;
                        }
                    }
                }

            }
        }

        protected bool tryGetRoomsNumber(HtmlNode infoRow, out int roomsNumber)
        {
            roomsNumber = 0;
            bool res = false;
            HtmlNode roomsCell = getCellByIndex(infoRow, 1);
            if (roomsCell != null)
            {
                HtmlNode roomSpan = roomsCell.Element("span");
                if (roomSpan != null)
                {
                    HtmlNode roomSpanSpan = roomSpan.Element("span");
                    if (roomSpanSpan != null)
                    {
                        res = int.TryParse(roomSpanSpan.InnerText.Trim(), out roomsNumber);
                    }
                }
            }
            return res;
        }

        protected bool tryGetNotice(HtmlNode noticeRow, out string notice)
        {
            bool res = false;
            notice = string.Empty;
            if (noticeRow != null)
            {
                notice = HtmlConverter.GetPlainTextFromNode(noticeRow);
                res = true;
            }
            return res;
        }

        protected bool tryGetPrice(HtmlNode infoRow, out decimal price, out Currency currency)
        {
            price = 0;
            currency = Currency.Dollar;
            bool res = false;
            HtmlNode priceCell = getCellByIndex(infoRow, 4);
            if (priceCell != null)
            {
                HtmlNode priceNode = priceCell.Descendants("b").FirstOrDefault();
                if (priceNode != null)
                {
                    string priceString = HtmlConverter.RemoveWhiteSpaces(HtmlConverter.GetPlainTextFromNode(priceNode));
                    priceString = HtmlConverter.RemoveSpaces(priceString);
                    priceString = priceString.Replace("$", "");
                    res = decimal.TryParse(priceString, out price);
                }

                HtmlNode currencyNode = priceCell.Element("span");
                if (currencyNode != null)
                {
                    string currencyString = HtmlConverter.RemoveWhiteSpaces(HtmlConverter.GetPlainTextFromNode(currencyNode));
                    currency = getCurrency(currencyString);
                }
            }
            return res;
        }

        protected Currency getCurrency(string c)
        {
            Currency currency = Currency.Hryvna;
            if (c == "грн.")
            {
                currency = Currency.Hryvna;
            }
            else
            {
                if (c == "дол.")
                {
                    currency = Currency.Dollar;
                }
            }
            return currency;
        }

        protected char floorSpliter = '/';
        protected bool tryGetFloor(HtmlNode infoRow, out int floor, out int maxFloor)
        {
            floor = 0;
            maxFloor = 0;
            bool res = false;
            HtmlNode floorCell = getCellByIndex(infoRow, 2);
            if (floorCell != null)
            {
                string floorInfo = floorCell.InnerText;
                if (floorInfo.Contains(floorSpliter))
                {
                    string[] floors = floorInfo.Split(floorSpliter);
                    if (floors.Length == 2)
                    {
                        res = int.TryParse(floors[0].Trim(), out floor);
                        int.TryParse(floors[1].Trim(), out maxFloor);
                    }
                }
            }
            return res;
        }

        private bool tryGetWallsMaterial(HtmlNode infoRow, out string material)
        {
            material = string.Empty;
            bool res = false;
            HtmlNode materialCell = getCellByIndex(infoRow, 1);
            if (materialCell != null)
            {
                HtmlNode materialNodeImg = materialCell.Element("img");
                if (materialNodeImg != null)
                {
                    HtmlNode materialNode = materialNodeImg.NextSibling;
                    while ((materialNode != null) && ((materialNode.Name != "#text") || (materialNode.InnerText.Trim().Length == 0)))
                    {
                        materialNode = materialNode.NextSibling;
                    }
                    if (materialNode != null)
                    {
                        material = materialNode.InnerText.Trim();
                        res = true;
                    }
                }
            }
            return res;
        }

        #region - streetName -

        protected bool tryGetStreetName(HtmlNode row, out string streetName)
        {
            bool res = false;
            HtmlNode cell = getCellFromRow(row, isStreetNameCell);
            streetName = string.Empty;
            if (cell != null)
            {
                streetName = HtmlConverter.GetPlainTextFromNode(cell);
                res = true;
            }
            return res;
        }

        protected bool isStreetNameCell(HtmlNode cell)
        {
            bool res = false;
            if (cell != null)
            {
                HtmlAttribute valueClassAttribute = cell.Attributes[classAttribute];
                if (valueClassAttribute != null)
                {
                    res = valueClassAttribute.Value == classStreetCell;
                }
            }
            return res;
        }

        protected HtmlNode getCellFromRow(HtmlNode row, Predicate<HtmlNode> condition)
        {
            HtmlNode cell = null;
            if (row.HasChildNodes)
            {
                foreach (HtmlNode child in row.ChildNodes)
                {
                    if (condition(child))
                    {
                        cell = child;
                        break;
                    }
                }
            }
            return cell;
        }

        #endregion

        #region - square -
        protected char squareSpliter = '/';

        protected bool tryGetSquare(HtmlNode row, out Square square)
        {
            square = new Square();
            square.Unit = SquareUnit.SquareMeters;
            square.Value = 0;
            bool res = false;
            HtmlNode squareCell = getCellByIndex(row, 3);
            if (squareCell != null)
            {
                string wholeSquare = squareCell.InnerText;
                if (wholeSquare.Contains(squareSpliter))
                {
                    string[] squares = wholeSquare.Split(squareSpliter);
                    wholeSquare = squares[0].Trim();
                }
                float floatSquare;
                var format = new NumberFormatInfo();

                format.NumberDecimalSeparator = ",";
                res = float.TryParse(wholeSquare, NumberStyles.Float, format, out floatSquare);

                square.Value = floatSquare;
            }
            return res;
        }

        protected HtmlNode getCellByIndex(HtmlNode row, int index)
        {
            HtmlNode cell = null;
            if (row.HasChildNodes)
            {
                int count = 0;
                foreach (HtmlNode child in row.ChildNodes)
                {
                    if (child.Name.ToLower() == "td")
                    {
                        count++;
                        if (count == index)
                        {
                            cell = child;
                            break;
                        }
                    }
                }
            }
            return cell;
        }

        #endregion

        protected IEnumerable<HtmlNode> getRowsWithApartmentsData(string pageHtml)
        {
            IEnumerable<HtmlNode> rows = null;

            HtmlDocument page = new HtmlDocument();
            page.LoadHtml(pageHtml);
            bool hasError = containErrorBlock(page);
            if (!hasError)
            {
                HtmlNode table = page.DocumentNode.SelectSingleNode(xpathRows);//xpathRows
                rows = table.Elements("tr");
            }
            return rows;
        }

        protected string getDistrictFromSubrubric(Subrubrics subrubric)
        {
            string district = string.Empty;
            switch (subrubric)
            {
                case Subrubrics.Frankivskyj: district = "Франківський"; break;
                case Subrubrics.Galyckyj: district = "Галицький"; break;
                case Subrubrics.Shevchenkivskyj: district = "Шевченківський"; break;
                case Subrubrics.Zaliznychnyj: district = "Залізничний"; break;
                case Subrubrics.Syhivskyj: district = "Сихівський"; break;
                case Subrubrics.Lychakivskyj: district = "Личаківський"; break;
            }
            return district;
        }
    }
}
