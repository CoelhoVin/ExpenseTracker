using ExpenseTracker.Application.DTOs.Expenses;
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
    public class ExpenseService : IExpenseService
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICategoryRepository _categoryRepository;

        public ExpenseService(IExpenseRepository expenseRepository, ICurrentUserService currentUserService, ICategoryRepository categoryRepository)
        {
            _expenseRepository = expenseRepository;
            _currentUserService = currentUserService;
            _categoryRepository = categoryRepository;
        }


        public async Task<ExpenseResponse> CreateAsync(CreateExpenseRequest request)
        {
            var userId = _currentUserService.UserId;

            var category = await _categoryRepository.GetByIdAndUserIdAsync(request.CategoryId, userId);
            
            if(category is null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            

            var expense = new Expense { 
                Id = Guid.NewGuid(),
                Description = request.Description,
                Amount = request.Amount,
                Date = request.Date,
                CategoryId = request.CategoryId,
                UserId = userId
            };

            await _expenseRepository.AddAsync(expense);
            await _expenseRepository.SaveChangesAsync();

            var createdExpense = await _expenseRepository.GetByIdAsync(expense.Id);

            if (createdExpense is null)
            {
                throw new InvalidOperationException("Expense could not be retrieved after creation.");
            }

            return new ExpenseResponse
            {
                Id = createdExpense.Id,
                Description = createdExpense.Description,
                Amount = createdExpense.Amount,
                Date = createdExpense.Date,
                CategoryId = createdExpense.CategoryId,
                CategoryName = createdExpense.Category?.Name ?? string.Empty
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var userId = _currentUserService.UserId;

            var expense = await _expenseRepository.GetByIdAsync(id);

            if(expense is null || expense.UserId != userId)
            {
                throw new KeyNotFoundException("Expense not found.");
            }
            _expenseRepository.Delete(expense);
            await _expenseRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExpenseResponse>> GetAllAsync()
        {
            var userId = _currentUserService.UserId;

            var expenses = await _expenseRepository.GetByUserIdAsync(userId);

            return expenses.Select(expense => new ExpenseResponse {
                Id= expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Name ?? string.Empty
            });
        }

        public async Task<ExpenseResponse> GetByIdAsync(Guid id)
        {
            var userId = _currentUserService.UserId;

            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense is null || expense.UserId != userId)
            {
                throw new KeyNotFoundException("Expense not found.");
            }

            return new ExpenseResponse
            {
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Name ?? string.Empty
            };
        }

        public async Task<ExpenseResponse> UpdateAsync(Guid id, UpdateExpenseRequest request)
        {
            var userId = _currentUserService.UserId;

            var category = await _categoryRepository.GetByIdAndUserIdAsync(request.CategoryId, userId);

            if (category is null)
            {
                throw new KeyNotFoundException("Category not found.");
            }

            var expense = await _expenseRepository.GetByIdAsync(id);

            if (expense is null || expense.UserId != userId) {
                throw new KeyNotFoundException("Expense not found.");
            }

            expense.Description = request.Description;
            expense.Amount = request.Amount;
            expense.Date = request.Date;
            expense.CategoryId = request.CategoryId;

            _expenseRepository.Update(expense);
            await _expenseRepository.SaveChangesAsync();

            return new ExpenseResponse { 
                Id = expense.Id,
                Description = expense.Description,
                Amount = expense.Amount,
                Date = expense.Date,
                CategoryId = expense.CategoryId,
                CategoryName = expense.Category?.Name ?? string.Empty
            };

        }
    }
}
