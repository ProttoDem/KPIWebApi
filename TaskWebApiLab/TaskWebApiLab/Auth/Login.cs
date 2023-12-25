using Ardalis.Result;
using FastEndpoints;
using MediatR;
using NuGet.Protocol.Plugins;
using TaskWebApiLab.Goals;
using TaskWebApiLab.UseCases.Auth.Login;

namespace TaskWebApiLab.Auth
{
    public class Login(IMediator _mediator) : Endpoint<ApiLoginRequest, TokenRecord>
    {
        public override void Configure()
        {
            Post(ApiLoginRequest.Route);
            AllowAnonymous();
        }

        public override async Task HandleAsync(ApiLoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new LoginQuery(request.Username, request.Password));

            if (result.Status == ResultStatus.Invalid)
            {
                await SendUnauthorizedAsync(cancellationToken);
                return;
            }

            if (result.IsSuccess)
            {
                Response = new TokenRecord(result.Value.Token, result.Value.Expiration);
                return;
            }
        }
    }
}
