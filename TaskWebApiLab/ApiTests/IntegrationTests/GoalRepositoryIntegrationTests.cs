using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiTests.Fixtures;
using Data.Models;

namespace ApiTests.IntegrationTests
{
    public class GoalRepositoryIntegrationTests : GoalEfRepoTestFixture
    {
        [Fact]
        public async Task AddsGoalAndSetsId()
        {
            var goalTitle = "TestGoalByEfFixture";
            var goalStatus = Status.InProgress;
            var userId = "793779c1-bfec-4b1e-8ba6-0bd23866508a";
            var goal = new Goal
            {
                UserId = userId,
                CategoryId = 1,
                ParentTaskId = 0,
                Title = goalTitle,
                Description = "description",
                CreatedAt = DateTime.Now,
                DueTime = DateTime.Now.AddDays(5),
                Status = goalStatus
            };
            var repository = GetRepository();
            repository.SetCurrentUser(userId);
            repository.InsertGoal(goal);

            var newGoal = (await repository.GetGoals())
                .FirstOrDefault(x => x.Title == goal.Title);
            var test = await repository.GetGoals();

            Assert.Equal(goalTitle, newGoal?.Title);
            Assert.Equal(goalStatus, newGoal?.Status);
            Assert.True(newGoal?.Id > 0);
        }
    }
}
