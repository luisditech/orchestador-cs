namespace TropicFeel.Domain.Dtos.Netsuit;

public class RequestSearchOrderDto
{
    public required string Action { get; set; }
    public required string RecordType { get; set; }
    public int Id { get; set; }   
}
