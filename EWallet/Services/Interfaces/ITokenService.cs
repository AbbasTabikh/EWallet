using EWallet.Entities;

namespace EWallet.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
