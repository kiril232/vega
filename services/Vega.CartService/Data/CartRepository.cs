using Microsoft.EntityFrameworkCore;
using Vega.CartService.Domain;

namespace Vega.CartService.Data;

public class CartRepository : ICartRepository
{
    private readonly CartDbContext _db;

    public CartRepository(CartDbContext db) => _db = db;

    // read-only — EF with AsNoTracking is fine here
    public Task<Cart?> GetByUserAsync(Guid userId, CancellationToken ct = default)
        => _db.Carts
              .Include(c => c.Items)
              .AsNoTracking()
              .FirstOrDefaultAsync(c => c.UserId == userId, ct);

    public async Task EnsureCartAsync(Guid userId, CancellationToken ct = default)
    {
        var newId = Guid.NewGuid();
        var now   = DateTime.UtcNow;

        await _db.Database.ExecuteSqlAsync(
            $"""
            INSERT INTO carts.carts ("Id", "UserId", "CreatedAt", "UpdatedAt")
            VALUES ({newId}, {userId}, {now}, {now})
            ON CONFLICT ("UserId") DO NOTHING
            """,
            ct);
    }

    public async Task UpsertItemAsync(
        Guid userId, Guid productId, string productName,
        string imageUrl, decimal price, int quantity,
        CancellationToken ct = default)
    {
        await EnsureCartAsync(userId, ct);

        var itemId = Guid.NewGuid();
        var now    = DateTime.UtcNow;

        // insert the item (keyed on CartId+ProductId); if it already exists,
        // add the incoming quantity on top instead of replacing it
        await _db.Database.ExecuteSqlAsync(
            $"""
            INSERT INTO carts.cart_items
                ("Id", "CartId", "ProductId", "ProductName", "ImageUrl", "PriceSnapshot", "Quantity")
            SELECT {itemId}, c."Id", {productId}, {productName}, {imageUrl}, {price}, {quantity}
            FROM   carts.carts c
            WHERE  c."UserId" = {userId}
            ON CONFLICT ("CartId", "ProductId") DO UPDATE
                SET "Quantity"      = carts.cart_items."Quantity" + EXCLUDED."Quantity",
                    "PriceSnapshot" = EXCLUDED."PriceSnapshot",
                    "ProductName"   = EXCLUDED."ProductName",
                    "ImageUrl"      = EXCLUDED."ImageUrl"
            """,
            ct);

        await _db.Database.ExecuteSqlAsync(
            $"""UPDATE carts.carts SET "UpdatedAt" = {now} WHERE "UserId" = {userId}""",
            ct);
    }

    public async Task SetQuantityAsync(
        Guid userId, Guid productId, int quantity,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        await _db.Database.ExecuteSqlAsync(
            $"""
            UPDATE carts.cart_items ci
            SET    "Quantity" = {quantity}
            FROM   carts.carts c
            WHERE  c."UserId"    = {userId}
              AND  ci."CartId"   = c."Id"
              AND  ci."ProductId" = {productId}
            """,
            ct);

        await _db.Database.ExecuteSqlAsync(
            $"""UPDATE carts.carts SET "UpdatedAt" = {now} WHERE "UserId" = {userId}""",
            ct);
    }

    public async Task RemoveItemAsync(
        Guid userId, Guid productId,
        CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        await _db.Database.ExecuteSqlAsync(
            $"""
            DELETE FROM carts.cart_items ci
            USING  carts.carts c
            WHERE  c."UserId"    = {userId}
              AND  ci."CartId"   = c."Id"
              AND  ci."ProductId" = {productId}
            """,
            ct);

        await _db.Database.ExecuteSqlAsync(
            $"""UPDATE carts.carts SET "UpdatedAt" = {now} WHERE "UserId" = {userId}""",
            ct);
    }

    public async Task ClearItemsAsync(Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        await _db.Database.ExecuteSqlAsync(
            $"""
            DELETE FROM carts.cart_items ci
            USING  carts.carts c
            WHERE  c."UserId"  = {userId}
              AND  ci."CartId" = c."Id"
            """,
            ct);

        await _db.Database.ExecuteSqlAsync(
            $"""UPDATE carts.carts SET "UpdatedAt" = {now} WHERE "UserId" = {userId}""",
            ct);
    }
}
