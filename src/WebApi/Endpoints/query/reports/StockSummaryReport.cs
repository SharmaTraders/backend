using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.reports;

public class StockSummaryReport : QueryEndpointBase
    .WithRequest<StockSummaryReportRequest>
    .WithResponse<QueryContracts.reports.StockSummaryReport.Answer> {
    private readonly IMediator _mediator;

    public StockSummaryReport(IMediator mediator) {
        _mediator = mediator;
    }

    [HttpGet, Route("reports/stock-summary")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.reports.StockSummaryReport.Answer>> HandleAsync(
        StockSummaryReportRequest request
    ) {
        var query = new QueryContracts.reports.StockSummaryReport.Query(request.FromDate, request.ToDate);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class StockSummaryReportRequest {
    [Required] public string FromDate { get; set; } = null!;
    [Required] public string ToDate { get; set; } = null!;
}