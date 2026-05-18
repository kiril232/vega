using System.ComponentModel.DataAnnotations;

namespace Vega.CartService.Contracts;

public record CartResponse(Guid Id, Guid UserId, IReadOnlyList<CartItemResponse> Items, decimal Total);

public record CartItemResponse(
    Guid Id,
    Guid ProductId,
    string ProductName,
    string ImageUrl,
    decimal Price,
    int Quantity,
    decimal LineTotal);

public record AddItemRequest(
    [Required] Guid ProductId,
    [Range(1, 999)] int Quantity);

public record UpdateItemRequest([Range(0, 999)] int Quantity);
