using System.Net.Http.Headers;
using System.Text.Json;

namespace Vega.OrderService.Services;

public class CartClient : ICartClient
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web);

    public CartClient(HttpClient http) => _http = http;

    public async Task<CartDto?> GetAsync(string bearerToken, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Get, "/cart");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        using var res = await _http.SendAsync(req, ct);
        if (!res.IsSuccessStatusCode) return null;

        await using var stream = await res.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        var root = doc.RootElement;

        var items = new List<CartItemDto>();
        foreach (var el in root.GetProperty("items").EnumerateArray())
        {
            items.Add(new CartItemDto(
                el.GetProperty("productId").GetGuid(),
                el.GetProperty("productName").GetString() ?? string.Empty,
                el.GetProperty("price").GetDecimal(),
                el.GetProperty("quantity").GetInt32()));
        }

        return new CartDto(
            root.GetProperty("id").GetGuid(),
            root.GetProperty("userId").GetGuid(),
            items,
            root.GetProperty("total").GetDecimal());
    }

    public async Task ClearAsync(string bearerToken, CancellationToken ct = default)
    {
        using var req = new HttpRequestMessage(HttpMethod.Delete, "/cart/clear");
        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        using var res = await _http.SendAsync(req, ct);
        res.EnsureSuccessStatusCode();
    }
}
