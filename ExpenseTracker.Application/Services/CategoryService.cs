using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ICurrentUserService currentUserService)
        {
            _categoryRepository = categoryRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CategoryResponse> CreateAsync(
            CreateCategoryRequest request)
        {
            var userId = _currentUserService.UserId;

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                UserId = userId
            };

            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
        {
            var userId = _currentUserService.UserId;

            var categories = await _categoryRepository.GetByUserIdAsync(userId);

            return categories.Select(category => new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            });
        }

        public async Task<CategoryResponse> GetByIdAsync(Guid id)
        {
            var userId = _currentUserService.UserId;

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null || category.UserId != userId)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryResponse> UpdateAsync(
            Guid id,
            UpdateCategoryRequest request)
        {
            var userId = _currentUserService.UserId;

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null || category.UserId != userId)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            category.Name = request.Name;

            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();

            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var userId = _currentUserService.UserId;

            var category = await _categoryRepository.GetByIdAsync(id);

            if (category is null || category.UserId != userId)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync();
        }
    }
}
