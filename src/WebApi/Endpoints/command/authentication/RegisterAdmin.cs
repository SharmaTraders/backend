using CommandContracts.authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.authentication;

public class RegisterAdmin : CommandEndPointBase.WithRequest<RegisterAdminRequest>
    .WithoutResponse {
    private readonly IMediator _mediator;

    public RegisterAdmin(IMediator mediator) {
        _mediator = mediator;
    }


    [HttpPost, Route("auth/register/admin")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult> HandleAsync(RegisterAdminRequest request) {
        var commandRequest = new RegisterAdminCommand.Request(request.RequestBody.Email, request.RequestBody.Password);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class RegisterAdminRequest {
    [FromBody] public Body RequestBody { get; set; }= null!;

    public record Body(string Email, string Password);
}