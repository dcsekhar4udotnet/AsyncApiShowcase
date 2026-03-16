using System.Net.Http.Json;
using ApiGateway.Models;

namespace ApiGateway.HttpClients;

public sealed class Service1Client : IService1Client
{
    private readonly HttpClient _httpClient;

    public Service1Client(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ServiceDataResponse> GetDataAsync(CancellationToken cancellationToken)
    {
        // Relative path uses the BaseAddress configured in Program.cs.
        var response = await _httpClient.GetFromJsonAsync<ServiceDataResponse>(
            "api/service1/data",
            cancellationToken);

        return response ?? throw new InvalidOperationException("Service1 returned no data.");
    }

    public async Task<ServiceDataResponse> PostDataAsync(ServiceDataRequest request, CancellationToken cancellationToken)
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(
            "api/service1/data",
            request,
            cancellationToken);

        // Throw if the downstream API returned non-success.
        httpResponse.EnsureSuccessStatusCode();

        var response = await httpResponse.Content.ReadFromJsonAsync<ServiceDataResponse>(
            cancellationToken: cancellationToken);

        return response ?? throw new InvalidOperationException("Service1 returned no data.");
    }
}
