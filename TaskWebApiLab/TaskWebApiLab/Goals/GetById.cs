using Ardalis.Result;
using FastEndpoints;
using MediatR;
using TaskWebApiLab.UseCases.Goals.Get;

namespace TaskWebApiLab.Goals
{
    public class GetById(IMediator _mediator)
        : Endpoint<GetGoalByIdRequest, GoalRecord>
    {
        public override void Configure()
        {
            Get(GetGoalByIdRequest.Route);
        }

        public override async Task HandleAsync(GetGoalByIdRequest request,
            CancellationToken cancellationToken)
        {
            var command = new GetGoalQuery(request.GoalId);

            var result = await _mediator.Send(command);

            if (result.Status == ResultStatus.NotFound)
            {
                await SendNotFoundAsync(cancellationToken);
                return;
            }

            if (result.IsSuccess)
            {
                Response = new GoalRecord(result.Value.id, result.Value.Title, result.Value.Description, result.Value.CreatedAt, result.Value.DueTime, result.Value.Status, result.Value.CategoryId, result.Value.ParentGoalId);
            }
        }
    }
}
