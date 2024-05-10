using CommandContracts.item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.item;

public class ReduceStock : CommandEndPointBase
    .WithRequest<ReduceStockRequest>
    .WithoutResponse {

    private readonly IMediator _mediator;

    public ReduceStock(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPatch, Route("item/{id}/reduce-stock")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult> HandleAsync(ReduceStockRequest request) {
        var commandRequest = new ReduceStockCommand.Request(request.Id, request.RequestBody.Weight, request.RequestBody.Date);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class ReduceStockRequest {
    [FromRoute] public string Id { get; set; } = null!;

    [FromBody] public Body RequestBody { get; set; }= null!;
    
    public record Body(double Weight, string Date);
}