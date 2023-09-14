using MoneyTransfer_API.Models;
using MoneyTransfer_API.Services;
using MoneyTransfer_API_Test.FakeServices;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API_Test.Tests
{
    public class AdminServiceTests
    {
        private IAdminService _adminService;

        [SetUp]
        public void Setup()
        {
            var fakeUsers = new List<User>
            {
                new User { Id = 1, UserName = "user1", Balance = 1000 },
                new User { Id = 2, UserName = "user2", Balance = 2000 },
                new User { Id = 3, UserName = "user3", Balance = 3000 }
            };

            _adminService = new FakeAdminService(fakeUsers);
        }

        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            var result = _adminService.GetAllUsers();

            Assert.AreEqual(3, result.Count());
        }

        [Test]
        public void SortUsersByBalance_ValidBalance_ReturnsMatchingUsers()
        {
            double balanceToSort = 2000;

            var result = _adminService.SortUsersByBalance(balanceToSort);

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.First().Id);
        }
        [Test]
        public void BlockUser_ExistingUserId_ReturnsBlockedUser()
        {
            // Arrange
            int existingUserId = 1; 
            var result = _adminService.BlockUser(existingUserId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsBlocked);
        }

        [Test]
        public void BlockUser_NonExistentUserId_ReturnsNull()
        {
            // Arrange
            int nonExistentUserId = 999;

            var result = _adminService.BlockUser(nonExistentUserId);

            Assert.IsNull(result);
        }

        [Test]
        public void UnblockUser_ExistingUserId_ReturnsUnblockedUser()
        {
            int existingUserId = 1;

            var result = _adminService.UnblockUser(existingUserId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsBlocked);
        }

        [Test]
        public void UnblockUser_NonExistentUserId_ReturnsNull()
        {
            int nonExistentUserId = 999;
            var result = _adminService.UnblockUser(nonExistentUserId);

            Assert.IsNull(result);
        }

    }
}
