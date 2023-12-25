using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskWebApiLab.Core.GoalAggregate.Specifications;
using TaskWebApiLab.Core.GoalAggregate;
using TaskWebApiLab.Core.Interfaces;
using TaskWebApiLab.UseCases.Goals.Get;

namespace TaskWebApiLab.UseCases.Goals.GetAll
{
    public class GetAllGoalsHandler(UserManager<IdentityUser> _userManager, IUserAccessor _userAccessor, IRepository<Goal> _goalRepository) : IQueryHandler<GetAllGoalsQuery, Result<IEnumerable<GoalDTO>>>
    {

        public async Task<Result<IEnumerable<GoalDTO>>> Handle(GetAllGoalsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.User.Identity.Name);
            var userId = user.Id;
            var spec = new GoalsSpec(userId);
            var entity = await _goalRepository.ListAsync(spec, cancellationToken);
            if (entity == null || entity.Count == 0) return Result.NotFound();

            var goals = entity.Select( x=> new GoalDTO(x.Id, x.Title, x.Description, x.CategoryId, x.CreatedAt, x.DueTime, x.Status, x.ParentGoalId)).ToList();
            return goals;
        }
    }
}
