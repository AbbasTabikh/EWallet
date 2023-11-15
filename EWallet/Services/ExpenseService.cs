using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Models;
using EWallet.Repository;
using EWallet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Services
{
    public class ExpenseService : BaseService, IExpenseService
    {
        private readonly IRepository<Expense> _expenseRepository;

        public ExpenseService(IRepository<Expense> expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public void BulkDelete(IEnumerable<Expense> expenses)
        {
            _expenseRepository.BulkDelete(expenses);
        }

        public async Task<ExpenseDto> Create(CreateExpenseInputModel expenseInputModel, CancellationToken cancellation)
        {
            var expense = expenseInputModel.ToExpense();
            var addedExpense = await _expenseRepository.Add(expense, cancellation);
            return addedExpense.ToDto();
        }

        public void Delete(Expense expense)
        {
            _expenseRepository.Delete(expense);
        }

        public Task<PagedResponse<ExpenseDto>> Get([FromQuery] ExpenseQueryParameters expenseQueryParameters, CancellationToken cancellationToken)
        {
            //var query = _expenseRepository.GetAsQueryable(x => x.BudgetID == expenseQueryParameters.BudgetID, string);\
            return null;
        }

        public Task<Budget?> GetByID(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BudgetDto?> GetDtoByID(Guid id, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Budget>> GetExistingIDs(IEnumerable<Guid> budgetsIDs, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Save(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Update(UpdateBudgetInputModel updateBudgetInputModel, Budget budget, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
