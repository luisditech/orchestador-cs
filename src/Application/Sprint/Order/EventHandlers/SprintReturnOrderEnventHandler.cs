using Microsoft.Extensions.Logging;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Enums;
using TropicFeel.Domain.Events;
using TropicFeel.Domain.Exceptions;

namespace TropicFeel.Application.Sprint.Order.EventHandlers;

public class SprintReturnOrderEnventHandler(ILogger<SprintReturnOrderEnventHandler> logger,
        ISprintRequestService sprintRequestService,
        IApplicationDbContext context)
    : INotificationHandler<SprintReturnOrderEnvent>
{
    public async Task Handle(SprintReturnOrderEnvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("TropicFeel Domain Event: {DomainEvent}", notification.GetType().Name);

        var orderSale = await context.SalesOrder
            .FirstOrDefaultAsync(o => !string.IsNullOrEmpty(o.OrderReferenceJlp) && o.OrderReferenceJlp == notification.ReturnDto.JlpSalesOrder, cancellationToken);

        Guard.Against.NotFound(notification.ReturnDto.JlpSalesOrder, orderSale);

        try
        {
            Guard.Against.Null(orderSale.OrderSprint, nameof(orderSale.OrderSprint));
            //get order in sprint
            var sprintOrder = await sprintRequestService.GetOrderByIdAsync(orderSale.OrderSprint);
            Guard.Against.Null(sprintOrder, nameof(sprintOrder));

            var orderReturn = sprintOrder.Result;
            Guard.Against.Null(orderReturn, nameof(orderReturn));
            
            var rma = Guid.NewGuid().ToString();
            var productSprint = await sprintRequestService.CreateReturnAsync(new CreateReturnRequestDto()
            {
                OrderId = orderSale.OrderSprint,
                CustomerReference = orderReturn.CustomerRef1,
                RMANumber = rma,
                AttnOf = orderReturn.AttnOf,
                Telephone = orderReturn.Telephone,
                EmailAddr = orderReturn.EmailAddr,
                CompanyName = orderReturn.CompanyName,
                Address1 = orderReturn.Address1,
                Address2 = orderReturn.Address2,
                City = orderReturn.City,
                State = orderReturn.State,
                PostCode = orderReturn.PostCode,
                CountryCode = orderReturn.CountryID,
                //Town = 
                //NOP =
                StockOrderItems = orderReturn.StockOrderItems?.Select(item => new StockOrderItem()
                {
                    SKU = item.SKU,
                    Name = item.Description,
                    Quantity = item.Quantity,
                    //SellingPricePerUnit = 
                }).ToList(),
                ShipmentDimensions = new ShipmentDimensions()
                {
                    Length = 0,
                    Breadth = 0,
                    Height = 0,
                    Weight = 0
                },
                ReturnLabel = true,
                LabelType = "AtShop",
                
            });
            

            //FIXME: we need to add the log to the database and then publish the event 
            var log = new SalesOrderLog()
            {
                SalesOrderId = orderSale.Id,
                Status = nameof(StatusOrderSale.Return),
                Message =
                    $"Se hace devolución de la orden {orderSale.OrderSprint} en sprint",
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
