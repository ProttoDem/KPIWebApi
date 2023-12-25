using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApiLab.Core.GoalAggregate.Specifications
{
    public class GoalByIdSpec : Specification<Goal>
    {
        public GoalByIdSpec(int goalId, string userId)
        {
            Query
                .Where(goal => goal.Id == goalId && goal.UserId == userId);
        }
    }
}
