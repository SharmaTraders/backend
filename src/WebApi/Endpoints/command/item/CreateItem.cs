using CommandContracts.item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.item;

public class CreateItem : CommandEndPointBase
    .WithRequest<CreateItemRequest>
    .WithResponse<CreateItemResponse> {

    private readonly IMediator _mediator;

    public CreateItem(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("item")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult<CreateItemResponse>> HandleAsync(CreateItemRequest request) {
        var commandRequest = new CreateItemCommand.Request(request.RequestBody.Name, request.RequestBody.StockWeight, request.RequestBody.EstimatedValuePerKilo);
        var result =await _mediator.Send(commandRequest);
        return Ok(new CreateItemResponse() {
            Id = result.Id
        });
    }
}


public class CreateItemRequest {

    [FromBody] public Body RequestBody { get; set; }= null!;
    public record Body(string Name, double? StockWeight, double? EstimatedValuePerKilo);
}

public class CreateItemResponse {
    public string Id { get; set; }
}