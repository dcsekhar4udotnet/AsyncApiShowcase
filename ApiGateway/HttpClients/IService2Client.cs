using ApiGateway.Models;

namespace ApiGateway.HttpClients;

public interface IService2Client
{
    Task<ServiceDataResponse> GetDataAsync(CancellationToken cancellationToken);
    Task<ServiceDataResponse> PostDataAsync(ServiceDataRequest request, CancellationToken cancellationToken);
}
