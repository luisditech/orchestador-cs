using System.Linq.Expressions;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Enums;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TropicFeel.Application.SalesOrders.Queries.GetSO;
public class SalesOrderByFilterVm
{
    public int Id { get; set; }
    public string? OrderReferenceJlp { get; set; }
    public DateTime? OrderDateJlp { get; set; }
    public string? DateFormat { get { return OrderDateJlp.HasValue ? OrderDateJlp.Value.ToString("dd-MM-yyyy") : string.Empty; } }
    public string? OrderNetSuite { get; set; }
    public string? OrderSprint { get; set; }
    public required string Status { get; set; }
    public string? StatusString { get; set; }
    public bool HasError { get; set; }
    public DateTimeOffset Created { get; set; }

    public static Expression<Func<SalesOrder, SalesOrderByFilterVm>> SalesOrderToSalesOrderVm => s => new SalesOrderByFilterVm()
    {
        Id = s.Id,
        OrderReferenceJlp = s.OrderReferenceJlp,
        OrderDateJlp = s.OrderDateJlp,
        OrderNetSuite = s.OrderNetSuite,
        OrderSprint = s.OrderSprint,
        Status = s.Status,
        Created = s.Created,
        HasError = s.Logs != null && s.Logs.OrderBy(x => x.Id).Last().Status == "Error"
    };


}

