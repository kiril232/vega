using Vega.CartService.Contracts;
using Vega.CartService.Data;
using Vega.CartService.Domain;

namespace Vega.CartService.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repo;
    private readonly IProductClient _products;

    public CartService(ICartRepository repo, IProductClient products)
    {
        _repo = repo;
        _products = products;
    }

    public async Task<CartResponse> GetAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await _repo.GetOrCreateAsync(userId, ct);
        // save in case GetOrCreateAsync just built a new cart
        await _repo.SaveChangesAsync(ct);
        return ToDto(cart);
    }

    public async Task<CartResponse> AddItemAsync(Guid userId, AddItemRequest req, CancellationToken ct = default)
    {
        var product = await _products.GetAsync(req.ProductId, ct)
            ?? throw new InvalidOperationException("product not found");

        if (product.Stock < req.Quantity)
            throw new InvalidOperationException("insufficient stock");

        var cart = await _repo.GetOrCreateAsync(userId, ct);
        var existing = cart.Items.FirstOrDefault(i => i.ProductId == req.ProductId);
        if (existing is null)
        {
            cart.Items.Add(new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = cart.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ImageUrl = product.ImageUrl,
                PriceSnapshot = product.Price,
                Quantity = req.Quantity
            });
        }
        else
        {
            existing.Quantity += req.Quantity;
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _repo.SaveChangesAsync(ct);
        return ToDto(cart);
    }

    public async Task<CartResponse?> UpdateItemAsync(Guid userId, Guid productId, UpdateItemRequest req, CancellationToken ct = default)
    {
        var cart = await _repo.GetByUserAsync(userId, ct);
        if (cart is null) return null;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return null;

        // quantity 0 acts as a remove — friendlier for the frontend stepper
        if (req.Quantity == 0)
        {
            cart.Items.Remove(item);
        }
        else
        {
            item.Quantity = req.Quantity;
        }

        cart.UpdatedAt = DateTime.UtcNow;
        await _repo.SaveChangesAsync(ct);
        return ToDto(cart);
    }

    public async Task<CartResponse?> RemoveItemAsync(Guid userId, Guid productId, CancellationToken ct = default)
    {
        var cart = await _repo.GetByUserAsync(userId, ct);
        if (cart is null) return null;

        var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null) return ToDto(cart);

        cart.Items.Remove(item);
        cart.UpdatedAt = DateTime.UtcNow;
        await _repo.SaveChangesAsync(ct);
        return ToDto(cart);
    }

    public async Task<CartResponse> ClearAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await _repo.GetOrCreateAsync(userId, ct);
        cart.Items.Clear();
        cart.UpdatedAt = DateTime.UtcNow;
        await _repo.SaveChangesAsync(ct);
        return ToDto(cart);
    }

    private static CartResponse ToDto(Cart cart)
    {
        var items = cart.Items.Select(i =>
            new CartItemResponse(i.Id, i.ProductId, i.ProductName, i.ImageUrl,
                i.PriceSnapshot, i.Quantity, i.PriceSnapshot * i.Quantity)).ToList();

        return new CartResponse(cart.Id, cart.UserId, items, cart.Total);
    }
}
