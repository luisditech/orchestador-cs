using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
public class JlpTokenDto
{
    public required string Refresh { get; set; }
    public required string Access { get; set; }
}
