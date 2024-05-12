using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.item;

public class GetStocksByItem : QueryEndpointBase
    .WithRequest<GetStocksByItemRequest>
    .WithResponse<QueryContracts.item.GetStocksByItem.Answer> {

    private readonly IMediator _mediator;

    public GetStocksByItem(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("item/{ItemId}/stocks")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult<QueryContracts.item.GetStocksByItem.Answer>> HandleAsync(GetStocksByItemRequest request) {
        QueryContracts.item.GetStocksByItem.Query query = new(request.ItemId, request.PageNumber, request.PageSize);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class GetStocksByItemRequest {
    [FromRoute] public string ItemId { get; set; } = null!;
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 5;
}