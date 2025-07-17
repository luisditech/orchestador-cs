using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Infrastructure.JLP.Athentication;
using TropicFeel.Domain.Exceptions;
using TropicFeel.Infrastructure.Options;
using TropicFeel.Infrastructure.Sprint.Authentication;
using Microsoft.Extensions.Caching.Memory;

namespace TropicFeel.Infrastructure.Common;

public class AuthorizationServiceFactory
{
    private readonly SprintConfig _sprintConfig;
    private readonly JlpConfig _jlpConfig;
    private readonly IMemoryCache _memoryCache;
    public AuthorizationServiceFactory(SprintConfig sprintConfig,JlpConfig jlpConfig,IMemoryCache memoryCache)
    {
        _sprintConfig = sprintConfig;
        _jlpConfig = jlpConfig;
        _memoryCache = memoryCache;
    }
    public IExternalAuthorizationService Create(string clientName)
    {
        IExternalAuthorizationService factory = clientName switch
        {
            "Sprint" => new SprintExternalAuthorizationService(_sprintConfig),
            "JLP" => new JlpExternalAuthorizationService(_jlpConfig, _memoryCache),
            _ => throw new ExternalAuthorizationException($"ClientName {clientName} doesn't support")
        };
        return factory;
    }
}
