using EWallet.Dtos;
using EWallet.InputModels;
using EWallet.Mappings;
using EWallet.Models;
using EWallet.Services.Interfaces;
using EWallet.Validations.ValidationModels;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;
        private readonly IValidator<BudgetValidationModel> _validator;

        public BudgetController(IBudgetService budgetService, IValidator<BudgetValidationModel> validator)
        {
            _budgetService = budgetService;
            _validator = validator;
        }

        [HttpPost]
        public async Task<ActionResult<BudgetDto>> Create([FromBody] CreateBudgetInputModel createBudgetInputModel, CancellationToken cancellationToken)
        {
            var validator = await _validator.ValidateAsync(createBudgetInputModel.ToValidationModel());
            if (!validator.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    FieldErrors = validator.Errors.ToDictionary(validationFailure => validationFailure.PropertyName,
                                                                                                        validationFailure => validationFailure.ErrorMessage)
                });
            }
            var budgetDto = await _budgetService.Create(createBudgetInputModel, cancellationToken);
            await _budgetService.Save(cancellationToken);
            return Ok(budgetDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BudgetDto>> Get([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var budgetDto = await _budgetService.GetDtoByID(id, cancellationToken);

            if(budgetDto == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = $"Budget with id {id} doesn't exist in the database"
                });
            }
            return Ok(budgetDto);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResponse<BudgetDto>>> Get([FromQuery] PageQueryParametersBase pageQueryParameters, CancellationToken cancellationToken)
        {
            var resultPage = await _budgetService.Get(pageQueryParameters, cancellationToken);
            return Ok(resultPage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateBudgetInputModel updateBudgetInputModel, CancellationToken cancellationToken)
        {
            var validator = await _validator.ValidateAsync(updateBudgetInputModel.ToValidationModel(), cancellationToken);
            if (!validator.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessage = validator.Errors.Select(x => x.ErrorMessage).First()
                });
            }

            var budget = await _budgetService.GetByID(id, cancellationToken);

            if (budget == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = $"Budget with id {id} doesn't exist in the database"
                });
            }
            _budgetService.Update(updateBudgetInputModel, budget);
            await _budgetService.Save(cancellationToken);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var budget = await _budgetService.GetByID(id, cancellationToken);
            if (budget == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = $"Budget with id {id} doesn't exist in the database"
                });
            }
            _budgetService.Delete(budget);
            await _budgetService.Save(cancellationToken);
            return NoContent();
        }

        [HttpDelete("bulkDelete")]
        public async Task<IActionResult> BulkDelete([FromBody] IEnumerable<Guid> budgetsIDs, CancellationToken cancellationToken)
        {
            var existingBudgets = await _budgetService.GetExistingIDs(budgetsIDs, cancellationToken);
            _budgetService.BulkDelete(existingBudgets);
            await _budgetService.Save(cancellationToken);
            return NoContent();
        }
    }
}
