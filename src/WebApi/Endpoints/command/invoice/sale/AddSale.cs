using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.invoice.sale;

public class AddSale : CommandEndPointBase
    .WithRequest<AddSaleRequest>
    .WithResponse<CommandContracts.invoice.sale.AddSale.Response> {
    
    private readonly IMediator _mediator;

    public AddSale(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost, Route("sale")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<CommandContracts.invoice.sale.AddSale.Response>> HandleAsync(AddSaleRequest request)
    {
        var commandRequest = new CommandContracts.invoice.sale.AddSale.Request(
            request.RequestBody.BillingPartyId,
            request.RequestBody.SaleLines.Select(x => new CommandContracts.invoice.sale.AddSale.SaleLines(
                x.ItemId,
                x.Quantity,
                x.UnitPrice,
                x.Report
            )).ToList(),
            request.RequestBody.Date,
            request.RequestBody.Remarks,
            request.RequestBody.VatAmount,
            request.RequestBody.TransportFee,
            request.RequestBody.ReceivedAmount ?? 0,
            request.RequestBody.InvoiceNumber
        );
         var response = await _mediator.Send(commandRequest);
         return Ok(response);
    }
}

public class AddSaleRequest
{
    [FromBody] public Body RequestBody { get; set; }= null!;
    public record Body( 
        string BillingPartyId,
        List<SaleLines> SaleLines,
        string Date,
        string? Remarks,
        double? VatAmount,
        double? TransportFee,
        double? ReceivedAmount,
        int? InvoiceNumber
        );
    
    public record SaleLines(
        string ItemId,
        [Required]
        double Quantity,
        [Required]
        double UnitPrice,
        double? Report
        );
}
