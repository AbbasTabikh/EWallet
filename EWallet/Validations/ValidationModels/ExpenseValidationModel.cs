using System.ComponentModel.DataAnnotations;

namespace EWallet.Validations.ValidationModels
{
    public class ExpenseValidationModel
    {
        public string? Name { get; set; }
        public decimal? Price { get; set; }
    }
}
