using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using ApiGateway.HttpClients;
using ApiGateway.Models;

namespace AsyncApiShowcase.Tests;

public sealed class Service1ClientTests
{
    [Fact]
    public async Task GetDataAsync_ReturnsResponse()
    {
        var expected = new ServiceDataResponse("ApiService1", "ok-1", null, 10, DateTimeOffset.UtcNow);
        var client = CreateClient(request =>
        {
            Assert.Equal("/api/service1/data", request.RequestUri?.AbsolutePath);
            return JsonResponse(expected);
        });

        var sut = new Service1Client(client);

        var result = await sut.GetDataAsync(CancellationToken.None);

        Assert.Equal(expected.Source, result.Source);
        Assert.Equal(expected.Message, result.Message);
    }

    [Fact]
    public async Task PostDataAsync_ReturnsResponse()
    {
        var expected = new ServiceDataResponse("ApiService1", "ok-1", "hello", 10, DateTimeOffset.UtcNow);
        var client = CreateClient(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("/api/service1/data", request.RequestUri?.AbsolutePath);
            return JsonResponse(expected);
        });

        var sut = new Service1Client(client);

        var result = await sut.PostDataAsync(new ServiceDataRequest("hello"), CancellationToken.None);

        Assert.Equal("hello", result.Input);
    }

    [Fact]
    public async Task PostDataAsync_NonSuccess_Throws()
    {
        var client = CreateClient(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var sut = new Service1Client(client);

        await Assert.ThrowsAsync<HttpRequestException>(() =>
            sut.PostDataAsync(new ServiceDataRequest("boom"), CancellationToken.None));
    }

    private static HttpClient CreateClient(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        return new HttpClient(new StubHandler(handler))
        {
            BaseAddress = new Uri("http://localhost/")
        };
    }

    private static HttpResponseMessage JsonResponse<T>(T payload)
    {
        var json = JsonSerializer.Serialize(payload);
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_handler(request));
        }
    }
}

public sealed class Service2ClientTests
{
    [Fact]
    public async Task GetDataAsync_ReturnsResponse()
    {
        var expected = new ServiceDataResponse("ApiService2", "ok-2", null, 20, DateTimeOffset.UtcNow);
        var client = CreateClient(request =>
        {
            Assert.Equal("/api/service2/data", request.RequestUri?.AbsolutePath);
            return JsonResponse(expected);
        });

        var sut = new Service2Client(client);

        var result = await sut.GetDataAsync(CancellationToken.None);

        Assert.Equal(expected.Source, result.Source);
        Assert.Equal(expected.Message, result.Message);
    }

    [Fact]
    public async Task GetDataAsync_NullPayload_Throws()
    {
        var client = CreateClient(_ =>
            new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null", Encoding.UTF8, "application/json")
            });

        var sut = new Service2Client(client);

        await Assert.ThrowsAsync<InvalidOperationException>(() => sut.GetDataAsync(CancellationToken.None));
    }

    private static HttpClient CreateClient(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        return new HttpClient(new StubHandler(handler))
        {
            BaseAddress = new Uri("http://localhost/")
        };
    }

    private static HttpResponseMessage JsonResponse<T>(T payload)
    {
        var json = JsonSerializer.Serialize(payload);
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private sealed class StubHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

        public StubHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            _handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_handler(request));
        }
    }
}

