using MediatR;

namespace CommandContracts.invoice.purchase;

public static class AddPurchase
{
    public record Request(
        string BillingPartyId,
        List<PurchaseLines> PurchaseLines,
        string Date,
        string? Remarks,
        double? VatAmount,
        double? TransportFee,
        double PaidAmount,
        int? InvoiceNumber
        ) : IRequest<Response>;

    public record PurchaseLines(
        string ItemId,
        double Quantity,
        double UnitPrice,
        double? Report);
    
    public record Response(string PurchaseId);
}