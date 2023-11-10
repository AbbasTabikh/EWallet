using EWallet.Entities;
using EWallet.Enums;
using EWallet.InputModels;

namespace EWallet.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> SignUp(UserInput user, CancellationToken cancellationToken);
        Task<(User? user, LoginStatus status)> SignIn(UserInput user, CancellationToken cancellationToken);
        Task<User?> GetByUsername(string username, CancellationToken cancellationToken);
        Task Save(CancellationToken cancellationToken);
    }
}
