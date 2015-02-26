using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RielAp.Web.Areas.Admin.Models
{
    public class UsersViewModel
    {
        public IEnumerable<User> Users { get; set; }
    }
}