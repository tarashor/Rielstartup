using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories
{
    public interface IProfilesRepository:IRepository<Profile>
    {
        Profile GetBasicProfile();
        Profile GetProfileByName(string profileName);
        IEnumerable<Profile> GetAll();
    }
}
