using System.ComponentModel.DataAnnotations;

namespace EWallet.InputModels
{
    public class CreateExpenseInputModel
    {
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid BudgetID { get; set; }
    }
}
