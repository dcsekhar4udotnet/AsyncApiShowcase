using ApiFrontend.Models;

namespace AsyncApiShowcase.Tests;

public sealed class ApiFrontendModelTests
{
    [Fact]
    public void AggregateResponse_StoresValues()
    {
        var now = DateTimeOffset.UtcNow;
        var service1 = new ServiceDataResponse("Service1", "ok-1", "input", 10, now);
        var service2 = new ServiceDataResponse("Service2", "ok-2", null, 20, now);

        var response = new AggregateResponse(service1, service2, now);

        Assert.Equal("Service1", response.Service1.Source);
        Assert.Equal("Service2", response.Service2.Source);
        Assert.Equal(now, response.Timestamp);
    }
}
