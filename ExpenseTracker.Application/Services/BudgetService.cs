using ExpenseTracker.Application.DTOs.Budgets;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Application.Services;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public BudgetService(
        IBudgetRepository budgetRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<BudgetResponse> CreateAsync(
        CreateBudgetRequest request)
    {
        var userId = _currentUserService.UserId;

        var category = await _categoryRepository
            .GetByIdAndUserIdAsync(request.CategoryId, userId);

        if (category is null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        var budget = new Budget
        {
            Id = Guid.NewGuid(),
            Amount = request.Amount,
            Month = request.Month,
            Year = request.Year,
            UserId = userId,
            CategoryId = request.CategoryId
        };

        await _budgetRepository.AddAsync(budget);
        await _budgetRepository.SaveChangesAsync();

        var createdBudget = await _budgetRepository.GetByIdAsync(budget.Id);

        if (createdBudget is null)
        {
            throw new InvalidOperationException(
                "Budget could not be retrieved after creation.");
        }

        return MapToResponse(createdBudget);
    }

    public async Task<IEnumerable<BudgetResponse>> GetAllAsync()
    {
        var userId = _currentUserService.UserId;

        var budgets = await _budgetRepository.GetByUserIdAsync(userId);

        return budgets.Select(MapToResponse);
    }

    public async Task<BudgetResponse> GetByIdAsync(Guid id)
    {
        var userId = _currentUserService.UserId;

        var budget = await _budgetRepository.GetByIdAsync(id);

        if (budget is null || budget.UserId != userId)
        {
            throw new KeyNotFoundException("Budget not found.");
        }

        return MapToResponse(budget);
    }

    public async Task<BudgetResponse> UpdateAsync(
        Guid id,
        UpdateBudgetRequest request)
    {
        var userId = _currentUserService.UserId;

        var budget = await _budgetRepository.GetByIdAsync(id);

        if (budget is null || budget.UserId != userId)
        {
            throw new KeyNotFoundException("Budget not found.");
        }

        var category = await _categoryRepository
            .GetByIdAndUserIdAsync(request.CategoryId, userId);

        if (category is null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        budget.Amount = request.Amount;
        budget.Month = request.Month;
        budget.Year = request.Year;
        budget.CategoryId = request.CategoryId;

        _budgetRepository.Update(budget);
        await _budgetRepository.SaveChangesAsync();

        return MapToResponse(budget);
    }

    public async Task DeleteAsync(Guid id)
    {
        var userId = _currentUserService.UserId;

        var budget = await _budgetRepository.GetByIdAsync(id);

        if (budget is null || budget.UserId != userId)
        {
            throw new KeyNotFoundException("Budget not found.");
        }

        _budgetRepository.Delete(budget);
        await _budgetRepository.SaveChangesAsync();
    }

    private static BudgetResponse MapToResponse(Budget budget)
    {
        return new BudgetResponse
        {
            Id = budget.Id,
            Amount = budget.Amount,
            Month = budget.Month,
            Year = budget.Year,
            CategoryId = budget.CategoryId,
            CategoryName = budget.Category?.Name ?? string.Empty
        };
    }
}