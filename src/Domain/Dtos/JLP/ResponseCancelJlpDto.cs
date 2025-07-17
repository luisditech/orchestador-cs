using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class ResponseCancelJlpDto
{
    public required string part_number { get; set; }
    public required string line_ref { get; set; }
    public int quantity { get; set; }
    public required string comment { get; set; }
}
