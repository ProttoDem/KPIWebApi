using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System;
using System.Linq;
using Data.Models;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.Controllers;
using TaskWebApiLab.UnitOfWork;

public class NotificationsControllerTests
{
    private readonly Mock<UserManager<IdentityUser>> mockUserManager;
    private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;
    private readonly Mock<IGoalRepository> mockGoalRepo;
    private readonly string testUserName = "testuser@example.com";

    public NotificationsControllerTests()
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
    }

    [Fact]
    public async Task GetGoalsTime_ReturnsNotificationsForUpcomingGoals()
    {
        // Arrange
        var testHours = 48; // For example, get goals within the next 48 hours
        var testGoals = new List<Goal>
        {
            new Goal { Id = 1, DueTime = DateTime.Now.AddHours(24), Status = Status.InProgress },
            new Goal { Id = 2, DueTime = DateTime.Now.AddHours(49), Status = Status.InProgress },
            new Goal { Id = 3, DueTime = DateTime.Now.AddHours(10), Status = Status.Done }
        };

        mockGoalRepo.Setup(repo => repo.GetGoals()).ReturnsAsync(testGoals);
        var controller = new NotificationsController(mockUserManager.Object, mockHttpContextAccessor.Object, mockGoalRepo.Object);
        // Act
        var result = await controller.GetGoalsTime(testHours);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<NotificationModel>>>(result);
        var notifications = Assert.IsAssignableFrom<IEnumerable<NotificationModel>>(actionResult.Value);
        var notificationList = notifications.ToList();
        Assert.Single(notificationList);
        Assert.Equal(1, notificationList.First().GoalId);
    }
}
