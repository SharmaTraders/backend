using System.Collections.Immutable;
using Domain.Entity;
using Domain.Repository;
using Domain.utils;
using Dto;

namespace Domain.item;

public class ItemDomain : IItemDomain {
    private readonly IItemRepository _itemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ItemDomain(IItemRepository itemRepository, IUnitOfWork unitOfWork) {
        _itemRepository = itemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateItem(CreateItemRequest createItemRequest) {
        ItemEntity entity = new ItemEntity() {
            Name = createItemRequest.Name
        };

        ItemEntity? itemFromDb = await _itemRepository.GetByNameAsync(entity.Name);
        if (itemFromDb is not null) {
            throw new DomainValidationException("ItemName", ErrorCode.Conflict,
                ErrorMessages.ItemNameAlreadyExists(createItemRequest.Name));
        }

        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = createItemRequest.Name
        };

        await _itemRepository.AddAsync(itemEntity);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ICollection<ItemDto>> GetAllItems() {
        List<ItemEntity> entities = await _itemRepository.GetAllAsync();
        return entities.Select(entity =>
            new ItemDto(entity.Id.ToString(), entity.Name)
        ).ToImmutableList();
    }

}