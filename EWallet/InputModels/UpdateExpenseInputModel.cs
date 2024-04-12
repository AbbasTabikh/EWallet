namespace EWallet.InputModels
{
    public record UpdateExpenseInputModel
    {
        public string? Name { get; set; }
        public decimal? NewPrice { get; set; }
    }
}
