namespace TropicFeel.Domain.Dtos.Sprint;

public class CreateProductRequestDto
{
    public string? SKU { get; set; }
    public string? ProductDescription { get; set; }
    public string? ExpiryDate { get; set; } 
    public string? ReplenishmentDate { get; set; }

    // Optional properties
    public string? Date { get; set; }
    public int MinimumStockLevel { get; set; }
    public int ItemsPerbox { get; set; }
    public string? ProductCategory { get; set; }
    public string? SubCategory { get; set; }
    public string? UnitValue { get; set; }
    public string? LanguageID { get; set; }
    public string? DepartmentID { get; set; }
    public string? UniWeightGrams { get; set; }
    public string? Misc1 { get; set; }
    public string? Misc2 { get; set; }
    public string? Notes { get; set; }
}
