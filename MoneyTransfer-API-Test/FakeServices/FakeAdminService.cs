using MoneyTransfer_API.Models;
using MoneyTransfer_API.Services;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API_Test.FakeServices
{
    public class FakeAdminService : IAdminService
    {
        private readonly List<User> _users;

        public FakeAdminService(List<User> users)
        {
            _users = users;
        }

        public User BlockUser(int userId)
        {
            var existingUser = _users.SingleOrDefault(x => x.Id == userId);
            if (existingUser == null)
            {
                return null;
            }
            else
            {
                existingUser.IsBlocked = true;
                return existingUser;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users;
        }

        public User GetById(int userId)
        {
            return _users.SingleOrDefault(x => x.Id == userId);
        }

        public IEnumerable<User> SortUsersByBalance(double balance)
        {
            return _users.Where(x => x.Balance == balance);
        }

        public User UnblockUser(int userId)
        {
            var existingUser = _users.SingleOrDefault(x => x.Id == userId);
            if (existingUser == null)
            {
                return null;
            }
            else
            {
                existingUser.IsBlocked = false;
                return existingUser;
            }
        }
    }
}
