namespace Vega.OrderService.Services;

public record PaymentResult(Guid TransactionId, string Status, string? Reason);

public interface IPaymentClient
{
    Task<PaymentResult> ProcessAsync(Guid orderId, Guid userId, decimal amount, string currency, CancellationToken ct = default);
}
