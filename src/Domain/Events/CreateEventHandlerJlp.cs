using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TropicFeel.Domain.Dtos.JLP;

namespace TropicFeel.Domain.Events;
public class CreateEventHandlerJlp : BaseEvent
{
    public object? OrderReference { get; }

    public CreateEventHandlerJlp( OrderJlpDto? orderjlp)
    {
        OrderReference = orderjlp;
    }
}
