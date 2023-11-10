namespace EWallet.Models
{
    public class AccessToken
    {
        public string Token { get; set; } = string.Empty;   
        public long Expiration { get; set; }
    }
}
