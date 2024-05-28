using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.employee;

public class GetEmployeeWorkShifts : QueryEndpointBase
    .WithRequest<GetEmployeeWorkShiftsRequest>
    .WithResponse<QueryContracts.employee.GetEmployeeWorkShifts.Answer> {

    private readonly IMediator _mediator;

    public GetEmployeeWorkShifts(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("employees/{Id}/work-shifts")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.employee.GetEmployeeWorkShifts.Answer>> HandleAsync(GetEmployeeWorkShiftsRequest request) {
        var query = new QueryContracts.employee.GetEmployeeWorkShifts.Query(request.Id, request.PageNumber, request.PageSize);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class GetEmployeeWorkShiftsRequest {

    [FromRoute] public string Id { get; set; } = null!;
    [FromQuery] public int PageNumber { get; set; } = 1;
    [FromQuery] public int PageSize { get; set; } = 10;
}