using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
 public class ResponseTrackingJlpDto
{
    public string? part_number { get; set; }
    public string? line_ref { get; set; }
    public int? quantity { get; set; }
    public string? sub_status { get; set; }
    public DateTime supplier_dispatch_date { get; set; }
    public DateTime supplier_delivery_date { get; set; }
    public string? comment { get; set; }
   
}
