using Ardalis.Result;
using FastEndpoints;
using MediatR;
using TaskWebApiLab.UseCases.Auth.Register;

namespace TaskWebApiLab.Auth
{
    public class Register(IMediator _mediator) : Endpoint<ApiRegisterRequest, ApiRegisterResponse>
    {
        public override void Configure()
        {
            Post(ApiRegisterRequest.Route);
            AllowAnonymous();
        }

        public override async Task HandleAsync(ApiRegisterRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new RegisterCommand(request.Username, request.Password, request.Email));
            
            if (result.IsSuccess)
            {
                Response = new ApiRegisterResponse(result.Value.Status, result.Value.Message);
                return;
            }
        }
    }
}
