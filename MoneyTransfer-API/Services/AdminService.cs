using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer_API.Data;
using MoneyTransfer_API.Models;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API.Services
{
    public interface IAdminService
    {
        IEnumerable<User> GetAllUsers();
        User BlockUser (int userId);
        User UnblockUser (int userId);
        IEnumerable<User> SortUsersByBalance(double balance);
        User GetById(int userId);
    }
    public class AdminService : IAdminService
    {
        private readonly MoneyTransferContext _context;
        public AdminService(MoneyTransferContext context)
        {
            _context = context;
        }

        public User BlockUser(int userId)
        {
            var existingUser = _context.Users.SingleOrDefault(x => x.Id == userId);
            if(existingUser == null)
            {
                return null;
            }
            else
            {
                existingUser.IsBlocked = true;
                _context.Users.Update(existingUser);
                _context.SaveChanges();
                return existingUser;
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            var userList = _context.Users;
            return (userList);
        }

        public User GetById(int userId)
        {
            var existingUser = _context.Users.Find(userId);
            if( existingUser == null)
            {
                return null;
            }
            else
            {
                return existingUser;
            }
        }

        public IEnumerable<User> SortUsersByBalance(double balance)
        {
            var sortUsersByBalance = _context.Users.Where(x => x.Balance == balance).ToList();
            return (sortUsersByBalance);
        }


        public User UnblockUser(int userId)
        {
            var existingUser = _context.Users.SingleOrDefault(x => x.Id == userId);
            if(existingUser == null)
            {
                return null;
            }
            else
            {
                existingUser.IsBlocked = false;
                _context.Users.Update(existingUser);
                _context.SaveChanges();
                return existingUser;
            }
        }
    }
}
