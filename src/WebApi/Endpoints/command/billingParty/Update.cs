using CommandContracts.billingParty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.billingParty;

public class Update : CommandEndPointBase
    .WithRequest<UpdateBillingPartyRequest>
    .WithoutResponse{

    private readonly IMediator _mediator;

    public Update(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPut, Route("billingParty/{id}")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult> HandleAsync(UpdateBillingPartyRequest request) {
        var commandRequest = new UpdateBillingPartyCommand.Request(request.Id, request.RequestBody.Name, request.RequestBody.Address, request.RequestBody.PhoneNumber, request.RequestBody.Email, request.RequestBody.VatNumber);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class UpdateBillingPartyRequest {
    [FromRoute] public string Id { get; set; }= null!;
    
    [FromBody] public Body RequestBody { get; set; }= null!;

    public record Body(
        string Name,
        string Address,
        string? PhoneNumber,
        string? Email,
        string? VatNumber); 
}