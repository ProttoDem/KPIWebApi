
using Ardalis.SharedKernel;

namespace TaskWebApiLab.Core.GoalAggregate
{
    public class Goal : EntityBase, IAggregateRoot
    {
        public Goal(string userId, int categoryId, int parentGoalId, string title, string description, DateTime createdAt, DateTime? dueTime, Status status)
        {
            UserId = userId;
            CategoryId = categoryId;
            ParentGoalId = parentGoalId;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            DueTime = dueTime;
            Status = status;
        }

        public string UserId { get; set; }
        public int CategoryId { get; set; }
        public int ParentGoalId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueTime { get; set; }
        public Status Status { get; set; }
    }
}
