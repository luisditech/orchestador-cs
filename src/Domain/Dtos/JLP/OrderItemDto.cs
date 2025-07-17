using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class OrderItemDto
{
    public string? url { get; set; }
    public string? part_number { get; set; }
    public required string retailer_sku_reference { get; set; }
    public required string supplier_sku_reference { get; set; }
    public string? line_reference { get; set; }
    public string? quantity { get; set; }
    public string? name { get; set; }
    public string? description { get; set; }
    public string? status { get; set; }
    public decimal? unit_cost_price { get; set; }
    public decimal? subtotal { get; set; }
    public decimal? tax { get; set; }
    public decimal? tax_Rate { get; set; }
    public decimal? total { get; set; }
    public DateTime promised_date { get; set; }
    public string? retailer_additional_reference { get; set; }
}
