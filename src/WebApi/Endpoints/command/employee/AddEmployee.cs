using CommandContracts.employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.employee;

public class AddEmployee : CommandEndPointBase
    .WithRequest<AddEmployeeRequest>
    .WithResponse<AddEmployeeResponse> {

    private readonly IMediator _mediator;

    public AddEmployee(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("employee")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult<AddEmployeeResponse>> HandleAsync(AddEmployeeRequest request) {
        var commandRequest = new AddEmployeeCommand.Request(
            request.RequestBody.Name,
            request.RequestBody.Address,
            request.RequestBody.PhoneNumber,
            request.RequestBody.Email,
            request.RequestBody.OpeningBalance,
            request.RequestBody.NormalDailyWorkingHours,
            request.RequestBody.SalaryPerHour,
            request.RequestBody.OvertimeSalaryPerHour
        );
        var result = await _mediator.Send(commandRequest);
        return Ok(new AddEmployeeResponse() {
            Id = result.Id
        });
    }
}

public class AddEmployeeRequest {
    
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string Name,
        string Address,
        string? PhoneNumber,
        string? Email,
        double? OpeningBalance,
        string NormalDailyWorkingHours,
        double SalaryPerHour,
        double OvertimeSalaryPerHour
        ); 
}

public class AddEmployeeResponse {
    public string Id { get; set; }
}