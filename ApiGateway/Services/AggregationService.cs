using ApiGateway.HttpClients;
using ApiGateway.Models;

namespace ApiGateway.Services;

public sealed class AggregationService : IAggregationService
{
    private readonly IService1Client _service1Client;
    private readonly IService2Client _service2Client;

    public AggregationService(IService1Client service1Client, IService2Client service2Client)
    {
        _service1Client = service1Client;
        _service2Client = service2Client;
    }

    public async Task<AggregateResponse> GetAggregateAsync(CancellationToken cancellationToken)
    {
        var service1Task = _service1Client.GetDataAsync(cancellationToken);
        var service2Task = _service2Client.GetDataAsync(cancellationToken);

        await Task.WhenAll(service1Task, service2Task);

        return new AggregateResponse(
            await service1Task,
            await service2Task,
            DateTimeOffset.UtcNow);
    }

    public async Task<AggregateResponse> PostAggregateAsync(
        AggregateRequest request,
        CancellationToken cancellationToken)
    {
        var serviceRequest = new ServiceDataRequest(request.Input);

        var service1Task = _service1Client.PostDataAsync(serviceRequest, cancellationToken);
        var service2Task = _service2Client.PostDataAsync(serviceRequest, cancellationToken);

        await Task.WhenAll(service1Task, service2Task);

        return new AggregateResponse(
            await service1Task,
            await service2Task,
            DateTimeOffset.UtcNow);
    }
}
