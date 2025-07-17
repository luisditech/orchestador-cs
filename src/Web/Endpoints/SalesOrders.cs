using Microsoft.AspNetCore.Mvc;
using TropicFeel.Application.Common.Models;
using TropicFeel.Application.Order.Commands.ReturnOrder;
using TropicFeel.Application.SalesOrders.Commands.CreateSO;
using TropicFeel.Application.SalesOrders.Commands.UpdateSO;
using TropicFeel.Application.SalesOrders.Queries.GetSO;

namespace TropicFeel.Web.Endpoints;

public class SalesOrders : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetSaleOrder, "SalesOrderReference")
            .MapGet(GetSalesOrder, "GetSalesOrderByFilterQuery")
            .MapGet(GetSalesOrderLogs, "GetSalesOrderLogsQuery")
            .MapPost(CreateSalesOrder)
            .MapPost(ReprocessOrder, "ReprocessOrder")
            .MapPut(UpdateSaleOrder, "{id}")
            .MapPut(ReturnSalesOrder, "ReturnSalesOrder");

    }

    private async Task<IActionResult> ReprocessOrder(ISender sender, string orderReference)
    {
        try
        {
            var result = await sender.Send(new CreateSaleOrderCommand() { OrderReference = orderReference });
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
    private async Task<IActionResult> GetSalesOrder(ISender sender, [AsParameters] GetSalesOrderByFilterQuery query)
    {
        var result = await sender.Send(query);
        return new OkObjectResult(result);
    }

    private async Task<IActionResult> GetSalesOrderLogs(ISender sender, [AsParameters] GetSalesOrderLogsQuery query)
    {
        var result =  await sender.Send(query);
        return new OkObjectResult(result);
    }

    private async Task<SalesOrderVm?> GetSaleOrder(ISender sender, [AsParameters] GetSalesOrderQuery query)
    {
        //await sender.Send(new CreateSaleOrderCommand() { OrderReference = "Y301064032" });
        return await sender.Send(query);
    }

    private async Task<IActionResult> CreateSalesOrder(ISender sender, CreateSaleOrderCommand command)
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

    private async Task<IResult> UpdateSaleOrder(ISender sender, string id, UpdateSalesOrderCommand command)
    {
        if (id != command.OrderReference) return Results.BadRequest();
        await sender.Send(command);
        return Results.NoContent();
    }
    
    private async Task<IActionResult> ReturnSalesOrder(ISender sender, ReturnOrderCommand command)
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

