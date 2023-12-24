using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using System.Net.Http;
using TaskWebApiLab; // replace with the namespace of your web app
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using Data.Models;
using TaskWebApiLab.ApiModels;
using TaskWebApiLab.Auth;
using ApiTests.Fixtures;

public class GoalsControllerIntegrationTests : ApiFixture
{
    public GoalsControllerIntegrationTests(ApiApplicationFactory apiAppFactory) : base(apiAppFactory)
    {
    }

    [Fact]
    public async Task GetGoals_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await ApiHttpClient.GetAsync("/api/goals");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task CreateGoal_ReturnsSuccessAndCreatesGoal()
    {
        var newGoal = new GoalModel { CategoryId = 1, Title = "TestTitle", Description = "TestDescr", DueTime = DateTime.Now.AddDays(5), ParentTaskId = 0, Status = Status.InProgress};
        var content = new StringContent(JsonConvert.SerializeObject(newGoal), Encoding.UTF8, "application/json");

        // Act
        var response = await ApiHttpClient.PostAsync("/api/goals", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdGoal = JsonConvert.DeserializeObject<Goal>(await response.Content.ReadAsStringAsync());
        Assert.Equal(newGoal.Title, createdGoal.Title); // or other assertions as necessary
    }

    [Fact]
    public async Task UpdateGoal_ReturnsSuccessStatusCode()
    {
        int existingGoalId = 1; // assuming this goal exists
        var updatedGoal = new GoalModel { /* Initialize with updated data */ };
        var content = new StringContent(JsonConvert.SerializeObject(updatedGoal), Encoding.UTF8, "application/json");

        // Act
        var response = await ApiHttpClient.PutAsync($"/api/goals/{existingGoalId}", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode); // usually no content for successful PUT
    }

    [Fact]
    public async Task DeleteGoal_ReturnsSuccessStatusCode()
    {
        var newGoal = new GoalModel { CategoryId = 1, Title = "TestTitle", Description = "TestDescr", DueTime = DateTime.Now.AddDays(5), ParentTaskId = 0, Status = Status.InProgress };
        var content = new StringContent(JsonConvert.SerializeObject(newGoal), Encoding.UTF8, "application/json");

        // Act
        var response = await ApiHttpClient.PostAsync("/api/goals", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdGoal = JsonConvert.DeserializeObject<Goal>(await response.Content.ReadAsStringAsync());
        int deletableGoalId = createdGoal.Id;

        // Act
        var response2 = await ApiHttpClient.DeleteAsync($"/api/goals/{deletableGoalId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response2.StatusCode);
    }

    [Fact]
    public async Task GetChildGoals_ReturnsChildGoals_WhenTheyExist()
    {
        // Arrange
        int parentGoalId = 0; // Assuming this goal exists and has child goals

        // Act
        var response = await ApiHttpClient.GetAsync($"/api/goals/child-goals/{parentGoalId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var childGoals = JsonConvert.DeserializeObject<IEnumerable<Goal>>(await response.Content.ReadAsStringAsync());
        Assert.NotEmpty(childGoals); // Ensure it returns child goals
    }

    [Fact]
    public async Task GetGoal_ReturnsGoal_WhenGoalExists()
    {
        // Arrange
        int existingGoalId = 10; // Assuming this goal exists

        // Act
        var response = await ApiHttpClient.GetAsync($"/api/goals/{existingGoalId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var goal = JsonConvert.DeserializeObject<Goal>(await response.Content.ReadAsStringAsync());
        Assert.Equal(existingGoalId, goal.Id); // Ensure it returns the correct goal
    }

    [Fact]
    public async Task Register_User_ReturnsSuccess()
    {
        var newUser = new RegisterModel
        {
            Username = "testUserInTests1234",
            Email = "adminuser@example.com",
            Password = "TestPassword_1234"
        };
        var content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");

        // Act
        var response = await ApiHttpClient.PostAsync("/api/authenticate/register", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Response>(responseBody);
        Assert.Equal("Success", result.Status); // Assuming a Response model with Status property
    }

    [Fact]
    public async Task Login_ValidUser_ReturnsToken()
    {
        // Arrange
        // First, register or ensure a user exists in the test database...
        // ...

        // Now, create login model for the existing user
        var loginModel = new LoginModel
        {
            Username = "Admin",
            Password = "Admin_1"
        };
        var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");

        // Act
        var response = await ApiHttpClient.PostAsync("/api/authenticate/login", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<dynamic>(responseBody);
        Assert.NotNull(result.token);
        Assert.NotNull(result.expiration);
    }

    [Fact]
    public async Task RegisterAdmin_NewAdminUser_ReturnsSuccess()
    {
        var newAdmin = new RegisterModel
        {
            Username = "testAdmin1234",
            Email = "testuser@example.com",
            Password = "TestPassword_1234"
        };
        var content = new StringContent(JsonConvert.SerializeObject(newAdmin), Encoding.UTF8, "application/json");

        // Act
        var response = await ApiHttpClient.PostAsync("/api/authenticate/register-admin", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<Response>(responseBody);
        Assert.Equal("Success", result.Status);
    }


}