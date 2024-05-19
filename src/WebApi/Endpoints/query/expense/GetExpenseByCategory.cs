using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.expense;

public class GetExpenseByCategory : QueryEndpointBase
    .WithRequest<GetExpenseByCategoryRequest>
    .WithResponse<QueryContracts.Expense.GetExpenseByCategory.Answer> {
    private readonly IMediator _mediator;

    public GetExpenseByCategory(IMediator mediator) {
        _mediator = mediator;
    }


    [HttpGet, Route("expenses")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.Expense.GetExpenseByCategory.Answer>> HandleAsync(
        GetExpenseByCategoryRequest request) {
        var commandRequest = new QueryContracts.Expense.GetExpenseByCategory.Query(
            request.Category,
            request.PageNumber,
            request.PageSize
        );
        var answer = await _mediator.Send(commandRequest);
        return Ok(answer);
    }
}

public class GetExpenseByCategoryRequest {
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
    [FromQuery] [Required] public string Category { get; set; } = null!;
}