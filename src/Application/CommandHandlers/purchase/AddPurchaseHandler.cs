using CommandContracts.purchase;
using Domain.common;
using Domain.DomainServices;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.purchase;

public class AddPurchaseHandler : IRequestHandler<AddPurchase.Request, AddPurchase.Response> {
    
    private readonly IPurchaseRepository _purchaseRepository;
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddPurchaseHandler(IPurchaseRepository purchaseRepository, IBillingPartyRepository billingPartyRepository, IItemRepository itemRepository, IUnitOfWork unitOfWork) {
        _purchaseRepository = purchaseRepository;
        _billingPartyRepository = billingPartyRepository;
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AddPurchase.Response> Handle(AddPurchase.Request request, CancellationToken cancellationToken) {
        var (date, billingParty) = await CheckForValidDataExistence(request);

        List<PurchaseLineItem> lineItems = new List<PurchaseLineItem>();
        foreach (AddPurchase.PurchaseLines line in request.PurchaseLines) {
            ItemEntity item = await IfExists(line.ItemId);
            PurchaseLineItem lineItem = new PurchaseLineItem()
            {
                ItemEntity = item,
                Price = line.UnitPrice,
                Quantity = line.Quantity,
                Report = line.Report
            };
            lineItems.Add(lineItem);
        }

        PurchaseEntity purchaseEntity = new PurchaseEntity() {
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

        // All the domain logic are inside this domain service, this way we dont put any logic in the handler
        // That is the whole point of domain driven design, and then it will be tested in an unit test
        AddPurchaseService.AddPurchase(purchaseEntity);
        await _purchaseRepository.AddAsync(purchaseEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddPurchase.Response(purchaseEntity.Id.ToString());

    }


    private async Task<ItemEntity> IfExists(string id) {
        bool tryParseItemId = Guid.TryParse(id, out Guid itemId);
        if (!tryParseItemId)
        {
            throw new DomainValidationException("ItemId", ErrorCode.BadRequest,
                ErrorMessages.IdInvalid(id));
        }

        ItemEntity? item = await _itemRepository.GetByIdAsync(itemId);
        if (item is null)
        {
            throw new DomainValidationException("ItemId", ErrorCode.NotFound,
                ErrorMessages.ItemNotFound(itemId));
        }

        return item;
    }

    private async Task<(DateOnly,BillingPartyEntity)> CheckForValidDataExistence(AddPurchase.Request request) {
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
        return (date, billingParty);

    }
}