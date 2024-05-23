using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.reports;

public class ExpenseByCategoryReport : QueryEndpointBase
    .WithRequest<ExpenseByCategoryReportRequest>
    .WithResponse<QueryContracts.reports.ExpenseByCategoryReport.Answer> {
    private readonly IMediator _mediator;

    public ExpenseByCategoryReport(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("reports/expense-by-category")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.reports.ExpenseByCategoryReport.Answer>> HandleAsync(
        ExpenseByCategoryReportRequest request) {
        var query = new QueryContracts.reports.ExpenseByCategoryReport.Query(request.DateFrom, request.DateTo);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class ExpenseByCategoryReportRequest {
    [FromQuery] public string? DateFrom { get; set; } = null;
    [FromQuery] public string? DateTo { get; set; } = null;
}