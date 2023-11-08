using EWallet.Entities;

namespace EWallet.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> SignUp(User user, CancellationToken cancellationToken);
        Task<User?> SignIn(User user, CancellationToken cancellationToken);
    }
}
