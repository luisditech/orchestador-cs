using Microsoft.Extensions.Logging;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.JLP.OrdersJlp.Queries.GetOrders;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Enums;
using TropicFeel.Domain.Events;
using TropicFeel.Domain.Exceptions;

namespace TropicFeel.Application.Netsuit.Order.EventHandlers;

public class NetSuitCreateOrderEventHandler(
    ILogger<NetSuitCreateOrderEventHandler> logger,
    IApplicationDbContext context,
    INetsuitClientService netsuitClientService,
    IMediator mediator)
    : INotificationHandler<NetSuitCreateOrderEvent>
{
    public async Task Handle(NetSuitCreateOrderEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("TropicFeel Domain Event: {DomainEvent}", notification.GetType().Name);
        var orderDto = notification.Order;
        if (orderDto == null)
        {
            throw new Exception("Order is null");
        }

        var orderSale = await context.SalesOrder
            .FirstOrDefaultAsync(o => o.Id == orderDto.OrderId, cancellationToken);

        Guard.Against.NotFound(orderDto.OrderId, orderSale);

        try
        {
            var items = orderDto?.Items;
            if (items != null)
            {
                foreach (var item in items)
                {
                    if (item.Ean == null)
                    {
                        throw new BusinessException("EAN(RetailerAdditionalReference) is null");
                    }

                    var searchRequests = new List<RequestSearchDto>
                    {
                        new RequestSearchDto
                        {
                            Action = "_search",
                            Type = "item",
                            SearchId = "customsearch_gd_tf_find_internalid_upc",
                            Start = 0,
                            End = 1,
                            Filters = new List<Filter>
                            {
                                new Filter { Name = "upccode", Operator = "is", Values = item.Ean ?? string.Empty }
                            }
                        }
                    };
                    var responseItem = await netsuitClientService.GetItem(searchRequests);

                    var itemSearch = responseItem.Body
                        .SelectMany(list => list)
                        .FirstOrDefault();

                    if (itemSearch?.InternalID is null)
                    {
                        throw new BusinessException($"does not exist item with sku {item.ItemCode} in netsuit");
                    }

                    var modelNumber = itemSearch?.ModelNumber.Trim();
                    item.Item = itemSearch?.InternalID;
                    item.ModelNumber = item.ModelNumber !=  modelNumber && itemSearch?.ModelNumber is not null ? modelNumber : item.ModelNumber;
                }
            }

            if (orderDto != null)
            {
                if (string.IsNullOrEmpty(orderSale.OrderNetSuite))
                {
                    var orders = new List<OrderDto> { orderDto };
                    var orderCreatedNetsuit = await netsuitClientService.CreateOrder(orders);

                    var netsuitInternalId = orderCreatedNetsuit?.Body?.FirstOrDefault()?.Id ??
                                            throw new Exception("Order not created in netsuit");
                    var entity = new Domain.Entities.Order()
                    {
                        OrderId = netsuitInternalId,
                        Action = orderDto.Action,
                        RecordType = orderDto.RecordType,
                        Customform =
                            new Domain.Entities.CustomForm() { Text = orderDto.Customform?.Text ?? string.Empty },
                        Entity = orderDto.Entity,
                        Memo = orderDto.Memo,
                        CustbodyPwkTipoSo =
                            new Domain.Entities.CustbodyPwkTipoSo()
                            {
                                Text = orderDto.CustbodyPwkTipoSo?.Text ?? string.Empty
                            },
                        Location = orderDto.Location,
                        Class = orderDto.Class,
                        Department = orderDto.Department,
                        Currency = new Domain.Entities.Currency() { InternalId = orderDto.Currency?.InternalId ?? 0 },
                        Orderstatus = orderDto.Orderstatus,
                        CustbodyPwkShipmentOptionEvx = orderDto.CustbodyPwkShipmentOptionEvx,
                        Shipaddresslist = orderDto.Shipaddresslist,
                        Shipaddress = orderDto.Shipaddress,
                        CustbodyPwkCourier = orderDto.CustbodyPwkCourier,
                        CustbodyPwkOrderType = orderDto.CustbodyPwkOrderType,
                        Shipmethod = orderDto.Shipmethod,
                        Terms = orderDto.Terms,
                        Items = orderDto.Items?.Select(x => new Domain.Entities.Item()
                        {
                            ItemReference = x.Item,
                            Amount = x.Amount,
                            Description = x.Description,
                            Grossamt = x.Grossamt,
                            ItemDisplay = x.ItemDisplay,
                            Quantity = x.Quantity,
                            Taxcode = x.Taxcode
                        }).ToList()
                    };

                    context.Orders.Add(entity);

                    orderSale.Status = nameof(StatusOrderSale.SentToNetsuite);
                    orderSale.OrderNetSuite = netsuitInternalId.ToString();

                    await context.SaveChangesAsync(cancellationToken);
                    logger.LogInformation($"Created Order {entity.OrderId}");

                    //FIXME: we need to add the log to the database and then publish the event 
                    var log = new SalesOrderLog()
                    {
                        SalesOrderId = orderSale.Id,
                        Status = orderSale.Status,
                        Message =
                            $"Se recibe Orden desde JLP con referencia {orderSale.OrderReferenceJlp} y se crea en Netsuite con referencia {orderSale.OrderNetSuite}",
                        Created = DateTime.Now,
                        Timestamp = DateTime.Now,
                    };
                    context.SalesOrderLog.Add(log);
                    await context.SaveChangesAsync(cancellationToken);
                }
                
                
                if (string.IsNullOrEmpty(orderSale.OrderSprint))
                {
                    //split of Shipaddress for comma separated values
                    var shippingAddress = orderDto.Shipaddress?.Split("\n");
                    if (shippingAddress == null)
                    {
                        throw new BusinessException("Shipping Address is null");
                    }

                    var sprintOrder = new CreateOrderRequestDto()
                    {
                        SalesOrderId = orderSale.Id,
                        Address1 = shippingAddress[0],
                        Address2 = shippingAddress[1],
                        City = shippingAddress[2].Replace(",", ""),
                        PostCode = shippingAddress[3],
                        AttnOf = shippingAddress[4],
                        CountryCode = shippingAddress[6].Replace(",", ""),
                        CountryID = shippingAddress[6].Replace(",", ""),
                        EmailAddr = shippingAddress[7],
                        CustomerRef1 = $"JLP-{orderSale.OrderReferenceJlp}",
                        StockOrderItems = orderDto.Items?.Select(x => new StockOrderItemDto()
                        {
                            SKU = x.ModelNumber ?? string.Empty,
                            Quantity = x.Quantity ?? string.Empty,
                            Description = x.ItemDisplay ?? string.Empty
                        }).ToList(),
                    };
                    var createOrder = new SprintCreateOrderEvent(sprintOrder);
                    // publish with mediator for create order in sprint
                    await mediator.Publish(createOrder, cancellationToken);
                }
            }
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
    }
}
