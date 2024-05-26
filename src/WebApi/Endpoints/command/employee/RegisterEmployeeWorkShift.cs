using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.employee;

public class RegisterEmployeeWorkShift: CommandEndPointBase
    .WithRequest<RegisterEmployeeWorkShiftRequest>
    .WithResponse<CommandContracts.employee.RegisterEmployeeWorkShiftCommand.Response> {
    
    private readonly IMediator _mediator;

    public RegisterEmployeeWorkShift(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("employees/{Id}/work-shift")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<CommandContracts.employee.RegisterEmployeeWorkShiftCommand.Response>> HandleAsync(RegisterEmployeeWorkShiftRequest request)
    {
        var commandRequest = new CommandContracts.employee.RegisterEmployeeWorkShiftCommand.Request(request.Id, request.RequestBody.StartTime, request.RequestBody.EndTime, request.RequestBody.Date, request.RequestBody.BreakInMinutes);
        var result = await _mediator.Send(commandRequest);
        return Ok(result);
    }
}

public class RegisterEmployeeWorkShiftRequest
{
    [FromRoute] public string Id { get; set; } = null!;

    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string StartTime,
        string EndTime,
        string Date,
        [Required]
        int BreakInMinutes);
}
