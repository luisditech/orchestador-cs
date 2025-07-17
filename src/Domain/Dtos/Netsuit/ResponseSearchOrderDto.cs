namespace TropicFeel.Domain.Dtos.Netsuit;

public class ResponseSearchOrderDto
{
    public List<Body>? Body { get; set; }
}

public class Body
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public bool IsDynamic { get; set; }
    //public Fields Fields { get; set; }
    public Sublists? Sublists { get; set; }
}

public class Sublists
{
    public PaymentEvent? PaymentEvent { get; set; }
    public Dictionary<string, ItemSearch>? Items { get; set; }
    public Dictionary<string, BillingSchedule>? BillingSchedules { get; set; }
    public Dictionary<string, Link>? Links { get; set; }
}

public class PaymentEvent
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public string? Status { get; set; }
    public string? Amount { get; set; }
    public string? PaymentDate { get; set; }
    public string? Currency { get; set; }
    public string? TransactionId { get; set; }
    public string? PaymentMethod { get; set; }
    public bool IsSuccessful { get; set; }
    public string? ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ItemSearch
{
    public string? Amount { get; set; }
    public string? Amounthasbeenset { get; set; }
}

public class BillingSchedule
{
    public string? BillingScheduleId { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string? Frequency { get; set; }
    public string? Amount { get; set; }
    public string? Currency { get; set; }
    public bool IsActive { get; set; }
    public string? NextBillingDate { get; set; }
    public string? Status { get; set; }
    public int NumberOfOccurrences { get; set; }
    public string? RemainingBalance { get; set; }
    public string? BillingType { get; set; }
    public string? Terms { get; set; }
}
public class Link
{
    public string? Id { get; set; }
    public string? LinkType { get; set; }
    public string? LinkUrl { get; set; }
    public string? Status { get; set; }
    public string? SysId { get; set; }
    public string? SysParentId { get; set; }
    public string? Total { get; set; }
    public string? TranDate { get; set; }
    public string? TranId { get; set; }
    public string? Type { get; set; }
}


