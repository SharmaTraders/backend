using CommandContracts.item;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.item;

public class ReduceStockHandler : IRequestHandler<ReduceStockCommand.Request> {
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ReduceStockHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork) {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(ReduceStockCommand.Request request, CancellationToken cancellationToken) {
        Guid itemId = GuidParser.ParseGuid(request.ItemId, "ItemId");

        ItemEntity? entity = await _itemRepository.GetByIdAsync(itemId);
        if (entity is null) {
            throw new DomainValidationException("ItemId", ErrorCode.NotFound, ErrorMessages.ItemNotFound(itemId));
        }
        DateOnly date = DateParser.ParseDate(request.Date);

        Stock stock = new Stock() {
            Date = date,
            EntryCategory = StockEntryCategory.ReduceStockEntry,
            Weight = request.Weight,
            Remarks = request.Remarks
        };
        entity.ReduceStock(stock);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}