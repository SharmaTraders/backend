using Application.services.item;
using CommandContracts.item;
using Domain.common;
using Domain.Entity;
using Domain.Repository;
using MediatR;
using Tools;

namespace Application.CommandHandlers.item;

public class UpdateItemHandler : IRequestHandler<UpdateItemCommand.Request> {
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueItemNameChecker _uniqueItemNameChecker;

    public UpdateItemHandler(IItemRepository itemRepository, IUnitOfWork unitOfWork,
        IUniqueItemNameChecker uniqueItemNameChecker) {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
        _uniqueItemNameChecker = uniqueItemNameChecker;
    }

    public async Task Handle(UpdateItemCommand.Request request, CancellationToken cancellationToken) {
        Guid id = GuidParser.ParseGuid(request.Id, "Id");

        ItemEntity? itemEntity = await _itemRepository.GetByIdAsync(id);
        if (itemEntity is null) {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.ItemNotFound(id));
        }

        bool isUnique = await _uniqueItemNameChecker.IsUniqueAsync(request.Name, id);
        if (!isUnique) {
            throw new DomainValidationException("ItemName", ErrorCode.Conflict,
                ErrorMessages.ItemNameAlreadyExists(request.Name));
        }

        itemEntity.Name = request.Name;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}