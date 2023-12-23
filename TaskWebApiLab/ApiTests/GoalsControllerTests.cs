using Moq;
using TaskWebApiLab.Controllers;
using Microsoft.AspNetCore.Mvc;
using Data.Models;
using TaskWebApiLab.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using TaskWebApiLab.ApiModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class GoalsControllerTests
{
    private readonly Mock<IGoalRepository> mockGoalRepo;
    private readonly Mock<UserManager<IdentityUser>> mockUserManager;

    public GoalsControllerTests()
    {
        mockGoalRepo = new Mock<IGoalRepository>();
        mockUserManager = new Mock<UserManager<IdentityUser>>();
    }

    [Fact]
    public async Task GetGoals_ReturnsGoals_WhenGoalsExist()
    {
        var userManagerMock = new Mock<UserManager<IdentityUser>>(
            new Mock<IUserStore<IdentityUser>>().Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<IdentityUser>>().Object,
            new IUserValidator<IdentityUser>[0],
            new IPasswordValidator<IdentityUser>[0],
            new Mock<ILookupNormalizer>().Object,
            new Mock<IdentityErrorDescriber>().Object,
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<IdentityUser>>>().Object);

        // Arrange
        var fakeGoals = new List<Goal> {
            new Goal {
                Id = 1,
                UserId = "user1",
                CategoryId = 10,
                ParentTaskId = null, // Assuming this is a top-level goal
                Title = "Goal 1",
                Description = "First test goal",
                CreatedAt = DateTime.Now.AddDays(-10), // Created 10 days ago
                DueTime = DateTime.Now.AddDays(5), // Due in 5 days
                Status = Status.InProgress
            },
            new Goal {
                Id = 2,
                UserId = "user2",
                CategoryId = 20,
                ParentTaskId = 1, // Assuming this is a child of the first goal
                Title = "Goal 2",
                Description = "Second test goal",
                CreatedAt = DateTime.Now.AddDays(-5),
                DueTime = DateTime.Now.AddDays(10),
                Status = Status.OnHold
            },
            new Goal {
                Id = 3,
                UserId = "user1",
                CategoryId = 10,
                ParentTaskId = null,
                Title = "Goal 3",
                Description = "Third test goal",
                CreatedAt = DateTime.Now.AddDays(-15),
                DueTime = DateTime.Now.AddDays(1), // Due very soon
                Status = Status.Done
            }/* Populate with test goals */ 
        };
        userManagerMock
            .Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
            .Returns(new IdentityUser());
        mockGoalRepo.Setup(repo => repo.GetGoals()).ReturnsAsync(fakeGoals);
        var controller = new GoalsController(userManagerMock.Object, mockGoalRepo.Object);

        // Act
        var result = await controller.GetGoals();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedGoals = Assert.IsType<List<GoalModel>>(okResult.Value);
        Assert.Equal(fakeGoals.Count, returnedGoals.Count); // Adjust as per your actual return type
    }

    [Fact]
    public async Task GetGoals_ReturnsNotFound_WhenNoGoalsExist()
    {
        // Arrange
        var emptyGoals = new List<Goal>();
        mockGoalRepo.Setup(repo => repo.GetGoals()).ReturnsAsync(emptyGoals);
        var controller = new GoalsController(mockUserManager.Object, mockGoalRepo.Object);

        // Act
        var result = await controller.GetGoals();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    // Add additional tests for other scenarios and controller methods
}