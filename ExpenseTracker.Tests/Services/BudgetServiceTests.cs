using ExpenseTracker.Application.DTOs.Budgets;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using Moq;
using Xunit;

namespace ExpenseTracker.Tests.Services;

public class BudgetServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCategoryDoesNotBelongToCurrentUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var budgetRepository = new Mock<IBudgetRepository>();
        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        categoryRepository
            .Setup(x => x.GetByIdAndUserIdAsync(categoryId, userId))
            .ReturnsAsync((Category?)null);

        var service = new BudgetService(
            budgetRepository.Object,
            categoryRepository.Object,
            currentUserService.Object);

        var request = new CreateBudgetRequest
        {
            Amount = 1000.00m,
            Month = 10,
            Year = 2026,
            CategoryId = categoryId
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.CreateAsync(request));
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateBudget_WhenCategoryBelongsToCurrentUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var category = new Category
        {
            Id = categoryId,
            Name = "Food",
            UserId = userId
        };

        var budgetRepository = new Mock<IBudgetRepository>();
        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        categoryRepository
            .Setup(x => x.GetByIdAndUserIdAsync(categoryId, userId))
            .ReturnsAsync(category);

        var request = new CreateBudgetRequest
        {
            Amount = 1000.00m,
            Month = 10,
            Year = 2026,
            CategoryId = categoryId
        };

        budgetRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => new Budget
            {
                Id = id,
                Amount = request.Amount,
                Month = request.Month,
                Year = request.Year,
                UserId = userId,
                CategoryId = categoryId,
                Category = category
            });

        var service = new BudgetService(
            budgetRepository.Object,
            categoryRepository.Object,
            currentUserService.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(1000.00m, result.Amount);
        Assert.Equal(10, result.Month);
        Assert.Equal(2026, result.Year);
        Assert.Equal(categoryId, result.CategoryId);
        Assert.Equal("Food", result.CategoryName);

        budgetRepository.Verify(
            x => x.AddAsync(It.Is<Budget>(budget =>
                budget.Amount == 1000.00m &&
                budget.Month == 10 &&
                budget.Year == 2026 &&
                budget.CategoryId == categoryId &&
                budget.UserId == userId)),
            Times.Once);

        budgetRepository.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
}