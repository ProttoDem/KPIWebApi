using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Specifications;
using TaskWebApiLab.Auth;
using Microsoft.AspNetCore.Authorization;

namespace TaskWebApiLab.UnitOfWork
{
    public class GoalRepository : IGoalRepository, IDisposable
    {
        private ApplicationDbContext _context;
        private string UserId;

        public GoalRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public Task<List<Goal>> GetGoals()
        {
            return _context.Goals.Where( x => x.UserId == UserId).ToListAsync();
        }

        public Task<List<Goal>> GetChildGoals(int goalId)
        {
            return _context.Goals.Where(x => x.UserId == UserId && x.ParentTaskId == goalId).ToListAsync();
        }

        public IEnumerable<Goal> GetGoals(ExpressionSpecification<Goal> expressionSpecification)
        {
            var goals = _context.Goals
                    .Where(expressionSpecification.Expression)
                    .TagWith("User Repository Fetch User according to Specification")
                    .ToList();

            return goals;
        }

        public Goal GetGoalByID(int id)
        {
            var goal = _context.Goals.Find(id);
            if (goal.UserId == UserId)
            {
                return goal;
            }
            return null;
        }

        public void InsertGoal(Goal goal)
        {
            _context.Goals.Add(goal);
        }

        public void DeleteGoal(int goalId)
        {
            Goal goal = _context.Goals.Find(goalId);
            if (goal != null && goal.UserId == UserId)
            {
                _context.Goals.Remove(goal);
            }
        }

        public void UpdateGoal(int id, Goal goal)
        {
            var goalData = _context.Goals.Find(id);
            if (goalData != null && goalData.UserId == UserId)
            {
                _context.Entry(goal).State = EntityState.Modified;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetCurrentUser(string userId)
        {
            this.UserId = userId;
        }
    }
}
