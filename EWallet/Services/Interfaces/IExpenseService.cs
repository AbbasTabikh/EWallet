using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Services.Interfaces
{
    public interface IExpenseService
    {
        Task<Expense?> GetByID(Guid id, CancellationToken cancellationToken);
        Task<Expense?> GetByID(Guid id, string? includedProperties, CancellationToken cancellationToken);
        Task<ExpenseDto> Create(CreateExpenseInputModel expenseInputModel, CancellationToken cancellation);
        void Update(UpdateExpenseInputModel updateExpenseInputModel, Expense expense, CancellationToken cancellationToken);
        void Delete(Expense expense);
        void BulkDelete(IEnumerable<Expense> expenses);
        Task Save(CancellationToken cancellationToken);
        Task<IEnumerable<Expense>> GetExistingIDs(IEnumerable<Guid> expensesIDs, CancellationToken cancellationToken);
    }
}
