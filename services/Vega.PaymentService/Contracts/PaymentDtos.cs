using System.ComponentModel.DataAnnotations;

namespace Vega.PaymentService.Contracts;

public record PaymentRequest(
    [Required] Guid OrderId,
    [Required] Guid UserId,
    [Range(0.01, 1_000_000)] decimal Amount,
    [Required, MaxLength(8)] string Currency);

public record PaymentResponse(
    Guid TransactionId,
    Guid OrderId,
    string Status,
    string? Reason,
    DateTime ProcessedAt);
