using Vega.ProductService.Contracts;

namespace Vega.ProductService.Services;

public interface IProductService
{
    Task<IReadOnlyList<ProductResponse>> ListAsync(string? category, CancellationToken ct = default);
    Task<ProductResponse?> GetAsync(Guid id, CancellationToken ct = default);
    Task<ProductResponse> CreateAsync(CreateProductRequest req, CancellationToken ct = default);
    Task<ProductResponse?> UpdateAsync(Guid id, UpdateProductRequest req, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
