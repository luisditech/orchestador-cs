using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos;

namespace TropicFeel.Application.JLP.OrderJlpLists.Queries.GetOrders;


public record GetOrdersQuery : IRequest<OrdersVm?>
{
}

public class GetOrdersQueryValidator : AbstractValidator<GetOrdersQuery>
{
    public GetOrdersQueryValidator()
    {
    }
}

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, OrdersVm?>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly IMapper _mapper;
    public GetOrdersQueryHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService, IMapper mapper)
    {
        _context = context;
        _jlpRequestService = jlpRequestService;
        _mapper = mapper;
    }

    public async Task<OrdersVm?> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var ordersJlp = await _jlpRequestService.GetOrders();

        var order = new OrdersVm()
        {
            Count = ordersJlp?.Count,
            Next = ordersJlp?.Next,
            Previous = ordersJlp?.Previous,
            Results = ordersJlp?.Results

        };

        return order;

        //return new OrdersVm();
    }
}
