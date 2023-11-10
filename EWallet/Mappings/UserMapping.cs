using EWallet.Entities;
using EWallet.InputModels;

namespace EWallet.Mappings
{
    public static class UserMapping
    {
        internal static User ToUser(this UserInput userInput)
        {
            return new User
            {
                Username = userInput.Username,
                Password = userInput.Password
            };
        }
    }
}
