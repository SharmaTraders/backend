using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.reports;

public class StockSummaryReport : QueryEndpointBase
    .WithoutRequest
    .WithResponse<QueryContracts.reports.StockSummaryReport.Answer> {
    private readonly IMediator _mediator;

    public StockSummaryReport(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("reports/stock-summary")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.reports.StockSummaryReport.Answer>> HandleAsync() {
        var query = new QueryContracts.reports.StockSummaryReport.Query();
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}