using EWallet.Enums;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Models;
using EWallet.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IValidator<UserInput> _userValidator;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        public UserController(IValidator<UserInput> userValidator, ITokenService tokenService, IUserService userService)
        {
            _userValidator = userValidator;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<AccessToken>> SignUp([FromBody] UserInput userInput, CancellationToken cancellationToken)
        {
            var result = await _userValidator.ValidateAsync(userInput, cancellationToken);

            if (!result.IsValid)
            {
                var errors = result.Errors.ToDictionary(validationFailure => validationFailure.PropertyName,
                                                                                                validationFailure => validationFailure.ErrorMessage);

                return BadRequest(new ErrorResponse
                {
                    FieldErrors = errors
                });
            }

            var user = await _userService.SignUp(userInput, cancellationToken);
            await _userService.Save(cancellationToken);
            return Ok(_tokenService.GenerateToken(user));
        }

        [HttpPost("signin")]
        public async Task<ActionResult<AccessToken>> SignIn([FromBody] UserInput userInput, CancellationToken cancellation)
        {
            var userLoginStatus = await _userService.SignIn(userInput, cancellation);

            var errorResponse = new ErrorResponse
            {
                FieldErrors = new Dictionary<string, string>()
            };

            switch (userLoginStatus.status)
            {
                case LoginStatus.UserNotFound:
                    errorResponse.FieldErrors.Add("Username", "Username doesn't exist");
                    break;
                case LoginStatus.PasswordIncorrect:
                    errorResponse.FieldErrors.Add("Password", "Incorrect password");
                    break;
            }

            if (errorResponse.FieldErrors.Count > 0)
            {
                return NotFound(errorResponse);
            }

            return Ok(_tokenService.GenerateToken(userLoginStatus.user!));
        }


    }
}
