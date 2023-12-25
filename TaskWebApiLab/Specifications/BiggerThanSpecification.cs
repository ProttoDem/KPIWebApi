/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Specifications
{
    public class BiggerThanSpecification : ExpressionSpecification<Goal>
    {
        public BiggerThanSpecification(Status threshold) : base(u => u.Status >= threshold)
        {
        }
        
    }

    public static class BiggerThanSpecificationExtension
    {
        public static bool BiggerThan(this Goal goalModel, Status threshold)
        {
            var specification = new BiggerThanSpecification(threshold);
            bool result = specification.IsSatisfied(goalModel);
            return result;
        }

        public static IEnumerable<Goal> BiggerThan(this IEnumerable<Goal> goalModels, Status threshold)
        {
            return goalModels.Where(x => x.BiggerThan(threshold));
        }

        public static IQueryable<Goal> BiggerThan(this IQueryable<Goal> goalModels, Status threshold)
        {
            var specification = new BiggerThanSpecification(threshold);
            return goalModels.Where(specification.Expression);
        }
    }
}
*/
