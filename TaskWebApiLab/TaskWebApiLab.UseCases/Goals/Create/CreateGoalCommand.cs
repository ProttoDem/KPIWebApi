using Ardalis.Result;
using TaskWebApiLab.Core.GoalAggregate;

namespace TaskWebApiLab.UseCases.Goals.Create
{
    public record CreateGoalCommand(string Title, string? Description, int ParentGoalId, int CategoryId,
        DateTime? DueTime, Status Status) : Ardalis.SharedKernel.ICommand<Result<int>>;

}
