using System.Net.Http.Json;
using ApiGateway.Models;

namespace ApiGateway.HttpClients;

public sealed class Service2Client : IService2Client
{
    private readonly HttpClient _httpClient;

    public Service2Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServiceDataResponse> GetDataAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<ServiceDataResponse>(
            "api/service2/data",
            cancellationToken);

        return response ?? throw new InvalidOperationException("Service2 returned no data.");
    }

    public async Task<ServiceDataResponse> PostDataAsync(ServiceDataRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(
            "api/service2/data",
            request,
            cancellationToken);

        httpResponse.EnsureSuccessStatusCode();

        var response = await httpResponse.Content.ReadFromJsonAsync<ServiceDataResponse>(
            cancellationToken: cancellationToken);

        return response ?? throw new InvalidOperationException("Service2 returned no data.");
    }
}
