using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Common.Interfaces;

public interface ISprintRequestService
{
    Task<FulfillSprintDto?> GetFulfill(int id);
    Task<TrackingSprintDto?> GetTracking(int id);
    Task<ResponseDto<ProductSprintDto>> GetProductBySkuAsync(string sku);
    Task<ResponseDto<CreateReturnResponseDto> > CreateReturnAsync(CreateReturnRequestDto createReturnRequestDto);
    Task<ResponseDto<List<ProductSprintDto>>> GetProductsAsync();
    Task<ResponseDto<ProductSprintDto>> CreateProductsAsync(CreateProductRequestDto productRequestSprintDto);

    Task<ResponseDto<OrderResponseDto>> CreateOrderAsync(CreateOrderRequestDto createOrderRequestDto);
    Task<ResponseDto<OrderResponseDto>> GetOrderByIdAsync(string id);
    Task<ResponseDto<List<TrackingResponseDto>>> CreateTracking(CreateTrackingRequestDto createTrackingRequestDto);
    Task<ResponseDto<ProductSprintDto>> UpdateStockAsync(UpdateStockDto updateStockDto);
}
