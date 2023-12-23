using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.Controllers;
using TaskWebApiLab.UnitOfWork;

public class CategoriesControllerTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly CategoriesController _controller;

    public CategoriesControllerTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        _controller = new CategoriesController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetCategories_ReturnsAllCategories()
    {
        // Arrange
        var fakeCategories = new List<Category> { /* populate with test categories */ };
        _mockRepo.Setup(repo => repo.GetCategories()).ReturnsAsync(fakeCategories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<Category>>(actionResult.Value);
        Assert.Equal(fakeCategories.Count, ((List<Category>)returnValue).Count);
    }

    [Fact]
    public async Task GetCategory_ReturnsNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        int testCategoryId = 1;
        _mockRepo.Setup(repo => repo.GetCategoryByID(testCategoryId)).Returns((Category)null);

        // Act
        var result = await _controller.GetCategory(testCategoryId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCategory_ReturnsCategory_WhenCategoryExists()
    {
        // Arrange
        int testCategoryId = 1;
        var testCategory = new Category { Id = testCategoryId, Title = "Test", Description = "Test Description" };
        _mockRepo.Setup(repo => repo.GetCategoryByID(testCategoryId)).Returns(testCategory);

        // Act
        var result = await _controller.GetCategory(testCategoryId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        var returnedCategory = Assert.IsType<Category>(actionResult.Value);
        Assert.Equal(testCategory.Id, returnedCategory.Id);
        Assert.Equal(testCategory.Title, returnedCategory.Title);
    }

    [Fact]
    public async Task PostCategory_AddsNewCategory()
    {
        // Arrange
        var newCategory = new CategoryModel { Title = "New Category", Description = "New Description" };
        var newCategoryId = 123;
        _mockRepo.Setup(repo => repo.InsertCategory(It.IsAny<Category>()));
        _mockRepo.Setup(repo => repo.Save()).Verifiable();

        // Act
        var result = await _controller.PostCategory(newCategory);

        // Assert
        var actionResult = Assert.IsType<ActionResult<CategoryModel>>(result);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        _mockRepo.Verify();
        Assert.Equal("GetCategory", createdAtActionResult.ActionName);
    }


}