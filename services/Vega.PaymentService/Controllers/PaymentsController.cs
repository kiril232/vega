using Microsoft.AspNetCore.Mvc;
using Vega.PaymentService.Contracts;
using Vega.PaymentService.Services;

namespace Vega.PaymentService.Controllers;

[ApiController]
[Route("payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentProcessor _processor;

    public PaymentsController(IPaymentProcessor processor) => _processor = processor;

    [HttpPost("process")]
    public async Task<IActionResult> Process([FromBody] PaymentRequest req, CancellationToken ct)
    {
        var result = await _processor.ProcessAsync(req, ct);
        return Ok(result);
    }
}
