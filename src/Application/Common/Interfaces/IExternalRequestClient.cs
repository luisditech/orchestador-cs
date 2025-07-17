using System;

namespace TropicFeel.Application.Common.Interfaces;
public interface IExternalRequestClient
{
    Task<TResponse?> Get<TResponse>(string clientName, string url, string apiEndpoint);
    Task Post<TRequest>(TRequest requestDto, string clientName, string url, string apiEndpoint);
    Task Patch<TRequest>(TRequest requestDto, string clientName, string url, string apiEndpoint);
    Task<TResponse> Post<TRequest, TResponse>(TRequest requestDto, string clientName, string url, string apiEndpoint);
}
