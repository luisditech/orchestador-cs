using TropicFeel.Domain.Entities;

namespace TropicFeel.Application.SalesOrders.Queries.GetSO;
public class SalesOrderLogVm
{
    public int Id { get; set; }
    public int SalesOrderId { get; set; }
    public virtual SalesOrder? SalesOrder { get; set; }
    public required string Status { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }



}

