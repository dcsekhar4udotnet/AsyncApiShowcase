using ApiService1.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiService1.Controllers;

[ApiController]
[Route("api/service1")]
public sealed class Service1Controller : ControllerBase
{
    private const int DelayMs = 3000;

    [HttpGet("data")]
    public async Task<ActionResult<ServiceDataResponse>> GetData(CancellationToken cancellationToken)
    {
        await Task.Delay(DelayMs, cancellationToken);

        var response = new ServiceDataResponse(
            "ApiService1",
            "Response from ApiService1",
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
        if (request is null || string.IsNullOrWhiteSpace(request.Input))
        {
            return BadRequest("Input is required.");
        }

        await Task.Delay(DelayMs, cancellationToken);

        var response = new ServiceDataResponse(
            "ApiService1",
            "Response from ApiService1",
            request.Input,
            DelayMs,
            DateTimeOffset.UtcNow);

        return Ok(response);
    }
}
