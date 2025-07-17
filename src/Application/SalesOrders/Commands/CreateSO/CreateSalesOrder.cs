using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Netsuit;
using TropicFeel.Domain.Entities;
using TropicFeel.Domain.Enums;
using TropicFeel.Domain.Events;

namespace TropicFeel.Application.SalesOrders.Commands.CreateSO;

public record CreateSaleOrderCommand : IRequest<int>
{
    public required string OrderReference { get; init; }
    public DateTime OrderDate { get; init; }
}

public class CreateSaleOrderCommandHandler : IRequestHandler<CreateSaleOrderCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly IMediator _mediator;
    private readonly IAppConfigService _appConfigService;


    public CreateSaleOrderCommandHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService,
        IMediator mediator, IAppConfigService appConfigService)
    {
        _context = context;
        _jlpRequestService = jlpRequestService;
        _mediator = mediator;
        _appConfigService = appConfigService;
    }

    public async Task<int> Handle(CreateSaleOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = new SalesOrder()
        {
            OrderReferenceJlp = request.OrderReference, OrderDateJlp = request.OrderDate, Status = "Created"
        };
        
        try
        {
            var jlpOrder = await _jlpRequestService.GetOrdersByIdReference(entity.OrderReferenceJlp);
            var order = jlpOrder?.Results?.FirstOrDefault();
            if (order == null)
            {
                throw new Exception("Order not found in JLP");
            }
            var shippingAddressLines = jlpOrder?.Results?
                .Select(x =>
                    $"{x.Shipping_Address?.line_1},\n" +
                    $"{x.Shipping_Address?.line_2},\n" +
                    $"{x.Shipping_Address?.city},\n" +
                    $"{x.Shipping_Address?.postal_code},\n" +
                    $"{x.Shipping_Address?.full_name},\n" +
                    $"{x.Shipping_Address?.phone},\n" +
                    $"{x.Shipping_Address?.country},\n" +
                    $"{x.Shipping_Address?.email}\n")
                .Where(part => part != null)
                .ToList();
            
            var checkOrder = await _context.SalesOrder.FirstOrDefaultAsync(x => x.OrderReferenceJlp == order.Order_Reference, cancellationToken);
            if (checkOrder != null)
            {
                
                entity = checkOrder;
                
            }
            else
            {
                entity.Status = nameof(StatusOrderSale.SentToJlp);
                entity.OrderReferenceJlp = order.Order_Reference;
                entity.OrderDateJlp = order.Order_Date;
            
                _context.SalesOrder.Add(entity);
                await _context.SaveChangesAsync(cancellationToken);
                
                //FIXME: we need to add the log to the database and then publish the event 
                var log = new SalesOrderLog()
                {
                    SalesOrderId = entity.Id,
                    Status = entity.Status,
                    Message = $"Se recibe Orden desde JLP con referencia {entity.OrderReferenceJlp} y se crea en Netsuite",
                    Created = DateTime.Now,
                    Timestamp = DateTime.Now
                };
                _context.SalesOrderLog.Add(log);
                await _context.SaveChangesAsync(cancellationToken);
            }
            
            
            var createOrderRequest = new OrderDto()
            {
                OrderId = entity.Id, // Order from DB
                Action = "create",
                RecordType = "salesorder",
                Customform =
                    new CustomFormDto() { Text = "TROPIC FEEL - Sales Order" },
                Entity = _appConfigService.Entity,
                Memo = order?.Order_Reference,
                CustbodyPwkTipoSo = new CustbodyPwkTipoSoDto() { Text = "B2B" },
                Location = _appConfigService.Location,
                Class = _appConfigService.Class,
                Department = _appConfigService.Department,
                Currency = new CurrencyDto() { InternalId = _appConfigService.Currency }, // OrderJlpDto.currency_Code como sera el mapeo de este campo?
                Orderstatus = "B", //order?.Status,
                CustbodyPwkShipmentOptionEvx = _appConfigService.CustbodyPwkShipmentOptionEvx,
                Shipaddresslist = -2,
                Shipaddress = string.Join(Environment.NewLine, shippingAddressLines ?? Enumerable.Empty<string>()),
                CustbodyPwkCourier = _appConfigService.CustbodyPwkCourier,
                Shipmethod = _appConfigService.Shipmethod,
                Terms = _appConfigService.Terms,
                CustbodyPwkOrderType = _appConfigService.CustbodyPwkOrderType,
                Items = order?.Items?.Select(item => new ItemDto
                {
                    //Item = item.item_id,
                    Amount = item.total.ToString(),
                    Description = item.description,
                    Grossamt = item.subtotal.ToString(),
                    ItemCode = item.part_number,
                    ItemDisplay = $"{item.part_number} : {item.name}",
                    Quantity = item.quantity?.ToString(),
                    Taxcode = _appConfigService.Taxcode,
                    Ean = item.retailer_additional_reference, //retailer_additional_reference is EAN
                }).ToList() ?? new List<ItemDto>()
            };
            foreach (var item in createOrderRequest.Items )
            {
                item.ModelNumber = await GetModelNumber(item?.ItemCode ?? string.Empty);
            }
            if (createOrderRequest.Items.Any(x => string.IsNullOrEmpty(x.ItemCode)))
            {
                //FIXME: we need to add the log to the database and then publish the event 
                _context.SalesOrderLog.Add(new SalesOrderLog()
                {
                    SalesOrderId = entity.Id,
                    Status = nameof(StatusOrderSale.Error),
                    Message = "a SKU received from JLP is null",
                    Created = DateTime.Now,
                    Timestamp = DateTime.Now
                });
                await _context.SaveChangesAsync(cancellationToken);
                return 0;
            }
            
            if(string.IsNullOrEmpty(entity.OrderSprint) || string.IsNullOrEmpty(entity.OrderNetSuite))
            {
                var createOrder = new NetSuitCreateOrderEvent(createOrderRequest);
                await _mediator.Publish(createOrder, cancellationToken);
            }
            
            return entity.Id;
        }
        catch (Exception ex)
        {
            // Handle the exception as needed
            Console.WriteLine($"An error occurred: {ex.Message}");
            // You may want to log the exception or take other appropriate actions
            return 0;
        }
    }
    private async Task<string> GetModelNumber(string partNumber)
    {
        var result = await _jlpRequestService.GetProductsByPartNumber(partNumber);
         var product = result?.results?.FirstOrDefault();
        return product?.data?.modelNo ?? string.Empty;
    } 
}
