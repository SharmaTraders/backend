using System.Collections.Immutable;
using System.Runtime.InteropServices;
using Domain;
using Domain.Entity;
using Domain.Repository;
using Dto;

namespace DomainEntry.item;

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

    public async Task UpdateItem(string id, UpdateItemRequest updateItemRequest)
    {
        bool tryParse = Guid.TryParse(id, out Guid guid);
        if (!tryParse) {
            throw new DomainValidationException("Id", ErrorCode.BadRequest, ErrorMessages.IdInvalid);
        }
        
        ItemEntity? itemEntity = await _itemRepository.GetByIdAsync(guid);
        if (itemEntity is null) {
            throw new DomainValidationException("Id", ErrorCode.NotFound, ErrorMessages.ItemNotFound(guid));
        }
        
        await CheckForUniqueName(updateItemRequest.Name, guid);
        
        itemEntity.Name = updateItemRequest.Name;
        await _unitOfWork.SaveChangesAsync();
   }
    
    
    private async Task CheckForUniqueName(string name, [Optional] Guid idToExclude) {
        bool isUniqueName = await _itemRepository.IsUniqueNameAsync(name, idToExclude);
        if (!isUniqueName) {
            throw new DomainValidationException("ItemName", ErrorCode.Conflict, ErrorMessages.ItemNameAlreadyExists(name));
        }
    }
}