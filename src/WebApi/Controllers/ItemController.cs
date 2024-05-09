using DomainEntry.item;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class ItemController : ControllerBase {
    private readonly IItemDomain _itemDomain;

    public ItemController(IItemDomain itemDomain) {
        _itemDomain = itemDomain;
    }

    [HttpPost]
    public async Task<ActionResult> CreateItem(CreateItemRequest createItemRequest) {
        await _itemDomain.CreateItem(createItemRequest);
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<GetItemsResponse>> GetAllItems() {
        ICollection<ItemDto> items = await _itemDomain.GetAllItems();
        return Ok(new GetItemsResponse(items));
    }
    
    [HttpPut, Route("{id}")]
    public async Task<ActionResult> UpdateItem(UpdateItemRequest updateItemRequest, [FromRoute] string id) {
        await _itemDomain.UpdateItem(id, updateItemRequest);
        return Ok();
    }
}