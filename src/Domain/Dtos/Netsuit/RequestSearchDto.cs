namespace TropicFeel.Domain.Dtos.Netsuit;

public class RequestSearchDto
{
    public required string Action { get; set; }
    public required string Type { get; set; }
    public required string SearchId { get; set; }
    public int Start { get; set; }
    public int End { get; set; }
    public List<Filter> Filters { get; set; } =  new ();
}

public class Filter
{
    public required string Name { get; set; }
    public required string Operator { get; set; }
    public required string Values { get; set; }
}
