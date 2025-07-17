using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Infrastructure.Options;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TropicFeel.Domain.Dtos.JLP;

namespace TropicFeel.Infrastructure.JLP.Athentication;

public class JlpExternalAuthorizationService : IExternalAuthorizationService
{
    private readonly JlpConfig _jlpConfig;
    private string JlpAccessToken
    {
        get
        {
            var tokenmemoryCache = _memoryCache.Get("JlpaccessToken");

            return tokenmemoryCache?.ToString() ?? string.Empty;
        }
        set
        {
            _memoryCache.Set("JlpaccessToken", value, TimeSpan.FromMinutes(4));
        }
    }
    private string JlpRefreshToken
    {
        get { var tokenmemoryCacheRefresh = _memoryCache.Get("JlprefreshToken"); return tokenmemoryCacheRefresh?.ToString() ?? string.Empty; }
        set { _memoryCache.Set("JlprefreshToken", value, TimeSpan.FromHours(4)); }
    }
    private readonly IMemoryCache _memoryCache;
    public JlpExternalAuthorizationService(JlpConfig jlpConfig, IMemoryCache memoryCache)
    {
        _jlpConfig = jlpConfig;
        _memoryCache = memoryCache;


    }

    public AuthenticationHeaderValue Authorize()
    {
        var accessToken = GetTokenAsync().Result;

        return new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private async Task AuthenticateAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_jlpConfig.BaseUrl}/restapi/v4/token");
        var content =
            new StringContent($"{{\"username\": \"{_jlpConfig.User}\", \"password\": \"{_jlpConfig.Password}\"}}",
                Encoding.UTF8, "application/json");
        request.Content = content;

        var client = new HttpClient();
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadAsStringAsync();

        var tokenObject = JsonConvert.DeserializeObject<JlpTokenDto>(tokenResponse);
        if (tokenObject != null)
        {
            JlpAccessToken = tokenObject.Access;
            JlpRefreshToken = tokenObject.Refresh;
        }
    }

    private async Task UseRefreshTokenAsync()
    {
        var refreshRequest = new HttpRequestMessage(HttpMethod.Post, $"{_jlpConfig.BaseUrl}/v4/token/refresh");
        var refreshContent =
            new StringContent($"{{\"refresh\": \"{JlpRefreshToken}\"}}", Encoding.UTF8, "application/json");
        refreshRequest.Content = refreshContent;

        var client = new HttpClient();
        refreshRequest.Content = refreshContent;
        var refreshResponse = await client.SendAsync(refreshRequest);

        if (refreshResponse.IsSuccessStatusCode)
        {
            var refreshedTokenResponse = await refreshResponse.Content.ReadAsStringAsync();
            var tokenObject = JsonConvert.DeserializeObject<JlpRefreshTokenDto>(refreshedTokenResponse);
            JlpAccessToken = tokenObject?.Access ?? string.Empty;
        }
    }
    
    private async Task<string> GetTokenAsync()
    {
        if (IsAccessTokenExpired())
        {
            if (IsRefreshTokenExpired())
            {
                await AuthenticateAsync();
            }
            else
            {
                await RefreshAccessTokenAsync();
            }
        }

        return JlpAccessToken ?? throw new InvalidOperationException("No se pudo obtener un token de acceso.");
    }

    private async Task RefreshAccessTokenAsync()
    {
        await UseRefreshTokenAsync();
        if (string.IsNullOrEmpty(JlpAccessToken))
        {
            await AuthenticateAsync();
        }
    }

    private bool IsAccessTokenExpired()
    {
        return IsExpired(JlpAccessToken);
    }

    private bool IsRefreshTokenExpired()
    {
        return IsExpired(JlpRefreshToken);
    }

    private static bool IsExpired(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return true;
        }

        try
        {
            var jwtToken = new JwtSecurityToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch (Exception)
        {
            return true;
        }
    }
}


