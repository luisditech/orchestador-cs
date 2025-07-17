using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Events;
public class CreateEventSaleOrder : BaseEvent
{
    public CreateEventSaleOrder(SalesOrder salesOrder)
    {
         SalesOrder = salesOrder;
    }

    public SalesOrder SalesOrder { get; }
}
