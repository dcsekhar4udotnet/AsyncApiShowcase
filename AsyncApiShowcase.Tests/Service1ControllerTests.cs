using ApiService1.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AsyncApiShowcase.Tests;

public sealed class Service1ControllerTests
{
    [Fact]
    public async Task PostData_WithMissingInput_ReturnsBadRequest()
    {
        var controller = new Service1Controller();

        var result = await controller.PostData(new ApiService1.Models.ServiceDataRequest(""), CancellationToken.None);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
