using System.Linq;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.Dtos;
using TropicFeel.Domain.Enums;

namespace TropicFeel.Application.SalesOrders.Queries.GetSO;


public record GetSalesOrderByFilterQuery(int? PageNumber, int? PageSize, string? OrderReference, DateTime? Date, string? Status) : IRequest<PagedResultDto<SalesOrderByFilterVm>>;
public class GetSaleOrderByFilterQueryValidator : AbstractValidator<GetSalesOrderQuery>
{
    public GetSaleOrderByFilterQueryValidator()
    {
    }
}

public class GetSaleOrderByFilterQueryHandler : IRequestHandler<GetSalesOrderByFilterQuery, PagedResultDto<SalesOrderByFilterVm>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSaleOrderByFilterQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResultDto<SalesOrderByFilterVm>> Handle(GetSalesOrderByFilterQuery request, CancellationToken cancellationToken)
    {

        int skip = (request.PageNumber.HasValue && request.PageSize.HasValue) ? (request.PageNumber.Value - 1) * request.PageSize.Value : 0;
        int take = request.PageSize.HasValue ? request.PageSize.Value : int.MaxValue;

        var orders = _context.SalesOrder.Include(x => x.Logs).AsQueryable();
        
        // Aplicar filtros
        if (!string.IsNullOrEmpty(request.OrderReference))
        {
            orders = orders.Where(j => j.OrderDateJlp.HasValue && j.OrderReferenceJlp == request.OrderReference);
        }
        if (request.Date.HasValue)
        {
            var requestDate = request.Date.Value.Date; // Obtener solo la parte de la fecha sin la hora
            orders = orders.Where(j => j.OrderDateJlp.HasValue && j.OrderDateJlp.Value.Date == requestDate);
        }
        if (!string.IsNullOrEmpty(request.Status))
        {
            orders = orders.Where(j => j.Status.ToString() == request.Status);
        }

        // Obtener el total de registros después de aplicar los filtros
        var totalRecords = orders.Count();

        // Calcular el total de páginas
        var totalPages = request.PageSize.HasValue ? (int)Math.Ceiling((double)totalRecords / request.PageSize.Value) : 1;

        // Ordenar y paginar los resultados
        orders = orders.OrderByDescending(x => x.OrderDateJlp).Skip(skip).Take(take);

        // Ejecutar la consulta final
        var query = orders;

        var result = await  query.Select(SalesOrderByFilterVm.SalesOrderToSalesOrderVm).ToListAsync();
        result.ForEach(so => 
        {
            so.StatusString = EnumAtrib.GetEnumDescription((StatusOrderSale)Enum.Parse(typeof(StatusOrderSale), so.Status));
        });
        
        var pagedResult = new PagedResultDto<SalesOrderByFilterVm>
        {
            Items = result,
            TotalRecords = totalRecords,
            TotalPages = totalPages
        };

        return pagedResult;
    }
}
