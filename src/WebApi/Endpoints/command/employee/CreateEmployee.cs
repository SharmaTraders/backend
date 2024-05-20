using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.employee;

public class CreateEmployee : CommandEndPointBase
    .WithRequest<CreateEmployeeRequest>
    .WithResponse<CreateEmployeeResponse> {

    private readonly IMediator _mediator;

    public CreateEmployee(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("employee")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult<CreateEmployeeResponse>> HandleAsync(CreateEmployeeRequest request) {
        var commandRequest = new CommandContracts.employee.CreateEmployeeCommand.Request(request.RequestBody.FullName, request.RequestBody.Address, request.RequestBody.PhoneNumber, request.RequestBody.Email, request.RequestBody.Status);
        var result = await _mediator.Send(commandRequest);
        return Ok(new CreateEmployeeResponse() {
            Id = result.Id
        });
    }
}

public class CreateEmployeeRequest {
    
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string FullName,
        string Address,
        string? PhoneNumber,
        string? Email,
        string Status); 
}

public class CreateEmployeeResponse {
    public string Id { get; set; }
}