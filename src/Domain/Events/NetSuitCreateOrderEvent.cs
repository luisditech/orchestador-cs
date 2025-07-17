using TropicFeel.Domain.Dtos.Netsuit;

namespace TropicFeel.Domain.Events;

public class NetSuitCreateOrderEvent  : BaseEvent
{
    public NetSuitCreateOrderEvent(OrderDto order)
    {
        Order = order;
    }

    public OrderDto  Order { get; }
}
