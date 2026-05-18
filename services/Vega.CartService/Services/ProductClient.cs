using System.Net;
using System.Text.Json;

namespace Vega.CartService.Services;

public class ProductClient : IProductClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public ProductClient(HttpClient http) => _http = http;

    public async Task<ProductSnapshot?> GetAsync(Guid productId, CancellationToken ct = default)
    {
        var res = await _http.GetAsync($"/products/{productId}", ct);
        if (res.StatusCode == HttpStatusCode.NotFound) return null;
        res.EnsureSuccessStatusCode();

        // product service shape: { id, name, description, category, price, stock, imageUrl }
        await using var stream = await res.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        var root = doc.RootElement;
        return new ProductSnapshot(
            root.GetProperty("id").GetGuid(),
            root.GetProperty("name").GetString() ?? string.Empty,
            root.TryGetProperty("imageUrl", out var img) ? img.GetString() ?? string.Empty : string.Empty,
            root.GetProperty("price").GetDecimal(),
            root.GetProperty("stock").GetInt32());
    }
}
