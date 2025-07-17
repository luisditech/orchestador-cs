using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.Common.Interfaces;
public interface IJlpRequestService
{
    Task<OrderJlpDto?> GetOrders();
    Task<OrderJlpDto?> GetOrdersByIdReference(string order_reference);
    Task<OrderJlpDto?> GetOrdersByStatus(string status);
    Task<ProductResponseJlpDto?> GetProductsByPartNumber(string partNumber);
    Task<OrderResponseJlpDto> CreateOrder(OrderRequestJlpDto createOrderDto);
    Task CreateTrackingJlp(RootObject createTrackig, int orderUrl);
    Task CreateDispatchJlp(DispatchBody createDispatch, int orderUrl);
    Task UpdateStockJlp(StockJlpDto updateStock, int productUrl);
    Task<ResponseCancelJlpDto> CreateCancelationJlp(CancelJlpDto createDispatch, int orderUrl);
}
