using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class ProfilesRepository : Repository<Profile>, IProfilesRepository
    {
        public ProfilesRepository(RealtyDBContext context)
            : base(context)
        {
        }


        public Profile GetBasicProfile()
        {
            Profile basic = null;
            var profiles = SearchFor(p => p.IsBasic);
            if (profiles.Count() > 0)
            {
                basic = profiles.First();
            }
            return basic;
        }


        public Profile GetProfileByName(string profileName)
        {
            Profile profile = null;
            var profiles = SearchFor(p => p.ProfileName == profileName);
            if (profiles.Count() > 0)
            {
                profile = profiles.First();
            }
            return profile;
        }


        public IEnumerable<Profile> GetAll()
        {
            return Context.Profiles.OrderBy(p => p.Priority);
        }
    }
}
