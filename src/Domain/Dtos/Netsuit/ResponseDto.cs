namespace TropicFeel.Domain.Dtos.Netsuit;

public class ResponseDto
{
    public List<BodyItem> Body { get; set; } = new ();
}
public class BodyItem
{
    public int Id { get; set; }
    public string? RecordType { get; set; }
}
