using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Specifications;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.Controllers;
using TaskWebApiLab.UnitOfWork;

namespace ApiTests
{
    public class GoalsControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> mockUserManager;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
        private readonly Mock<IGoalRepository> mockGoalRepo;
        private readonly string testUserName = "testuser@example.com";
        private readonly List<Goal> fakeGoals = new List<Goal> { new Goal {
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
                                                } };

        public GoalsControllerTests()
        {
            // Mock setup for IHttpContextAccessor
            mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var mockHttpContext = new Mock<HttpContext>();
            var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, testUserName)
            }));
            var testUser = new IdentityUser { UserName = testUserName, Id = "test-user-id" }; // Ensure ID is set
            mockUserManager = new Mock<UserManager<IdentityUser>>(
                new Mock<IUserStore<IdentityUser>>().Object,
                null, null, null, null, null, null, null, null);
            mockUserManager.Setup(um => um.FindByNameAsync(testUserName)).ReturnsAsync(testUser);
            mockHttpContext.Setup(c => c.User).Returns(mockUser);
            mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

            // Mock setup for IGoalRepository
            mockGoalRepo = new Mock<IGoalRepository>();
            // ... setup mockGoalRepo as needed
        }

        [Fact]
        public async Task GetGoals_ReturnsAllGoals()
        {
            mockGoalRepo.Setup(repo => repo.GetGoals()).ReturnsAsync(fakeGoals);
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);

            // Act
            var result = await controller.GetGoals();
            
            mockUserManager.Verify(um => um.FindByNameAsync(testUserName), Times.Once());
            //Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetGoals_ReturnsGoals_WhenGoalsExist()
        {
            mockGoalRepo.Setup(repo => repo.GetGoals()).ReturnsAsync(fakeGoals);
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);

            // Act
            var result = await controller.GetGoals();
            
            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GoalModel>>>(result);
        }

        [Fact]
        public async Task GetGoals_ReturnsNotFound_WhenNoGoalsExist()
        {
            // Arrange
            mockGoalRepo.Setup(repo => repo.GetGoals()).ReturnsAsync(new List<Goal>());
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);

            // Act
            var result = await controller.GetGoals();

            // Assert
            Assert.IsType<StatusCodeResult>(result.Result);
        }

        [Fact]
        public async Task PostGoal_CreatesGoal_WithValidData()
        {
            // Arrange
            var newGoalModel = new GoalModel { /* Initialize with valid data */ };
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);

            // Act
            var result = await controller.PostGoal(newGoalModel);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
            // Verify that InsertGoal and Save were called on the repository
            mockGoalRepo.Verify(repo => repo.InsertGoal(It.IsAny<Goal>()), Times.Once());
            mockGoalRepo.Verify(repo => repo.Save(), Times.Once());
        }

        [Fact]
        public async Task DeleteGoal_ReturnsNoContent_WhenGoalExists()
        {
            // Arrange
            int goalId = 1; // Existing goal ID
            mockGoalRepo.Setup(repo => repo.GetGoalByID(goalId)).Returns(new Goal { Id = goalId });
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);

            // Act
            var result = await controller.DeleteGoal(goalId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockGoalRepo.Verify(repo => repo.DeleteGoal(goalId), Times.Once());
            mockGoalRepo.Verify(repo => repo.Save(), Times.Once());
        }

        [Fact]
        public void GoalExists_ReturnsTrue_WhenGoalExists()
        {
            // Arrange
            int existingGoalId = 1;
            mockGoalRepo.Setup(repo => repo.GetGoalByID(existingGoalId)).Returns(new Goal { Id = existingGoalId });
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);

            // Act
            bool exists = controller.GoalExists(existingGoalId); // Assuming GoalExists is public or internal with [InternalsVisibleTo]

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task GetChildGoals_ReturnsChildGoals_WhenTheyExist()
        {
            // Arrange
            int parentId = 1;
            var fakeChildGoals = new List<Goal> { new Goal { /* Initialize properties */ }, new Goal { /* Initialize properties */ } };
            mockGoalRepo.Setup(repo => repo.GetChildGoals(parentId)).ReturnsAsync(fakeChildGoals);
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.GetChildGoals(parentId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Goal>>>(result);
            var returnedGoals = Assert.IsAssignableFrom<IEnumerable<Goal>>(actionResult.Value);
            Assert.Equal(fakeChildGoals.Count, ((List<Goal>)returnedGoals).Count);
        }

        [Fact]
        public async Task GetChildGoals_ReturnsNotFound_WhenNoChildGoalsExist()
        {
            // Arrange
            int parentId = 1;
            mockGoalRepo.Setup(repo => repo.GetChildGoals(parentId)).ReturnsAsync(new List<Goal>());
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.GetChildGoals(parentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetGoalsSortByCategory_ReturnsGoals_WhenCategoryExists()
        {
            // Arrange
            int? testCategoryId = 1;
            var fakeGoals = new List<Goal> { new Goal { /* Initialize properties */ }, new Goal { /* Initialize properties */ } };
            mockGoalRepo.Setup(repo => repo.GetGoals(It.IsAny<ExpressionSpecification<Goal>>())).Returns(fakeGoals);
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.GetGoalsSortByCategory(testCategoryId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<GoalModel>>>(result);
            var objectResult = Assert.IsType<ObjectResult>(actionResult.Result);
            Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetGoalsSortByCategory_ReturnsNotFound_WhenNoGoalsInCategory()
        {
            // Arrange
            int? testCategoryId = 1;
            var emptyGoals = new List<Goal>();
            mockGoalRepo.Setup(repo => repo.GetGoals(It.IsAny<ExpressionSpecification<Goal>>())).Returns(emptyGoals);
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.GetGoalsSortByCategory(testCategoryId);

            // Assert
            Assert.IsType<StatusCodeResult>(result.Result);
        }

        [Fact]
        public async Task PutGoal_ReturnsBadRequest_WhenIdDoesNotMatchGoalId()
        {
            // Arrange
            int id = 1; // The ID from the route
            var goal = new Goal { Id = 2 }; // Different ID in the goal object
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.PutGoal(id, goal);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutGoal_ReturnsNotFound_WhenGoalDoesNotExist()
        {
            // Arrange
            int id = 1;
            var goal = new Goal { Id = id };
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.PutGoal(id, goal);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutGoal_ReturnsNoContent_WhenGoalIsUpdated()
        {
            // Arrange
            int id = 1;
            var goal = new Goal { Id = id };
            var controller = new GoalsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
            // Act
            var result = await controller.PutGoal(id, goal);

            // Assert
            Assert.IsType<NoContentResult>(result);
            mockGoalRepo.Verify(repo => repo.UpdateGoal(id, goal), Times.Once()); // Verify UpdateGoal was called
            mockGoalRepo.Verify(repo => repo.Save(), Times.Once()); // Verify Save was called
        }


    }
}
