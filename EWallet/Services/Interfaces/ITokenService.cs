using EWallet.Entities;
using EWallet.Models;

namespace EWallet.Services.Interfaces
{
    public interface ITokenService
    {
        AccessToken GenerateToken(User user);
    }
}
