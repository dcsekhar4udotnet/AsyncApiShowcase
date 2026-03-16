using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/aggregate")]
public sealed class AggregateController : ControllerBase
{
    private readonly IAggregationService _aggregationService;

    public AggregateController(IAggregationService aggregationService)
    {
        _aggregationService = aggregationService;
    }

    [HttpGet]
    public async Task<ActionResult<AggregateResponse>> Get(CancellationToken cancellationToken)
    {
        var response = await _aggregationService.GetAggregateAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<AggregateResponse>> Post(
        [FromBody] AggregateRequest request,
        CancellationToken cancellationToken)
    {
        if (request is null || string.IsNullOrWhiteSpace(request.Input))
        {
            return BadRequest("Input is required.");
        }

        var response = await _aggregationService.PostAggregateAsync(request, cancellationToken);
        return Ok(response);
    }
}
