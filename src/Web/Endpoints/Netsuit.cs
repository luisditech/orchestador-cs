using Microsoft.AspNetCore.Mvc;
using TropicFeel.Application.Netsuit.Fulfill.Commands.CreateFulfill;
using TropicFeel.Application.Order.Commands.NetSuitCreateOrder;

namespace TropicFeel.Web.Endpoints;

public class Netsuit : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapPost(CreateOrderNetSuit, "CreateOrderNetSuit")
            .MapPost(CreateTrackingNetSuit, "CreateTrackingNetSuite");
    }
    private async Task<IActionResult> CreateOrderNetSuit(ISender sender, NetSuitCreateOrderCommand command)
    {
        try
        {
            var result = await sender.Send(command);
            return new OkObjectResult(result);

        }
        catch (Exception e)
        {
            return new ObjectResult(new { message = e.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    private async Task<IActionResult> CreateTrackingNetSuit(ISender sender, NetSuiteCreateFulfillCommand command)
    {
        try
        {
            var result = await sender.Send(command);
            return new OkObjectResult(result);

        }
        catch (Exception e)
        {
            return new ObjectResult(new { message = e.Message })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}
