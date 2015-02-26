using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class FeedbacksRepository : Repository<Feedback>, IFeedbacksRepository
    {
        public FeedbacksRepository(RealtyDBContext context)
            : base(context)
        {
        }

    }
}
