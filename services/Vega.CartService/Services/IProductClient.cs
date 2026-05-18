namespace Vega.CartService.Services;

public record ProductSnapshot(Guid Id, string Name, string ImageUrl, decimal Price, int Stock);

public interface IProductClient
{
    Task<ProductSnapshot?> GetAsync(Guid productId, CancellationToken ct = default);
}
