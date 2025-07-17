namespace TropicFeel.Domain.Dtos.Sprint;
public class TrackingSprintDto
{
    public bool Status { get; set; }
    public required string Message { get; set; }
    public int Count { get; set; }
    public required TrackingResultData Result { get; set; }
}

public class TrackingResultData
{
    public required string HAWB { get; set; }
    public required string Carrier { get; set; }
    public required string TrackingID { get; set; }
    public required string TrackingLink { get; set; }
    public required string SupplierDispatchedDate { get; set; }
    public required string SupplierDeliveryDate { get; set; }
   
}

