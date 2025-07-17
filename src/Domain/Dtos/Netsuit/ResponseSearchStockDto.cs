using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TropicFeel.Domain.Dtos.Netsuit;
public class ResponseSearchStockDto
{

    public List<List<BodyItemStock>> Body { get; set; } = new List<List<BodyItemStock>>();
}

public class BodyItemStock
{
    public string? tq { get; set; }
}
