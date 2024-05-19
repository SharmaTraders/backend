using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.query.sale;

public class GetAllSales : QueryEndpointBase.WithRequest<GetAllSalesRequest>.WithResponse< QueryContracts.sale.GetAllSales.Answer>
{
    private readonly IMediator _mediator;
    
    public GetAllSales(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet, Route("sale")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<QueryContracts.sale.GetAllSales.Answer>> HandleAsync(GetAllSalesRequest request)
    {
        var query = new QueryContracts.sale.GetAllSales.Query(request.PageNumber, request.PageSize);
        var answer = await _mediator.Send(query);
        return Ok(answer);
    }
}

public class GetAllSalesRequest
{
    [FromQuery] public int PageNumber { get; set; } = 1;

    [FromQuery] public int PageSize { get; set; } = 10;
}