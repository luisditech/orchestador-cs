using Newtonsoft.Json;

namespace TropicFeel.Domain.Dtos.Sprint;

public class StockOrderItemDto
{
    public required string SKU { get; set; }
    public required string Quantity { get; set; }
    [JsonIgnore]
    public string? Description { get; set; }
    
}
