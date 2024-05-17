using MediatR;

namespace CommandContracts.invoice.sale;

public static class AddSale
{
    public record Request(
        string BillingPartyId,
        List<SaleLines> SaleLines,
        string Date,
        string? Remarks,
        double? VatAmount,
        double? TransportFee,
        double? ReceivedAmount,
        int? InvoiceNumber) : IRequest<Response>;
    
    public record SaleLines(
        string ItemId,
        double Quantity,
        double UnitPrice,
        double? Report);
    
    public record Response(string SaleId);
}