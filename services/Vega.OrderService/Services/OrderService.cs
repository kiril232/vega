using Vega.OrderService.Contracts;
using Vega.OrderService.Data;
using Vega.OrderService.Domain;

namespace Vega.OrderService.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly ICartClient _cart;
    private readonly IPaymentClient _payments;
    private readonly ILogger<OrderService> _log;

    public OrderService(
        IOrderRepository repo,
        ICartClient cart,
        IPaymentClient payments,
        ILogger<OrderService> log)
    {
        _repo = repo;
        _cart = cart;
        _payments = payments;
        _log = log;
    }

    public async Task<OrderResponse> PlaceFromCartAsync(Guid userId, string bearerToken, CancellationToken ct = default)
    {
        var cart = await _cart.GetAsync(bearerToken, ct)
            ?? throw new InvalidOperationException("cart unavailable");

        if (cart.Items.Count == 0)
            throw new InvalidOperationException("cart is empty");

        // recompute total server-side, never trust the client total
        var total = cart.Items.Sum(i => i.Price * i.Quantity);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Total = total,
            Currency = "EUR",
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Items = cart.Items.Select(i => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.Price,
                Quantity = i.Quantity
            }).ToList()
        };

        await _repo.AddAsync(order, ct);
        await _repo.SaveChangesAsync(ct);

        var payment = await _payments.ProcessAsync(order.Id, userId, total, "EUR", ct);
        order.PaymentTransactionId = payment.TransactionId == Guid.Empty ? null : payment.TransactionId;
        order.Status = payment.Status == "Paid" ? OrderStatus.Paid : OrderStatus.Failed;
        order.FailureReason = payment.Status == "Paid" ? null : payment.Reason;
        order.UpdatedAt = DateTime.UtcNow;
        await _repo.SaveChangesAsync(ct);

        if (order.Status == OrderStatus.Paid)
        {
            // best effort — if the cart can't be cleared we still keep the paid order
            try { await _cart.ClearAsync(bearerToken, ct); }
            catch (Exception ex) { _log.LogWarning(ex, "failed to clear cart for {UserId}", userId); }
        }

        return ToDto(order);
    }

    public async Task<OrderResponse?> GetAsync(Guid id, CancellationToken ct = default)
    {
        var order = await _repo.FindAsync(id, ct);
        return order is null ? null : ToDto(order);
    }

    public async Task<IReadOnlyList<OrderResponse>> ListByUserAsync(Guid userId, CancellationToken ct = default)
    {
        var orders = await _repo.ListByUserAsync(userId, ct);
        return orders.Select(ToDto).ToList();
    }

    private static OrderResponse ToDto(Order o) => new(
        o.Id,
        o.UserId,
        o.Total,
        o.Currency,
        o.Status.ToString(),
        o.FailureReason,
        o.CreatedAt,
        o.Items.Select(i => new OrderItemResponse(i.ProductId, i.ProductName, i.UnitPrice, i.Quantity, i.LineTotal)).ToList());
}
