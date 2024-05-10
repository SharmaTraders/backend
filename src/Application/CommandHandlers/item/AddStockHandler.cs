using CommandContracts.item;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.item;

public class AddStockHandler : IRequestHandler<AddStockCommand.Request> {
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;


    public AddStockHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork) {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(AddStockCommand.Request request, CancellationToken cancellationToken) {
        bool tryParse = Guid.TryParse(request.ItemId, out Guid itemId);
        if (!tryParse) {
            throw new DomainValidationException("ItemId", ErrorCode.BadRequest, ErrorMessages.IdInvalid(request.ItemId));
        }

        ItemEntity? entity = await _itemRepository.GetByIdAsync(itemId);
        if (entity is null) {
            throw new DomainValidationException("ItemId", ErrorCode.NotFound, ErrorMessages.ItemNotFound(itemId));
        }

        bool parsed = DateOnly.TryParseExact(request.Date, Constants.DateFormat, out DateOnly date);
        if (!parsed) {
            throw new DomainValidationException("Date", ErrorCode.BadRequest, ErrorMessages.DateFormatInvalid); 
        }

        Stock stock = new Stock() {
            Date = date,
            EntryCategory = StockEntryCategory.AddStockEntry,
            Weight = request.Weight,
            ExpectedValuePerKilo = request.ExpectedValuePerKilo,
        };
        entity.AddStock(stock);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}