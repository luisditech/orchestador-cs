using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Sprint;
using TropicFeel.Infrastructure.Options;

namespace TropicFeel.Infrastructure.Sprint;

public class SprintRequestService(IExternalRequestClient externalRequestClient, SprintConfig sprintConfig)
    : ISprintRequestService
{
    public async Task<ResponseDto<ProductSprintDto>> GetProductBySkuAsync(string sku)
    {
        var products = await externalRequestClient.Get<ResponseDto<ProductSprintDto>>("Sprint", sprintConfig.BaseUrl, $"api/product?SKU={sku}");
        return products ?? new ResponseDto<ProductSprintDto>();
    }

    public async Task<ResponseDto<CreateReturnResponseDto>> CreateReturnAsync(CreateReturnRequestDto createReturnRequestDto)
    {
        var response =
            await externalRequestClient.Post<CreateReturnRequestDto, ResponseDto<CreateReturnResponseDto>>(createReturnRequestDto,
                "Sprint", sprintConfig.BaseUrl,
                "/api/returns/generatereturnlabel");
        return response;
    }

    public async Task<ResponseDto<List<ProductSprintDto>>> GetProductsAsync()
    {
        var products = await externalRequestClient.Get<ResponseDto<List<ProductSprintDto>>>("Sprint", sprintConfig.BaseUrl, $"api/product");
        return products ?? new ResponseDto<List<ProductSprintDto>>();
    }

    public async Task<ResponseDto<ProductSprintDto>> CreateProductsAsync(CreateProductRequestDto productRequestSprintDto)
    {
        var response =
            await externalRequestClient.Post<CreateProductRequestDto, ResponseDto<ProductSprintDto>>(productRequestSprintDto,
                "Sprint", sprintConfig.BaseUrl,
                "/api/product");
        return response;
    }

    public async Task<ResponseDto<OrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto)
    {
        var response =
            await externalRequestClient.Post<CreateOrderRequestDto, ResponseDto<OrderResponseDto>>(createOrderRequestDto,
                "Sprint", sprintConfig.BaseUrl,
                "/api/orders/uniqueorder");
        return response;
    }

    public async Task<ResponseDto<OrderResponseDto>> GetOrderByIdAsync(string id)
    {
        var order = await externalRequestClient.Get<ResponseDto<OrderResponseDto>>("Sprint", sprintConfig.BaseUrl, $"api/Orders/{id}");
        return order ?? new ResponseDto<OrderResponseDto>();
    }

    public async Task<ResponseDto<List<TrackingResponseDto>>> CreateTracking(CreateTrackingRequestDto createTrackingRequestDto)
    {
        var response =
            await externalRequestClient.Post<CreateTrackingRequestDto, ResponseDto<List<TrackingResponseDto>>>(createTrackingRequestDto,
                "Sprint", sprintConfig.BaseUrl,
                "/api/shipment/tracking/");
        return response;
    }

    public async Task<ResponseDto<ProductSprintDto>> UpdateStockAsync(UpdateStockDto updateStockDto)
    {
        var response =
            await externalRequestClient.Post<UpdateStockDto, ResponseDto<ProductSprintDto>>(updateStockDto,
                "Sprint", sprintConfig.BaseUrl,
                "/api/product/UpdateStock");
        return response;
    }

    public async Task<FulfillSprintDto?> GetFulfill(int id)
    {
        var fullfil =
            await externalRequestClient.Get<FulfillSprintDto?>("Sprint", sprintConfig.BaseUrl,
                $"api/shipment/pod?HAWB={id}");
        return fullfil;
    }

    public async Task<TrackingSprintDto?> GetTracking(int id)
    {
        var tracking = await externalRequestClient.Get<TrackingSprintDto?>("Sprint", sprintConfig.BaseUrl,
            $"api/shipment/tracking?HAWB={id}");

        return tracking;
    }
}
