using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Exceptions;

[Serializable]
public class ExternalRequestException : Exception
{
    public ExternalRequestException()
    {
    }

    public ExternalRequestException(string? message) : base(message)
    {
    }

    public ExternalRequestException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
