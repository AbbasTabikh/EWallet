using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<BudgetDto?> GetDtoByID(Guid id, CancellationToken cancellation);
        Task<Budget?> GetByID(Guid id, CancellationToken cancellationToken);
        Task<PagedResponse<BudgetDto>> Get([FromQuery] PageQueryParametersBase queryParameters, CancellationToken cancellationToken);
        Task<ExpenseDto> Create(CreateExpenseInputModel expenseInputModel, CancellationToken cancellation);
        void Update(UpdateBudgetInputModel updateBudgetInputModel, Budget budget, CancellationToken cancellationToken);
        void Delete(Expense expense);
        void BulkDelete(IEnumerable<Expense> expenses);
        Task Save(CancellationToken cancellationToken);
        Task<IEnumerable<Budget>> GetExistingIDs(IEnumerable<Guid> expensesIDs, CancellationToken cancellationToken);
    }
}
