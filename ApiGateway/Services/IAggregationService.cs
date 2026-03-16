using ApiGateway.Models;

namespace ApiGateway.Services;

public interface IAggregationService
{
    Task<AggregateResponse> GetAggregateAsync(CancellationToken cancellationToken);
    Task<AggregateResponse> PostAggregateAsync(AggregateRequest request, CancellationToken cancellationToken);
}
