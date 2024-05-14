using CommandContracts.purchase;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.purchase;

public class AddPurchaseHandler : IRequestHandler<AddPurchase.Request, AddPurchase.Response>
{
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPurchaseHandler(IPurchaseRepository purchaseRepository, IUnitOfWork unitOfWork,
        IBillingPartyRepository billingPartyRepository, IItemRepository itemRepository)
    {
        _purchaseRepository = purchaseRepository;
        _unitOfWork = unitOfWork;
        _billingPartyRepository = billingPartyRepository;
        _itemRepository = itemRepository;
    }

    public async Task<AddPurchase.Response> Handle(AddPurchase.Request request, CancellationToken cancellationToken)
    {
        bool parsed = DateOnly.TryParseExact(request.Date, Constants.DateFormat, out DateOnly date);
        if (!parsed)
        {
            throw new DomainValidationException("Date", ErrorCode.BadRequest, ErrorMessages.DateFormatInvalid);
        }

        bool tryParse = Guid.TryParse(request.BillingPartyId, out Guid billingPartyId);
        if (!tryParse)
        {
            throw new DomainValidationException("BillingPartyId", ErrorCode.BadRequest,
                ErrorMessages.IdInvalid(request.BillingPartyId));
        }

        BillingPartyEntity? billingParty = await _billingPartyRepository.GetByIdAsync(billingPartyId);
        if (billingParty is null)
        {
            throw new DomainValidationException("BillingPartyId", ErrorCode.NotFound,
                ErrorMessages.BillingPartyNotFound(billingPartyId));
        }

        List<PurchaseLineItem> lineItems = new();
        double totalAmount = 0.00;
        foreach (var purchase in request.PurchaseLines)
        {
            bool tryParseItemId = Guid.TryParse(purchase.ItemId, out Guid itemId);
            if (!tryParseItemId)
            {
                throw new DomainValidationException("ItemId", ErrorCode.BadRequest,
                    ErrorMessages.IdInvalid(purchase.ItemId));
            }

            ItemEntity? item = await _itemRepository.GetByIdAsync(itemId);
            if (item is null)
            {
                throw new DomainValidationException("ItemId", ErrorCode.NotFound,
                    ErrorMessages.ItemNotFound(itemId));
            }

            PurchaseLineItem lineItem = new PurchaseLineItem()
            {
                ItemEntity = item,
                Price = purchase.UnitPrice,
                Quantity = purchase.Quantity,
                Report = purchase.Report
            };
            totalAmount += (lineItem.Quantity * lineItem.Price);
            totalAmount -= lineItem.Report ?? 0;
            lineItems.Add(lineItem);
            
            Stock stock = new Stock()
            {
                Date = date,
                EntryCategory = StockEntryCategory.Purchase,
                Weight = purchase.Quantity,
                ExpectedValuePerKilo = purchase.UnitPrice,
                Remarks = "Added from purchase entry."
            };
            item.AddStock(stock);
        }

        PurchaseEntity purchaseEntity = new PurchaseEntity()
        {
            Id = Guid.NewGuid(),
            BillingParty = billingParty,
            Date = date,
            InvoiceNumber = request.InvoiceNumber,
            PaidAmount = request.PaidAmount,
            Remarks = request.Remarks,
            VatAmount = request.VatAmount,
            TransportFee = request.TransportFee,
            Purchases = lineItems
        };
        totalAmount += purchaseEntity.TransportFee ?? 0;
        totalAmount += purchaseEntity.VatAmount ?? 0;
        double amount = 0.0;
        if (purchaseEntity.PaidAmount.HasValue)
        {
            amount =purchaseEntity.PaidAmount.Value - totalAmount;
        }else
        {
            amount = -1* totalAmount;
        }
        
        billingParty.UpdateBalance(amount);
        await _purchaseRepository.AddAsync(purchaseEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddPurchase.Response(purchaseEntity.Id.ToString());
    }
}