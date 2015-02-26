using RielAp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RielAp.Domain.Repositories.Implementations
{
    public class PhotosRepository : Repository<Photo>, IPhotosRepository
    {
        public PhotosRepository(RealtyDBContext context)
            : base(context)
        {
        }
    }
}
