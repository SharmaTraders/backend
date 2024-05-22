using MediatR;

namespace QueryContracts.billingParty;                           

public static class GetAllBillingPartyTransaction {

    public record Query(string BillingPartyId, int PageNumber, int PageSize) : IRequest<Answer>;

    public record Answer(ICollection<TransactionDto> Transactions, int TotalCount, int PageNumber, int PageSize);
    
    public record TransactionDto(string Date, string? Remarks, string Category, double Amount, string Type);

}