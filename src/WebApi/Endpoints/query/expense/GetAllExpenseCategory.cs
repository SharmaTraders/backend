using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.expense;

public class GetAllExpenseCategory : QueryEndpointBase
    .WithoutRequest 
    .WithResponse<QueryContracts.Expense.GetAllExpenseCategory.Answer> {

    private readonly IMediator _mediator;

    public GetAllExpenseCategory(IMediator mediator) {
        _mediator = mediator;
    }


    [HttpGet, Route("expense-category")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.Expense.GetAllExpenseCategory.Answer>> HandleAsync() {
        var query = new QueryContracts.Expense.GetAllExpenseCategory.Query();
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}