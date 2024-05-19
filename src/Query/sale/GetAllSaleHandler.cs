using Domain.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryContracts.sale;

namespace Query.sale
{
    public class GetAllSaleHandler : IRequestHandler<GetAllSales.Query, GetAllSales.Answer>
    {
        private readonly SharmaTradersContext _context;

        public GetAllSaleHandler(SharmaTradersContext context)
        {
            _context = context;
        }

        public async Task<GetAllSales.Answer> Handle(GetAllSales.Query request, CancellationToken cancellationToken)
        {
            var queryable = _context.Sales
                .Include(sale => sale.SaleLineItems)
                .Include(sale => sale.BillingParty)
                .OrderByDescending(sale => sale.Date)
                .AsQueryable();

            int totalCount = await queryable.CountAsync(cancellationToken);

            List<GetAllSales.SaleDto> saleDtos = await queryable
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(sale => new GetAllSales.SaleDto(
                    sale.Id.ToString(),
                    sale.BillingParty.Name,
                    sale.InvoiceNumber,
                    sale.Date.ToString(),
                    GetTotalAmount(sale.SaleLineItems, sale.TransportFee, sale.VatAmount),
                    GetRemainingAmount(sale.SaleLineItems, sale.TransportFee, sale.VatAmount, sale.ReceivedAmount)
                ))
                .ToListAsync(cancellationToken);

            return new GetAllSales.Answer(saleDtos, totalCount, request.PageNumber, request.PageSize);
        }

        private static double GetTotalAmount(ICollection<SaleLineItem> saleLineItems, double? transportFee, double? vatAmount)
        {
            double totalAmount = saleLineItems.Sum(item => ((item.Price * item.Quantity) - (item.Report ?? 0)));
            totalAmount += transportFee ?? 0;
            totalAmount += vatAmount ?? 0;
            return Math.Round(totalAmount, 2);
        }

        private static double GetRemainingAmount(ICollection<SaleLineItem> saleLineItems, double? transportFee, double? vatAmount, double? receivedAmount)
        {
            double totalAmount = GetTotalAmount(saleLineItems, transportFee, vatAmount);
            return totalAmount - (receivedAmount ?? 0);
        }
    }
}
