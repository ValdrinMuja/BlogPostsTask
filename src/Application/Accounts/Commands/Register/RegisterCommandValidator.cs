using Application.BlogPosts.Commands.Create;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Accounts.Commands.Register
{
    internal class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(u => u.FirstName).NotEmpty();
            RuleFor(u => u.LastName).NotEmpty();
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
            RuleFor(u => u.Password).NotEmpty()
                .Must(StartWithUppercase)
                .WithMessage("The password must start with an uppercase letter.");
        }

        private bool StartWithUppercase(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            return char.IsUpper(password[0]);
        }
    }
}
