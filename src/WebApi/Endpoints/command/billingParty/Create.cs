using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.billingParty;

public class Create : CommandEndPointBase
    .WithRequest<CreateBillingPartyRequest>
    .WithResponse<CreateBillingPartyResponse> {

    private readonly IMediator _mediator;

    public Create(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("billingParty")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult<CreateBillingPartyResponse>> HandleAsync(CreateBillingPartyRequest request) {
        var commandRequest = new CommandContracts.billingParty.CreateCommand.Request(request.RequestBody.Name, request.RequestBody.Address, request.RequestBody.PhoneNumber, request.RequestBody.OpeningBalance, request.RequestBody.Email, request.RequestBody.VatNumber);
        var result =await _mediator.Send(commandRequest);
        return Ok(new CreateBillingPartyResponse() {
            Id = result.Id
        });
    }
}

public class CreateBillingPartyRequest {
    
    [FromBody] public Body RequestBody { get; set; }= null!;

    public record Body(
        string Name,
        string Address,
        string? PhoneNumber,
        double? OpeningBalance,
        string? Email,
        string? VatNumber); 
}

public class CreateBillingPartyResponse {
    public string Id { get; set; }
}