using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskWebApiLab.Core.GoalAggregate.Specifications
{
    public class GoalsSpec : Specification<Goal>
    {
        public GoalsSpec(string userId)
        {
            Query
                .Where(goal => goal.UserId == userId);
        }
    }
}
