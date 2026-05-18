using Vega.PaymentService.Contracts;

namespace Vega.PaymentService.Services;

public class PaymentProcessor : IPaymentProcessor
{
    // sandbox processor — we approve most charges, reject a small slice to
    // exercise the "Failed" path on the order service
    public async Task<PaymentResponse> ProcessAsync(PaymentRequest req, CancellationToken ct = default)
    {
        // pretend the gateway takes a moment
        var delay = Random.Shared.Next(200, 700);
        await Task.Delay(delay, ct);

        // reject suspiciously round huge orders and ~1/10 randomly
        var failsAmount = req.Amount > 5000m && req.Amount % 1000m == 0;
        var failsRandom = Random.Shared.NextDouble() < 0.10;

        if (failsAmount || failsRandom)
        {
            return new PaymentResponse(
                Guid.NewGuid(),
                req.OrderId,
                "Failed",
                failsAmount ? "amount flagged for manual review" : "issuer declined",
                DateTime.UtcNow);
        }

        return new PaymentResponse(Guid.NewGuid(), req.OrderId, "Paid", null, DateTime.UtcNow);
    }
}
