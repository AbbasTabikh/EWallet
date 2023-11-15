using EWallet.Enums;

namespace EWallet.Entities
{
    public class Expense : BaseEntity
    {
        public Category Category { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Budget Budget { get; set; }
        public Guid BudgetID { get; set; }
    }
}
