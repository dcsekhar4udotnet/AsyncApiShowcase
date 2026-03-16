using ApiGateway.HttpClients;
using ApiGateway.Models;
using ApiGateway.Services;

namespace AsyncApiShowcase.Tests;

public sealed class AggregationServiceTests
{
    [Fact]
    public async Task GetAggregateAsync_ReturnsBothResponses()
    {
        var service1Response = new ServiceDataResponse(
            "ApiService1",
            "ok-1",
            null,
            10,
            DateTimeOffset.UtcNow);

        var service2Response = new ServiceDataResponse(
            "ApiService2",
            "ok-2",
            null,
            20,
            DateTimeOffset.UtcNow);

        var sut = new AggregationService(
            new FakeService1Client(service1Response),
            new FakeService2Client(service2Response));

        var result = await sut.GetAggregateAsync(CancellationToken.None);

        Assert.Equal("ApiService1", result.Service1.Source);
        Assert.Equal("ApiService2", result.Service2.Source);
        Assert.Equal("ok-1", result.Service1.Message);
        Assert.Equal("ok-2", result.Service2.Message);
    }

    [Fact]
    public async Task PostAggregateAsync_PassesInputToBothServices()
    {
        var service1 = new FakeService1Client(new ServiceDataResponse(
            "ApiService1",
            "ok-1",
            null,
            10,
            DateTimeOffset.UtcNow));

        var service2 = new FakeService2Client(new ServiceDataResponse(
            "ApiService2",
            "ok-2",
            null,
            20,
            DateTimeOffset.UtcNow));

        var sut = new AggregationService(service1, service2);

        var result = await sut.PostAggregateAsync(
            new AggregateRequest("hello"),
            CancellationToken.None);

        // Both clients should see the same input.
        Assert.Equal("hello", service1.LastInput);
        Assert.Equal("hello", service2.LastInput);
        Assert.Equal("hello", result.Service1.Input);
        Assert.Equal("hello", result.Service2.Input);
    }

    private sealed class FakeService1Client : IService1Client
    {
        private readonly ServiceDataResponse _response;

        public FakeService1Client(ServiceDataResponse response)
        {
            _response = response;
        }

        public string? LastInput { get; private set; }

        public Task<ServiceDataResponse> GetDataAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }

        public Task<ServiceDataResponse> PostDataAsync(
            ServiceDataRequest request,
            CancellationToken cancellationToken)
        {
            LastInput = request.Input;
            var response = _response with { Input = request.Input };
            return Task.FromResult(response);
        }
    }

    private sealed class FakeService2Client : IService2Client
    {
        private readonly ServiceDataResponse _response;

        public FakeService2Client(ServiceDataResponse response)
        {
            _response = response;
        }

        public string? LastInput { get; private set; }

        public Task<ServiceDataResponse> GetDataAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }

        public Task<ServiceDataResponse> PostDataAsync(
            ServiceDataRequest request,
            CancellationToken cancellationToken)
        {
            LastInput = request.Input;
            var response = _response with { Input = request.Input };
            return Task.FromResult(response);
        }
    }
}
