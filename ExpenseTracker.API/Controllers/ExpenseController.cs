using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateExpenseRequest request)
        {
            var expense = await _expenseService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new {id = expense.Id },
                expense
                );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var expenses = await _expenseService.GetAllAsync();

            return Ok( expenses );
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var expense = await _expenseService.GetByIdAsync(id);

            return Ok( expense );
        }


        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateExpenseRequest request)
        {
            var expense = await _expenseService.UpdateAsync(id, request);

            return Ok( expense );
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _expenseService.DeleteAsync(id);
            return NoContent();
        }



    }
}
