using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.employee;

namespace WebApi.Endpoints.query.employee;

public class GetAllEmployees : QueryEndpointBase
    .WithoutRequest
    .WithResponse<QueryContracts.employee.GetAllEmployees.Answer> {

    private readonly IMediator _mediator;

    public GetAllEmployees(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("employees")]
    // [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.employee.GetAllEmployees.Answer>> HandleAsync() {
        var query = new QueryContracts.employee.GetAllEmployees.Query();
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}