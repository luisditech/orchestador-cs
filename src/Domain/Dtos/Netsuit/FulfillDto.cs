namespace TropicFeel.Domain.Dtos.Netsuit;
public class FulfillDto
{
    public string? Action { get; set; }
    public string? RecordType { get; set; }
    public int? Id { get; set; }
    public string? ToType { get; set; }
    public string? CustBody_Pwk_Tracking_Codes { get; set; }
    public string? CustBody_Tf_Sprint_Fulfill_Hawb { get; set; }
    public List<ItemFulfill>? Item { get; set; }
}

public class ItemFulfill
{
    public string? Item { get; set; }
    public int? Quantity { get; set; }
}

