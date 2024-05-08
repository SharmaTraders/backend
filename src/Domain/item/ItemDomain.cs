using System.Collections.Immutable;
using Domain.Entity;
using Domain.Repositories;
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
        createItemRequest = new CreateItemRequest(createItemRequest.Name.Trim());
        ValidateItemName(createItemRequest.Name);

        ItemEntity? itemFromDb = await _itemRepository.GetByNameAsync(createItemRequest.Name);
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

    public async Task UpdateItem(string id, UpdateItemRequest updateItemRequest)
    {
        bool tryParse = Guid.TryParse(id, out Guid guid);
        if (!tryParse) {
            throw new DomainValidationException("Id", ErrorCode.BadRequest, ErrorMessages.IdInvalid);
        }
        
        ValidateItemName(updateItemRequest.Name);
        
        ItemEntity? itemEntity = await _itemRepository.GetByIdAsync(guid);
        if (itemEntity is null) {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.ItemNotFound(guid));
        }
        
        // Check if the new name is already used by another item
        ItemEntity? existingItemWithSameName = await _itemRepository.GetByNameAsync(updateItemRequest.Name);
        if (existingItemWithSameName != null && existingItemWithSameName.Id != guid) {
            throw new DomainValidationException("ItemName", ErrorCode.Conflict, ErrorMessages.ItemNameAlreadyExists(updateItemRequest.Name));
        }
        
        itemEntity.Name = updateItemRequest.Name;
        await _unitOfWork.SaveChangesAsync();
   }
    

    private static void ValidateItemName(string itemName) {
        if (string.IsNullOrEmpty(itemName)) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        if (itemName.Length is < 3 or > 20) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }
}