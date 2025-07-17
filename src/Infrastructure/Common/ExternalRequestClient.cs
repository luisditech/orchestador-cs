using System;

using System.Net.Http.Headers;
using System.Text;
using Azure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Exceptions;

namespace TropicFeel.Infrastructure.Common;

public class ExternalRequestClient(AuthorizationServiceFactory authorizationServiceFactory, ILogger<ExternalRequestClient> logger)
    : IExternalRequestClient
{
    public async Task<TResponse?> Get<TResponse>(string clientName, string url, string apiEndpoint)
    {
        var uri = new Uri(url);
        var baseUrl = uri.GetLeftPart(UriPartial.Authority);
        var pathAndQuery = uri.PathAndQuery;
        var endPoint = string.IsNullOrEmpty(pathAndQuery.Trim()) ? apiEndpoint : $"{pathAndQuery}{apiEndpoint}";
        try
        {
            using var handler = new HttpClientHandler();
            using var client = new HttpClient(handler);

            client.BaseAddress = new Uri($"{baseUrl}/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            logger.LogInformation($"Requesting external service: {url}{endPoint}");
                
            if (!string.IsNullOrEmpty(clientName))
            {
                var authorizationService = authorizationServiceFactory.Create(clientName);
                var authentication = authorizationService.Authorize();
                client.DefaultRequestHeaders.Authorization = authentication;
            }

            var response = await client.GetAsync(endPoint.Replace("//", "/"));

            var jsonResult = await response.Content.ReadAsStringAsync();

            logger.LogInformation($"Response from external service: {jsonResult}");
            
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<TResponse>(jsonResult);
                    return result;
                }
                catch (JsonSerializationException jsonEx)
                {
                    logger.LogError(jsonEx, $"Failed to deserialize success response: {jsonEx.Message} Raw response: {jsonResult}");
                    throw new ExternalRequestException(
                        $"Failed to deserialize success response: {jsonEx.Message} Raw response: {jsonResult}");
                }
            }
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, $"Connection error: {e.Message}");
            throw new ExternalRequestException($"Connection error: {e.Message}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new ExternalRequestException(e.Message);
        }

        return default;
    }

    public async Task Post<TRequest>(TRequest requestDto, string clientName, string url, string apiEndpoint)
    {
        try
        {
            var response = await PostRequest(requestDto, url, apiEndpoint, clientName);
            var jsonResult = await response.Content.ReadAsStringAsync();
            
            logger.LogInformation($"Response from external service: {jsonResult}");
            
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"StatusCode {response.StatusCode} ReasonPhrase {response.ReasonPhrase} - Message: {jsonResult} - url: {url}{apiEndpoint}");
                throw new ExternalRequestException(
                $"StatusCode {response.StatusCode} ReasonPhrase {response.ReasonPhrase} - Message: {jsonResult} - url: {url}{apiEndpoint}");
            }

            
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, $"Connection error: {e.Message}");
            throw new ExternalRequestException($"Connection error: {e.Message}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new ExternalRequestException(e.Message);
        }

    }

    public async Task Patch<TRequest>(TRequest requestDto, string clientName, string url, string apiEndpoint)
    {
        try
        {
            var response = await PatchRequest(requestDto, url, apiEndpoint, clientName);
            var jsonResult = await response.Content.ReadAsStringAsync();

            logger.LogInformation($"Response from external service: {jsonResult}");

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"StatusCode {response.StatusCode} ReasonPhrase {response.ReasonPhrase} - Message: {jsonResult} - url: {url}{apiEndpoint}");
                throw new ExternalRequestException(
                $"StatusCode {response.StatusCode} ReasonPhrase {response.ReasonPhrase} - Message: {jsonResult} - url: {url}{apiEndpoint}");
            }


        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, $"Connection error: {e.Message}");
            throw new ExternalRequestException($"Connection error: {e.Message}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new ExternalRequestException(e.Message);
        }

    }

    public async Task<TResponse> Post<TRequest, TResponse>(TRequest requestDto, string clientName, string url,
        string apiEndpoint)
    {
        try
        {
            var response = await PostRequest(requestDto, url, apiEndpoint, clientName);
            var jsonResult = await response.Content.ReadAsStringAsync();

            logger.LogInformation($"Response from external service: {jsonResult}");
            
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var result = JsonConvert.DeserializeObject<TResponse>(jsonResult);
                    return result ?? throw new ExternalRequestException("Response is null");
                }
                catch (JsonSerializationException jsonEx)
                {
                    logger.LogError(jsonEx, $"Failed to deserialize success response: {jsonEx.Message} Raw response: {jsonResult}");
                    throw new ExternalRequestException(
                        $"Failed to deserialize success response: {jsonEx.Message} Raw response: {jsonResult}");
                }
            }

            logger.LogError($"StatusCode {response.StatusCode} ReasonPhrase {response.ReasonPhrase} - Message: {jsonResult} - url: {url}{apiEndpoint}");
            throw new ExternalRequestException(
                $"StatusCode {response.StatusCode} ReasonPhrase {response.ReasonPhrase} - Message: {jsonResult} - url: {url}{apiEndpoint}");
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, $"Connection error: {e.Message}");
            throw new ExternalRequestException($"Connection error: {e.Message}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new ExternalRequestException(e.Message);
        }
    }

    private async Task<HttpResponseMessage> PostRequest<TRequest>(TRequest requestDto, string url, string apiEndpoint, string clientName)
    {
        var uri = new Uri(url);
        var baseUrl = uri.GetLeftPart(UriPartial.Authority);
        var pathAndQuery = uri.PathAndQuery;
        var endPoint = string.IsNullOrEmpty(pathAndQuery.Trim()) || pathAndQuery.Equals("/")
            ? apiEndpoint
            : $"{pathAndQuery}{apiEndpoint}";
        
        logger.LogInformation($"Requesting external service: {url}{endPoint}");
        
        var json = JsonConvert.SerializeObject(requestDto);
        
        logger.LogInformation($"Request body: {json}");


        using var handler = new HttpClientHandler();
        using var client = new HttpClient(handler);

        client.BaseAddress = new Uri($"{baseUrl}/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(clientName))
        {
            var authorizationService = authorizationServiceFactory.Create(clientName);
            var authentication = authorizationService.Authorize();
            client.DefaultRequestHeaders.Authorization = authentication;
        }

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PostAsync(endPoint.Replace("//", "/"), stringContent);

       logger.LogInformation($"Response from external service: {response}");
        return response;
    }

    private async Task<HttpResponseMessage> PatchRequest<TRequest>(TRequest requestDto, string url, string apiEndpoint, string clientName)
    {
        var uri = new Uri(url);
        var baseUrl = uri.GetLeftPart(UriPartial.Authority);
        var pathAndQuery = uri.PathAndQuery;
        var endPoint = string.IsNullOrEmpty(pathAndQuery.Trim()) || pathAndQuery.Equals("/")
            ? apiEndpoint
            : $"{pathAndQuery}{apiEndpoint}";

        logger.LogInformation($"Requesting external service: {url}{endPoint}");

        var json = JsonConvert.SerializeObject(requestDto);

        logger.LogInformation($"Request body: {json}");


        using var handler = new HttpClientHandler();
        using var client = new HttpClient(handler);

        client.BaseAddress = new Uri($"{baseUrl}/");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (!string.IsNullOrEmpty(clientName))
        {
            var authorizationService = authorizationServiceFactory.Create(clientName);
            var authentication = authorizationService.Authorize();
            client.DefaultRequestHeaders.Authorization = authentication;
        }

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await client.PatchAsync(endPoint.Replace("//", "/"), stringContent);

        logger.LogInformation($"Response from external service: {response}");
        return response;
    }

}
