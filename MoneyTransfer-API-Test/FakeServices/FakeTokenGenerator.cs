using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoneyTransfer_API.Helpers;
using MoneyTransfer_API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoneyTransfer_API_Test.FakeServices
{
    public interface ITokenGenerator
    {
        public string GenerateToken(User user);
    }

    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<AppSettings> _appSettings;

        public TokenGenerator(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Value.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
