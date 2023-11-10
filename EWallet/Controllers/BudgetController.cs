using EWallet.Dtos;
using EWallet.InputModels;
using EWallet.Models;
using EWallet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EWallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BudgetDto>> Create([FromBody] CreateBudgetInputModel createBudgetInputModel, CancellationToken cancellationToken)
        {
            var budgetDto = await _budgetService.Create(createBudgetInputModel, cancellationToken);
            await _budgetService.Save(cancellationToken);
            return Ok(budgetDto);
        }

        [HttpGet]
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

        [HttpDelete]
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
    }
}
