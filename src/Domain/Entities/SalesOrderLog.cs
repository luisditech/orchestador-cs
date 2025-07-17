namespace TropicFeel.Domain.Entities;

public class SalesOrderLog : BaseAuditableEntity
{
    public int SalesOrderId { get; set; }
    public virtual SalesOrder? SalesOrder { get; set; }
    public required string? Status { get; set; }
    public string? Message { get; set; }
    public DateTime Timestamp { get; set; }
}
