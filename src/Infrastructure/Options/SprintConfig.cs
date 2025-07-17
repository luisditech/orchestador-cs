using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Infrastructure.Options;
public class SprintConfig
{
    public required string BaseUrl { get; set; }
    public required string Apikey { get; set; }
    public required string Password { get; set; }
}
