using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using Moq;
using Xunit;

namespace ExpenseTracker.Tests.Services;

public class DashboardServiceTests
{
    [Fact]
    public async Task GetSummaryAsync_ShouldCalculateTotalsCorrectly()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var foodCategoryId = Guid.NewGuid();
        var transportCategoryId = Guid.NewGuid();

        var foodCategory = new Category
        {
            Id = foodCategoryId,
            Name = "Food",
            UserId = userId
        };

        var transportCategory = new Category
        {
            Id = transportCategoryId,
            Name = "Transport",
            UserId = userId
        };

        var budgets = new List<Budget>
        {
            new Budget
            {
                Id = Guid.NewGuid(),
                Amount = 1000.00m,
                Month = 10,
                Year = 2026,
                UserId = userId,
                CategoryId = foodCategoryId,
                Category = foodCategory
            },
            new Budget
            {
                Id = Guid.NewGuid(),
                Amount = 500.00m,
                Month = 10,
                Year = 2026,
                UserId = userId,
                CategoryId = transportCategoryId,
                Category = transportCategory
            }
        };

        var expenseTotals =
            new List<(Guid CategoryId, string CategoryName, decimal TotalSpent)>
            {
                (foodCategoryId, "Food", 350.00m),
                (transportCategoryId, "Transport", 200.00m)
            };

        var budgetRepository = new Mock<IBudgetRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        budgetRepository
            .Setup(x => x.GetByUserIdAndPeriodAsync(userId, 10, 2026))
            .ReturnsAsync(budgets);

        expenseRepository
            .Setup(x => x.GetTotalsByCategoryAndPeriodAsync(userId, 10, 2026))
            .ReturnsAsync(expenseTotals);

        var service = new DashboardService(
            budgetRepository.Object,
            expenseRepository.Object,
            currentUserService.Object);

        // Act
        var result = await service.GetSummaryAsync(10, 2026);

        // Assert
        Assert.Equal(10, result.Month);
        Assert.Equal(2026, result.Year);

        Assert.Equal(1500.00m, result.TotalBudget);
        Assert.Equal(550.00m, result.TotalSpent);
        Assert.Equal(950.00m, result.Remaining);

        Assert.Equal(2, result.Categories.Count());

        var food = result.Categories
            .Single(x => x.CategoryName == "Food");

        Assert.Equal(1000.00m, food.Budget);
        Assert.Equal(350.00m, food.Spent);
        Assert.Equal(650.00m, food.Remaining);

        var transport = result.Categories
            .Single(x => x.CategoryName == "Transport");

        Assert.Equal(500.00m, transport.Budget);
        Assert.Equal(200.00m, transport.Spent);
        Assert.Equal(300.00m, transport.Remaining);
    }

    [Fact]
    public async Task GetSummaryAsync_ShouldIncludeCategory_WhenItHasExpensesButNoBudget()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var budgetRepository = new Mock<IBudgetRepository>();
        var expenseRepository = new Mock<IExpenseRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        budgetRepository
            .Setup(x => x.GetByUserIdAndPeriodAsync(userId, 10, 2026))
            .ReturnsAsync(new List<Budget>());

        expenseRepository
            .Setup(x => x.GetTotalsByCategoryAndPeriodAsync(userId, 10, 2026))
            .ReturnsAsync(
                new List<(Guid CategoryId, string CategoryName, decimal TotalSpent)>
                {
                (categoryId, "Food", 300.00m)
                });

        var service = new DashboardService(
            budgetRepository.Object,
            expenseRepository.Object,
            currentUserService.Object);

        // Act
        var result = await service.GetSummaryAsync(10, 2026);

        // Assert
        Assert.Single(result.Categories);

        var category = result.Categories.Single();

        Assert.Equal(categoryId, category.CategoryId);
        Assert.Equal("Food", category.CategoryName);
        Assert.Equal(0.00m, category.Budget);
        Assert.Equal(300.00m, category.Spent);
        Assert.Equal(-300.00m, category.Remaining);

        Assert.Equal(0.00m, result.TotalBudget);
        Assert.Equal(300.00m, result.TotalSpent);
        Assert.Equal(-300.00m, result.Remaining);
    }
}