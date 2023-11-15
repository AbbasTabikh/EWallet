namespace EWallet.InputModels
{
    public class ExpenseQueryParameters : PageQueryParametersBase
    {
        public Guid BudgetID { get; set; }
    }
}
