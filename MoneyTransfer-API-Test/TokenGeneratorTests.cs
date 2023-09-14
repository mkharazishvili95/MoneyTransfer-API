using Microsoft.Extensions.Options;
using MoneyTransfer_API.Helpers;
using MoneyTransfer_API.Models;
using MoneyTransfer_API_Test.FakeServices;
using NUnit.Framework;

namespace MoneyTransfer_API_Test.Tests
{
    public class TokenGeneratorTests
    {
        [Test]
        public void GenerateToken_Returns_Valid_Token()
        {
            var appSettings = new AppSettings { Secret = "Do you want to break my secret key?! Kiss my ASS! =P" };
            var options = Options.Create(appSettings);
            var user = new User { UserName = "testUser", Id = 1, Role = "user" };
            var tokenGenerator = new TokenGenerator(options);

            var generatedToken = tokenGenerator.GenerateToken(user);
            Assert.IsNotNull(generatedToken);
        }
    }
}
