using MoneyTransfer_API.Models;
using MoneyTransfer_API.Services;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API_Test.FakeServices
{
    public class FakeUserService : IUserService
    {
        private readonly List<User> _users;

        public FakeUserService(List<User> users)
        {
            _users = users;
        }

        public User GetUserById(int userId)
        {
            return _users.FirstOrDefault(u => u.Id == userId);
        }

        public IEnumerable<User> GetOwnBalance()
        {
            return _users;
        }

        public User Login(UserLoginModel loginModel)
        {
            return _users.FirstOrDefault(u => u.UserName == loginModel.UserName && u.Password == loginModel.Password);
        }

        public bool MakeTransfer(int senderId, int receiverId, double amount)
        {
            return true;
        }

        public User Register(UserRegisterModel registerModel)
        {
            if (string.IsNullOrEmpty(registerModel.UserName) || string.IsNullOrEmpty(registerModel.Password))
            {
                return null;
            }

            var newUser = new User
            {
                Id = _users.Count + 1, 
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName,
                UserName = registerModel.UserName,
                Password = registerModel.Password,
                Age = registerModel.Age
            };
            _users.Add(newUser);
            return newUser;
        }

    }
}
