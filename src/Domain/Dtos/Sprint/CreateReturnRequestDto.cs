namespace TropicFeel.Domain.Dtos.Sprint;

public class CreateReturnRequestDto
{
    public string? OrderId { get; set; }
    public string? CustomerReference { get; set; }
    public string? RMANumber { get; set; }
    public string? AttnOf { get; set; }
    public string? Telephone { get; set; }
    public string? EmailAddr { get; set; }
    public string? CompanyName { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Address3 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostCode { get; set; }
    public string? CountryCode { get; set; }
    public string? Town { get; set; }
    public int NOP { get; set; }
    public List<StockOrderItem>? StockOrderItems { get; set; }
    public ShipmentDimensions? ShipmentDimensions { get; set; }
    public bool ReturnLabel { get; set; }
    public string? LabelType { get; set; }
    public string? Type { get; set; }
    public string? Notes { get; set; }
}

public class StockOrderItem
{
    public string? Name { get; set; }
    public string? SKU { get; set; }
    public decimal SellingPricePerUnit { get; set; }
    public string? Quantity { get; set; } 
}

public class ShipmentDimensions
{
    public decimal Length { get; set; }
    public decimal Breadth { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
}
