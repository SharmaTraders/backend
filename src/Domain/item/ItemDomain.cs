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

    public async Task CreateItem(AddItemRequest addItemRequest) {
        addItemRequest = new AddItemRequest(addItemRequest.Name.Trim());
        ValidateItem(addItemRequest);

        ItemEntity? itemFromDb = await _itemRepository.GetByNameAsync(addItemRequest.Name);
        if (itemFromDb is not null) {
            throw new DomainValidationException("ItemName", ErrorCode.Conflict,
                ErrorMessages.ItemNameAlreadyExists(addItemRequest.Name));
        }

        ItemEntity itemEntity = new ItemEntity() {
            Id = Guid.NewGuid(),
            Name = addItemRequest.Name
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

    private static void ValidateItem(AddItemRequest addItemRequest) {
        if (string.IsNullOrEmpty(addItemRequest.Name)) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameIsRequired);
        }

        if (addItemRequest.Name.Length is < 3 or > 20) {
            throw new DomainValidationException("ItemName", ErrorCode.BadRequest, ErrorMessages.ItemNameLength);
        }
    }
}