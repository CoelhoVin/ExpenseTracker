using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController :ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.Id },
                category);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return Ok(category);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateCategoryRequest request)
        {
            var category = await _categoryService.UpdateAsync(id, request);

            return Ok(category);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _categoryService.DeleteAsync(id);

            return NoContent();
        }
    }
}
