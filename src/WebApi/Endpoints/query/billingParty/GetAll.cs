using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.billingParty;

namespace WebApi.Endpoints.query.billingParty;

public class GetAll : QueryEndpointBase
    .WithoutRequest
    .WithResponse<GetAllBillingParties.Answer> {

    private readonly IMediator _mediator;

    public GetAll(IMediator mediator) {
        _mediator = mediator;
    }


    [HttpGet, Route("billingParty")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<GetAllBillingParties.Answer>> HandleAsync() {
        var query = new GetAllBillingParties.Query();
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

