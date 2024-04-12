using System.ComponentModel.DataAnnotations;

namespace EWallet.InputModels
{
    public record CreateExpenseInputModel
    {
        public string Category { get; set; } = string.Empty;
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid BudgetID { get; set; }
    }
}
