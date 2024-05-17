using CommandContracts.sale;
using Domain.common;
using Domain.DomainServices;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.invoice.sale;

public class AddSaleHandler: IRequestHandler<AddSale.Request, AddSale.Response>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IBillingPartyRepository _billingPartyRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public AddSaleHandler(ISaleRepository saleRepository, IBillingPartyRepository billingPartyRepository, IItemRepository itemRepository, IUnitOfWork unitOfWork)
    {
        _saleRepository = saleRepository;
        _billingPartyRepository = billingPartyRepository;
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<AddSale.Response> Handle(AddSale.Request request, CancellationToken cancellationToken)
    {
        var (date, billingParty) = await CheckForValidDataExistenceAsync(request);
        
        List<SaleLineItem> lineItems = new List<SaleLineItem>();
        foreach (AddSale.SaleLines line in request.SaleLines)
        {
            ItemEntity item = await IfExists(line.ItemId);
            SaleLineItem lineItem = new SaleLineItem()
            {
                ItemEntity = item,
                Price = line.UnitPrice,
                Quantity = line.Quantity,
                Report = line.Report
            };
            lineItems.Add(lineItem);
        }
        
        SaleEntity saleEntity = new SaleEntity()
        {
            Id = Guid.NewGuid(),
            BillingParty = billingParty,
            Date = date,
            InvoiceNumber = request.InvoiceNumber,
            ReceivedAmount = request.ReceivedAmount,
            Remarks = request.Remarks,
            VatAmount = request.VatAmount,
            TransportFee = request.TransportFee,
            Sales = lineItems
        };
        
        AddSaleService.AddSale(saleEntity);
        await _saleRepository.AddAsync(saleEntity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new AddSale.Response(saleEntity.Id.ToString());
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

    private async Task<(DateOnly,BillingPartyEntity)> CheckForValidDataExistenceAsync(AddSale.Request request) {
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