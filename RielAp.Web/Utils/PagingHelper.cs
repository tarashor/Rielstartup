using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using RielAp.Web.Models;

namespace RielAp.Web.Utils
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pagingInfo, Func<int, string> pageUrl)
        {
            TagBuilder pagesPlaceholder = new TagBuilder("div");
            pagesPlaceholder.AddCssClass("pagination pagination-centered");

            TagBuilder pagesList = new TagBuilder("ul");

            StringBuilder pageListHtml = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder page = new TagBuilder("li");
                TagBuilder pageLink = new TagBuilder("a"); // Construct an <a> tag
                pageLink.MergeAttribute("href", pageUrl(i));
                pageLink.InnerHtml = i.ToString();
                page.InnerHtml = pageLink.ToString();
                if (i == pagingInfo.CurrentPage)
                    page.AddCssClass("active");
                pageListHtml.Append(page.ToString());
            }

            pagesList.InnerHtml = pageListHtml.ToString();

            pagesPlaceholder.InnerHtml = pagesList.ToString();
            return MvcHtmlString.Create(pagesPlaceholder.ToString());
        }
    }
}