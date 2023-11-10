namespace EWallet.Models
{
    public class ErrorResponse
    {
        public Dictionary<string,string>? FieldErrors { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
