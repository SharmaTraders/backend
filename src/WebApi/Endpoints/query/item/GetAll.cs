using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.item;

namespace WebApi.Endpoints.query.item;

public class GetAll : QueryEndpointBase
    .WithoutRequest
    .WithResponse<GetAllItems.Answer> {

    private readonly IMediator _mediator;

    public GetAll(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("item")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult<GetAllItems.Answer>> HandleAsync() {
        var query = new GetAllItems.Query();
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}