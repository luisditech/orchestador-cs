using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.Sprint;
public class FulfillSprintDto
{

    public bool Status { get; set; }
    public required string Message { get; set; }
    public int Count { get; set; }
    public   required ResultData Result { get; set; }
}

public class ResultData
{
    public required string HAWB { get; set; }
    public required string Status { get; set; }
    public required string PODName { get; set; }
    public required string PODDate { get; set; }
    public required string PODTime { get; set; }
}

