using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.income;

namespace WebApi.Endpoints.query.income;

public class GetIncomesByBillingParty: QueryEndpointBase
    .WithRequest<GetIncomesByBillingPartyRequest>
    .WithResponse<GetIncomeByBillingParty.Answer> {

    private readonly IMediator _mediator;

    public GetIncomesByBillingParty(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("billing-party/{Id}/incomes") ]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<GetIncomeByBillingParty.Answer>> HandleAsync(GetIncomesByBillingPartyRequest request) { 
        var query = new GetIncomeByBillingParty.Query(request.Id, request.PageNumber, request.PageSize);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class GetIncomesByBillingPartyRequest {

    [FromRoute] public string Id { get; set; } = null!;

    [FromQuery] public int PageNumber { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = 10;

}