using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.JLP.OrderJlpLists.Queries.GetOrders;
using TropicFeel.Application.Sprint.TrackingSprint.Queries.GetTracking;


namespace TropicFeel.Application.JLP.OrdersJlp.Queries.GetOrders;


public record OrderReference(string order_reference): IRequest<OrdersVm?>
{

}
public class OrderReferenceValidator : AbstractValidator<OrderReference>
{
    public OrderReferenceValidator()
    {
    }
}

public class OrderReferenceHandler : IRequestHandler<OrderReference, OrdersVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;

    public OrderReferenceHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService)
    {
        _context = context;
        _jlpRequestService = jlpRequestService;
    }

    public async Task<OrdersVm?> Handle(OrderReference request, CancellationToken cancellationToken)
    {
        var orderReference = await _jlpRequestService.GetOrdersByIdReference(request.order_reference) ;
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
}
