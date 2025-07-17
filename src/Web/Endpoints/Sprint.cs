using Microsoft.AspNetCore.Mvc;
using TropicFeel.Application.Order.Commands.CreateOrder;
using TropicFeel.Application.Product.Commands.UpdateStock;
using TropicFeel.Application.Sprint.FulfillSprint.Queries.GetFulfill;
using TropicFeel.Application.Sprint.Product.Queries.GetProducts;
using TropicFeel.Application.Sprint.TrackingSprint.Commands;
using TropicFeel.Application.Sprint.TrackingSprint.Queries.GetTracking;

namespace TropicFeel.Web.Endpoints;

public class Sprint : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapGet(GetProductLists,"Product/List")
            .MapPost(UpdateStock,"Product/UpdateStock")
            .MapPost(CreateOrder,"Order/CreateOrder")
            .MapPost(CreateTracking,"Tracking/CreateTracking")
            .MapGet(GetOrderFulfill,"Order/Fulfill/{id}")
            .MapGet(GetOrderTracking,"Order/Tracking/{id}");
    }

    private async Task<IActionResult> GetProductLists(ISender sender)
    {
        try
        {
            var result = await sender.Send(new GetProductsQuery());
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
    
    private async Task<IActionResult> UpdateStock(ISender sender, UpdateStockCommand updateStockCommand)
    {
        try
        {
            var result = await sender.Send(updateStockCommand);
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

    private async Task<IActionResult> CreateOrder(ISender sender, CreateOrderCommand command)
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

    private async Task<IActionResult> CreateTracking(ISender sender, CreateTrackingCommand command)
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


    private async Task<FulfillVm?> GetOrderFulfill(ISender sender, int id)
    {
        return await sender.Send(new GetFulfillQuery(id));
    }

    private async Task<TrackingVm?> GetOrderTracking(ISender sender,int id)
    {
        return await sender.Send(new GetTrackingQuery(id));
    }
}
