using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Vega.ProductService.Contracts;
using Vega.ProductService.Services;

namespace Vega.ProductService.Controllers;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;

    public ProductsController(IProductService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] string? category, CancellationToken ct)
        => Ok(await _svc.ListAsync(category, ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct)
    {
        var p = await _svc.GetAsync(id, ct);
        return p is null ? NotFound() : Ok(p);
    }

    // admin-ish endpoints; auth required so random users can't mutate the catalog
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest req, CancellationToken ct)
    {
        var p = await _svc.CreateAsync(req, ct);
        return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest req, CancellationToken ct)
    {
        var p = await _svc.UpdateAsync(id, req, ct);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => await _svc.DeleteAsync(id, ct) ? NoContent() : NotFound();
}
