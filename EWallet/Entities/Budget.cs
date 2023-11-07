namespace EWallet.Entities
{
    public class Budget : BaseEntity
    {
        public decimal Total { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public User User { get; set; }
        public Guid UserID { get; set; }
    }
}
