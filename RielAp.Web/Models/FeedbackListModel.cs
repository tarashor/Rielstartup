using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Models
{
    public class FeedbackListModel
    {
        public IEnumerable<Email> FeedbacksPerPage { get; set; }
        public PageInfo Page { get; set; }
    }
}