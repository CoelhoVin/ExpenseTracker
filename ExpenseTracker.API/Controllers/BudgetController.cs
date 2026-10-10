using ExpenseTracker.Application.DTOs.Budgets;
using ExpenseTracker.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBudgetRequest request)
    {
        var budget = await _budgetService.CreateAsync(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = budget.Id },
            budget);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var budgets = await _budgetService.GetAllAsync();

        return Ok(budgets);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var budget = await _budgetService.GetByIdAsync(id);

        return Ok(budget);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateBudgetRequest request)
    {
        var budget = await _budgetService.UpdateAsync(id, request);

        return Ok(budget);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _budgetService.DeleteAsync(id);

        return NoContent();
    }
}