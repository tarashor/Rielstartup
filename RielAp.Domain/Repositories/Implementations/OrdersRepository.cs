using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class OrdersRepository : Repository<Order>, IOrdersRepository
    {
        public OrdersRepository(RealtyDBContext context)
            : base(context)
        {
        }

    }
}
