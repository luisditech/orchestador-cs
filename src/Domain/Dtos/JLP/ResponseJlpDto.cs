using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class ResponseJlpDto<T>
{
    public string? Status { get; set; }
    public string? Message { get; set; }
    public int Count { get; set; }
    public  T? Result { get; set; }
}
