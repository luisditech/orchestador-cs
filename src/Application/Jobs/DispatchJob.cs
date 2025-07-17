using System;
using System.Collections.Generic;
using System.ComponentModel;
using Hangfire.Server;
using Hangfire;
using TropicFeel.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Application.Jobs;

public class DispatchJob
{
    private readonly ILogger<DispatchJob> _logger;
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly ISprintRequestService _sprintRequestService;

    public DispatchJob(ILogger<DispatchJob> logger, IApplicationDbContext context, IJlpRequestService jlpRequestService,
        ISprintRequestService sprintRequestService)
    {
        _logger = logger;
        _context = context;
        _jlpRequestService = jlpRequestService;
        _sprintRequestService = sprintRequestService;
    }

    [AutomaticRetry(Attempts = 0)]
    [Hangfire.Dashboard.Management.Support.Job]
    [DisplayName(nameof(DispatchJob))]
    [Description("DispatchJob Jlp")]
    public async Task ExecuteDispatch(PerformContext? context)
    {
        _logger.LogInformation($"Start job");
        var salesOrders = await _context.SalesOrder.Where(x => x.Status != StatusOrderSale.Invoiced.ToString() &&
                                                               x.Status != StatusOrderSale.Dispatch.ToString()
                                                               && x.FulfillmentId != null
                                                               && !string.IsNullOrEmpty(x.OrderNetSuite)
                                                               && !string.IsNullOrEmpty(x.OrderSprint)).ToListAsync();

        foreach (var saleOrder in salesOrders)
        {
            var listitemsDispatch = new List<RequestDispatchJlpDto>();

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
                var logTracking = new SalesOrderLog()
                {
                    SalesOrderId = saleOrder?.Id ?? 0,
                    Status = nameof(StatusOrderSale.ProcessTrackingJlp),
                    Message = $"Se obtiene tracking TrackingLink {tracking?.Result.TrackingLink }  TrackingID {tracking?.Result.TrackingID} Carrier {tracking?.Result.Carrier} desde sprint la con el numero de sales order {saleOrder?.OrderSprint} " +
                              $"y se envia a JLP",
                    Created = DateTime.Now,
                    Timestamp = DateTime.Now
                };
                _context.SalesOrderLog.Add(logTracking);
                await _context.SaveChangesAsync(default);
                
                var orderReference =
                    await _jlpRequestService.GetOrdersByIdReference(saleOrder?.OrderReferenceJlp ?? string.Empty);
                var orderfirts = orderReference?.Results?.FirstOrDefault();

                var cutUrl = orderfirts?.Url ?? string.Empty;
                string[] parts = cutUrl.Split('/');
                string url = parts[parts.Length - 2];

                if (orderfirts?.Items != null)
                {
                    foreach (var item in orderfirts.Items.Where(x => x.status == "ORDER_ACK"))
                    {
                        var supplierDeliveryDate = tracking?.Result.SupplierDeliveryDate != null ?
                                                   DateTime.Parse(tracking.Result.SupplierDeliveryDate).ToString("yyyy-MM-dd") :
                                                   string.Empty;

                        var supplierDispatchDate = tracking?.Result.SupplierDispatchedDate != null ?
                                                    DateTime.Parse(tracking.Result.SupplierDispatchedDate).ToString("yyyy-MM-dd") :
                                                    string.Empty;
                        var itemRequest = new RequestDispatchJlpDto()
                        {
                            part_number = item.part_number ?? string.Empty,
                            line_ref = item.line_reference ?? string.Empty,
                            quantity = Convert.ToInt32(item.quantity ?? string.Empty),
                            supplier_delivery_date = supplierDeliveryDate.ToString(),
                            supplier_dispatch_date = supplierDispatchDate.ToString(),
                            carrier = tracking?.Result.Carrier ?? string.Empty,
                            tracking_number = tracking?.Result.TrackingID,
                            tracking_url = tracking?.Result.TrackingLink ?? string.Empty,
                        };

                        listitemsDispatch.Add(itemRequest);
                    }

                    await _jlpRequestService.CreateDispatchJlp(new DispatchBody() { items = listitemsDispatch },
                        Convert.ToInt32(url));
                    if (saleOrder != null)
                    {
                        saleOrder.Status = nameof(StatusOrderSale.Dispatch);
                        _context.SalesOrder.Update(saleOrder);
                        await _context.SaveChangesAsync(default(CancellationToken));
                    }

                    //log report JLP
                    var log = new SalesOrderLog()
                    {
                        SalesOrderId = saleOrder?.Id ?? 0,
                        Status = nameof(StatusOrderSale.Dispatch),
                        Message =
                            $"Envia el tracking a Jlp de la salesOrder proveniente de sprint {saleOrder?.OrderSprint} " +
                            $"y posteriormente cambia el estado de la orden a despachado",
                        Created = DateTime.Now,
                        Timestamp = DateTime.Now
                    };
                    _context.SalesOrderLog.Add(log);
                    await _context.SaveChangesAsync(default);
                }
            }
        }

        _logger.LogInformation($"End job");
    }
}
