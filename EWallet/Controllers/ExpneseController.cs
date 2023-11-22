using EWallet.Dtos;
using EWallet.Entities;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Models;
using EWallet.Services.Interfaces;
using EWallet.Validations.ValidationModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpneseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IBudgetService _budgetService;
        private readonly IValidator<ExpenseValidationModel> _validator;

        public ExpneseController(IExpenseService expenseService, IBudgetService budgetService, IValidator<ExpenseValidationModel> validator)
        {
            _expenseService = expenseService;
            _budgetService = budgetService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<ExpenseDto>> Create([FromBody] CreateExpenseInputModel createExpenseInputModel, CancellationToken cancellationToken)
        {
            await ValidateModelAsync(createExpenseInputModel.ToValidationModel(), cancellationToken);
            var budget = await _budgetService.GetByID(createExpenseInputModel.BudgetID, cancellationToken);
            
            if(budget == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = $"Budget with {createExpenseInputModel.BudgetID} doesn't exist in the database"
                });
            }

            var expenseDto = await _expenseService.Create(createExpenseInputModel, cancellationToken);
            _budgetService.UpdateBudgetTotal(budget, -createExpenseInputModel.Price, 0);
            await _expenseService.Save(cancellationToken);
            await _budgetService.Save(cancellationToken);

            return Ok(expenseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateExpenseInputModel updateExpenseInputModel, CancellationToken cancellationToken)
        {
            await ValidateModelAsync(updateExpenseInputModel.ToValidationModel(), cancellationToken);
            var expense = await _expenseService.GetByID(id,"Budget", cancellationToken);

            if(expense == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = $"expense with ID {id} doesn't exist in the database"
                });
            }

            if(updateExpenseInputModel.NewPrice != null)
            {
                var oldPrice = expense.Price;
                _expenseService.Update(updateExpenseInputModel, expense, cancellationToken);
                _budgetService.UpdateBudgetTotal(expense.Budget, oldPrice, updateExpenseInputModel.NewPrice.Value);
            }
            else
            {
                _expenseService.Update(updateExpenseInputModel, expense, cancellationToken);
            }
            await _expenseService.Save(cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id,  CancellationToken cancellationToken)
        {
            var expense = await _expenseService.GetByID(id,"Budget", cancellationToken);
            if (expense == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = $"expense with ID {id} doesn't exist in the database"
                });
            }

            _expenseService.Delete(expense);
            _budgetService.UpdateBudgetTotal(expense.Budget, expense.Price, 0);
            await _expenseService.Save(cancellationToken);
            return NoContent();
        }

        [HttpDelete("bulkdelete({budgetID})")]
        public async Task<IActionResult> BulkDelete([FromRoute] Guid budgetID, [FromBody] IEnumerable<Guid> expensesIDs, CancellationToken cancellationToken)
        {
            var existingExpenses = await _expenseService.GetExistingIDs(expensesIDs, cancellationToken);

            if(existingExpenses.Any())
            {
                var budget = await _budgetService.GetByID(budgetID, cancellationToken);
                if (budget == null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        ErrorMessage = $"Budget with ID {budgetID} doesn't exist in the database"
                    });
                }
                var sum = existingExpenses.Sum(x => x.Price);
                _expenseService.BulkDelete(existingExpenses);
                _budgetService.UpdateBudgetTotal(budget, sum, 0);
                await _expenseService.Save(cancellationToken);
            }

            return NoContent();
        }

        private async Task ValidateModelAsync(ExpenseValidationModel model, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(model, cancellationToken);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

        }

    }
}
