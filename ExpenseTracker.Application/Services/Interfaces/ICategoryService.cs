using ExpenseTracker.Application.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponse> CreateAsync(CreateCategoryRequest request);
        Task<IEnumerable<CategoryResponse>> GetAllAsync();
        Task<CategoryResponse> GetByIdAsync(Guid id);
        Task<CategoryResponse> UpdateAsync(Guid id, UpdateCategoryRequest request);
        Task DeleteAsync(Guid id);
    }
}
