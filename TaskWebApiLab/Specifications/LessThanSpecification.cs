// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using Data.Models;
//
// namespace Specifications
// {
//     public class LessThanSpecification : ExpressionSpecification<Goal>
//     {
//         public LessThanSpecification(Status threshold) : base(u => u.Status <= threshold)
//         {
//         }
//         
//     }
//
//     public static class LessThanSpecificationExtension
//     {
//         public static bool LessThan(this Goal goalModel, Status threshold)
//         {
//             var specification = new LessThanSpecification(threshold);
//             bool result = specification.IsSatisfied(goalModel);
//             return result;
//         }
//
//         public static IEnumerable<Goal> LessThan(this IEnumerable<Goal> goalModels, Status threshold)
//         {
//             return goalModels.Where(x => x.LessThan(threshold));
//         }
//
//         public static IQueryable<Goal> LessThan(this IQueryable<Goal> goalModels, Status threshold)
//         {
//             var specification = new LessThanSpecification(threshold);
//             return goalModels.Where(specification.Expression);
//         }
//     }
// }
