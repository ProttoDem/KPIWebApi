using Ardalis.Result;
using FastEndpoints;
using MediatR;
using TaskWebApiLab.Core.GoalAggregate;
using TaskWebApiLab.UseCases.Goals.Get;
using TaskWebApiLab.UseCases.Goals.GetAll;

namespace TaskWebApiLab.Goals
{
    public class GetAll(IMediator _mediator)
        : Endpoint<GetAllRequest, GetAllResponce>
    {
        public override void Configure()
        {
            Get(GetAllRequest.Route);
        }

        public override async Task HandleAsync(GetAllRequest request,
            CancellationToken cancellationToken)
        {
            var command = new GetAllGoalsQuery();

            var result = await _mediator.Send(command);

            if (result.Status == ResultStatus.NotFound)
            {
                await SendNotFoundAsync(cancellationToken);
                return;
            }

            if (result.IsSuccess)
            {
                Response = new GetAllResponce(result.Value.Select(x => new GoalRecord(x.id, x.Title, x.Description,
                    x.CreatedAt, x.DueTime, x.Status, x.CategoryId, x.ParentGoalId)).ToList());}
        }
    }
}
