using CommandContracts.item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.item;

public class UpdateItem : CommandEndPointBase
    .WithRequest<UpdateItemRequest>
    .WithoutResponse {


    private readonly IMediator _mediator;

    public UpdateItem(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPut, Route("item/{id}")]
    [Authorize(Roles = "Admin")]

    public override async Task<ActionResult> HandleAsync(UpdateItemRequest request) {
        var commandRequest = new UpdateItemCommand.Request(request.Id, request.RequestBody.Name);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class UpdateItemRequest {

    [FromRoute] public string Id { get; set; }= null!;

    [FromBody]
    public Body RequestBody { get; set; }= null!;

    public record Body(string Name);
}