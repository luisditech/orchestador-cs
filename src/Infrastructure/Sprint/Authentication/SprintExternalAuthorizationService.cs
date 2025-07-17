using System.Net.Http.Headers;
using System.Text;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Infrastructure.Options;

namespace TropicFeel.Infrastructure.Sprint.Authentication;

public class SprintExternalAuthorizationService : IExternalAuthorizationService
{
    private readonly SprintConfig _sprintConfig;
    public SprintExternalAuthorizationService(SprintConfig sprintConfig)
    {
        _sprintConfig = sprintConfig;
    }
    public AuthenticationHeaderValue Authorize()
    {
        string credentials = $"{_sprintConfig.Apikey}:{_sprintConfig.Password}"; 
        var bytes = Encoding.UTF8.GetBytes(credentials);
        string base64Credentials = Convert.ToBase64String(bytes);
        
        return new AuthenticationHeaderValue("Basic", base64Credentials);
    }
}
