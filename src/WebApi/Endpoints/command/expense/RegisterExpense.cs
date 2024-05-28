using System.ComponentModel.DataAnnotations;
using CommandContracts.expense;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.expense;

public class RegisterExpense : CommandEndPointBase
    .WithRequest<RegisterExpenseRequest>
    .WithResponse<RegisterExpenseCommand.Answer> {
    private readonly IMediator _mediator;

    public RegisterExpense(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("expenses")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<RegisterExpenseCommand.Answer>>
        HandleAsync(RegisterExpenseRequest request) {
        var commandRequest = new RegisterExpenseCommand.Request(
            request.RequestBody.Date,
            request.RequestBody.Category,
            request.RequestBody.Amount,
            request.RequestBody.Remarks,
            request.RequestBody.BillingPartyId,
            request.RequestBody.EmployeeId
        );
        var answer = await _mediator.Send(commandRequest);
        return Ok(answer);
    }
}

public class RegisterExpenseRequest {
    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(
        string Date,
        string? Category,
        [Required]
        double Amount,
        string? Remarks,
        string? BillingPartyId,
        string? EmployeeId);
}