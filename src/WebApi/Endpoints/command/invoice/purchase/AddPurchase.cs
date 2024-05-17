using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints.command.invoice.purchase;

public class AddPurchase : CommandEndPointBase
    .WithRequest<AddPurchaseRequest>
    .WithResponse<CommandContracts.invoice.purchase.AddPurchase.Response> {
    
    private readonly IMediator _mediator;

    public AddPurchase(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost, Route("purchase")]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<CommandContracts.invoice.purchase.AddPurchase.Response>> HandleAsync(AddPurchaseRequest request)
    {
        var commandRequest = new CommandContracts.invoice.purchase.AddPurchase.Request(
            request.RequestBody.BillingPartyId,
            request.RequestBody.PurchaseLines.Select(x => new CommandContracts.invoice.purchase.AddPurchase.PurchaseLines(
                x.ItemId,
                x.Quantity,
                x.UnitPrice,
                x.Report
            )).ToList(),
            request.RequestBody.Date,
            request.RequestBody.Remarks,
            request.RequestBody.VatAmount,
            request.RequestBody.TransportFee,
            request.RequestBody.PaidAmount ?? 0,
            request.RequestBody.InvoiceNumber
        );
         var response = await _mediator.Send(commandRequest);
         return Ok(response);
    }
}

public class AddPurchaseRequest
{
    [FromBody] public Body RequestBody { get; set; }= null!;
    public record Body( 
        string BillingPartyId,
        List<PurchaseLines> PurchaseLines,
        string Date,
        string? Remarks,
        double? VatAmount,
        double? TransportFee,
        double? PaidAmount,
        int? InvoiceNumber
        );
    
    public record PurchaseLines(
        string ItemId,
        [Required]
        double Quantity,

        [Required]
        double UnitPrice,

        double? Report);
}