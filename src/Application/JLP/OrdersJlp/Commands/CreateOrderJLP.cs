using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TropicFeel.Application.Common.Interfaces;
using TropicFeel.Application.Order.Commands.CreateOrder;
using TropicFeel.Domain.Dtos.JLP;
using TropicFeel.Domain.Dtos.Sprint;

namespace TropicFeel.Application.JLP.OrderJlpLists.Commands;

public record CreateOrderJlpCommand : IRequest<OrderResponseJlpDto>
{
    public string? url { get; set; }
    public string? retailer { get; set; }
    public string? order_reference { get; set; }
    public DateTime order_date { get; set; }
    public string? status { get; set; }
    public string? purchase_order_reference { get; set; }
    public string? end_user_purchase_order_reference { get; set; }
    public string? additional_order_reference { get; set; }
    public string? comment { get; set; }
    public bool test_flag { get; set; }
    public string? supplier { get; set; }
    public  List<OrderItemDto>? items { get; set; }
    public string? currency_code { get; set; }
    public string? sub_total { get; set; }
    public string? tax { get; set; }
    public string? total { get; set; }
    public  ShippingAddressDto? shipping_address { get; set; }
}

    public class CreateOrderJLPCommandValidator : AbstractValidator<CreateOrderJlpCommand>
    {
        public CreateOrderJLPCommandValidator()
        {
        }
    }

public class CreateOrderJlpCommandHandler : IRequestHandler<CreateOrderJlpCommand, OrderResponseJlpDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IJlpRequestService _jlpRequestService;
    private readonly IMapper _mapper;

    public CreateOrderJlpCommandHandler(IApplicationDbContext context, IJlpRequestService jlpRequestService, IMapper mapper)
    {
        _context = context;
         _jlpRequestService = jlpRequestService;
        _mapper = mapper;
    }

    public async Task <OrderResponseJlpDto> Handle(CreateOrderJlpCommand request, CancellationToken cancellationToken)
    {
        var createOrderRequest =   _mapper.Map<OrderRequestJlpDto>(request);
          var orderRequest = await _jlpRequestService.CreateOrder(createOrderRequest);
           
         
        return orderRequest;
    }
}

