using ExpenseTracker.Application.DTOs.Dashboard;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ExpenseTracker.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly IExpenseRepository _expenseRepository;
        private readonly ICurrentUserService _currentUserService;

        public DashboardService(IBudgetRepository budgetRepository, IExpenseRepository expenseRepository, ICurrentUserService currentUserService)
        {
            _budgetRepository = budgetRepository;
            _expenseRepository = expenseRepository;
            _currentUserService = currentUserService;

        }


        public async Task<DashboardSummaryResponse> GetSummaryAsync(int month, int year)
        {
            var userId = _currentUserService.UserId;

            var budgets = await _budgetRepository.GetByUserIdAndPeriodAsync(userId, month, year);

            var expenseTotals = await _expenseRepository.GetTotalsByCategoryAndPeriodAsync(userId, month, year);

            var budgetByCategory = budgets.ToDictionary(
                                            budget => budget.CategoryId,
                                            budget => budget);

            var expenseByCategory = expenseTotals.ToDictionary(
                                            expense => expense.CategoryId,
                                            expense => expense);

            var categoryIds = budgetByCategory.Keys.Union(expenseByCategory.Keys);

            var categories = categoryIds.Select(categoryId =>
            {
                var hasBudget = budgetByCategory.TryGetValue(categoryId, out var budget);

                var hasExpense = expenseByCategory.TryGetValue(categoryId, out var expense);

                var budgetAmount = hasBudget ? budget!.Amount : 0;

                var spentAmount = hasExpense ? expense.TotalSpent : 0;

                var categoryName = hasBudget ? budget!.Category?.Name ?? string.Empty : expense!.CategoryName;

                return new DashboardCategorySummary {
                    CategoryId = categoryId,
                    CategoryName = categoryName,
                    Budget = budgetAmount,
                    Spent = spentAmount,
                    Remaining = budgetAmount - spentAmount
                };

            }).OrderBy(category => category.CategoryName).ToList();

            var totalBudget = categories.Sum(categoryIds => categoryIds.Budget);
            var totalSpent = categories.Sum(categoryIds => categoryIds.Spent);

            return new DashboardSummaryResponse
            {
                Month = month,
                Year = year,
                TotalBudget = totalBudget,
                TotalSpent = totalSpent,
                Remaining = totalBudget - totalSpent,
                Categories = categories
            };
        }
    }
}
