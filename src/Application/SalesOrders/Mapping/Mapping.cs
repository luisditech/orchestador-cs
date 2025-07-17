using TropicFeel.Application.SalesOrders.Queries.GetSO;
using TropicFeel.Domain.Entities;

namespace TropicFeel.Application.SalesOrders.Mapping;
public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<SalesOrder, SalesOrderByFilterVm>();
        CreateMap<SalesOrder, SalesOrderVm>();
        CreateMap<SalesOrderLog, SalesOrderLogVm>();
    }
}
