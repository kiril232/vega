namespace Vega.OrderService.Services;

public record CartItemDto(Guid ProductId, string ProductName, decimal Price, int Quantity);
public record CartDto(Guid Id, Guid UserId, IReadOnlyList<CartItemDto> Items, decimal Total);

public interface ICartClient
{
    Task<CartDto?> GetAsync(string bearerToken, CancellationToken ct = default);
    Task ClearAsync(string bearerToken, CancellationToken ct = default);
}
