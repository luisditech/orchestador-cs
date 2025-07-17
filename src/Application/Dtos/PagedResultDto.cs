namespace TropicFeel.Application.Dtos;
public class PagedResultDto<T>
{
    public List<T>? Items { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}
