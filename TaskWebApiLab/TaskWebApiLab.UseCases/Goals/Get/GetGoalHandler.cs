using System.Formats.Asn1;
using Ardalis.Result;
using Ardalis.SharedKernel;
using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;

using TaskWebApiLab.Core.GoalAggregate;
using TaskWebApiLab.Core.GoalAggregate.Specifications;
using TaskWebApiLab.Core.Interfaces;
using TaskWebApiLab.UseCases.Goals.Create;

namespace TaskWebApiLab.UseCases.Goals.Get
{
    public class GetGoalHandler(UserManager<IdentityUser> _userManager, IUserAccessor _userAccessor, IRepository<Goal> _goalRepository) : IQueryHandler<GetGoalQuery, Result<GoalDTO>>
    {

        public async Task<Result<GoalDTO>> Handle(GetGoalQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.User.Identity.Name);
            var userId = user.Id;
            var spec = new GoalByIdSpec(request.id, userId);
            var entity = await _goalRepository.FirstOrDefaultAsync(spec, cancellationToken);
            if (entity == null) return Result.NotFound();


            return new GoalDTO(entity.Id, entity.Title, entity.Description, entity.CategoryId, entity.CreatedAt,
                entity.DueTime, entity.Status, entity.ParentGoalId);
        }
    }
}
