using Vega.CartService.Domain;

namespace Vega.CartService.Data;

public interface ICartRepository
{
    Task<Cart?> GetByUserAsync(Guid userId, CancellationToken ct = default);

    // ensures the cart row exists; idempotent
    Task EnsureCartAsync(Guid userId, CancellationToken ct = default);

    // inserts or increments quantity for the product
    Task UpsertItemAsync(
        Guid userId, Guid productId, string productName,
        string imageUrl, decimal price, int quantity,
        CancellationToken ct = default);

    Task SetQuantityAsync(Guid userId, Guid productId, int quantity, CancellationToken ct = default);
    Task RemoveItemAsync(Guid userId, Guid productId, CancellationToken ct = default);
    Task ClearItemsAsync(Guid userId, CancellationToken ct = default);
}
