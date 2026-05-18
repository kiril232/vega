using Vega.CartService.Domain;

namespace Vega.CartService.Data;

public interface ICartRepository
{
    Task<Cart?> GetByUserAsync(Guid userId, CancellationToken ct = default);
    Task<Cart> GetOrCreateAsync(Guid userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
