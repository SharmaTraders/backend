using System.ComponentModel.DataAnnotations;
using CommandContracts.income;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.income;

public class RegisterIncome : CommandEndPointBase
    .WithRequest<RegisterIncomeRequest> 
    .WithoutResponse{

    private readonly IMediator _mediator;

    public RegisterIncome(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("income")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult> HandleAsync(RegisterIncomeRequest request) {
        var commandRequest = new RegisterIncomeCommand.Request(request.RequestBody.Date, request.RequestBody.BillingPartyId, request.RequestBody.Amount, request.RequestBody.Remarks);
        var response =await _mediator.Send(commandRequest);
        return Ok(response);
    }
}


public class RegisterIncomeRequest {

    [FromBody] public Body RequestBody { get; set; }= null!;

    public record Body(string Date, string BillingPartyId, [Required] double Amount, string? Remarks);
}