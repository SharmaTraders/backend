﻿using CommandContracts.purchase;
using Domain.common;
using Domain.DomainServices;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.invoice.purchase;

public class AddPurchaseHandler : IRequestHandler<RegisterPurchase.Request, RegisterPurchase.Response> {
    
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

    public async Task<RegisterPurchase.Response> Handle(RegisterPurchase.Request request, CancellationToken cancellationToken) {
        var (date, billingParty) = await CheckForValidDataExistenceAsync(request);

        List<PurchaseLineItem> lineItems = new List<PurchaseLineItem>();
        foreach (RegisterPurchase.PurchaseLines line in request.PurchaseLines) {
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
        return new RegisterPurchase.Response(purchaseEntity.Id.ToString());

    }


    private async Task<ItemEntity> IfExists(string id) {
        Guid itemId = GuidParser.ParseGuid(id, "ItemId");
        ItemEntity? item = await _itemRepository.GetByIdAsync(itemId);
        if (item is null)
        {
            throw new DomainValidationException("ItemId", ErrorCode.NotFound,
                ErrorMessages.ItemNotFound(itemId));
        }

        return item;
    }

    private async Task<(DateOnly,BillingPartyEntity)> CheckForValidDataExistenceAsync(RegisterPurchase.Request request) {
        DateOnly date = DateParser.ParseDate(request.Date);

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