using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Infrastructure.Options;
public class JlpConfig
{
    public required string BaseUrl { get; set; }

    public required string User { get; set; }
    public required string Password { get; set; }
}
