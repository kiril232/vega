using Vega.OrderService.Contracts;

namespace Vega.OrderService.Services;

public interface IOrderService
{
    Task<OrderResponse> PlaceFromCartAsync(Guid userId, string bearerToken, CancellationToken ct = default);
    Task<OrderResponse?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<OrderResponse>> ListByUserAsync(Guid userId, CancellationToken ct = default);
}
