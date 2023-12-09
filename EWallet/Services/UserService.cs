using EWallet.Entities;
using EWallet.Enums;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Repository;
using EWallet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EWallet.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ITokenService tokenService, IRepository<User> userRepository, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task<User?> GetByUsername(string username, CancellationToken cancellationToken)
        {
            return await _userRepository.GetSingleByExpression(x => x.Username == username, string.Empty, cancellationToken);
        }
        public async Task Save(CancellationToken cancellationToken)
        {
            await _userRepository.Save(cancellationToken);
        }
        public async Task<(User? user, LoginStatus status)> SignIn(UserInput userInput, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetSingleByExpression(x => x.Username == userInput.Username, string.Empty, cancellationToken);
            if(user is null)
            {
                return (null, LoginStatus.UserNotFound);
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, userInput.Password);
            return result == PasswordVerificationResult.Success ? (user, LoginStatus.Success) : (null, LoginStatus.PasswordIncorrect);
        }
        public async Task<User?> SignUp(UserInput userInput, CancellationToken cancellationToken)
        {
            var user = userInput.ToUser();
            user.Password = _passwordHasher.HashPassword(user, user.Password);
            return await _userRepository.Add(user, cancellationToken);
        }
    }
}
