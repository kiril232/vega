namespace Vega.OrderService.Contracts;

public record CreateOrderRequest(); // user is taken from the jwt; nothing else needed

public record OrderResponse(
    Guid Id,
    Guid UserId,
    decimal Total,
    string Currency,
    string Status,
    string? FailureReason,
    DateTime CreatedAt,
    IReadOnlyList<OrderItemResponse> Items);

public record OrderItemResponse(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
