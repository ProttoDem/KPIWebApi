using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskWebApiLab.Core.GoalAggregate;

namespace TaskWebApiLab.UseCases.Goals
{
    public record GoalDTO(int id, string Title, string Description, int CategoryId, DateTime CreatedAt,
        DateTime? DueTime, Status Status, int ParentGoalId);
}
