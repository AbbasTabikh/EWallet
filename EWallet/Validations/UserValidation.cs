using EWallet.InputModels;
using EWallet.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Rewrite;

namespace EWallet.Validations
{
    public class UserValidation : AbstractValidator<UserInput>
    {
        private readonly IUserService _userService;
        public UserValidation(IUserService userService)
        {
            _userService = userService;

            RuleFor(x => x.Password).NotEmpty()
                                    .NotNull()
                                    .WithMessage("Password Cannot be empty");

            RuleFor(x => x.Password).Must(HasValidLength)
                                    .WithMessage("Password must be 8 characters and above");

            RuleFor(x => x.Username).Must(x => !HasWhiteSpace(x))
                                    .WithMessage("Username cannot contain white space");

            RuleFor(x => x.Username).Must(x => x.Length < 42)
                                    .WithMessage("Username must be 42 characters and lesser");

            RuleFor(x => x.Username).MustAsync(async (x, token) => !await UsernameExists(x))
                                    .WithMessage("Username already exists");
        }


        private static bool HasWhiteSpace(string username)
        {
            return username.IndexOf(" ") > -1;
        }

        private async Task<bool> UsernameExists(string username)
        {
            return await _userService.GetByUsername(username, CancellationToken.None) != null;
        }

        private static bool HasValidLength(string password)
        {
            return password.Length >= 8;
        }
    }
}
