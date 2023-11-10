using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Repository;
using EWallet.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Services
{
    public class BudgetService : IBudgetService
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

       
    }
}
