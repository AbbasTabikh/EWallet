using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Models;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetDto?> GetDtoByID(Guid id, CancellationToken cancellation);
        Task<Budget?> GetByID(Guid id, CancellationToken cancellationToken);
        Task<PagedResponse<BudgetDto>> Get([FromQuery] PageQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<BudgetDto> Create(CreateBudgetInputModel budgetInput, CancellationToken cancellation);
        void Update(UpdateBudgetInputModel updateBudgetInputModel, Budget budget, CancellationToken cancellationToken);
        void Delete(Budget budget);
        void BulkDelete(IEnumerable<Budget> budgets);
        Task Save(CancellationToken cancellationToken);
        Task<IEnumerable<Budget>> GetExistingIDs(IEnumerable<Guid> budgetsIDs, CancellationToken cancellationToken);
    }
}
