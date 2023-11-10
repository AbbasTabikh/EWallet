using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;

namespace EWallet.Services.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetDto?> GetDtoByID(Guid id, CancellationToken cancellation);
        Task<Budget?> GetByID(Guid id, CancellationToken cancellationToken);
        Task<BudgetDto> Create(CreateBudgetInputModel budgetInput, CancellationToken cancellation);
        void Delete(Budget budget);
        Task Save(CancellationToken cancellationToken);
    }
}
