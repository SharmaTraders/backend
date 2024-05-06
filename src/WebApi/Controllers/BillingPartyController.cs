using Domain.billingParty;
using Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class BillingPartyController : ControllerBase{
    private readonly IBillingPartyDomain _billingPartyDomain;

    public BillingPartyController(IBillingPartyDomain billingPartyDomain) {
        _billingPartyDomain = billingPartyDomain;
    }

    [HttpPost]
    public async Task<ActionResult> CreateBillingParty(CreateBillingPartyRequest request) {
        await _billingPartyDomain.CreateBillingParty(request);
        return Ok();
    }

    [HttpGet]

    public async Task<ActionResult<GetBillingPartiesResponse>> GetBillingParties() {
        ICollection<BillingPartyDto> billingParties = await _billingPartyDomain.GetBillingParties();
        GetBillingPartiesResponse response = new(billingParties);
        return Ok(response);
    }
}