using Data.Models;
using Specifications;

namespace TaskWebApiLab.UnitOfWork
{
    public interface IGoalRepository : IDisposable
    {
        Task<List<Goal>> GetGoals();
        Task<List<Goal>> GetChildGoals(int goalId);
        IEnumerable<Goal> GetGoals(ExpressionSpecification<Goal> expressionSpecification);
        Goal GetGoalByID(int goalId);
        void InsertGoal(Goal goal);
        void DeleteGoal(int goalId);
        void UpdateGoal(int id, Goal goal);
        void SetCurrentUser(string userId);
        void Save();
    }
}
