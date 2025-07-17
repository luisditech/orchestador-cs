using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Sprint.TrackingSprint.Queries.GetTracking;
public class TrackingVm
{
    public bool Status { get; set; }
    public required string Message { get; set; }
    public int Count { get; set; }
    public required TrackingResultData Result { get; set; }
}
