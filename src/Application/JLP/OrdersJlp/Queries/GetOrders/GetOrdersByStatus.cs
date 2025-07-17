using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.JLP.OrderJlpLists.Queries.GetOrders;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Application.JLP.OrdersJlp.Queries.GetOrders;
public record OrderStatus(OrderStatusEnum status) : IRequest<OrdersVm?>
{

}
public class OrderStatusValidator : AbstractValidator<OrderStatus>
{
    public OrderStatusValidator()
    {
    }
}

public class OrderStatusHandler : IRequestHandler<OrderStatus, OrdersVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;

    public OrderStatusHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService)
    {
        _context = context;
        _jlpRequestService = jlpRequestService;
    }

    public async Task<OrdersVm?> Handle(OrderStatus request, CancellationToken cancellationToken)
    {
        
       
        var orderReference = await _jlpRequestService.GetOrdersByStatus(request.status.ToString());
        if (orderReference != null)
        {
            return new OrdersVm()
            {
                Count = orderReference.Count,
                Previous = orderReference.Previous,
                Next = orderReference.Next,
                Results = orderReference.Results
            };
        }
        return null;

    }
    private string? GetEnumDescription(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field != null)
        {
            var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }
        else
        {
            return null;
        }
    }
}
