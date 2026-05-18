using Microsoft.EntityFrameworkCore;
using Vega.ProductService.Domain;

namespace Vega.ProductService.Data;

public class ProductRepository : IProductRepository
{
    private readonly ProductDbContext _db;

    public ProductRepository(ProductDbContext db) => _db = db;

    public async Task<IReadOnlyList<Product>> ListAsync(string? category, CancellationToken ct = default)
    {
        var q = _db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(category))
            q = q.Where(p => p.Category == category);
        return await q.OrderBy(p => p.Name).ToListAsync(ct);
    }

    public Task<Product?> FindAsync(Guid id, CancellationToken ct = default)
        => _db.Products.FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task AddAsync(Product product, CancellationToken ct = default)
        => await _db.Products.AddAsync(product, ct);

    public void Remove(Product product) => _db.Products.Remove(product);

    public Task SaveChangesAsync(CancellationToken ct = default)
        => _db.SaveChangesAsync(ct);
}
