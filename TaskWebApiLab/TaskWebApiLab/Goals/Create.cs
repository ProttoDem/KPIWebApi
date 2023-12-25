using FastEndpoints;
using MediatR;
using TaskWebApiLab.Goals;
using TaskWebApiLab.UseCases.Goals.Create;

namespace TaskWebApiLab
{
    public class Create(IMediator _mediator) : Endpoint<CreateGoalRequest, CreateGoalResponse>
    {
        public override void Configure()
        {
            Post(CreateGoalRequest.Route);

        }

        public override async Task HandleAsync(CreateGoalRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CreateGoalCommand(request.Title, request.Description,
                request.ParentGoalId, request.CategoryId, request.DueTime, request.Status));

            if (result.IsSuccess)
            {
                Response = new CreateGoalResponse(result.Value, request.Title);
                return;
            }
        }
    }


}
