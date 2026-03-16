using ApiGateway.Models;

namespace ApiGateway.HttpClients;

public interface IService1Client
{
    Task<ServiceDataResponse> GetDataAsync(CancellationToken cancellationToken);
    Task<ServiceDataResponse> PostDataAsync(ServiceDataRequest request, CancellationToken cancellationToken);
}
