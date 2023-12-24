using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clean.Architecture.IntegrationTests.Data;
using Data.Models;

namespace ApiTests
{
    public class GoalRepositoryIntegrationTests : GoalEfRepoTestFixture
    {
        [Fact]
        public async Task AddsGoalAndSetsId()
        {
            var goalTitle = "TestGoalByEfFixture";
            var goalStatus = Status.InProgress;
            var goal = new Goal
            {
                UserId = "793779c1-bfec-4b1e-8ba6-0bd23866508a",
                CategoryId = 1,
                ParentTaskId = 0,
                Title = goalTitle,
                Description = "description",
                CreatedAt = DateTime.Now,
                DueTime = DateTime.Now.AddDays(5),
                Status = goalStatus
            };
            var repository = GetRepository();

            repository.InsertGoal(goal);

            var newGoal = (await repository.GetGoals())
                .FirstOrDefault(x => x.Title == goal.Title);

            Assert.Equal(goalTitle, newGoal?.Title);
            Assert.Equal(goalStatus, newGoal?.Status);
            Assert.True(newGoal?.Id > 0);
        }
    }
}
