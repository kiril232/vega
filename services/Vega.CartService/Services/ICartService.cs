using Vega.CartService.Contracts;

namespace Vega.CartService.Services;

public interface ICartService
{
    Task<CartResponse> GetAsync(Guid userId, CancellationToken ct = default);
    Task<CartResponse> AddItemAsync(Guid userId, AddItemRequest req, CancellationToken ct = default);
    Task<CartResponse?> UpdateItemAsync(Guid userId, Guid productId, UpdateItemRequest req, CancellationToken ct = default);
    Task<CartResponse?> RemoveItemAsync(Guid userId, Guid productId, CancellationToken ct = default);
    Task<CartResponse> ClearAsync(Guid userId, CancellationToken ct = default);
}
