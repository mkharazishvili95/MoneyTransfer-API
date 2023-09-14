using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyTransfer_API.Data;
using MoneyTransfer_API.Helpers;
using MoneyTransfer_API.Models;
using MoneyTransfer_API.Validation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API.Services
{
    public interface IUserService
    {
        User Register(UserRegisterModel registerModel);
        User Login(UserLoginModel loginModel);
        bool MakeTransfer(int senderId, int receiverId, double amount);
        User GetUserById(int userId);
        IEnumerable<User> GetOwnBalance();


    }
    public class UserService : IUserService
    {
        private readonly MoneyTransferContext _context;
        public UserService(MoneyTransferContext context)
        {
            _context = context;
        }

        public User Login(UserLoginModel loginModel)
        {
            if (string.IsNullOrEmpty(loginModel.UserName) || string.IsNullOrEmpty(loginModel.Password))
            {
                return null;
            }
            var user = _context.Users.FirstOrDefault(x => x.UserName == loginModel.UserName);
            if (user == null)
            {
                return null;
            }
            if (HashSettings.HashPassword(loginModel.Password) != user.Password)
            {
                return null;
            }
            _context.SaveChanges();
            return user;
        }

        public User Register(UserRegisterModel registerModel)
        {
            var validator = new NewUserRegisterValidator(_context);
            var validatorResult = validator.Validate(registerModel);
            if (!validatorResult.IsValid)
            {
                return null;
            }
            else
            {
                var newUser = new User
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName,
                    Password = HashSettings.HashPassword(registerModel.Password),
                    Age = registerModel.Age,

                };
                _context.Add(newUser);
                _context.SaveChanges();
                return (newUser);
            }
        }
        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }

        public bool MakeTransfer(int senderId, int receiverId, double amount)
        {
            var sender = _context.Users.FirstOrDefault(u => u.Id == senderId);
            var receiver = _context.Users.FirstOrDefault(u => u.Id == receiverId);

            if (sender == null || receiver == null)
            {
                return false; 
            }
            if(sender == receiver)
            {
                return false;
            }

            if (sender.Balance >= amount)
            {
                sender.Balance -= amount;
                receiver.Balance += amount;
                _context.Update(sender);
                _context.Update(receiver);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public IEnumerable<User> GetOwnBalance()
        {
            var ownBalance = _context.Users.Include(x => x.Balance);
            return ownBalance;
        }

    }
}
