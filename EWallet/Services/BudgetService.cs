using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Models;
using EWallet.Repository;
using EWallet.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Services
{
    public class BudgetService : BaseService, IBudgetService
    {
        private readonly IRepository<Budget> _budgetRepository;
        private readonly IHttpContextService _contextService;
        public BudgetService(IRepository<Budget> budgetRepository, IHttpContextService contextService)
        {
            _budgetRepository = budgetRepository;
            _contextService = contextService;
        }

        public async Task<BudgetDto?> GetDtoByID(Guid id, CancellationToken cancellation)
        {
            var budget = await _budgetRepository.GetSingleByExpression(x => x.ID == id, "Expenses", cancellation);
            return budget?.ToDto(true);
        }

        public async Task<Budget?> GetByID(Guid id, CancellationToken cancellationToken)
        {
            return await _budgetRepository.GetByID(id, cancellationToken);
        }

        public async Task<BudgetDto> Create(CreateBudgetInputModel budgetInput, CancellationToken cancellation)
        {
            var currentUserID = _contextService.GetCurrentUserID();
            var budget = budgetInput.ToBudget(currentUserID);
            var addedbudget = await _budgetRepository.Add(budget, cancellation);
            return addedbudget.ToDto();
        }

        public void Delete(Budget budget)
        {
            _budgetRepository.Delete(budget);
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _budgetRepository.Save(cancellationToken);
        }

        public void Update(UpdateBudgetInputModel updateBudgetInputModel, Budget budget, CancellationToken cancellationToken)
        {
            budget.Total = updateBudgetInputModel.Total;
        }

        public async Task<PagedResponse<BudgetDto>> Get(PageQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            var userID = _contextService.GetCurrentUserID();
            var query = _budgetRepository.GetAsQueryable(x => x.UserID == userID, string.Empty);
            var pagedResponse = await GeneratedPaginatedResponse(query, queryParameters.PageNumber, queryParameters.PageSize, cancellationToken);
            return pagedResponse.ToDto();
        }

        public void BulkDelete(IEnumerable<Budget> budgets)
        {
            _budgetRepository.BulkDelete(budgets);
        }

        public async Task<IEnumerable<Budget>> GetExistingIDs(IEnumerable<Guid> budgetsIDs, CancellationToken cancellationToken)
        {
            var budgets = await _budgetRepository.GetManyByExpression(x => budgetsIDs.Contains(x.ID), string.Empty, cancellationToken);
            return budgets;
        }
    }
}
