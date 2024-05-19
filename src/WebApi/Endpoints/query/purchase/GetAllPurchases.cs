using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.purchase;

public class GetAllPurchases : QueryEndpointBase
    .WithRequest<GetAllPurchasesRequest>
    .WithResponse<QueryContracts.purchase.GetAllPurchases.Answer>
{
    private readonly IMediator _mediator;

    public GetAllPurchases(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Route("purchase")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.purchase.GetAllPurchases.Answer>> HandleAsync(
        GetAllPurchasesRequest request)
    {
        var query = new QueryContracts.purchase.GetAllPurchases.Query(request.PageNumber, request.PageSize);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class GetAllPurchasesRequest
{
    [FromQuery] public int PageNumber { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = 10;
}