using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vega.OrderService.Services;

namespace Vega.OrderService.Controllers;

[ApiController]
[Authorize]
[Route("orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _svc;

    public OrdersController(IOrderService svc) => _svc = svc;

    [HttpPost]
    public async Task<IActionResult> Place(CancellationToken ct)
    {
        var userId = GetUserId();
        if (userId is null) return Unauthorized();

        var token = ExtractBearer();
        if (token is null) return Unauthorized();

        try
        {
            var order = await _svc.PlaceFromCartAsync(userId.Value, token, ct);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var order = await _svc.GetAsync(id, ct);
        if (order is null) return NotFound();

        var userId = GetUserId();
        if (userId is null || order.UserId != userId) return Forbid();

        return Ok(order);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> ListByUser(Guid userId, CancellationToken ct)
    {
        var me = GetUserId();
        if (me is null || me != userId) return Forbid();
        return Ok(await _svc.ListByUserAsync(userId, ct));
    }

    private Guid? GetUserId()
    {
        var sub = User.FindFirstValue("sub") ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(sub, out var id) ? id : null;
    }

    private string? ExtractBearer()
    {
        var header = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;
        return header["Bearer ".Length..].Trim();
    }
}
