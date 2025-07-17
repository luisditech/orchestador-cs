namespace TropicFeel.Domain.Dtos.Sprint;

public class ProductSprintDto
{
    public int ID { get; set; }
    public string? SKU { get; set; }
    public string? Date { get; set; }
    public int MinimumStockLevel { get; set; }
    public string? ProductDescription { get; set; }
    public int ItemsPerBox { get; set; }
    public string? ProductCategory { get; set; }
    public string? SubCategory { get; set; }
    public double UnitValue { get; set; }
    public string? Language { get; set; }
    public string? CostCentre { get; set; }
    public int UnitWeightGrams { get; set; }
    public string? Misc1 { get; set; }
    public string? Misc2 { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public DateTime? ReplenishmentDate { get; set; }
    public string? Notes { get; set; }
    public int QtyAvailable { get; set; }
}
