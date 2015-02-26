using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class WallMaterialRepository: Repository<WallMaterial>, IWallMaterialsRepository
    {
        public WallMaterialRepository(RealtyDBContext context)
            : base(context)
        {
        }

        public WallMaterial GetWallMaterialByName(string wallMaterialName)
        {
            WallMaterial wallMaterial = null;
            var wallMaterials = SearchFor(s => s.WallMaterialName == wallMaterialName);
            if (wallMaterials.Count() > 0)
            {
                wallMaterial = wallMaterials.First();
            }
            return wallMaterial;
        }
    }
}
