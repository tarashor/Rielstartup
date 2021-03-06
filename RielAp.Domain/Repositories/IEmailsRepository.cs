﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface IEmailsRepository : IRepository<Email>
    {
        IEnumerable<Email> GetVisibleParentEmails();
    }
}
