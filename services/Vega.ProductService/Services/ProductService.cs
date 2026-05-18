using Vega.ProductService.Contracts;
using Vega.ProductService.Data;
using Vega.ProductService.Domain;

namespace Vega.ProductService.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<IReadOnlyList<ProductResponse>> ListAsync(string? category, CancellationToken ct = default)
    {
        var items = await _repo.ListAsync(category, ct);
        return items.Select(ToDto).ToList();
    }

    public async Task<ProductResponse?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.FindAsync(id, ct);
        return p is null ? null : ToDto(p);
    }

    public async Task<ProductResponse> CreateAsync(CreateProductRequest req, CancellationToken ct = default)
    {
        var p = new Product
        {
            Id = Guid.NewGuid(),
            Name = req.Name,
            Description = req.Description,
            Category = req.Category,
            Price = req.Price,
            Stock = req.Stock,
            ImageUrl = req.ImageUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(p, ct);
        await _repo.SaveChangesAsync(ct);
        return ToDto(p);
    }

    public async Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest req, CancellationToken ct = default)
    {
        var p = await _repo.FindAsync(id, ct);
        if (p is null) return null;

        p.Name = req.Name;
        p.Description = req.Description;
        p.Category = req.Category;
        p.Price = req.Price;
        p.Stock = req.Stock;
        p.ImageUrl = req.ImageUrl;

        await _repo.SaveChangesAsync(ct);
        return ToDto(p);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.FindAsync(id, ct);
        if (p is null) return false;
        _repo.Remove(p);
        await _repo.SaveChangesAsync(ct);
        return true;
    }

    private static ProductResponse ToDto(Product p) =>
        new(p.Id, p.Name, p.Description, p.Category, p.Price, p.Stock, p.ImageUrl);
}
