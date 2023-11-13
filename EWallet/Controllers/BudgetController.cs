﻿using EWallet.Dtos;
using EWallet.Entities;
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
        public async Task<ActionResult<BudgetDto>> Create([FromBody] CreateBudgetInputModel createBudgetInputModel, CancellationToken cancellationToken)
        {
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
        public async Task<ActionResult<PagedResponse<BudgetDto>>> Get([FromQuery] PageQueryParameters pageQueryParameters, CancellationToken cancellationToken)
        {
            var resultPage = await _budgetService.Get(pageQueryParameters, cancellationToken);
            return Ok(resultPage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id,[FromBody] UpdateBudgetInputModel updateBudgetInputModel, CancellationToken cancellationToken)
        {
            var budget = await _budgetService.GetByID(id, cancellationToken);

            if (budget == null)
            {
                return NotFound(new ErrorResponse
                {
                    ErrorMessage = $"Budget with id {id} doesn't exist in the database"
                });
            }
            _budgetService.Update(updateBudgetInputModel, budget, cancellationToken);
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

        [HttpDelete]
        public async Task<IActionResult> BulkDelete([FromBody] IEnumerable<Guid> budgetsIDs, CancellationToken cancellationToken)
        {
            var allExists = await _budgetService.AllExists(budgetsIDs, cancellationToken);
            
            if (allExists)
            {
                _budgetService.BulkDelete(budgetsIDs);
                await _budgetService.Save(cancellationToken);
                return NoContent();
            }

            return BadRequest(new ErrorResponse
            {
                ErrorMessage = "Error in sending data, please try again"
            });
        }


    }
}
