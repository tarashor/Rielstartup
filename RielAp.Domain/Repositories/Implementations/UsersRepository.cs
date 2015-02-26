using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RielAp.Domain.Models;

namespace RielAp.Domain.Repositories.Implementations
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        public UsersRepository(RealtyDBContext context)
            : base(context)
        {
        }


        public User GetUserByName(string username)
        {
            User user = null;
            if (!string.IsNullOrEmpty(username))
            {
                var users = SearchFor(u => u.Username == username);
                user = users.FirstOrDefault();
            }
            return user;
        }


        public User GetUserByPhone(string phone)
        {
            User user = null;
            var users = SearchFor(u => u.Phone == phone);
            if (users.Count() > 0)
            {
                user = users.First();


            }
            return user;
        }


        public IEnumerable<User> GetAllUsers()
        {
            return Context.Users.ToList();
        }
    }
}
