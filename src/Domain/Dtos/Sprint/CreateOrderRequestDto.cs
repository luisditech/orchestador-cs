namespace TropicFeel.Domain.Dtos.Sprint;

public class CreateOrderRequestDto
{
    public int SalesOrderId { get; set; }
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
    public string? CountryID { get; set; }
    public string? CountryCode { get; set; }
    public string? SpecialInstructions { get; set; }
    public string? PackingNote { get; set; }
    public string? CustomerRef1 { get; set; }
    public string? CustomerRef2 { get; set; }
    public List<StockOrderItemDto>? StockOrderItems { get; set; }
}
