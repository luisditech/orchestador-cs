using TropicFeel.Application.Common.Interfaces;

namespace TropicFeel.Application.SalesOrders.Queries.GetSO;


public record GetSalesOrderQuery(string? OrderReference) : IRequest<SalesOrderVm?>
{
}
public class GetSaleOrderQueryValidator : AbstractValidator<GetSalesOrderQuery>
{
    public GetSaleOrderQueryValidator()
    {
    }
}

public class GetSaleOrderQueryHandler : IRequestHandler<GetSalesOrderQuery, SalesOrderVm?>
{
    private readonly IApplicationDbContext _context;

    public GetSaleOrderQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
    }

    public async Task<SalesOrderVm?> Handle(GetSalesOrderQuery request, CancellationToken cancellationToken)
    {
        var order =   await _context.SalesOrder
                .Where(x => x.OrderReferenceJlp == request.OrderReference)
                .FirstOrDefaultAsync(cancellationToken);

        return new SalesOrderVm()
        {
            OrderReference = order?.OrderReferenceJlp
        };

    }
}
