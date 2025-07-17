using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class OrderJlpDto
{
    public int? Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<OrderResultDto>? Results { get; set; }
}

public class OrderResultDto
{
    public string? Url { get; set; }
    public string? Retailer { get; set; }
    public string? Order_Reference { get; set; }
    public DateTime Order_Date { get; set; }
    public string? Status { get; set; }
    public string? Purchase_Order_Reference { get; set; }
    public string? End_User_Purchase_Order_Reference { get; set; }
    public string? Additional_Order_Reference { get; set; }
    public string? Comment { get; set; }
    public bool Tes_tFlag { get; set; }
    public string? Supplier { get; set; }
    public List<OrderItemDto>? Items { get; set; }
    public string? Currency_Code { get; set; }
    public string? Sub_total { get; set; }
    public string? Tax { get; set; }
    public string? Total { get; set; }
    public ShippingAddressDto? Shipping_Address { get; set; }
}

