using Microsoft.Extensions.Logging;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Enums;
using TropicFeel.Domain.Events;
using TropicFeel.Domain.Exceptions;

namespace TropicFeel.Application.Sprint.Order.EventHandlers;

public class SprintCreateOrderEventHandler(ILogger<SprintCreateOrderEventHandler> logger, 
    ISprintRequestService sprintRequestService,
    IApplicationDbContext context)
    : INotificationHandler<SprintCreateOrderEvent>
{
    public async Task Handle(SprintCreateOrderEvent notification, CancellationToken cancellationToken)
    {
        
        logger.LogInformation("TropicFeel Domain Event: {DomainEvent}", notification.GetType().Name);
        
        var orderSale = await context.SalesOrder
            .FirstOrDefaultAsync(o => o.Id == notification.Order.SalesOrderId, cancellationToken);

        Guard.Against.NotFound(notification.Order.SalesOrderId, orderSale);
        
        try
        {
           /* Mapping of products*/
           if (notification.Order.StockOrderItems != null)
           {
               foreach (var item in notification.Order.StockOrderItems)
               {
                     //find sku in products in productmapping 
                     var productMapping = context.ProductMapping
                         .FirstOrDefault(p => p.NetSuiteSku == item.SKU);
                     if (productMapping != null)
                     {
                         item.SKU = productMapping.SprintSku;
                     }
               }
           }
           
            var resultSprint = await sprintRequestService.CreateOrderAsync(notification.Order);
            
            var result = resultSprint?.Result;
            
            orderSale.Status = nameof(StatusOrderSale.SentToSprint);
            orderSale.OrderSprint = result?.AWB;
                
            await context.SaveChangesAsync(cancellationToken);
            
            //FIXME: we need to add the log to the database and then publish the event 
            var log = new SalesOrderLog()
            {
                SalesOrderId = orderSale.Id,
                Status = nameof(StatusOrderSale.SentToSprint),
                Message = $"Se recibe Orden desde Netsuit y se envia a Sprint con referencia  AWB {result?.AWB}",
                Created = DateTime.Now,
                Timestamp = DateTime.Now

            };
            context.SalesOrderLog.Add(log);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception e)
        {
            //FIXME: we need to add the log to the database and then publish the event 
            var log = new SalesOrderLog()
            {
                SalesOrderId = orderSale.Id,
                Status = nameof(StatusOrderSale.Error),
                Message = e.Message,
                Created = DateTime.Now,
                Timestamp = DateTime.Now
            };
            context.SalesOrderLog.Add(log);
            await context.SaveChangesAsync(cancellationToken);
        }
        logger.LogInformation("End TropicFeel Domain Event: {DomainEvent}", notification.GetType().Name);
    }
}
