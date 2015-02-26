using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class MobileNumbersRepository : Repository<MobileNumber>, IMobileNumbersRepository
    {
        public MobileNumbersRepository(RealtyDBContext context)
            : base(context)
        {
        }

    }
}
