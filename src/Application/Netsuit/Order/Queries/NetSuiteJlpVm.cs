using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Domain.Dtos.JLP;

namespace TropicFeel.Application.Netsuit.Order.Queries;
public class NetSuiteJlpVm
{
    public int? Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<OrderResultDto>? Results { get; set; }
}
