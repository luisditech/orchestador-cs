using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Domain.Exceptions;
using TropicFeel.Infrastructure.Options;

namespace TropicFeel.Infrastructure.Netsuit;

public class NetsuitClientService(IOAuthAuthenticationService oAuthAuthenticationService, NetSuiteConfig netSuiteConfig)
    : INetsuitClientService
{
    public async Task<ResponseDto> CreateOrder(List<OrderDto> order)
    {
        return await Post<List<OrderDto>, ResponseDto>(order);
    }

    public async Task<ResponseDto> CreateFulfill(List<FulfillDto> fulfill)
    {
        return await Post<List<FulfillDto>, ResponseDto>(fulfill);
    }


    private async Task<TResponse> Post<T, TResponse>(T body) where TResponse : new()
    {
        string baseUrl = netSuiteConfig.BaseUrl;
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}?script={netSuiteConfig.ScriptId}&deploy={netSuiteConfig.DeployId}");
        httpRequest.Headers.Authorization = oAuthAuthenticationService.CreateAuthenticationHeaderValue(HttpMethod.Post);
        string jsonContent = JsonConvert.SerializeObject(body, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        httpRequest.Content = content;

        using HttpClient _httpClient = new();
        var httpResponse = await _httpClient.SendAsync(httpRequest);
        if (httpResponse.IsSuccessStatusCode)
        {
            var responseJson = await httpResponse.Content.ReadAsStringAsync();
            try
            {
                var salesOrderDto = JsonConvert.DeserializeObject<TResponse>(responseJson);
                return salesOrderDto ?? new TResponse();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al deserializar {ex.Message} responseJson {responseJson}");
            }
        }
        else
        {
            throw new ExternalAuthorizationException($"Error en la solicitud: {httpResponse.StatusCode}");
        }
    }

    public async Task<ResponseSearchDto> GetItem(List<RequestSearchDto> request)
    {
        return await Post<List<RequestSearchDto>, ResponseSearchDto>(request);
    }

    public async Task<ResponseSearchStockDto> GetStock(List<RequestSearchDto> request)
    {
        return await Post<List<RequestSearchDto>, ResponseSearchStockDto>(request);
    }

    public async Task<ResponseSearchOrderDto> GetSalesOrderById(List<RequestSearchOrderDto> request)
    {
        return await Post<List<RequestSearchOrderDto>, ResponseSearchOrderDto>(request);
    }
}
