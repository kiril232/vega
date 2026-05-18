namespace Vega.OrderService.Domain;

public enum OrderStatus
{
    Created = 0,
    Paid = 1,
    Failed = 2
}

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Total { get; set; }
    public string Currency { get; set; } = "EUR";
    public OrderStatus Status { get; set; }
    public string? FailureReason { get; set; }
    public Guid? PaymentTransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal LineTotal => UnitPrice * Quantity;
}
