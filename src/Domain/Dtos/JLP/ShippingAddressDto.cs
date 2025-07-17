using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class ShippingAddressDto
{
    public  string? country { get; set; }
    public  string? line_1 { get; set; }
    public  string? line_2 { get; set; }
    public  string? city { get; set; }
    public  string? postal_code { get; set; }
    public  string? state { get; set; }
    public  string? phone { get; set; }
    public  string? full_name { get; set; }
    public  string? email { get; set; }
}
