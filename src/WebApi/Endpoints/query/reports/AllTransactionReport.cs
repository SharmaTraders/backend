using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.reports;

namespace WebApi.Endpoints.query.reports;

public class AllTransactionReport : QueryEndpointBase
    .WithRequest<AllTransactionReportRequest>
    .WithResponse<AllTransactionsReport.Answer> {

    private readonly IMediator _mediator;

    public AllTransactionReport(IMediator mediator) {
        _mediator = mediator;
    }


    [HttpGet, Route("reports/all-transactions")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<AllTransactionsReport.Answer>> HandleAsync(AllTransactionReportRequest request) {
        var  query = new AllTransactionsReport.Query(request.DateFrom, request.DateTo);
        var answer = await _mediator.Send(query);
        return Ok(answer);

    }
}

public class AllTransactionReportRequest {
    [FromQuery] [Required] public string DateFrom { get; set; } = null!;
    [FromQuery] [Required] public string DateTo { get; set; } = null!;

}