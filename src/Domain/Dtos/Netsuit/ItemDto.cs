using Newtonsoft.Json;

namespace TropicFeel.Domain.Dtos.Netsuit;

public class ItemDto
{
    public string? Item { get; set; }
    [JsonIgnore]
    public string? Amount { get; set; }
    [JsonIgnore]
    public string? Description { get; set; }
    [JsonIgnore]
    public string? Grossamt { get; set; }
    [JsonIgnore]
    public string? ItemCode { get; set; }
    [JsonIgnore]
    public string? ItemDisplay { get; set; }
    public string? Quantity { get; set; }
    public string? Taxcode { get; set; }
    [JsonIgnore]
    public string? ModelNumber { get; set; }
    
    [JsonIgnore]
    public string? Ean { get; set; }
}
