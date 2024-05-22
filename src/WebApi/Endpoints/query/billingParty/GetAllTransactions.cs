using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.billingParty;

namespace WebApi.Endpoints.query.billingParty;

public class GetAllPartyTransactions : QueryEndpointBase
    .WithRequest<GetAllPartyTransactionsRequest> 
    .WithResponse<GetAllBillingPartyTransaction.Answer>{

    private readonly IMediator _mediator;

    public GetAllPartyTransactions(IMediator mediator) {
        _mediator = mediator;
    }

    public override async Task<ActionResult<GetAllBillingPartyTransaction.Answer>> HandleAsync(GetAllPartyTransactionsRequest request) {
        var query = new GetAllBillingPartyTransaction.Query(request.Id, request.PageNumber, request.PageSize);
        var answer = await _mediator.Send(query);
        return Ok(answer);

    }
}

public class GetAllPartyTransactionsRequest {
    [Required] public string Id { get; set; } = null!;

    [FromQuery] public int PageNumber { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = 10;
}