using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Specifications;
using TaskWebApiLab.Auth;

namespace TaskWebApiLab.UnitOfWork
{
    public class GoalRepository : IGoalRepository, IDisposable
    {
        private ApplicationDbContext context;

        public GoalRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task<List<Goal>> GetGoals()
        {
            return context.Goals.ToListAsync();
        }

        public Task<List<Goal>> GetChildGoals(int goalId)
        {
            return context.Goals.Where(x => x.ParentTaskId == goalId).ToListAsync();
        }

        public IEnumerable<Goal> GetGoals(ExpressionSpecification<Goal> expressionSpecification)
        {
            var users = context.Goals
                    .Where(expressionSpecification.Expression)
                    .TagWith("User Repository Fetch User according to Specification")
                    .ToList();

            return users;
        }

        public Goal GetGoalByID(int id)
        {
            return context.Goals.Find(id);
        }

        public void InsertGoal(Goal goal)
        {
            context.Goals.Add(goal);
        }

        public void DeleteGoal(int goalId)
        {
            Goal goal = context.Goals.Find(goalId);
            context.Goals.Remove(goal);
        }

        public void UpdateGoal(Goal goal)
        {
            context.Entry(goal).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
