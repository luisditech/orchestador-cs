using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TropicFeel.Application.TodoItems.EventHandlers;
using TropicFeel.Domain.Events;

namespace TropicFeel.Application.SalesOrders.EventHandlres;
public class SalesOrderCreatedEventHandler : INotificationHandler<CreateEventSaleOrder>
{
    private readonly ILogger<SalesOrderCreatedEventHandler> _logger;

    public SalesOrderCreatedEventHandler(ILogger<SalesOrderCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CreateEventSaleOrder notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TropicFeel Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
