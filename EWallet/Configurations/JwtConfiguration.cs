namespace EWallet.Settings
{
    public class JwtConfiguration
    {
        public double AccessTokenExpiration { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
    }
}
