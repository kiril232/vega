using Microsoft.EntityFrameworkCore;
using Vega.CartService.Domain;

namespace Vega.CartService.Data;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _db;

    public CartRepository(CartDbContext db) => _db = db;

    public Task<Cart?> GetByUserAsync(Guid userId, CancellationToken ct = default)
        => _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task<Cart> GetOrCreateAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await GetByUserAsync(userId, ct);
        if (cart is not null) return cart;

        // only add to the context — the caller does the single SaveChanges
        // so cart creation and any subsequent mutation go in one transaction
        cart = new Cart
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        _db.Carts.Add(cart);
        return cart;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => _db.SaveChangesAsync(ct);
}
