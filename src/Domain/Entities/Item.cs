namespace TropicFeel.Domain.Entities;

public class Item : BaseAuditableEntity
{
    public string? ItemReference { get; set; }
    public int OrderId { get; set; }
    public virtual Order? Order { get; set; }
    public string? Amount { get; set; }
    public string? Description { get; set; }
    public string? Grossamt { get; set; }
    public string? ItemCode { get; set; }
    public string? ItemDisplay { get; set; }
    public string? Quantity { get; set; }
    public string? Taxcode { get; set; }
}
