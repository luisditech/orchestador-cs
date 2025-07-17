using System.ComponentModel;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TropicFeel.Application.JLP.OrderJlpLists.Commands;
using TropicFeel.Application.JLP.OrderJlpLists.Queries.GetOrders;
using TropicFeel.Application.JLP.OrdersJlp.Queries.GetOrders;
using TropicFeel.Application.JLP.OrdersJlp.Queries.GetProducts;
using TropicFeel.Application.Order.Commands.CreateOrder;
using TropicFeel.Domain.Dtos;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Web.Endpoints;

public class Jlp: EndpointGroupBase
{
   
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            //.RequireAuthorization()
            .MapGet(GetOrderLists, "GetOrders")
            .MapPost(CreateOrderJlp, "CreateOrderJlp")
            .MapGet(GetOrderByReferenceId, "GetOrderByReference")
            .MapGet(GetOrderByStatus, "GetOrderStatus")
            .MapGet(GetProductsByPartNumber, "GetProductsByPartNumber");
    }

    private async Task<OrdersVm?> GetOrderLists(ISender sender)
    {
        
        var ordersVm = await sender.Send(new GetOrdersQuery());

        return ordersVm;
    }

    private async Task<OrdersVm?> GetOrderByReferenceId(ISender sender,string order_reference)
    {

        var ordersVm = await sender.Send(new OrderReference(order_reference));

        return ordersVm;
    }

    private async Task<ProductVm?> GetProductsByPartNumber(ISender sender, string part_number)
    {

        var products = await sender.Send(new ProductPartNumber(part_number));

        return products;
    }


    private async Task<OrdersVm?> GetOrderByStatus(ISender sender, OrderStatusEnum status)
    {
        
        
        var ordersStatus = await sender.Send(new OrderStatus(status));

        return ordersStatus;
    }
    private async Task<IActionResult> CreateOrderJlp(ISender sender, CreateOrderJlpCommand command)
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

    private static string? GetEnumDescription(OrderStatusEnum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field != null)
        {
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        else
        {
            return null;
        }
    }


    

}


