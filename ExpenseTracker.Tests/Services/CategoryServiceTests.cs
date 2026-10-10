using ExpenseTracker.Application.DTOs.Categories;
using ExpenseTracker.Application.Repositories.Interfaces;
using ExpenseTracker.Application.Services;
using ExpenseTracker.Application.Services.Interfaces;
using ExpenseTracker.Domain;
using Moq;

namespace ExpenseTracker.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateCategoryForCurrentUser()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        var service = new CategoryService(
            categoryRepository.Object,
            currentUserService.Object);

        var request = new CreateCategoryRequest
        {
            Name = "Food"
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Food", result.Name);

        categoryRepository.Verify(
            x => x.AddAsync(It.Is<Category>(category =>
                category.Name == "Food" &&
                category.UserId == userId)),
            Times.Once);

        categoryRepository.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrow_WhenCategoryBelongsToAnotherUser()
    {
        // Arrange
        var currentUserId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var category = new Category
        {
            Id = categoryId,
            Name = "Food",
            UserId = Guid.NewGuid()
        };

        var categoryRepository = new Mock<ICategoryRepository>();
        var currentUserService = new Mock<ICurrentUserService>();

        currentUserService
            .Setup(x => x.UserId)
            .Returns(currentUserId);

        categoryRepository
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(category);

        var service = new CategoryService(
            categoryRepository.Object,
            currentUserService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => service.GetByIdAsync(categoryId));
    }
}