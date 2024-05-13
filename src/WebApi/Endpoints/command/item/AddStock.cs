using CommandContracts.item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.item;

public class AddStock : CommandEndPointBase
    .WithRequest<AddStockRequest>
    .WithoutResponse {
    private readonly IMediator _mediator;

    public AddStock(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPatch, Route("item/{Id}/add-stock")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult> HandleAsync(AddStockRequest request) {
        var commandRequest = new AddStockCommand.Request(request.Id,
            request.RequestBody.Weight,
            request.RequestBody.ExpectedValuePerKilo,
            request.RequestBody.Date,
            request.RequestBody.Remarks);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class AddStockRequest {
    [FromRoute] public string Id { get; set; } = null!;

    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(double Weight, double ExpectedValuePerKilo, string Date, string? Remarks);
}