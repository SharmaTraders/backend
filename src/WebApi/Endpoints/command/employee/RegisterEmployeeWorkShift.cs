using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.employee;

public class RegisterEmployeeWorkShift: CommandEndPointBase
    .WithRequest<RegisterEmployeeWorkShiftRequest>
    .WithResponse<RegisterEmployeeWorkShiftRequest.RegisterEmployeeWorkShiftResponse> {
    
    private readonly IMediator _mediator;

    public RegisterEmployeeWorkShift(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("employee/{id}")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<RegisterEmployeeWorkShiftRequest.RegisterEmployeeWorkShiftResponse>> HandleAsync(RegisterEmployeeWorkShiftRequest request)
    {
        var commandRequest = new CommandContracts.employee.RegisterEmployeeWorkShift.Request(request.Id, request.RequestBody.StartTime, request.RequestBody.EndTime, request.RequestBody.Date, request.RequestBody.BreakInMinutes);
        var result = await _mediator.Send(commandRequest);
        return Ok(new RegisterEmployeeWorkShiftRequest.RegisterEmployeeWorkShiftResponse() {
            Id = result.Id
        });
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
        int BreakInMinutes);
    
    public class RegisterEmployeeWorkShiftResponse {
        public string Id { get; set; }
    }
}
