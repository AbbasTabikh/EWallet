using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Models;
using EWallet.Repository;
using EWallet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

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

        public async Task<Expense?> GetByID(Guid id, CancellationToken cancellationToken)
        {
            return await _expenseRepository.GetByID(id, cancellationToken);
        }

        public async Task<Expense?> GetByID(Guid id, string? includedProperties, CancellationToken cancellationToken)
        {
            return await _expenseRepository.GetSingleByExpression(x => x.ID == id, includedProperties, cancellationToken);
        }

        public async Task<IEnumerable<Expense>> GetExistingIDs(IEnumerable<Guid> expensesIDs, CancellationToken cancellationToken)
        {
            var expenses = await _expenseRepository.GetManyByExpression(x => expensesIDs.Contains(x.ID), string.Empty, cancellationToken);
            return expenses;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            await _expenseRepository.Save(cancellationToken);
        }

        public void Update(UpdateExpenseInputModel updateExpenseInputModel, Expense expense, CancellationToken cancellationToken)
        {
            if(!string.IsNullOrEmpty(updateExpenseInputModel.Name!.Trim()) )
            {
                expense.Name = updateExpenseInputModel.Name;
            }

            if (updateExpenseInputModel?.NewPrice != null)
            {
                expense.Price = updateExpenseInputModel.NewPrice.Value;
            }
        }
    }
}
