using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class OrderRequestJlpDto
{
    public string? url { get; set; }
    public string? retailer { get; set; }
    public string? order_reference { get; set; }
    public DateTime order_date { get; set; }
    public string? status { get; set; }
    public string? purchase_order_reference { get; set; }
    public string? end_user_purchase_order_reference { get; set; }
    public string? additional_order_reference { get; set; }
    public string? comment { get; set; }
    public bool test_flag { get; set; }
    public string? supplier { get; set; }
    public  List<OrderItemDto>? items { get; set; }
    public string? currency_code { get; set; }
    public string? sub_total { get; set; }
    public string? tax { get; set; }
    public string? total { get; set; }
    public  ShippingAddressDto? shipping_address { get; set; }
}
