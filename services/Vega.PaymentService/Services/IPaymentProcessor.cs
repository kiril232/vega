using Vega.PaymentService.Contracts;

namespace Vega.PaymentService.Services;

public interface IPaymentProcessor
{
    Task<PaymentResponse> ProcessAsync(PaymentRequest req, CancellationToken ct = default);
}
