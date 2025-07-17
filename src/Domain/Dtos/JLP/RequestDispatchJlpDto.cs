using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class RequestDispatchJlpDto
{

    public required string part_number { get; set; }
    public  required string line_ref { get; set; }
    public int quantity { get; set; }
    public required string supplier_dispatch_date { get; set; }
    public required string  supplier_delivery_date { get; set; }
    public  string? tracking_number { get; set; }
    public required string carrier { get; set; }
    public required string tracking_url { get; set; }
}

public class DispatchBody
{
    public required List<RequestDispatchJlpDto> items { get; set; }
}
