using Vega.ProductService.Domain;

namespace Vega.ProductService.Data;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> ListAsync(string? category, CancellationToken ct = default);
    Task<Product?> FindAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Product product, CancellationToken ct = default);
    void Remove(Product product);
    Task SaveChangesAsync(CancellationToken ct = default);
}
