using System.Net.Http.Json;
using System.Text.Json;

namespace Vega.OrderService.Services;

public class PaymentClient : IPaymentClient
{
    private readonly HttpClient _http;

    public PaymentClient(HttpClient http) => _http = http;

    public async Task<PaymentResult> ProcessAsync(Guid orderId, Guid userId, decimal amount, string currency, CancellationToken ct = default)
    {
        var payload = new { orderId, userId, amount, currency };
        using var res = await _http.PostAsJsonAsync("/payments/process", payload, ct);

        // treat a downstream failure as a soft payment failure so the order
        // still gets persisted as Failed rather than crashing the request
        if (!res.IsSuccessStatusCode)
            return new PaymentResult(Guid.Empty, "Failed", "payment gateway unreachable");

        await using var stream = await res.Content.ReadAsStreamAsync(ct);
        using var doc = await JsonDocument.ParseAsync(stream, cancellationToken: ct);
        var root = doc.RootElement;
        return new PaymentResult(
            root.GetProperty("transactionId").GetGuid(),
            root.GetProperty("status").GetString() ?? "Failed",
            root.TryGetProperty("reason", out var r) && r.ValueKind != JsonValueKind.Null ? r.GetString() : null);
    }
}
