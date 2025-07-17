namespace TropicFeel.Domain.Dtos.Sprint;

public class UpdateStockDto
{
    public int WarehouseLocationId { get; set; }
    public required string Sku { get; set; }
    public required string Quantity { get; set; }
}
