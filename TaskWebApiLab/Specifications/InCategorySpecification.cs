// using Data.Models;
//
// namespace Specifications
// {
//     public class InCategorySpecification : ExpressionSpecification<Goal>
//     {
//         public InCategorySpecification(int threshold) : base(u => u.CategoryId == threshold)
//         {
//         }
//
//     }
//
//     public static class InCategorySpecificationExtension
//     {
//         public static bool Equal(this Goal goalModel, int threshold)
//         {
//             var specification = new InCategorySpecification(threshold);
//             bool result = specification.IsSatisfied(goalModel);
//             return result;
//         }
//
//         public static IEnumerable<Goal> Equal(this IEnumerable<Goal> goalModels, int threshold)
//         {
//             return goalModels.Where(x => x.Equal(threshold));
//         }
//
//         public static IQueryable<Goal> Equal(this IQueryable<Goal> goalModels, int threshold)
//         {
//             var specification = new InCategorySpecification(threshold);
//             return goalModels.Where(specification.Expression);
//         }
//     }
// }
