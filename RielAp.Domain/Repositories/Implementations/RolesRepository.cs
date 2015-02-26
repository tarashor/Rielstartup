using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class RolesRepository : Repository<Role>, IRolesRepository
    {
        public RolesRepository(RealtyDBContext context)
            : base(context)
        {
        }
        
    }
}
