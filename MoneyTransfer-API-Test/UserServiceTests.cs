using MoneyTransfer_API.Models;
using MoneyTransfer_API.Services;
using MoneyTransfer_API_Test.FakeServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API_Test.Tests
{
    public class UserServiceTests
    {
        private IUserService _userService;

        [SetUp]
        public void Setup()
        {
            var users = new List<User>
            {
                new User { Id = 1, UserName = "user1", Password = "password1" },
                new User { Id = 2, UserName = "user2", Password = "password2" }
            };

            _userService = new FakeUserService(users);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsUser()
        {
            var user = _userService.Login(new UserLoginModel { UserName = "user1", Password = "password1" });

            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.Id);
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsNull()
        {
            var user = _userService.Login(new UserLoginModel { UserName = "user1", Password = "invalidPassword" });

            Assert.IsNull(user);
        }
        [Test]
        public void Register_InvalidData_ReturnsNull()
        {
            
            var invalidRegisterModel = new UserRegisterModel
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "", 
                Password = "testPassword",
                Age = 30
            };

            var result = _userService.Register(invalidRegisterModel);

            Assert.IsNull(result);
        }
        [Test]
        public void Register_ValidData_ReturnsUser()
        {
            var validRegisterModel = new UserRegisterModel
            {
                FirstName = "John",
                LastName = "Doe",
                UserName = "johndoe",
                Password = "testPassword",
                Age = 30
            };

            var result = _userService.Register(validRegisterModel);

            Assert.IsNotNull(result);
            Assert.AreEqual(validRegisterModel.FirstName, result.FirstName);
            Assert.AreEqual(validRegisterModel.LastName, result.LastName);
            Assert.AreEqual(validRegisterModel.UserName, result.UserName);
            Assert.AreEqual(validRegisterModel.Age, result.Age);
            
        }
        [Test]
        public void GetUserById_ExistingUserId_ReturnsUser()
        {
            int existingUserId = 1;
            var result = _userService.GetUserById(existingUserId);
            Assert.IsNotNull(result);
            Assert.AreEqual(existingUserId, result.Id);
        }
        [Test]
        public void GetOwnBalance_ReturnsListOfUsersWithBalances()
        {
            var result = _userService.GetOwnBalance();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
        [Test]
        public void MakeTransfer_ValidTransfer_ReturnsTrue()
        {
            int senderId = 1;
            int receiverId = 2;
            double amount = 100.0;
            var result = _userService.MakeTransfer(senderId, receiverId, amount);
            Assert.IsTrue(result);
        }

    }
}
