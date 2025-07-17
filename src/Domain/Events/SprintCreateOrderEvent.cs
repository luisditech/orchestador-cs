using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Domain.Events;

public class SprintCreateOrderEvent  : BaseEvent
{
    public SprintCreateOrderEvent(CreateOrderRequestDto order)
    {
        Order = order;
    }

    public CreateOrderRequestDto  Order { get; }
}
