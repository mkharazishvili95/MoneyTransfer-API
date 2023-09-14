using FluentValidation;
using MoneyTransfer_API.Data;
using MoneyTransfer_API.Models;
using System.Linq;

namespace MoneyTransfer_API.Validation
{
    public class NewUserRegisterValidator : AbstractValidator<UserRegisterModel>
    {
        private readonly MoneyTransferContext _context;
        public NewUserRegisterValidator(MoneyTransferContext context) {
            _context = context;
            RuleFor(newUser => newUser.FirstName).NotEmpty().WithMessage("Enter your FirstName!")
                .Length(1, 50).WithMessage("FirstName length must be between 1 and 50 chars!");
            RuleFor(newUser => newUser.LastName).NotEmpty().WithMessage("Enter your LastName!")
                .Length(1, 50).WithMessage("LastName length must be between 1 and 50 chars!");
            RuleFor(newUser => newUser.Age).NotNull().NotEmpty().WithMessage("Enter your Age!")
                .GreaterThanOrEqualTo(18).LessThanOrEqualTo(65).WithMessage("Your age must be between 18 and 65 in order to register!");
            RuleFor(newUser => newUser.UserName).NotEmpty().WithMessage("Enter your UserName!")
                .Length(6, 15).WithMessage("UserName length must be between 6 and 15 chars or numbers!")
                .Must(differentUserName).WithMessage("UserName already exists!");
            RuleFor(newUser => newUser.Password).NotEmpty().WithMessage("Enter your Password!")
                .Length(6, 15).WithMessage("Password length must be between 6 and 15 chars or numbers!");
        }
        private bool differentUserName(string userName)
        {
            var different = _context.Users.FirstOrDefault(user => user.UserName.ToUpper() == userName.ToUpper());
            return different == null;
        }
    }
}
