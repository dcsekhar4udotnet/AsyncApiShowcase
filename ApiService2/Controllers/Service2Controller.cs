using ApiService2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiService2.Controllers;

[ApiController]
[Route("api/service2")]
public sealed class Service2Controller : ControllerBase
{
    // Simulate a longer-running operation.
    private const int DelayMs = 5000;

    [HttpGet("data")]
    public async Task<ActionResult<ServiceDataResponse>> GetData(CancellationToken cancellationToken)
    {
        // Simulate work so async behavior is visible.
        await Task.Delay(DelayMs, cancellationToken);

        var response = new ServiceDataResponse(
            "ApiService2",
            "Response from ApiService2",
            null,
            DelayMs,
            DateTimeOffset.UtcNow);

        return Ok(response);
    }

    [HttpPost("data")]
    public async Task<ActionResult<ServiceDataResponse>> PostData(
        [FromBody] ServiceDataRequest request,
        CancellationToken cancellationToken)
    {
        // Simple guard to keep the demo endpoints predictable.
        if (request is null || string.IsNullOrWhiteSpace(request.Input))
        {
            return BadRequest("Input is required.");
        }

        // Simulate work so async behavior is visible.
        await Task.Delay(DelayMs, cancellationToken);

        var response = new ServiceDataResponse(
            "ApiService2",
            "Response from ApiService2",
            request.Input,
            DelayMs,
            DateTimeOffset.UtcNow);

        return Ok(response);
    }
}
