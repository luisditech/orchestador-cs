using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire.Server;
using Hangfire;
using Microsoft.Extensions.Logging;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.JLP.OrdersJlp.Queries.GetOrders;
using TropicFeel.Domain.Enums;
using TropicFeel.Domain.Entities;
using System.Threading;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Events;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TropicFeel.Application.SalesOrders.Commands.CreateSO;

namespace TropicFeel.Application.Jobs;
public class CreateOrderJob
{
    private readonly IExternalRequestClient _externalRequestClient;
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly ISender _sender;
    private readonly ILogger<CreateOrderJob> _logger;

    public CreateOrderJob(IExternalRequestClient externalRequestClient, IApplicationDbContext context, IJlpRequestService jlpRequestService,
        ILogger<CreateOrderJob> logger,ISender sender)
    {
        _context = context;
        _externalRequestClient = externalRequestClient;
        _logger = logger;
        _jlpRequestService = jlpRequestService;
        _sender = sender;
    }

    [AutomaticRetry(Attempts = 0)]
    [Hangfire.Dashboard.Management.Support.Job]
    [DisplayName(nameof(CreateOrderJob))]
    [Description("CreateOrder")]
    public async Task ExecuteOrder(PerformContext? context)
    {
        _logger.LogInformation($"Start job");

        var jlpOrder = await _jlpRequestService.GetOrdersByStatus(nameof(OrderStatusEnum.ORDER).ToString());
        if (jlpOrder?.Results != null)
        {
            foreach (var ordeJlp in jlpOrder.Results)
            {
                if (ordeJlp.Order_Reference != null)
                {
                   await _sender.Send(new CreateSaleOrderCommand() { OrderReference = ordeJlp.Order_Reference });
                }

            }
        }
    }
}
