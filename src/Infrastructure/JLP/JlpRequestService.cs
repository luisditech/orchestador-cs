using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Newtonsoft.Json;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Infrastructure.Common;
using TropicFeel.Infrastructure.Options;

namespace TropicFeel.Infrastructure.JLP;
public class JlpRequestService : IJlpRequestService
{
    private readonly IExternalRequestClient _externalRequestClient;
    private readonly JlpConfig _jlpConfig;
    public JlpRequestService(IExternalRequestClient externalRequestClient, JlpConfig jlpConfig )
    {
        _externalRequestClient = externalRequestClient;
        _jlpConfig = jlpConfig;
    }
    public async Task<OrderJlpDto?> GetOrders()
    {
      var orders =  await _externalRequestClient.Get<OrderJlpDto?>("JLP",_jlpConfig.BaseUrl, "/restapi/v4/orders/?format=json");

        if (orders != null)
        {
            return orders;
        }
        else {
               return null;
             }
        

       // orders?.Results?.ToList().ToString() ?? string.Empty;
        //orders != null ? orders?.ToString() ?? string.Empty : string.Empty;
    }

    public async Task<OrderJlpDto?> GetOrdersByIdReference(string order_reference)
    {
        var orderByReference = await _externalRequestClient.Get<OrderJlpDto?>("JLP", _jlpConfig.BaseUrl, $"/restapi/v4/orders/?&order_reference={order_reference}");

        return orderByReference != null ? orderByReference : null;
    }

    public async Task<OrderJlpDto?> GetOrdersByStatus(string status)
    {
        var orderstatus = await _externalRequestClient.Get<OrderJlpDto?>("JLP", _jlpConfig.BaseUrl, $"/restapi/v4/orders/?&status={status}");

        return orderstatus != null ? orderstatus : null;
    }

    public async Task<ProductResponseJlpDto?> GetProductsByPartNumber(string partNumber)
    {
        var product = await _externalRequestClient.Get<ProductResponseJlpDto?>("JLP", _jlpConfig.BaseUrl, $"/api/v4/products/?part_number={partNumber}");

        return product ?? null;
    }

    public async Task<OrderResponseJlpDto> CreateOrder(OrderRequestJlpDto createOrderDto)
    {
        var response = await _externalRequestClient.Post<OrderRequestJlpDto,OrderResponseJlpDto>(createOrderDto,
                "JLP", _jlpConfig.BaseUrl, "/restapi/v4/orders/?format=json");
        
        return response;
    }

    public async Task CreateTrackingJlp(RootObject createTrackig,int orderUrl)
    {
         await _externalRequestClient.Post<RootObject>(createTrackig,
                "JLP", _jlpConfig.BaseUrl, $"/restapi/v4/orders/{orderUrl}/acknowledge/?format=json");
        
    }

    public async Task CreateDispatchJlp(DispatchBody createDispatch, int orderUrl)
    {
         await _externalRequestClient.Post<DispatchBody>(createDispatch,
                "JLP", _jlpConfig.BaseUrl, $"/restapi/v4/orders/{orderUrl}/dispatch/?format=json");

    }

    public async Task UpdateStockJlp(StockJlpDto updateStock, int productUrl)
    {
        await _externalRequestClient.Patch<StockJlpDto>(updateStock,
               "JLP", _jlpConfig.BaseUrl, $"/api/v4/products/{productUrl}/stock");

    }

    public async Task<ResponseCancelJlpDto> CreateCancelationJlp(CancelJlpDto createCancel, int orderUrl)
    {
        var response = await _externalRequestClient.Post<CancelJlpDto, ResponseCancelJlpDto>(createCancel,
                "JLP", _jlpConfig.BaseUrl, $"/restapi/v4/orders/{orderUrl}/cancel_acknowledge/?format=json");

        return response;
    }
}

