using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Domain.Dtos.JLP;

namespace TropicFeel.Application.JLP.OrdersJlp.Queries.GetProducts;
public class ProductVm
{
    public int? Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public required List<Result> results { get; set; }
}
