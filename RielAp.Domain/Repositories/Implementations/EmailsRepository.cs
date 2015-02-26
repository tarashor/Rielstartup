using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class EmailsRepository : Repository<Email>, IEmailsRepository
    {
        public EmailsRepository(RealtyDBContext context)
            : base(context)
        {
        }

        public IEnumerable<Email> GetVisibleParentEmails()
        {
            return Context.Emails.Where(e => (e.ParentEmailId == null && e.IsShown));
        }
    }
}
