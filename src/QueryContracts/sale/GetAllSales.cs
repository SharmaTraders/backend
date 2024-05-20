using MediatR;

namespace QueryContracts.sale;

public static class GetAllSales
{
    public record Query(int PageNumber, int PageSize) : IRequest<Answer>;
    
    public record Answer(ICollection<SaleDto> Sales , int TotalCount, int PageNumber, int PageSize);

    public record SaleDto(string Id, string PartyName , int? InvoiceNo , string Date, double TotalAmount, double? RemainingBalance, string? Remarks);
}