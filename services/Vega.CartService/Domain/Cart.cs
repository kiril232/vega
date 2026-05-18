namespace Vega.CartService.Domain;

public class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<CartItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.PriceSnapshot * i.Quantity);
}

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal PriceSnapshot { get; set; }
    public int Quantity { get; set; }
}
