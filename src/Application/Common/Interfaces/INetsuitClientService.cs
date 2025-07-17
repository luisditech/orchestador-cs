using TropicFeel.Domain.Dtos.Netsuit;

namespace TropicFeel.Application.Common.Interfaces;

public interface INetsuitClientService
{
    Task<ResponseDto> CreateOrder(List<OrderDto> order);
    Task<ResponseSearchDto> GetItem(List<RequestSearchDto> request);
    Task<ResponseDto> CreateFulfill(List<FulfillDto> fulfill);
    Task<ResponseSearchOrderDto> GetSalesOrderById(List<RequestSearchOrderDto> request);
    Task<ResponseSearchStockDto> GetStock(List<RequestSearchDto> request);


}
