using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class ResponseDispatchJlpDto
{
    public  string? PartNumber { get; set; }
    public  string? LineRef { get; set; }
    public int Quantity { get; set; }
    public DateTime SupplierDispatchDate { get; set; }
    public DateTime SupplierDeliveryDate { get; set; }
    public  string? TrackingNumber { get; set; }
    public  string? Carrier { get; set; }
}
