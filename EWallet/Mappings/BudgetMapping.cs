using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Validations;
using EWallet.Validations.ValidationModels;

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
                Expenses = budget.Expenses.Select(x => x.ToDto())
            };
        }

        internal static BudgetValidationModel ToValidationModel(this CreateBudgetInputModel createBudgetInputModel)
        {
            return new BudgetValidationModel
            {
                Total = createBudgetInputModel.Total
            };
        }
        internal static BudgetValidationModel ToValidationModel(this UpdateBudgetInputModel updateBudgetInputModel)
        {
            return new BudgetValidationModel
            {  
                Total = updateBudgetInputModel.Total
            };
        }
    }
}
