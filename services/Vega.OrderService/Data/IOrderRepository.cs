using Vega.OrderService.Domain;

namespace Vega.OrderService.Data;

public interface IOrderRepository
{
    Task<Order?> FindAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Order order, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
