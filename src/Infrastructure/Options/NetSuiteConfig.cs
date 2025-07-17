using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Infrastructure.Options;
public class NetSuiteConfig
{
    public required string BaseUrl { get; set; }
    public required string ConsumerKey { get; set; }
    public required string ConsumerSecret { get; set; }
    public required string TokenKey { get; set; }
    public required string TokenSecret { get; set; }
    public required string Realm { get; set; }
    public required string ScriptId { get; set; }
    public required string DeployId { get; set; }
}
