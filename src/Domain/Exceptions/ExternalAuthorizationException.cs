namespace TropicFeel.Domain.Exceptions;

[Serializable]
public class ExternalAuthorizationException : Exception
{
    public ExternalAuthorizationException()
    {
    }

    public ExternalAuthorizationException(string? message) : base(message)
    {
    }

    public ExternalAuthorizationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
