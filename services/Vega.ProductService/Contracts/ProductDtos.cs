using System.ComponentModel.DataAnnotations;

namespace Vega.ProductService.Contracts;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    string Category,
    decimal Price,
    int Stock,
    string ImageUrl);

public record CreateProductRequest(
    [Required, MaxLength(256)] string Name,
    [MaxLength(4000)] string Description,
    [Required, MaxLength(128)] string Category,
    [Range(0, 1_000_000)] decimal Price,
    [Range(0, 100_000)] int Stock,
    [MaxLength(1024)] string ImageUrl);

public record UpdateProductRequest(
    [Required, MaxLength(256)] string Name,
    [MaxLength(4000)] string Description,
    [Required, MaxLength(128)] string Category,
    [Range(0, 1_000_000)] decimal Price,
    [Range(0, 100_000)] int Stock,
    [MaxLength(1024)] string ImageUrl);
