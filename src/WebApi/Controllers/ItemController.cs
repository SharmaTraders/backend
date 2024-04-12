using Domain.dao;
using Domain.item;
using Dto;
using Dto.tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class ItemController : ControllerBase
{
    private readonly IItemDomain _itemDomain;

    public ItemController(IItemDomain itemDomain)
    {
        _itemDomain = itemDomain;
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItem(ItemDto itemDto)
    {
        ItemDto createdItem = await _itemDomain.CreateItem(itemDto);
        return Ok(createdItem);
    }

    [HttpGet]
    public async Task<ActionResult<GetItemsResponse>> GetItems()
    {
        ICollection<ItemDto> items = await _itemDomain.GetItems();
        GetItemsResponse response = new(items);
        return Ok(response);
    }
}