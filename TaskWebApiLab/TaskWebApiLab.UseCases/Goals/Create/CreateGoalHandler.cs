
using Ardalis.Result;
using Ardalis.SharedKernel;
using Microsoft.AspNetCore.Identity;
using TaskWebApiLab.Core.GoalAggregate;
using TaskWebApiLab.Core.Interfaces;

namespace TaskWebApiLab.UseCases.Goals.Create
{
    public class CreateGoalHandler(UserManager<IdentityUser> _userManager, IUserAccessor _userAccessor, IRepository<Goal> _goalRepository) : ICommandHandler<CreateGoalCommand, Result<int>>
    {
        
        public async Task<Result<int>> Handle(CreateGoalCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(_userAccessor.User.Identity.Name);
            var userId = user.Id;
            var newGoal = new Goal(userId, request.CategoryId, request.ParentGoalId, request.Title, request.Description,
                DateTime.Now, request.DueTime, request.Status);
            var createdItem = await _goalRepository.AddAsync(newGoal, cancellationToken);

            return createdItem.Id;
        }
    }
}
