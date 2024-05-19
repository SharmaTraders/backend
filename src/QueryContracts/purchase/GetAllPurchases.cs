using MediatR;

namespace QueryContracts.purchase;

public static class GetAllPurchases
{
    public record Query(int PageNumber, int PageSize) : IRequest<Answer>;
    
    public record Answer(ICollection<PurchaseDto> Purchases, int TotalCount, int PageNumber, int PageSize);
    
    public record PurchaseDto(string Id, string PartyName , int? InvoiceNo , string Date, double TotalAmount, double? DueAmount);
}
