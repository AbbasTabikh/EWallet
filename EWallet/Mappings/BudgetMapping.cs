using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;

namespace EWallet.Mappings
{
    public static class BudgetMapping
    {
        internal static Budget ToBudget(this CreateBudgetInputModel budgetInputModel, Guid currentUserID)
        {
            return new Budget
            {
                Total = budgetInputModel.Total,
                UserID = currentUserID,
                CreationDate = DateTime.UtcNow.Date
            };
        }

        internal static BudgetDto ToDto(this Budget budget, bool mapExpenses = false)
        {
            if (!mapExpenses)
            {
                return new BudgetDto
                {
                    ID = budget.ID,
                    Total = budget.Total,
                    CreationDate = budget.CreationDate!.Value.ToString("dd/MM/yyyy")
                };
            }

            return new BudgetDto
            {
                ID = budget.ID,
                Total = budget.Total,
                CreationDate = budget.CreationDate!.Value.ToString("dd/MM/yyyy"),
                Expenses = budget.Expenses
            };
        }
    }
}
