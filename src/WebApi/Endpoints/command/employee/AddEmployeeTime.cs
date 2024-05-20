using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.employee;

public class AddEmployeeTime: CommandEndPointBase
    .WithRequest<AddEmployeeTimeRequest>
    .WithoutResponse {
    
    private readonly IMediator _mediator;

    public AddEmployeeTime(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("employee/{id}")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult> HandleAsync(AddEmployeeTimeRequest request)
    {
        var commandRequest = new CommandContracts.employee.AddEmployeeTimeCommand.Request(request.Id, request.RequestBody.StartTime, request.RequestBody.EndTime, request.RequestBody.Date, request.RequestBody.BreakTime);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class AddEmployeeTimeRequest
{
    [FromRoute] public string Id { get; set; } = null!;

    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string StartTime,
        string EndTime,
        string Date,
        string BreakTime);
}
