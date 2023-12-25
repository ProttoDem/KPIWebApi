using TaskWebApiLab.Core.GoalAggregate;

namespace TaskWebApiLab.Goals
{
    public record GoalRecord(int id, string Title, string? Description, DateTime CreatedAt, DateTime? DueTime,
        Status Status, int CategoryId, int? ParentGoalId);

}
