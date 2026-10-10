using ExpenseTracker.Application.DTOs.Expenses;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using Moq;
using Xunit;

namespace ExpenseTracker.Tests.Services;

public class ExpenseServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenCategoryDoesNotBelongToCurrentUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var expenseRepository = new Mock<IExpenseRepository>();
        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        categoryRepository
            .Setup(x => x.GetByIdAndUserIdAsync(categoryId, userId))
            .ReturnsAsync((ExpenseTracker.Domain.Category?)null);

        var service = new ExpenseService(
            expenseRepository.Object,
            currentUserService.Object,
            categoryRepository.Object);

        var request = new CreateExpenseRequest
        {
            Description = "Lunch",
            Amount = 50.00m,
            Date = DateTime.UtcNow,
            CategoryId = categoryId
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.CreateAsync(request));
    }


   [Fact]
    public async Task CreateAsync_ShouldCreateExpense_WhenCategoryBelongsToCurrentUser()
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

        var expenseRepository = new Mock<IExpenseRepository>();
        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        categoryRepository
            .Setup(x => x.GetByIdAndUserIdAsync(categoryId, userId))
            .ReturnsAsync(category);

        var request = new CreateExpenseRequest
        {
            Description = "Lunch",
            Amount = 50.00m,
            Date = DateTime.UtcNow,
            CategoryId = categoryId
        };

        expenseRepository
            .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => new Expense
            {
                Id = id,
                Description = request.Description,
                Amount = request.Amount,
                Date = request.Date,
                CategoryId = categoryId,
                UserId = userId,
                Category = category
            });

        var service = new ExpenseService(
            expenseRepository.Object,
            currentUserService.Object,
            categoryRepository.Object);

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Lunch", result.Description);
        Assert.Equal(50.00m, result.Amount);
        Assert.Equal(categoryId, result.CategoryId);
        Assert.Equal("Food", result.CategoryName);

        expenseRepository.Verify(
            x => x.AddAsync(It.Is<Expense>(expense =>
                expense.Description == "Lunch" &&
                expense.Amount == 50.00m &&
                expense.CategoryId == categoryId &&
                expense.UserId == userId)),
            Times.Once);

        expenseRepository.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenExpenseBelongsToAnotherUser()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var expenseId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var expense = new Expense
        {
            Id = expenseId,
            Description = "Lunch",
            Amount = 50.00m,
            Date = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            CategoryId = categoryId
        };

        var expenseRepository = new Mock<IExpenseRepository>();
        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(currentUserId);

        expenseRepository
            .Setup(x => x.GetByIdAsync(expenseId))
            .ReturnsAsync(expense);

        var service = new ExpenseService(
            expenseRepository.Object,
            currentUserService.Object,
            categoryRepository.Object);

        var request = new UpdateExpenseRequest
        {
            Description = "Updated lunch",
            Amount = 60.00m,
            Date = DateTime.UtcNow,
            CategoryId = categoryId
        };

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.UpdateAsync(expenseId, request));

        expenseRepository.Verify(
            x => x.Update(It.IsAny<Expense>()),
            Times.Never);

        expenseRepository.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }
}