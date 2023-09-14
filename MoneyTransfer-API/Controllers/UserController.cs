using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MoneyTransfer_API.Data;
using MoneyTransfer_API.Helpers;
using MoneyTransfer_API.Models;
using MoneyTransfer_API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using MoneyTransfer_API.Validation;
using Microsoft.AspNetCore.Authorization;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoneyTransfer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly MoneyTransferContext _context;
        private readonly IUserService _userService;
        public UserController(AppSettings appSettings, MoneyTransferContext context, IUserService userService)
        {
            _appSettings = appSettings;
            _context = context;
            _userService = userService;
        }
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRegisterModel newUser)
        {
            var validator = new NewUserRegisterValidator(_context);
            var validatorResult = validator.Validate(newUser);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            else
            {
                var user = newUser;
                _userService.Register(user);
                return Ok(new { Message = $"User: {user.UserName} has successfully registered!" });
            }
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginModel login)
        {
            var userLogin = _userService.Login(login);
            if (userLogin == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect!" });
            }
            else
            {
                Loggs log = new Loggs
                {
                    UserLogged = $"User {userLogin.UserName} logged in.",
                    LoggDate = DateTime.Now
    };
                _context.Loggs.Add(log);
                _context.SaveChanges();


                var tokenString = GenerateToken(userLogin);

                return Ok(new
                {
                    Message = "You have successfully Logged!",
                    Id = userLogin.Id,
                    UserName = userLogin.UserName,
                    FirstName = userLogin.FirstName,
                    LastName = userLogin.LastName,
                    Age = userLogin.Age,
                    Role = userLogin.Role,
                    Token = tokenString
                });
            }
        }

        [Authorize]
        [HttpGet("GetOwnBalance")]
        public IActionResult GetOwnBalance()
        {
            var userId = int.Parse(User.Identity.Name);
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return NotFound("User not found!");
            }
            var formattedBalance = $"{user.Balance:N} GEL";
            return Ok($"You have: {formattedBalance} GEL on your account!");
        }


        [Authorize]
        [HttpPost]
        [Route("MoneyTransfer")]
        public IActionResult TransferMoney(TransferModel transferModel)
        {
            var userId = int.Parse(User.Identity.Name);
            var senderId = int.Parse(User.Identity.Name);
            var sender = _userService.GetUserById(userId);

            if (sender == null)
            {
                return NotFound("User not found!");
            }
            if(sender.Id == transferModel.ReceiverId)
            {
                return BadRequest("Error! You can not transfer money from your account to your account!");
            }

            if (sender.IsBlocked)
            {
                return BadRequest("You have no permission to transfer money, because you are blocked!");
            }

            if (transferModel == null || transferModel.ReceiverId == 0 || transferModel.Amount <= 0)
            {
                return BadRequest("Invalid transfer request!");
            }

            var receiver = _userService.GetUserById(transferModel.ReceiverId);

            if (receiver == null)
            {
                return NotFound("Receiver not found!");
            }

            if (sender.Id != senderId)
            {
                return BadRequest("You can only transfer money from your account!");
            }

            var success = _userService.MakeTransfer(userId, transferModel.ReceiverId, transferModel.Amount);

            if (success)
            {

                return Ok($"You have successfully transferred {transferModel.Amount} GEL to {receiver.UserName}'s account.");
            }
            else
            {
                return BadRequest($"You don't have enough money to transfer. Your balance is: {sender.Balance} GEL!");
            }
        }


        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserName.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}