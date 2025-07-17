using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Domain.Events;

public class SprintReturnOrderEnvent : BaseEvent
{
    public SprintReturnOrderEnvent(ReturnDto returnDto)
    {
        ReturnDto = returnDto;
    }

    public ReturnDto  ReturnDto { get; }
}

