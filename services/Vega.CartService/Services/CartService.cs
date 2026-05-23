using Vega.CartService.Contracts;
using Vega.CartService.Data;
using Vega.CartService.Domain;

namespace Vega.CartService.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repo;
    private readonly IProductClient  _products;

    public CartService(ICartRepository repo, IProductClient products)
    {
        _repo     = repo;
        _products = products;
    }

    public async Task<CartResponse> GetAsync(Guid userId, CancellationToken ct = default)
    {
        await _repo.EnsureCartAsync(userId, ct);
        var cart = (await _repo.GetByUserAsync(userId, ct))!;
        return ToDto(cart);
    }

    public async Task<CartResponse> AddItemAsync(
        Guid userId, AddItemRequest req, CancellationToken ct = default)
    {
        var product = await _products.GetAsync(req.ProductId, ct)
            ?? throw new InvalidOperationException("product not found");

        if (product.Stock < req.Quantity)
            throw new InvalidOperationException("insufficient stock");

        await _repo.UpsertItemAsync(
            userId, product.Id, product.Name,
            product.ImageUrl, product.Price, req.Quantity, ct);

        var cart = (await _repo.GetByUserAsync(userId, ct))!;
        return ToDto(cart);
    }

    public async Task<CartResponse?> UpdateItemAsync(
        Guid userId, Guid productId, UpdateItemRequest req,
        CancellationToken ct = default)
    {
        var cart = await _repo.GetByUserAsync(userId, ct);
        if (cart is null) return null;

        if (cart.Items.All(i => i.ProductId != productId)) return null;

        // quantity 0 acts as a remove — friendlier for the frontend stepper
        if (req.Quantity == 0)
            await _repo.RemoveItemAsync(userId, productId, ct);
        else
            await _repo.SetQuantityAsync(userId, productId, req.Quantity, ct);

        return ToDto((await _repo.GetByUserAsync(userId, ct))!);
    }

    public async Task<CartResponse?> RemoveItemAsync(
        Guid userId, Guid productId, CancellationToken ct = default)
    {
        var cart = await _repo.GetByUserAsync(userId, ct);
        if (cart is null) return null;

        await _repo.RemoveItemAsync(userId, productId, ct);
        return ToDto((await _repo.GetByUserAsync(userId, ct))!);
    }

    public async Task<CartResponse> ClearAsync(Guid userId, CancellationToken ct = default)
    {
        await _repo.EnsureCartAsync(userId, ct);
        await _repo.ClearItemsAsync(userId, ct);
        var cart = (await _repo.GetByUserAsync(userId, ct))!;
        return ToDto(cart);
    }

    private static CartResponse ToDto(Cart cart)
    {
        var items = cart.Items
            .Select(i => new CartItemResponse(
                i.Id, i.ProductId, i.ProductName, i.ImageUrl,
                i.PriceSnapshot, i.Quantity,
                i.PriceSnapshot * i.Quantity))
            .ToList();

        return new CartResponse(cart.Id, cart.UserId, items, cart.Total);
    }
}
