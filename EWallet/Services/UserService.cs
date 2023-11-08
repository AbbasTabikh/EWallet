using EWallet.Entities;
using EWallet.Repository;
using EWallet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EWallet.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ITokenService tokenService, IRepository<User> userRepository, IPasswordHasher<User> passwordHasher)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User?> SignIn(User userInput, CancellationToken cancellationToken)
        {
            return null;
        }

        public async Task<User?> SignUp(User newUser, CancellationToken cancellationToken)
        {
            //1. validations later..

            newUser.Password = _passwordHasher.HashPassword(newUser, newUser.Password);
            return await _userRepository.Add(newUser, cancellationToken);
        }
    }
}
