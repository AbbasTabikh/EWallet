using EWallet.Entities;

namespace EWallet.Dtos
{
    public class BudgetDto
    {
        public Guid ID { get; set; }
        public decimal Total { get; set; }
        public string CreationDate { get; set; } = string.Empty;
        public IEnumerable<ExpenseDto>? Expenses { get; set; }
    }
}
