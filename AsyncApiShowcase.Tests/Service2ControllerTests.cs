using ApiService2.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AsyncApiShowcase.Tests;

public sealed class Service2ControllerTests
{
    [Fact]
    public async Task PostData_WithMissingInput_ReturnsBadRequest()
    {
        var controller = new Service2Controller();

        var result = await controller.PostData(new ApiService2.Models.ServiceDataRequest(" "), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
