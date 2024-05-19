using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.purchase;

namespace Query.QueryHandlers.purchase;

public class GetAllPurchaseHandler : IRequestHandler<GetAllPurchases.Query, GetAllPurchases.Answer>
{
    private readonly SharmaTradersContext _context;

    public GetAllPurchaseHandler(SharmaTradersContext context)
    {
        _context = context;
    }

    public async Task<GetAllPurchases.Answer> Handle(GetAllPurchases.Query request, CancellationToken cancellationToken)
    {
        var queryable = _context.Purchases
            .Include(purchase => purchase.PurchaseLineItems)
            .Include(purchase => purchase.BillingParty)
            .OrderByDescending(purchase => purchase.Date)
            .AsQueryable();

        int totalCount = await queryable.CountAsync(cancellationToken);

        List<GetAllPurchases.PurchaseDto> purchaseDtos = await queryable
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(purchase => new GetAllPurchases.PurchaseDto(
                purchase.Id.ToString(),
                purchase.BillingParty.Name,
                purchase.InvoiceNumber,
                purchase.Date.ToString(),
                GetTotalAmount(purchase.PurchaseLineItems, purchase.TransportFee, purchase.VatAmount),
                GetRemainingAmount(purchase.PurchaseLineItems, purchase.TransportFee, purchase.VatAmount, purchase.PaidAmount)
            ))
            .ToListAsync(cancellationToken);

        return new GetAllPurchases.Answer(purchaseDtos, totalCount, request.PageNumber, request.PageSize);
    }

    private static double GetTotalAmount(ICollection<PurchaseLineItem> purchaseLineItems, double? transportFee, double? vatAmount)
    {
        double totalAmount = purchaseLineItems.Sum(item => ((item.Price * item.Quantity) - (item.Report ?? 0)));
        totalAmount += transportFee ?? 0;
        totalAmount += vatAmount ?? 0;
        return Math.Round(totalAmount, 2);
    }

    private static double GetRemainingAmount(ICollection<PurchaseLineItem> purchaseLineItems, double? transportFee, double? vatAmount, double? paidAmount)
    {
        double totalAmount = GetTotalAmount(purchaseLineItems, transportFee, vatAmount);
        return totalAmount - (paidAmount ?? 0);
    }
}
