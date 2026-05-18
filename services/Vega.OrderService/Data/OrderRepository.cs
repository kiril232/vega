using Microsoft.EntityFrameworkCore;
using Vega.OrderService.Domain;

namespace Vega.OrderService.Data;

public class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _db;

    public OrderRepository(OrderDbContext db) => _db = db;

    public Task<Order?> FindAsync(Guid id, CancellationToken ct = default)
        => _db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id, ct);

    public async Task<IReadOnlyList<Order>> ListByUserAsync(Guid userId, CancellationToken ct = default)
        => await _db.Orders.Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync(ct);

    public async Task AddAsync(Order order, CancellationToken ct = default)
        => await _db.Orders.AddAsync(order, ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
