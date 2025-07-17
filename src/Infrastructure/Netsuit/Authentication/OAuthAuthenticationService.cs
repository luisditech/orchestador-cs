using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Infrastructure.Options;

namespace TropicFeel.Infrastructure.Netsuit.Authentication;
public class OAuthAuthenticationService : IOAuthAuthenticationService
{
    private readonly NetSuiteConfig _netSuiteConfig;
    private HttpMethod? _httpMethod;

    public OAuthAuthenticationService(NetSuiteConfig netSuiteConfig)
    {
        _netSuiteConfig = netSuiteConfig;
    }

    public AuthenticationHeaderValue CreateAuthenticationHeaderValue(HttpMethod httpMethod)
    {
        _httpMethod = httpMethod;
        var nonce = GenerateNonce();
        var timestamp = GenerateTimeStamp();

        var parameter = GetAuthenticationHeaderValueParameter(nonce, timestamp);
        return new AuthenticationHeaderValue("OAuth", parameter);
    }

    private string GetAuthenticationHeaderValueParameter(string nonce, string timestamp)
    {
        var signature = GenerateSignature(nonce, timestamp);

        //NetSuite doc:
        //https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941295.html

        var parameters = new OrderedDictionary
            {
                { "realm", _netSuiteConfig.Realm },
                { "oauth_token", _netSuiteConfig.TokenKey },
                { "oauth_consumer_key", _netSuiteConfig.ConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_timestamp", timestamp },
                { "oauth_signature_method", "HMAC-SHA256" },
                { "oauth_version", "1.0" },
                { "oauth_signature", signature }
            };

        var combinedOauthParams = CombineOAuthHeaderParams(parameters);
        Debug.WriteLine($"CreateAuthenticationHeaderValue: combinedOauthParams: {combinedOauthParams}");

        return combinedOauthParams;
    }


    private static string CombineOAuthHeaderParams(OrderedDictionary parameters)
    {
        var sb = new StringBuilder();
        var first = true;

        foreach (var key in parameters.Keys)
        {
            if (!first)
                sb.Append(", ");

            var value = parameters[key]?.ToString();
            if( value is not null)
            {
                value = Uri.EscapeDataString(value);
                sb.Append($"{key}=\"{value}\"");
                first = false;
            }
        }

        return sb.ToString();
    }


    // From NetSuite doc:
    // https://docs.oracle.com/en/cloud/saas/netsuite/ns-online-help/section_1534941088.html
    private string GenerateSignature(string nonce, string timestamp)
    {
        var baseString = GenerateSignatureBaseString(nonce, timestamp);
        string key = GenerateSignatureKey();

        Debug.WriteLine($"GenerateSignature: baseString: {baseString}");
        Debug.WriteLine($"GenerateSignature: key: {key}");

        string signature = "";

        var encoding = new ASCIIEncoding();

        byte[] keyBytes = encoding.GetBytes(key);
        byte[] messageBytes = encoding.GetBytes(baseString);

        using (var hmaCsha256 = new HMACSHA256(keyBytes))
        {
            byte[] hash = hmaCsha256.ComputeHash(messageBytes);
            signature = Convert.ToBase64String(hash);
        }

        return signature;
    }

    private string GenerateSignatureKey()
    {
        var keyParams = new List<string>
                {
                    _netSuiteConfig.ConsumerSecret,
                    _netSuiteConfig.TokenSecret
                };

        var key = CombineKeyParams(keyParams);
        return key;
    }

    private string GenerateSignatureBaseString(string nonce, string timestamp)
    {
        var requestUri = new Uri($"{_netSuiteConfig.BaseUrl}?script={_netSuiteConfig.ScriptId}&deploy={_netSuiteConfig.DeployId}");
        var requestUrlPath = requestUri.GetLeftPart(UriPartial.Path);

        string baseString = string.Empty;
        if (_httpMethod is not null)
        {
            baseString = _httpMethod.ToString();
            baseString += "&";
            baseString += Uri.EscapeDataString(requestUrlPath);
            baseString += "&";
        }

        var baseStringParams = new Dictionary<string, string>()
            {
                { "oauth_consumer_key", _netSuiteConfig.ConsumerKey },
                { "oauth_nonce", nonce },
                { "oauth_signature_method", "HMAC-SHA256" },
                { "oauth_timestamp", timestamp },
                { "oauth_token", _netSuiteConfig.TokenKey },
                { "oauth_version", "1.0" }
            };

        //Handle query string
        //https://www.rfc-editor.org/rfc/rfc5849#section-3.4.1

        var requestUriQuery = requestUri.Query;
        if (requestUriQuery != string.Empty)
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(requestUri.Query);
            if(queryParams != null)
            {
                foreach (var key in queryParams.Keys)
                {
                    var value = queryParams[key.ToString()]; 
                    if(value != null)
                    {
                        baseStringParams.Add(key.ToString() ?? string.Empty, value);
                    }
                }
            }
        }

        var combinedBaseStringParams = CombineBaseStringParams(baseStringParams);
        baseString += Uri.EscapeDataString(combinedBaseStringParams);
        return baseString;
    }



    // https://www.rfc-editor.org/rfc/rfc5849#section-3.4.1
    // 3.4.1.3.2.  Parameters Normalization

    private static string CombineBaseStringParams(Dictionary<string, string> parameters)
    {
        var sortedPairs = new List<string>();
        foreach (var key in parameters.Keys)
        {
            var pair = Uri.EscapeDataString(key) + "=" + Uri.EscapeDataString(parameters[key]);
            sortedPairs.Add(pair);
        }
        sortedPairs.Sort();

        var sb = new StringBuilder();
        var first = true;
        var separator = "&";

        foreach (var pair in sortedPairs)
        {
            if (!first)
                sb.Append(separator);

            sb.Append(pair);

            first = false;
        }

        return sb.ToString();
    }

    private static string CombineKeyParams(List<string> parameters)
    {
        var sb = new StringBuilder();
        var first = true;

        foreach (var param in parameters)
        {
            if (!first)
                sb.Append("&");

            sb.Append(Uri.EscapeDataString(param));
            first = false;
        }

        return sb.ToString();
    }

    private static string GenerateTimeStamp()
    {
        // Default implementation of UNIX time of the current UTC time
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToInt64(ts.TotalSeconds).ToString();
    }

    private static string GenerateNonce()
    {
        var random = new Random();

        // Just a simple implementation of a random number between 123400 and 9999999
        return random.Next(123400, 9999999).ToString();
    }
}
