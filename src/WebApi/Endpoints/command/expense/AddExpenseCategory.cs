using CommandContracts.expense;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.expense;

public class AddExpenseCategory : CommandEndPointBase
    .WithRequest<AddExpenseCategoryRequest>
    .WithoutResponse {

    private readonly IMediator _mediator;

    public AddExpenseCategory(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpPost, Route("expense-category")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult> HandleAsync(AddExpenseCategoryRequest request) {
        var commandRequest = new AddExpenseCategoryCommand.Request(request.RequestBody.Name);
        await _mediator.Send(commandRequest);
        return Ok();
    }
}

public class AddExpenseCategoryRequest {

    [FromBody] public Body RequestBody { get; set; } = null!;

    public record Body(string Name);
}