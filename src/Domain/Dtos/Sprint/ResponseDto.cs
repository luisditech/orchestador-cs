namespace TropicFeel.Domain.Dtos.Sprint;

public class ResponseDto<T>
{
    public bool Status { get; set; }
    public string? Message { get; set; }
    public int Count { get; set; }
    public T? Result { get; set; }
}
