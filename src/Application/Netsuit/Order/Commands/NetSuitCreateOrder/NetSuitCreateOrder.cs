using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Domain.Dtos.Netsuit;

namespace TropicFeel.Application.Order.Commands.NetSuitCreateOrder;

public record NetSuitCreateOrderCommand : IRequest<int>
{
    public string? Action { get; set; }
    public string? RecordType { get; set; }
    public CustomFormCommand? Customform { get; set; }
    public string? Entity { get; set; }
    public string? Memo { get; set; }
    public CustbodyPwkTipoSoCommand? CustbodyPwkTipoSo { get; set; }
    public int Location { get; set; }
    public int Class { get; set; }
    public int Department { get; set; }
    public CurrencyCommand? Currency { get; set; }
    public string? Orderstatus { get; set; }
    public int CustbodyPwkShipmentOptionEvx { get; set; }
    public int Shipaddresslist { get; set; }
    public string? Shipaddress { get; set; }
    public int CustbodyPwkCourier { get; set; }
    public int Shipmethod { get; set; }
    public int Terms { get; set; }
    public List<ItemCommand>? Item { get; set; }
}

public class CustomFormCommand
{
    public string? Text { get; set; }
}

public class CustbodyPwkTipoSoCommand
{
    public string? Text { get; set; }
}

public class CurrencyCommand
{
    public int InternalId { get; set; }
}

public class ItemCommand
{
    public string? Amount { get; set; }
    public string? Description { get; set; }
    public string? Grossamt { get; set; }
    public string? Item { get; set; }
    public string? ItemDisplay { get; set; }
    public string? Quantity { get; set; }
    public string? Taxcode { get; set; }
}

public class NetSuitCreateOrderCommandValidator : AbstractValidator<NetSuitCreateOrderCommand>
{
    public NetSuitCreateOrderCommandValidator()
    {
    }
}

public class NetSuitCreateOrderCommandHandler : IRequestHandler<NetSuitCreateOrderCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly INetsuitClientService _netsuitClientService;

    public NetSuitCreateOrderCommandHandler(IApplicationDbContext context, INetsuitClientService netsuitClientService)
    {
        _context = context;
        _netsuitClientService = netsuitClientService;
    }

    public async Task<int> Handle(NetSuitCreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createOrderRequest = new OrderDto()
            {
                Action = request.Action,
                RecordType = request.RecordType,
                Customform = new CustomFormDto() { Text = request.Customform?.Text ?? string.Empty },
                Entity = request.Entity,
                Memo = request.Memo,
                CustbodyPwkTipoSo =
                    new CustbodyPwkTipoSoDto() { Text = request.CustbodyPwkTipoSo?.Text ?? string.Empty },
                Location = request.Location,
                Class = request.Class,
                Department = request.Department,
                Currency = new CurrencyDto() { InternalId = request.Currency?.InternalId ?? 0 },
                Orderstatus = request.Orderstatus,
                CustbodyPwkShipmentOptionEvx = request.CustbodyPwkShipmentOptionEvx,
                Shipaddresslist = request.Shipaddresslist,
                Shipaddress = request.Shipaddress,
                CustbodyPwkCourier = request.CustbodyPwkCourier,
                Shipmethod = request.Shipmethod,
                Terms = request.Terms,
                Items = request.Item?.Select(x => new ItemDto()
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    Grossamt = x.Grossamt,
                    ItemDisplay = x.ItemDisplay,
                    Quantity = x.Quantity,
                    Taxcode = x.Taxcode
                }).ToList()
            };
            var orders = new List<OrderDto>();
            orders.Add(createOrderRequest);
            await _netsuitClientService.CreateOrder(orders);

            //Se guarda en la base de datos local la orden
            var entity = new Domain.Entities.Order()
            {
                Action = request.Action,
                RecordType = request.RecordType,
                Customform = new Domain.Entities.CustomForm() { Text = request.Customform?.Text ?? string.Empty },
                Entity = request.Entity,
                Memo = request.Memo,
                CustbodyPwkTipoSo =
                    new Domain.Entities.CustbodyPwkTipoSo()
                    {
                        Text = request.CustbodyPwkTipoSo?.Text ?? string.Empty
                    },
                Location = request.Location,
                Class = request.Class,
                Department = request.Department,
                Currency = new Domain.Entities.Currency() { InternalId = request.Currency?.InternalId ?? 0 },
                Orderstatus = request.Orderstatus,
                CustbodyPwkShipmentOptionEvx = request.CustbodyPwkShipmentOptionEvx,
                Shipaddresslist = request.Shipaddresslist,
                Shipaddress = request.Shipaddress,
                CustbodyPwkCourier = request.CustbodyPwkCourier,
                Shipmethod = request.Shipmethod,
                Terms = request.Terms,
                Items = request.Item?.Select(x => new Domain.Entities.Item()
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    Grossamt = x.Grossamt,
                    ItemDisplay = x.ItemDisplay,
                    Quantity = x.Quantity,
                    Taxcode = x.Taxcode
                }).ToList()
            };
            _context.Orders.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
