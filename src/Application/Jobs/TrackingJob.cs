using System.ComponentModel;
using Hangfire;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Application.Jobs;
public class TrackingJob
{
    private readonly IExternalRequestClient _externalRequestClient;
    private readonly IApplicationDbContext _context;
    private readonly INetsuitClientService _netsuitClientService;
    private readonly ISprintRequestService _sprintRequestService;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly ILogger<TrackingJob> _logger;
   

    public TrackingJob(IExternalRequestClient externalRequestClient,
                     IApplicationDbContext context,
                     INetsuitClientService netsuitClientService,
                     IJlpRequestService jlpRequestService,
                     ISprintRequestService sprintRequestService,ILogger<TrackingJob> logger)
    {
        _externalRequestClient = externalRequestClient;
        _context = context;
        _netsuitClientService = netsuitClientService;
        _sprintRequestService = sprintRequestService;
        _jlpRequestService = jlpRequestService;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 0)]
    [Hangfire.Dashboard.Management.Support.Job]
    [DisplayName(nameof(TrackingJob))]
    [Description("Tracking Jlp")]
    public async Task ExecuteTracking(PerformContext? context)
    {
        _logger.LogInformation($"Start job");

        var salesOrders = await _context.SalesOrder.Where(x => x.Status != StatusOrderSale.Invoiced.ToString() && x.Status != StatusOrderSale.Dispatch.ToString()
        && x.FulfillmentId == null
        && !string.IsNullOrEmpty(x.OrderNetSuite)
        && !string.IsNullOrEmpty(x.OrderSprint)).ToListAsync();

        var ordersId = salesOrders.Select(x => Convert.ToInt32 (x.OrderNetSuite));
        var ordersNetSuite = await _context.Orders.Where(x => ordersId.Contains(x.OrderId)).Include(x => x.Items).ToListAsync();
        foreach (var saleOrder in salesOrders)
        {
            try 
            {
                var orderNetSuite = ordersNetSuite.FirstOrDefault(x => x.OrderId == Convert.ToInt32(saleOrder.OrderNetSuite));
                if (orderNetSuite != null)
                {
                    var tracking = await _sprintRequestService.GetTracking(Convert.ToInt32(saleOrder?.OrderSprint));

                    if (string.IsNullOrEmpty(tracking?.Result.TrackingLink) ||
                      string.IsNullOrEmpty(tracking.Result.TrackingID) ||
                      string.IsNullOrEmpty(tracking.Result.Carrier))
                    {
                        var log = new SalesOrderLog()
                        {
                            SalesOrderId = saleOrder?.Id ?? 0,
                            Status = nameof(StatusOrderSale.TrackingEmpty),
                            Message =
                                $"no se obtiene tracking desde sprint la con el numero de sales order {saleOrder?.OrderSprint}",
                            Created = DateTime.Now,
                            Timestamp = DateTime.Now
                        };
                        _context.SalesOrderLog.Add(log);
                        await _context.SaveChangesAsync(default);

                    }
                    else
                    {
                        var logs = new List<SalesOrderLog>()
                        {
                            new SalesOrderLog()
                            {
                                SalesOrderId = saleOrder?.Id ?? 0,
                                Status = nameof(StatusOrderSale.ProcessTrackingNS),
                                Message = $"se obtiene tracking desde sprint la con el número de sales order {saleOrder?.OrderSprint} Carrier {tracking.Result.Carrier} TrackingID {tracking.Result.TrackingID} TrackingLink {tracking?.Result.TrackingLink} HAWB {tracking?.Result.HAWB} ",
                                Created = DateTime.Now,
                                Timestamp = DateTime.Now
                            },
                            new SalesOrderLog()
                            {
                                SalesOrderId = saleOrder?.Id ?? 0,
                                Status = nameof(StatusOrderSale.ProcessTrackingNS),
                                Message = $"Se obtiene tracking desde sprint con  SupplierDispatchedDate {tracking?.Result.SupplierDispatchedDate} SupplierDeliveryDate {tracking?.Result.SupplierDeliveryDate} ",
                                Created = DateTime.Now,
                                Timestamp = DateTime.Now
                            }
                        };
                        _context.SalesOrderLog.AddRange(logs);
                        await _context.SaveChangesAsync(default);

                        var createfulfill = new FulfillDto()
                        {
                            Action = "transform",
                            RecordType = "salesorder",
                            Id = orderNetSuite.OrderId,
                            CustBody_Pwk_Tracking_Codes = tracking?.Result.TrackingLink,
                            CustBody_Tf_Sprint_Fulfill_Hawb = tracking?.Result.HAWB,
                            ToType = "itemfulfillment",
                            Item = orderNetSuite?.Items?.Where(x => x.OrderId == orderNetSuite.Id && x.Quantity != null && x.Quantity.Length != 0 && x.Quantity != "0")
                            .Select(x => new ItemFulfill()
                            {
                                Quantity = int.Parse(x.Quantity ?? string.Empty),
                                Item = x.ItemReference != null ? x.ItemReference : string.Empty
                            }).ToList(),
                        };
                        var fulfillList = new List<FulfillDto>();
                        fulfillList.Add(createfulfill);
                        var responseFulfillment = await _netsuitClientService.CreateFulfill(fulfillList);
                        //crea fulfillment en Ns
                        var fulfillmentData = responseFulfillment.Body.FirstOrDefault(); //consulta Id de fulfillment

                        //actualiza campo en BD tropicfeel de la tabla Order

                        var orderToUpdate = saleOrder; //_context.Orders.FirstOrDefault(o => o.OrderId == orderId.OrderId);
                        if (orderToUpdate != null)
                        {
                            orderToUpdate.FulfillmentId = fulfillmentData?.Id;
                            orderToUpdate.Status = nameof(StatusOrderSale.ProcessTrackingNS);
                            _context.SalesOrder.Update(orderToUpdate);
                            await _context.SaveChangesAsync(default(CancellationToken));
                        }
                        //log report

                        var logfulfillment = new SalesOrderLog()
                        {
                            SalesOrderId = saleOrder?.Id ?? 0,
                            Status = nameof(StatusOrderSale.ProcessTrackingNS),
                            Message = $"recibe el fulfillment NetSuite con la orden interna {orderNetSuite?.OrderId} y se actualiza el campo fulfillmentId creado por NetSuite en BD-TABLA " +
                            $"Order {fulfillmentData?.Id}",
                            Created = DateTime.Now,
                            Timestamp = DateTime.Now
                        };
                        _context.SalesOrderLog.Add(logfulfillment);
                        await _context.SaveChangesAsync(default);


                        var orderReference = await _jlpRequestService.GetOrdersByIdReference(saleOrder?.OrderReferenceJlp ?? string.Empty);
                        var orderfirts = orderReference?.Results?.FirstOrDefault();
                        var cutUrl = orderfirts?.Url ?? string.Empty;
                        string[] parts = cutUrl.Split('/');
                        string url = parts[parts.Length - 2];


                        //se extrae los items y se procede a crear el tracking en JLP
                        // var items = orderId?.Items?.ToList(); //_context.Items.Where(o => o.OrderId == orderId.Id).ToList();
                        var listitemsDispatch = new List<RequestDispatchJlpDto>();
                        var listitemsTracking = new List<RequestTrackingDto>();
                        //SECCION PROCCESING
                        if (orderfirts?.Items != null)
                        {
                            foreach (var itemtracking in orderfirts.Items)
                            {
                                var itemTrackingRequest = new RequestTrackingDto()
                                {

                                    part_number = itemtracking.part_number ?? string.Empty,
                                    line_ref = itemtracking.line_reference ?? string.Empty,
                                    sub_status = "Pending",
                                    quantity = int.Parse(itemtracking.quantity ?? string.Empty),
                                    comment = orderfirts?.Comment ?? string.Empty,
                                    supplier_delivery_date = DateTime.Parse(tracking?.Result.SupplierDeliveryDate ?? string.Empty),
                                    supplier_dispatch_date = DateTime.Parse(tracking?.Result.SupplierDispatchedDate ?? string.Empty),
                                    fulfillment_route = "Direct to Customer"
                                };

                                listitemsTracking.Add(itemTrackingRequest);

                            }
                            await _jlpRequestService.CreateTrackingJlp(new RootObject() { items = listitemsTracking }, Convert.ToInt32(url));

                            if (saleOrder != null)
                            {
                                saleOrder.Status = nameof(StatusOrderSale.ProcessTrackingJlp);
                                _context.SalesOrder.Update(saleOrder);
                                await _context.SaveChangesAsync(default(CancellationToken));
                            }

                            //log report JLP
                            var logJlp = new SalesOrderLog()
                            {
                                SalesOrderId = saleOrder?.Id ?? 0,
                                Status = nameof(StatusOrderSale.ProcessTrackingJlp),
                                Message = $"envia el tracking a Jlp de la salesOrder proveniente de sprint {saleOrder?.OrderSprint} ",
                                Created = DateTime.Now,
                                Timestamp = DateTime.Now
                            };
                            _context.SalesOrderLog.Add(logJlp);
                            await _context.SaveChangesAsync(default);
                        }
                    }
                    if (!string.IsNullOrEmpty(tracking?.Result.TrackingLink) &&
                      !string.IsNullOrEmpty(tracking.Result.TrackingID) &&
                      !string.IsNullOrEmpty(tracking.Result.Carrier))
                    {
                        if (orderNetSuite?.Items != null)
                        {
                            foreach (var itemstock in orderNetSuite.Items)
                            {
                                var searchRequestsStock = new List<RequestSearchDto>
                            {
                                new RequestSearchDto
                                {
                                       Action = "_search",
                                       Type = "item",
                                       SearchId = "customsearch_tf_items_stock_jlp",
                                       Start = 0,
                                       End = 1,
                                       Filters = new List<Filter>
                                       {
                                        new Filter { Name = "internalid", Operator = "is", Values = itemstock.ItemReference ?? string.Empty }
                                       }
                                }
                            };

                                var responseItemstock = await _netsuitClientService.GetStock(searchRequestsStock);

                                var itemSearch = responseItemstock.Body
                                .SelectMany(list => list)
                                .FirstOrDefault();
                                if (itemSearch?.tq == "")
                                {
                                    itemSearch.tq = "0";
                                }
                                string itemNumber = string.Empty;
                                int index = itemstock.ItemDisplay?.IndexOf(':') ?? -1;
                                if (index != -1 && itemstock.ItemDisplay != null)
                                {
                                    itemNumber = itemstock.ItemDisplay.Substring(0, index).Trim();
                                }

                                var productPartNumber = await _jlpRequestService.GetProductsByPartNumber(itemNumber ?? string.Empty);

                                var productId = productPartNumber?.results.FirstOrDefault();
                                var cutUrl = productId?.url ?? string.Empty;
                                string[] parts = cutUrl.Split('/');
                                string url = parts[parts.Length - 2];
                                await _jlpRequestService.UpdateStockJlp(new StockJlpDto() { supplier_free_stock = Convert.ToInt32(itemSearch?.tq) }, Convert.ToInt32(url));


                            }
                            var logJlpStock = new SalesOrderLog()
                            {
                                SalesOrderId = saleOrder?.Id ?? 0,
                                Status = nameof(StatusOrderSale.UpdateStockJlp),
                                Message = $"actualización de stock en Jlp proveniente de la orden {saleOrder?.OrderReferenceJlp} ",
                                Created = DateTime.Now,
                                Timestamp = DateTime.Now
                            };
                            _context.SalesOrderLog.Add(logJlpStock);
                            await _context.SaveChangesAsync(default);
                        }
                    }
                    else
                    {
                        var logJlpStock = new SalesOrderLog()
                        {
                            SalesOrderId = saleOrder?.Id ?? 0,
                            Status = nameof(StatusOrderSale.Error),
                            Message = $"no es posible actualizar el stock  de la dado que el tracking aun no es completado {saleOrder?.OrderReferenceJlp} ",
                            Created = DateTime.Now,
                            Timestamp = DateTime.Now
                        };
                        _context.SalesOrderLog.Add(logJlpStock);
                        await _context.SaveChangesAsync(default);
                    }
                     
                }
                else 
                {
                    var salesOrderId = saleOrder != null ? saleOrder.Id : 0;
                    var log = new SalesOrderLog()
                    {
                        SalesOrderId = salesOrderId,
                        Status = nameof(StatusOrderSale.Error),
                        Message = "los Ids de las ordenes no coinciden con para ejecutar el trancking proveniente de Sprint ",
                        Created = DateTime.Now,
                        Timestamp = DateTime.Now
                    };
                    _context.SalesOrderLog.Add(log);
                    await _context.SaveChangesAsync(default);
                }
            }

            catch (Exception e)
            {
                var salesOrderId = saleOrder != null ? saleOrder.Id : 0;
                var log = new SalesOrderLog()
                {
                    SalesOrderId = salesOrderId,
                    Status = nameof(StatusOrderSale.Error),
                    Message = e.Message,
                    Created = DateTime.Now,
                    Timestamp = DateTime.Now
                };
                _context.SalesOrderLog.Add(log);
                await _context.SaveChangesAsync(default);
            }
        }
        
        _logger.LogInformation($"End job");
    }
}

