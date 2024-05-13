using Application.services.item;
using CommandContracts.item;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.item;

public class CreateItemHandler : IRequestHandler<CreateItemCommand.Request, CreateItemCommand.Response> {
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueItemNameChecker _uniqueItemNameChecker;


    public CreateItemHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork, IUniqueItemNameChecker uniqueItemNameChecker) {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
        _uniqueItemNameChecker = uniqueItemNameChecker;
    }

    public async Task<CreateItemCommand.Response> Handle(CreateItemCommand.Request request, CancellationToken cancellationToken) {
        ItemEntity entity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CurrentStockAmount = request.StockWeight ?? 0,
            CurrentEstimatedStockValuePerKilo = request.EstimatedValuePerKilo ?? 0
        };

        bool isUnique = await _uniqueItemNameChecker.IsUniqueAsync(entity.Name);
        if (!isUnique) {
            throw new DomainValidationException("ItemName", ErrorCode.Conflict,
                ErrorMessages.ItemNameAlreadyExists(entity.Name));
        }

        await _itemRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateItemCommand.Response(entity.Id.ToString());
    }
}