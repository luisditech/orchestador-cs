namespace TropicFeel.Domain.Dtos.JLP;
public class RequestTrackingDto
{
    public required string part_number { get; set; }
    public required string line_ref { get; set; }
    public required int quantity { get; set; }
    public required string sub_status { get; set; }
    public DateTime supplier_dispatch_date { get; set; }
    public DateTime supplier_delivery_date { get; set; }
    public required string comment { get; set; }
    public required string fulfillment_route { get; set; }
}

public class RootObject
{
    public required List<RequestTrackingDto> items { get; set; }
}
