using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vega.CartService.Auth;
using Vega.CartService.Contracts;
using Vega.CartService.Services;

namespace Vega.CartService.Controllers;

[ApiController]
[Authorize]
[Route("cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _svc;

    public CartController(ICartService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var userId = CurrentUser.GetId(User);
        if (userId is null) return Unauthorized();
        return Ok(await _svc.GetAsync(userId.Value, ct));
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddItemRequest req, CancellationToken ct)
    {
        var userId = CurrentUser.GetId(User);
        if (userId is null) return Unauthorized();

        try
        {
            return Ok(await _svc.AddItemAsync(userId.Value, req, ct));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("items/{productId:guid}")]
    public async Task<IActionResult> UpdateItem(Guid productId, [FromBody] UpdateItemRequest req, CancellationToken ct)
    {
        var userId = CurrentUser.GetId(User);
        if (userId is null) return Unauthorized();

        var cart = await _svc.UpdateItemAsync(userId.Value, productId, req, ct);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpDelete("items/{productId:guid}")]
    public async Task<IActionResult> RemoveItem(Guid productId, CancellationToken ct)
    {
        var userId = CurrentUser.GetId(User);
        if (userId is null) return Unauthorized();

        var cart = await _svc.RemoveItemAsync(userId.Value, productId, ct);
        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpDelete("clear")]
    public async Task<IActionResult> Clear(CancellationToken ct)
    {
        var userId = CurrentUser.GetId(User);
        if (userId is null) return Unauthorized();
        return Ok(await _svc.ClearAsync(userId.Value, ct));
    }
}
