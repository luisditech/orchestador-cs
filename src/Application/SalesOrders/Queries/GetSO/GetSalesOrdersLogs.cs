using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.Dtos;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Application.SalesOrders.Queries.GetSO;


public record GetSalesOrderLogsQuery(int? PageNumber, int? PageSize, int OrderId) : IRequest<PagedResultDto<SalesOrderLogVm>>;

public class GetSalesOrderLogsQueryValidator : AbstractValidator<GetSalesOrderQuery>
{
    public GetSalesOrderLogsQueryValidator()
    {
    }
}

public class GetSalesOrderLogsQueryHandler : IRequestHandler<GetSalesOrderLogsQuery, PagedResultDto<SalesOrderLogVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSalesOrderLogsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<SalesOrderLogVm>> Handle(GetSalesOrderLogsQuery request, CancellationToken cancellationToken)
    {

        int skip = (request.PageNumber.HasValue && request.PageSize.HasValue) ? (request.PageNumber.Value - 1) * request.PageSize.Value : 0;
        int take = request.PageSize.HasValue ? request.PageSize.Value : int.MaxValue;

        var totalRecords =  _context.SalesOrder.Count();
        var totalPages = request.PageSize.HasValue ? (int)Math.Ceiling((double)totalRecords / request.PageSize.Value) : 1;

        var logs = _context.SalesOrderLog.AsQueryable();

        if (skip > 0)
        {
            logs = logs.Skip(skip);
        }

        //var query  = logs.Take(take);
       var query = logs.Where(j => j.SalesOrderId == request.OrderId );


        var result = await query.ToListAsync();
        
        result.ForEach(so =>
        {
            if (so.Status != null)
            {
                try
                {
                    so.Status = EnumAtrib.GetEnumDescription(
                        (StatusOrderSale)Enum.Parse(typeof(StatusOrderSale), so.Status));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
            }
        });
        
        var pagedResult = new PagedResultDto<SalesOrderLogVm>
        {
            Items = _mapper.Map<List<SalesOrderLogVm>>(result),
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };

        return pagedResult;
    }
}
