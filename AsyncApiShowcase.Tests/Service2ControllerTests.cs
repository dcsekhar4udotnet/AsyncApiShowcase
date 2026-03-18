using ApiService2.Controllers;
using ApiService2.Models;
using Microsoft.AspNetCore.Mvc;

namespace AsyncApiShowcase.Tests;

public sealed class Service2ControllerTests
{
    [Fact]
    public async Task GetData_ReturnsOk()
    {
        var controller = new Service2Controller();

        var result = await controller.GetData(CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<ServiceDataResponse>(ok.Value);
        Assert.Equal("ApiService2", payload.Source);
        Assert.Equal("Response from ApiService2", payload.Message);
        Assert.Null(payload.Input);
        Assert.True(payload.DelayMs > 0);
    }

    [Fact]
    public async Task PostData_WithInput_ReturnsOk()
    {
        var controller = new Service2Controller();

        var result = await controller.PostData(new ServiceDataRequest("hello"), CancellationToken.None);

        var ok = Assert.IsType<OkObjectResult>(result.Result);
        var payload = Assert.IsType<ServiceDataResponse>(ok.Value);
        Assert.Equal("ApiService2", payload.Source);
        Assert.Equal("Response from ApiService2", payload.Message);
        Assert.Equal("hello", payload.Input);
        Assert.True(payload.DelayMs > 0);
    }

    [Fact]
    public async Task PostData_WithMissingInput_ReturnsBadRequest()
    {
        var controller = new Service2Controller();

        var result = await controller.PostData(new ServiceDataRequest(" "), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
