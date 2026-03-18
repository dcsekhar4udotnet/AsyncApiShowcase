using ApiService1.Models;

namespace AsyncApiShowcase.Tests;

public sealed class ApiServiceModelTests
{
    [Fact]
    public void Service1Response_StoresValues()
    {
        var now = DateTimeOffset.UtcNow;
        var response = new ServiceDataResponse("ApiService1", "ok", "hi", 5, now);

        Assert.Equal("ApiService1", response.Source);
        Assert.Equal("ok", response.Message);
        Assert.Equal("hi", response.Input);
        Assert.Equal(5, response.DelayMs);
        Assert.Equal(now, response.Timestamp);
    }

    [Fact]
    public void Service2Response_StoresValues()
    {
        var now = DateTimeOffset.UtcNow;
        var response = new ApiService2.Models.ServiceDataResponse("ApiService2", "ok", null, 7, now);

        Assert.Equal("ApiService2", response.Source);
        Assert.Equal("ok", response.Message);
        Assert.Null(response.Input);
        Assert.Equal(7, response.DelayMs);
        Assert.Equal(now, response.Timestamp);
    }
}

