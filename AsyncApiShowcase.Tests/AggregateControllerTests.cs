using ApiGateway.Controllers;
using ApiGateway.Models;
using ApiGateway.Services;
using Microsoft.AspNetCore.Mvc;

namespace AsyncApiShowcase.Tests;

public sealed class AggregateControllerTests
{
    [Fact]
    public async Task Get_ReturnsOk()
    {
        var controller = new AggregateController(new FakeAggregationService());

        var result = await controller.Get(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<AggregateResponse>(ok.Value);
        Assert.Equal("ApiService1", payload.Service1.Source);
        Assert.Equal("ApiService2", payload.Service2.Source);
    }

    [Fact]
    public async Task Post_WithMissingInput_ReturnsBadRequest()
    {
        var controller = new AggregateController(new FakeAggregationService());

        var result = await controller.Post(new AggregateRequest(""), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    private sealed class FakeAggregationService : IAggregationService
    {
        public Task<AggregateResponse> GetAggregateAsync(CancellationToken cancellationToken)
        {
            var response = new AggregateResponse(
                new ServiceDataResponse("ApiService1", "ok-1", null, 10, DateTimeOffset.UtcNow),
                new ServiceDataResponse("ApiService2", "ok-2", null, 20, DateTimeOffset.UtcNow),
                DateTimeOffset.UtcNow);

            return Task.FromResult(response);
        }

        public Task<AggregateResponse> PostAggregateAsync(
            AggregateRequest request,
            CancellationToken cancellationToken)
        {
            var response = new AggregateResponse(
                new ServiceDataResponse("ApiService1", "ok-1", request.Input, 10, DateTimeOffset.UtcNow),
                new ServiceDataResponse("ApiService2", "ok-2", request.Input, 20, DateTimeOffset.UtcNow),
                DateTimeOffset.UtcNow);

            return Task.FromResult(response);
        }
    }
}
