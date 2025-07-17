using System.Net.Http.Headers;

namespace TropicFeel.Application.Common.Interfaces;

public interface IExternalAuthorizationService
{
    public AuthenticationHeaderValue Authorize();
}
